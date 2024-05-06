struct pixelData
{
    float4 positionScreenSpace : SV_Position;
    float4 positionWorldSpace : POSITION0;
    float4 normalWorldSpace : POSITION1;
    float4 color : COLOR0;
    float2 texCoord0 : TEXCOORD0;
};

#define MAX_LIGHTS 8

// Light types.
#define DIRECTIONAL_LIGHT	0
#define POINT_LIGHT			1
#define SPOT_LIGHT			2

Texture2D whiteTexture : register(t0);
sampler whiteSampler : register(s0);

Texture2D meshTexture : register(t1);
sampler meshSampler : register(s1);

cbuffer materialProperties : register(b0)
{
    float4 emissive;
    float4 ambient;
    float4 diffuse;
    float4 specular;
    float specularPower;
    bool textured;
    float2 mPadding; // Padding to 16 byte boundary
}

struct LightSourceProperties
{
    float4 position;
    float4 direction;
    float4 color;
    int lightType;
    float spotAngle;
    float constantAttenuation;
    float linearAttenuation;
    float quadraticAttenuation;
    int enabled;
    float2 lPadding; // Padding to 16 byte boundary
};

cbuffer lightProperties : register(b1)
{
    float4 eyePosition;
    float4 globalAmbient;
    LightSourceProperties lights[MAX_LIGHTS];
};

float4 computeDiffusePart(LightSourceProperties light, float3 l, float3 n)				// l - to light, n - normal
{
    //float toons[] = { 0.0, 0.1, 0.5, 1.5, 2.0};
    float n_dot_l = max(0, dot(n, l));
    //(0.2 + max(dot(n, l), 0.0));
    //max(0, dot(n, l));
   
    return light.color * n_dot_l;
}

float4 computeSpecularPart(LightSourceProperties light, float3 v, float3 l, float3 n)	// v - to eye, l - to light, n - normal, r - reflection, h - half between l & v
{
    // Blinn-Phong lighting
    float3 h = normalize(l + v);
    float n_dot_h = max(0, dot(n, h));
    
    return light.color * pow(n_dot_h, specularPower);
}

float computeAttenuation(LightSourceProperties light, float d)						// Fade off based on distance. d - distance
{
    return 1.0f / (light.constantAttenuation + light.linearAttenuation * d + light.quadraticAttenuation * d * d);
}

struct LightingResult												// Parts of light colors
{
    float4 diffusePart;
    float4 specularPart;
};

LightingResult computePointLight(LightSourceProperties light, float3 v, float4 p, float3 n) // v - to eye, p - point where pixel, n - normal
{
    LightingResult result;

    float3 l = (light.position - p).xyz;
    float distance = length(l);
    l = l / distance;

    float attenuation = computeAttenuation(light, distance);

    result.diffusePart = computeDiffusePart(light, l, n) * attenuation;
    result.specularPart = computeSpecularPart(light, v, l, n) * attenuation;

    return result;
}

LightingResult computeDirectionalLight(LightSourceProperties light, float3 v, float4 p, float3 n) // v - to eye, p - point where pixel, n - normal
{
    LightingResult result;

    float3 l = -light.direction.xyz;

    result.diffusePart = computeDiffusePart(light, l, n);
    result.specularPart = computeSpecularPart(light, v, l, n);

    return result;
}

LightingResult computeSpotLight(LightSourceProperties light, float3 v, float4 p, float3 n)	// v - to eye, p - point where pixel, n - normal
{
    LightingResult result;

    float3 l = (light.position - p).xyz;
    float distance = length(l);
    l = l / distance;

    float minCos = cos(light.spotAngle);
    float maxCos = (minCos + 1.0f) / 2.0f;
    float cosAngle = dot(light.direction.xyz, -l);
    float spotIntensity = smoothstep(minCos, maxCos, cosAngle);

    float attenuation = computeAttenuation(light, distance);

    result.diffusePart = computeDiffusePart(light, l, n) * spotIntensity * attenuation; // Можно убрать интенсивность для появления лесенки как от point light
    result.specularPart = computeSpecularPart(light, v, l, n) * spotIntensity * attenuation;
    
    return result;
}

float4 applyLights(pixelData input)
{
    float3 v = normalize(eyePosition.xyz - input.positionWorldSpace.xyz);
    float3 n = normalize(input.normalWorldSpace.xyz);

    float4 ambientPart = ambient * globalAmbient;

    float4 diffusePart = float4(0, 0, 0, 0);
    float4 specularPart = float4(0, 0, 0, 0);

    for (int i = 0; i < MAX_LIGHTS; i++)
    {
        LightSourceProperties light = lights[i];
        if (light.enabled == 0)
            continue;

        LightingResult lightingResult;

        if (light.lightType == DIRECTIONAL_LIGHT)
            lightingResult = computeDirectionalLight(light, v, input.positionWorldSpace, n);
        else if (light.lightType == POINT_LIGHT)
            lightingResult = computePointLight(light, v, input.positionWorldSpace, n);
        else if (light.lightType == SPOT_LIGHT)
            lightingResult = computeSpotLight(light, v, input.positionWorldSpace, n);

        diffusePart += lightingResult.diffusePart;
        specularPart += lightingResult.specularPart;
    }

    float4 finalColor = emissive + ambientPart + diffusePart + specularPart;
    
    if (textured)
    {
        float4 textureColor = meshTexture.Sample(meshSampler, input.texCoord0);
        finalColor *= textureColor;
    }
    else
    {
        finalColor *= input.color;
    }

    return finalColor;
}

float4 pixelShader(pixelData input) : SV_Target
{
    float4 color = applyLights(input);
    return color;
}