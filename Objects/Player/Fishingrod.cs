using Fishing_SharpDX.Graphics;
using Fishing_SharpDX.Helpers;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fishing_SharpDX.Objects.Player
{
    public class Fishingrod : MeshObject
    {
        private bool _isFishing = false;
        public bool IsFishing { get => _isFishing; }

        private Floater _floater;
        public Floater Floater {  get { return _floater; } }
        public Fishingrod(string name, DirectX3DGraphics directX3DGraphics, Renderer renderer,
            Vector4 initialPosition, Material material, Floater floater,float scale = 0.1f)
            : base(name, directX3DGraphics, renderer, initialPosition,
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
                material)
        {
            _floater = floater;
        }

        public bool StartFishing(Vector4 posiiton)
        {
            if(!_isFishing)
            {
                _isFishing = true;
                _floater.SetPosition(posiiton);
                return true;
            }

            return false;
        }

        public Fish EndFishing()
        {
            if (_isFishing)
            {
                _isFishing = false;
                return null;
            }
            return null;
            //return new Fish();
        }

        public void ResetFishing()
        {
            _isFishing = false;
        }

        public void RotateAroundPosition(Vector4 position, float yaw)
        {
            yaw *= -1;
            float newLocalX = position.X + (float)((Position.X - position.X) * Math.Cos(yaw) - (Position.Z - position.Z) * Math.Sin(yaw));
            float newLocalZ = position.Z + (float)((Position.X - position.X) * Math.Sin(yaw) + (Position.Z - position.Z) * Math.Cos(yaw));

            // Console.WriteLine(Math.Sqrt(Math.Pow(newLocalX, 2) + Math.Pow(newLocalZ, 2)));

            MoveTo(newLocalX, Position.Y, newLocalZ);

            YawBy(yaw * -1);
        }

        public override void Render(Matrix viewMatrix, Matrix projectionMatrix)
        {
            base.Render(viewMatrix, projectionMatrix);

            if (_isFishing)
            {
                _floater.Render(viewMatrix, projectionMatrix);
            }
        }
    }
}
