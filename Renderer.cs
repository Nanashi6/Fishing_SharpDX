using Fishing_SharpDX.Graphics;
using Fishing_SharpDX.Graphics.Light;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Runtime.InteropServices;
using Buffer11 = SharpDX.Direct3D11.Buffer;
using Device11 = SharpDX.Direct3D11.Device;

namespace Fishing_SharpDX
{
    public class Renderer : IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct PerObjectConstantBuffer // For vertex shader (b0)
        {
            public Matrix worldMatrix;
            public Matrix worldViewMatrix;
            public Matrix inverseTransposeMatrix;
            public Matrix worldViewProjectionMatrix;
            public float time;
            public int timeScaling;
            public Vector2 oPadding; // Padding to 16 byte boundary
        }

        private DirectX3DGraphics _directX3DGraphics;

        private VertexShader _vertexShader;

        private PixelShader _pixelShader;

        private ShaderSignature _shaderSignature;

        private InputLayout _inputLayout;

        private PerObjectConstantBuffer _perObjectConstantBuffer;

        private Buffer11 _perObjectConstantBufferObject;

        private Buffer11 _materialConstantBuffer;

        private Buffer11 _illuminationConstantBuffer;

        public SamplerState AnisotropicSampler { get; private set; }

        public Renderer(DirectX3DGraphics directX3DGraphics)
        {
            _directX3DGraphics = directX3DGraphics;
            Device11 device = _directX3DGraphics.Device;
            DeviceContext deviceContext = _directX3DGraphics.DeviceContext;

            // Compile Vertex and Pixel shaders
            CompilationResult vertexShaderByteCode = ShaderBytecode.CompileFromFile("Shaders\\vertex.hlsl", "vertexShader", "vs_5_0");
            _vertexShader = new VertexShader(device, vertexShaderByteCode);
            CompilationResult pixelShaderByteCode = ShaderBytecode.CompileFromFile("Shaders\\pixel.hlsl", "pixelShader", "ps_5_0");
            _pixelShader = new PixelShader(device, pixelShaderByteCode);

            // Input elements.
            InputElement[] inputElements = new[] {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                new InputElement("NORMAL", 0, Format.R32G32B32A32_Float, 16, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 32, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 48, 0)
            };

            // Layout from VertexShader input signature
            _shaderSignature = ShaderSignature.GetInputSignature(vertexShaderByteCode);
            _inputLayout = new InputLayout(device, _shaderSignature, inputElements);

            Utilities.Dispose(ref vertexShaderByteCode);
            Utilities.Dispose(ref pixelShaderByteCode);

            // Prepare All the stages
            deviceContext.InputAssembler.InputLayout = _inputLayout;
            deviceContext.VertexShader.Set(_vertexShader);
            deviceContext.PixelShader.Set(_pixelShader);

            SamplerStateDescription samplerStateDescription = new SamplerStateDescription
            {
                Filter = Filter.Anisotropic,
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                MipLodBias = 0.0f,
                MaximumAnisotropy = 16,
                ComparisonFunction = Comparison.Never,
                BorderColor = new SharpDX.Mathematics.Interop.RawColor4(1.0f, 1.0f, 1.0f, 1.0f),
                MinimumLod = 0,
                MaximumLod = float.MaxValue
            };

            AnisotropicSampler = new SamplerState(_directX3DGraphics.Device, samplerStateDescription);

        }

        public void SetPerObjectConstants(float time, int timeScaling)
        {
            _perObjectConstantBuffer.time = time;
            _perObjectConstantBuffer.timeScaling = timeScaling;
        }

        public void CreateConstantBuffers()
        {
            Device11 device = _directX3DGraphics.Device;
            DeviceContext deviceContext = _directX3DGraphics.DeviceContext;

            _perObjectConstantBufferObject = new Buffer11(
                device,
                Utilities.SizeOf<PerObjectConstantBuffer>(),
                ResourceUsage.Dynamic,
                BindFlags.ConstantBuffer,
            CpuAccessFlags.Write,
                ResourceOptionFlags.None,
                0);

            _materialConstantBuffer = new Buffer11(
                device,
                Utilities.SizeOf<Material.MaterialDescription>(),
                ResourceUsage.Dynamic,
                BindFlags.ConstantBuffer,
            CpuAccessFlags.Write,
                ResourceOptionFlags.None,
                0);

            _illuminationConstantBuffer = new Buffer11(
                device,
                Utilities.SizeOf<Illumination.IlluminationDescription>(),
                ResourceUsage.Dynamic,
                BindFlags.ConstantBuffer,
                CpuAccessFlags.Write,
                ResourceOptionFlags.None,
                0);
        }

        public void BeginRender()
        {
            _directX3DGraphics.ClearBuffers(Color.Wheat);
        }

        public void UpdatePerObjectConstantBuffer(int index, Matrix world, Matrix view, Matrix projection)
        {
            DeviceContext deviceContext = _directX3DGraphics.DeviceContext;
            _perObjectConstantBuffer.worldMatrix = world;
            _perObjectConstantBuffer.inverseTransposeMatrix = Matrix.Invert(world);
            _perObjectConstantBuffer.worldMatrix.Transpose();
            _perObjectConstantBuffer.worldViewMatrix = Matrix.Multiply(world, view);
            _perObjectConstantBuffer.worldViewProjectionMatrix = Matrix.Multiply(_perObjectConstantBuffer.worldViewMatrix, projection);
            _perObjectConstantBuffer.worldViewMatrix.Transpose();
            _perObjectConstantBuffer.worldViewProjectionMatrix.Transpose();
            DataStream dataStream;
            deviceContext.MapSubresource(_perObjectConstantBufferObject, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out dataStream);
            dataStream.Write(_perObjectConstantBuffer);
            deviceContext.UnmapSubresource(_perObjectConstantBufferObject, 0);
            deviceContext.VertexShader.SetConstantBuffer(0, _perObjectConstantBufferObject);
        }

        public void SetWhiteTexture(Texture whiteTexture)
        {
            DeviceContext deviceContext = _directX3DGraphics.DeviceContext;
            deviceContext.PixelShader.SetShaderResource(0, whiteTexture.ShaderResourceView);
            deviceContext.PixelShader.SetSampler(0, whiteTexture.SamplerState);
        }

        public void UpdateMaterialProperties(Material material)
        {
            DeviceContext deviceContext = _directX3DGraphics.DeviceContext;
            Material.MaterialDescription materialDescription = material.MaterialProperties;
            DataStream dataStream;
            deviceContext.MapSubresource(_materialConstantBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out dataStream);
            dataStream.Write(material.MaterialProperties);
            deviceContext.UnmapSubresource(_materialConstantBuffer, 0);
            deviceContext.PixelShader.SetConstantBuffer(0, _materialConstantBuffer);

            deviceContext.PixelShader.SetShaderResource(1, material.Texture.ShaderResourceView);
            deviceContext.PixelShader.SetSampler(1, material.Texture.SamplerState);
        }

        public void UpdateIlluminationProperties(Illumination illumination)
        {
            DeviceContext deviceContext = _directX3DGraphics.DeviceContext;
            DataStream dataStream;
            deviceContext.MapSubresource(_illuminationConstantBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out dataStream);
            dataStream.Write(illumination.IlluminationProperties);
            deviceContext.UnmapSubresource(_illuminationConstantBuffer, 0);
            deviceContext.PixelShader.SetConstantBuffer(1, _illuminationConstantBuffer);
        }

        public void EndRender()
        {
            _directX3DGraphics.SwapChain.Present(1, PresentFlags.Restart);
        }

        public void Dispose()
        {
            Utilities.Dispose(ref _illuminationConstantBuffer);
            Utilities.Dispose(ref _materialConstantBuffer);
            Utilities.Dispose(ref _perObjectConstantBufferObject);
            Utilities.Dispose(ref _inputLayout);
            Utilities.Dispose(ref _shaderSignature);
            Utilities.Dispose(ref _pixelShader);
            Utilities.Dispose(ref _vertexShader);
        }
    }
}
