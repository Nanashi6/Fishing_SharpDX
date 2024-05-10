using Fishing_SharpDX.Graphics;
using Fishing_SharpDX.Interface;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.WIC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fishing_SharpDX.Objects.Parsers
{
    public class ObjParser
    {
        static public MeshObject CreateObject(string nameObject, string filePath, DirectX3DGraphics directX3DGraphics, Renderer renderer, SamplerState samplerState, Vector4 position, float yaw, float pitch, float roll)
        {
            string matPath = "";
            string matName = "";
            List<Vector4> vertices = new List<Vector4>();
            List<Vector4> normals = new List<Vector4>();
            List<Vector2> vertexTex = new List<Vector2>();
            List<string> vertexData = new List<string>();

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] linePars = lines[i].Split(' ');

                switch (linePars[0])
                {
                    case "mtllib":
                        matPath = linePars[1];
                        break;
                    case "v":
                        vertices.Add(new Vector4(float.Parse(linePars[1].Replace('.', ',')), float.Parse(linePars[2].Replace('.', ',')), float.Parse(linePars[3].Replace('.', ',')), 1.0f));
                        break;
                    case "vn":
                        normals.Add(new Vector4(float.Parse(linePars[1].Replace('.', ',')), float.Parse(linePars[2].Replace('.', ',')), float.Parse(linePars[3].Replace('.', ',')), 1f));
                        break;
                    case "vt":
                        vertexTex.Add(new Vector2(float.Parse(linePars[1].Replace('.', ',')), 1.0f - float.Parse(linePars[2].Replace('.', ','))));
                        break;
                    case "usemtl":
                        matName = linePars[1];
                        break;
                    case "f":
                        vertexData.Add(linePars[1]);
                        vertexData.Add(linePars[2]);
                        vertexData.Add(linePars[3]);
                        break;
                }
            }

            List<MeshObject.VertexDataStruct> vertexDataStructs = new List<MeshObject.VertexDataStruct>();

            for (int i = 0; i < vertexData.Count; i++)
            {
                string[] param = vertexData[i].Split('/');

                int t = 0;
                int v = int.Parse(param[0]);
                if (!String.IsNullOrEmpty(param[1])) t = int.Parse(param[1]);
                int n = int.Parse(param[2]);

                vertexDataStructs.Add(new MeshObject.VertexDataStruct
                    {
                        position = vertices[v - 1],
                        texCoord0 = vertexTex.Count > 0 ? vertexTex[t - 1] : new Vector2(0f, 0f),
                        normal = normals[n - 1]
                    }
                ) ;
            }

            List<uint> indices = new List<uint>();
            for (int i = 0; i < vertexData.Count; i++)
            {
                indices.Add((uint)i);
            }

            Material material = null;
            Texture texture;

            string name = "";
            Vector4 ambients = new Vector4(0.1f, 0.1f, 0.1f, 1.0f);
            Vector4 specular = new Vector4(1f, 1f, 1f, 1.0f);
            Vector4 emissive = new Vector4(0.1f, 0.1f, 0.1f, 1.0f); ;
            Vector4 diffuse = new Vector4(1f, 1f, 1f, 1.0f);
            float specularPower = 0.0f;
            string texPath = "white.png";
            bool textured = false;

            lines = File.ReadAllLines("3D Objects and Textures/" + matPath);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] linePars = lines[i].Split(' ');

                switch (linePars[0])
                {
                    case "newmtl":
                        name = linePars[1];
                        break;
                    case "Ka":
                        ambients = new Vector4(float.Parse(linePars[1].Replace('.', ',')), float.Parse(linePars[2].Replace('.', ',')), float.Parse(linePars[3].Replace('.', ',')), 1.0f);
                        break;
                    case "Ks":
                        specular = new Vector4(float.Parse(linePars[1].Replace('.', ',')), float.Parse(linePars[2].Replace('.', ',')), float.Parse(linePars[3].Replace('.', ',')), 1.0f);
                        break;
                    case "Ke":
                        emissive = new Vector4(float.Parse(linePars[1].Replace('.', ',')), float.Parse(linePars[2].Replace('.', ',')), float.Parse(linePars[3].Replace('.', ',')), 1.0f);
                        break;
                    case "Kd":
                        diffuse = new Vector4(float.Parse(linePars[1].Replace('.', ',')), float.Parse(linePars[2].Replace('.', ',')), float.Parse(linePars[3].Replace('.', ',')), 1.0f);
                        break;
                    case "Ns":
                        specularPower = float.Parse(linePars[1].Replace('.', ','));
                        break;
                    case "map_Kd":
                        texPath = linePars[1].Split('/')[linePars[1].Split('/').Length - 1];
                        textured = true;
                        break;
                }
            }

            texture = LoadTextureFromFile(texPath, samplerState, directX3DGraphics);
            material = new Material(name, emissive, ambients, diffuse, specular, specularPower, textured, texture);

            return new MeshObject(nameObject, directX3DGraphics, renderer, position, vertexDataStructs.ToArray(), indices.ToArray(), material);
        }

        private static Texture LoadTextureFromFile(string fileName, SamplerState samplerState, DirectX3DGraphics _directX3DGraphics)
        {
            ImagingFactory _imagingFactory = new ImagingFactory();

            BitmapDecoder decoder = new BitmapDecoder(_imagingFactory, "3D Objects and Textures/" + fileName, DecodeOptions.CacheOnDemand);
            BitmapFrameDecode bitmapFirstFrame = decoder.GetFrame(0);

            Utilities.Dispose(ref decoder);

            FormatConverter imageFormatConverter = new FormatConverter(_imagingFactory);
            imageFormatConverter.Initialize(bitmapFirstFrame, PixelFormat.Format32bppRGBA, BitmapDitherType.None, null, 0.0, BitmapPaletteType.Custom);
            int stride = imageFormatConverter.Size.Width * 4;
            DataStream buffer = new DataStream(imageFormatConverter.Size.Height * stride, true, true);
            imageFormatConverter.CopyPixels(stride, buffer);

            int width = imageFormatConverter.Size.Width;
            int height = imageFormatConverter.Size.Height;

            Texture2DDescription textureDescription = new Texture2DDescription()
            {
                Width = width,
                Height = height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.R8G8B8A8_UNorm,
                SampleDescription = _directX3DGraphics.SampleDescription,
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };
            Texture2D textureObject = new Texture2D(_directX3DGraphics.Device, textureDescription, new DataRectangle(buffer.DataPointer, stride));

            ShaderResourceViewDescription shaderResourceViewDescription = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture2D,
                Format = Format.R8G8B8A8_UNorm,
                Texture2D = new ShaderResourceViewDescription.Texture2DResource
                {
                    MostDetailedMip = 0,
                    MipLevels = -1
                }
            };
            ShaderResourceView shaderResourceView = new ShaderResourceView(_directX3DGraphics.Device, textureObject, shaderResourceViewDescription);

            Utilities.Dispose(ref imageFormatConverter);

            return new Texture(textureObject, shaderResourceView, width, height, samplerState);
        }
    }
}
