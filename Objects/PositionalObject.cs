using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing_SharpDX.Objects
{
    public abstract class PositionalObject
    {
        internal Vector4 _position;
        public Vector4 Position => _position;

        internal float _yaw;
        public float Yaw { get => _yaw; set => _yaw = value; }

        internal float _pitch;
        public float Pitch { get => _pitch; set => _pitch = value; }

        internal float _roll;
        public float Roll { get => _roll; set => _roll = value; }

        public PositionalObject(Vector4 position, float yaw = 0.0f, float pitch = 0.0f, float roll = 0.0f)
        {
            _position = position;
            _yaw = yaw;
            _pitch = pitch;
            _roll = roll;
        }

        private void LimitAngleByPlusMinus2Pi(ref float angle)
        {
            if (angle > MathUtil.TwoPi)
                angle -= MathUtil.TwoPi;
            else if (angle < -MathUtil.TwoPi)
                angle += MathUtil.TwoPi;
        }

        public virtual void YawBy(float deltaYaw)
        {
            _yaw += deltaYaw;
            //LimitAngleByPlusMinus2Pi(ref _yaw);
        }

        public virtual void PitchBy(float deltaPitch)
        {
            _pitch += deltaPitch;
            LimitAngleByPlusMinus2Pi(ref _pitch);
        }

        public virtual void RollBy(float deltaRoll)
        {
            _roll += deltaRoll;
            LimitAngleByPlusMinus2Pi(ref _roll);
        }
        public virtual void MoveBy(float deltaX, float deltaY, float deltaZ)
        {
            _position.X += deltaX;
            _position.Y += deltaY;
            _position.Z += deltaZ;
        }

        public void MoveBy(Vector3 deltaDirection)
        {
            _position.X += deltaDirection.X;
            _position.Y += deltaDirection.Y;
            _position.Z += deltaDirection.Z;
        }

        public virtual void MoveTo(float x, float y, float z)
        {
            _position.X = x;
            _position.Y = y;
            _position.Z = z;
        }

        public void MoveTo(Vector3 newPosition)
        {
            _position.X = newPosition.X;
            _position.Y = newPosition.Y;
            _position.Z = newPosition.Z;
        }

        public void Translate(Vector4 translation)
        {
            _position += translation;
        }

        public Matrix GetWorldMatrix()
        {
            return Matrix.Multiply(Matrix.RotationYawPitchRoll(_yaw, _pitch, _roll), Matrix.Translation((Vector3)_position));
        }
    }
}
