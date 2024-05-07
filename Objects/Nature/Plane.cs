using Fishing_SharpDX.Graphics;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing_SharpDX.Objects.Nature
{
    public class Plane : MeshObject
    {
        public Plane(string name, DirectX3DGraphics directX3DGraphics, Renderer renderer, Vector4 initialPosition, Material material, int scale = 1)
            : base(name, directX3DGraphics, renderer, initialPosition,
                    new MeshObject.VertexDataStruct[4]
                    {
                        new MeshObject.VertexDataStruct
                        {
                            position = new Vector4(1f * scale, 0f, 1f * scale, 1f),
                            normal = new Vector4(0f, 1f, 0f, 1f),
                            color = new Vector4(0f, 0f, 0f, 1f)
                        },
                        new MeshObject.VertexDataStruct
                        {
                            position = new Vector4(1f * scale, 0f, -1f * scale, 1f),
                            normal = new Vector4(0f, 1f, 0f, 1f),
                            color = new Vector4(0f, 0f, 0f, 1f)
                        },
                        new MeshObject.VertexDataStruct
                        {
                            position = new Vector4(-1f * scale, 0f, -1f * scale, 1f),
                            normal = new Vector4(0f, 1f, 0f, 1f),
                            color = new Vector4(0f, 0f, 0f, 1f)
                        },
                        new MeshObject.VertexDataStruct
                        {
                            position = new Vector4(-1f * scale, 0f, 1f * scale, 1f),
                            normal = new Vector4(0f, 1f, 0f, 1f),
                            color = new Vector4(0f, 0f, 0f, 1f)
                        }
                    },
                  new uint[]
                  {
                    1, 0, 2,
                    2, 0, 3
                  },
                  material)
        {
        }
    }
}
