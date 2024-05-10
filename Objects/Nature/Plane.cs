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

        public bool IsRayCast(Vector3 rayStart, Vector3 rayDirection, out Vector3 intersectionPoint)
        {
            BoundingBox boundingBox = GetBoundingBox();

            float tMin = (boundingBox.Min.X - rayStart.X) / rayDirection.X;
            float tMax = (boundingBox.Max.X - rayStart.X) / rayDirection.X;

            if (tMin > tMax)
            {
                float temp = tMin;
                tMin = tMax;
                tMax = temp;
            }

            float tYMin = (boundingBox.Min.Y - rayStart.Y) / rayDirection.Y;
            float tYMax = (boundingBox.Max.Y - rayStart.Y) / rayDirection.Y;

            if (tYMin > tYMax)
            {
                float temp = tYMin;
                tYMin = tYMax;
                tYMax = temp;
            }

            if ((tMin > tYMax) || (tYMin > tMax))
            {
                intersectionPoint = Vector3.Zero;
                return false;
            }

            if (tYMin > tMin)
            {
                tMin = tYMin;
            }

            if (tYMax < tMax)
            {
                tMax = tYMax;
            }

            float tZMin = (boundingBox.Min.Z - rayStart.Z) / rayDirection.Z;
            float tZMax = (boundingBox.Max.Z - rayStart.Z) / rayDirection.Z;

            if (tZMin > tZMax)
            {
                float temp = tZMin;
                tZMin = tZMax;
                tZMax = temp;
            }

            if ((tMin > tZMax) || (tZMin > tMax))
            {
                intersectionPoint = Vector3.Zero;
                return false;
            }

            float t = Math.Max(tMin, Math.Max(tYMin, tZMin));
            intersectionPoint = rayStart + rayDirection * t;

            return true;
        }
    }
}
