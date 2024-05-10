using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing_SharpDX.Objects
{
    public class Camera : PositionalObject
    {
        private float _fovY;
        public float FOVY { get => _fovY; set => _fovY = value; }

        private float _aspect;
        public float Aspect { get => _aspect; set => _aspect = value; }

        public Camera(Vector4 position, float yaw = 0.0f, float pitch = 0.0f, float roll = 0.0f, float fovY = MathUtil.PiOverTwo, float aspect = 1.0f) : base(position, yaw, pitch, roll)
        {
            _fovY = fovY;
            _aspect = aspect;
        }

        public Matrix GetProjectionMatrix()
        {
            return Matrix.PerspectiveFovLH(_fovY, _aspect, 0.1f, 100.0f);
        }

        public Matrix GetViewMatrix()
        {
            Matrix rotation = Matrix.RotationYawPitchRoll(_yaw, _pitch, _roll);
            Vector3 viewTo = (Vector3)Vector4.Transform(Vector4.UnitZ, rotation);
            Vector3 viewUp = (Vector3)Vector4.Transform(Vector4.UnitY, rotation);
            return Matrix.LookAtLH((Vector3)_position, (Vector3)_position + viewTo, viewUp);
        }

        public Vector3 GetCameraPositionUpDown()
        {
            Matrix rotation = Matrix.RotationYawPitchRoll(_yaw, _pitch, _roll);
            return Vector3.TransformNormal(Vector3.UnitZ, rotation);
        }

        public Vector3 GetCameraPositionLeftRight()
        {
            Matrix rotation = Matrix.RotationYawPitchRoll(_yaw, _pitch, _roll);
            return Vector3.TransformNormal(Vector3.UnitX, rotation);
        }

        public Vector3 GetRayCast(Vector2 screenCenter)
        {
            Vector3 nearPoint = new Vector3(screenCenter.X, screenCenter.Y, 0.0f);
            Vector3 farPoint = new Vector3(screenCenter.X, screenCenter.Y, 1.0f);

            Matrix viewMatrix = GetViewMatrix();
            Vector3 nearPointWorld;
            Vector3 farPointWorld;

            Vector3.Unproject(ref nearPoint, 0, 0, screenCenter.X * 2, screenCenter.Y * 2, 0.0f, 200.0f, ref viewMatrix, out nearPointWorld);
            Vector3.Unproject(ref farPoint, 0, 0, screenCenter.X * 2, screenCenter.Y * 2, 0.0f, 200.0f, ref viewMatrix, out farPointWorld);

            return Vector3.Normalize(farPointWorld - nearPointWorld);
        }
    }
}
