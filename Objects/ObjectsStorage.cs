using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing_SharpDX.Objects
{
    internal static class ObjectsStorage
    {
        static List<MeshObject> objects = new List<MeshObject>();

        public static void AddObject(params MeshObject[] objects)
        {
            foreach(var obj in objects)
            {
                ObjectsStorage.objects.Add(obj);
            }
        }

        public static void Render(Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach(var obj in objects)
            {
                obj.Render(viewMatrix, projectionMatrix);
            }
        }
    }
}
