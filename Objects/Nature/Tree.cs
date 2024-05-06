using Fishing_SharpDX.Graphics;
using SharpDX;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing_SharpDX.Objects.Nature
{
    internal class Tree : MeshObject
    {
        public Tree(string name, DirectX3DGraphics directX3DGraphics, Renderer renderer, Vector4 position, Material material, float scale = 0.1f)
            : base(name, directX3DGraphics, renderer, position,
                new MeshObject.VertexDataStruct[24]
                {
                    new MeshObject.VertexDataStruct // front 0
                    {
                        position = new Vector4(-1.0f * scale, 3.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(0f,0f,-1f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 1.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // front 1
                    {
                        position = new Vector4(-1.0f * scale, -1.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(0f,0f,-1f,1f),
                        texCoord0 = new Vector2(0f, 1f),
                        color = new Vector4(0.0f, 1.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // front 2
                    {
                        position = new Vector4(1.0f * scale, -1.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(0f,0f,-1f,1f),
                        texCoord0 = new Vector2(1f, 0f),
                        color = new Vector4(0.0f, 1.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // front 3
                    {
                        position = new Vector4(1.0f * scale, 3.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(0f,0f,-1f,1f),
                        texCoord0 = new Vector2(1f, 1f),
                        color = new Vector4(0.0f, 1.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // right 4
                    {
                        position = new Vector4(1.0f * scale, 3.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // right 5
                    {
                        position = new Vector4(1.0f * scale, -1.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // right 6
                    {
                        position = new Vector4(1.0f * scale, -1.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // right 7
                    {
                        position = new Vector4(1.0f * scale, 3.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // back 8
                    {
                        position = new Vector4(1.0f * scale, 3.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(0f,0f,1f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // back 9
                    {
                        position = new Vector4(1.0f * scale, -1.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(0f,0f,1f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // back 10
                    {
                        position = new Vector4(-1.0f * scale, -1.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(0f,0f,1f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // back 11
                    {
                        position = new Vector4(-1.0f * scale, 3.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(0f,0f,1f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // left 12
                    {
                        position = new Vector4(-1.0f * scale, 3.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(-1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // left 13
                    {
                        position = new Vector4(-1.0f * scale, -1.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(-1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // left 14
                    {
                        position = new Vector4(-1.0f * scale, -1.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(-1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // left 15
                    {
                        position = new Vector4(-1.0f * scale, 3.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(-1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // top 16
                    {
                        position = new Vector4(-1.0f * scale, 3.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(0f,1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // top 17
                    {
                        position = new Vector4(-1.0f * scale, 3.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(0f,1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // top 18
                    {
                        position = new Vector4(1.0f * scale, 3.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(0f,1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // top 19
                    {
                        position = new Vector4(1.0f * scale, 3.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(0f,1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // bottom 20
                    {
                        position = new Vector4(-1.0f * scale, -1.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(0f,-1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // bottom 21
                    {
                        position = new Vector4(-1.0f * scale, -1.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(0f,-1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // bottom 22
                    {
                        position = new Vector4(1.0f * scale, -1.0f, 1.0f * scale, 1.0f),
                        normal = new Vector4(0f,-1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // bottom 23
                    {
                        position = new Vector4(1.0f * scale, -1.0f, -1.0f * scale, 1.0f),
                        normal = new Vector4(0f,-1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
                    }
                },
                new uint[36]
                {
                    8, 9, 10, 10, 11, 8,
                    12, 13, 14, 14, 15, 12,
                    20, 21, 22, 22, 23, 20,
                    0, 1, 2, 2, 3, 0,
                    4, 5, 6, 6, 7, 4,
                    16, 17, 18, 18, 19, 16
                },
                material
                )
        {
        }
    }
}
