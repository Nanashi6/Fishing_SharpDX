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
        private static List<MeshObject> _objects = new List<MeshObject>();

        public static List<MeshObject> Objects { get => _objects; }

        public static void AddObject(params MeshObject[] newObjects)
        {
            foreach(var obj in newObjects)
            {
                _objects.Add(obj);
            }
        }

        public static void Render(Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach(var obj in _objects)
            {
                obj.Render(viewMatrix, projectionMatrix);
            }
        }
    }
}
