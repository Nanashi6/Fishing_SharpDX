using Fishing_SharpDX.Graphics;
using SharpDX;
using SharpDX.Direct2D1.Effects;
using System;

namespace Fishing_SharpDX.Objects.Player
{
    public class Player : MeshObject
    {
        private int _score;
        public int Score { get => _score; }
        private Camera _camera;
        private Fishingrod _fishingrod;
        private float _mass = 5f;
        private float _actionRadius = 0.15f;
        private float _speed = 10f;
        private float _jumpForce = 0.0f;
        private float _gravityForce = 1f;
        private bool _isGround = true;
        private float _fallTime = 0.0f;

        public Camera Camera { get => _camera; }
        public Fishingrod Fishingrod { get =>  _fishingrod; }
        public Player(DirectX3DGraphics directX3DGraphics, Renderer renderer, Vector4 position, Camera camera, Fishingrod fishingrod)
        : base("Player", directX3DGraphics, renderer, Vector4.Zero,
               new MeshObject.VertexDataStruct[24]
                {
                    new MeshObject.VertexDataStruct // front 0
                    {
                        position = new Vector4(-1.0f, 1.0f, -1.0f, 1.0f),
                        normal = new Vector4(0f,0f,-1f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 1.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // front 1
                    {
                        position = new Vector4(-1.0f, -1.0f, -1.0f, 1.0f),
                        normal = new Vector4(0f,0f,-1f,1f),
                        texCoord0 = new Vector2(0f, 1f),
                        color = new Vector4(0.0f, 1.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // front 2
                    {
                        position = new Vector4(1.0f, -1.0f, -1.0f, 1.0f),
                        normal = new Vector4(0f,0f,-1f,1f),
                        texCoord0 = new Vector2(1f, 0f),
                        color = new Vector4(0.0f, 1.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // front 3
                    {
                        position = new Vector4(1.0f, 1.0f, -1.0f, 1.0f),
                        normal = new Vector4(0f,0f,-1f,1f),
                        texCoord0 = new Vector2(1f, 1f),
                        color = new Vector4(0.0f, 1.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // right 4
                    {
                        position = new Vector4(1.0f, 1.0f, -1.0f, 1.0f),
                        normal = new Vector4(1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // right 5
                    {
                        position = new Vector4(1.0f, -1.0f, -1.0f, 1.0f),
                        normal = new Vector4(1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // right 6
                    {
                        position = new Vector4(1.0f, -1.0f, 1.0f, 1.0f),
                        normal = new Vector4(1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // right 7
                    {
                        position = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        normal = new Vector4(1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // back 8
                    {
                        position = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        normal = new Vector4(0f,0f,1f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // back 9
                    {
                        position = new Vector4(1.0f, -1.0f, 1.0f, 1.0f),
                        normal = new Vector4(0f,0f,1f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // back 10
                    {
                        position = new Vector4(-1.0f, -1.0f, 1.0f, 1.0f),
                        normal = new Vector4(0f,0f,1f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // back 11
                    {
                        position = new Vector4(-1.0f, 1.0f, 1.0f, 1.0f),
                        normal = new Vector4(0f,0f,1f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // left 12
                    {
                        position = new Vector4(-1.0f, 1.0f, 1.0f, 1.0f),
                        normal = new Vector4(-1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // left 13
                    {
                        position = new Vector4(-1.0f, -1.0f, 1.0f, 1.0f),
                        normal = new Vector4(-1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // left 14
                    {
                        position = new Vector4(-1.0f, -1.0f, -1.0f, 1.0f),
                        normal = new Vector4(-1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // left 15
                    {
                        position = new Vector4(-1.0f, 1.0f, -1.0f, 1.0f),
                        normal = new Vector4(-1f,0f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // top 16
                    {
                        position = new Vector4(-1.0f, 1.0f, 1.0f, 1.0f),
                        normal = new Vector4(0f,1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // top 17
                    {
                        position = new Vector4(-1.0f, 1.0f, -1.0f, 1.0f),
                        normal = new Vector4(0f,1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // top 18
                    {
                        position = new Vector4(1.0f, 1.0f, -1.0f, 1.0f),
                        normal = new Vector4(0f,1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // top 19
                    {
                        position = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        normal = new Vector4(0f,1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(1.0f, 1.0f, 0.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // bottom 20
                    {
                        position = new Vector4(-1.0f, -1.0f, -1.0f, 1.0f),
                        normal = new Vector4(0f,-1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // bottom 21
                    {
                        position = new Vector4(-1.0f, -1.0f, 1.0f, 1.0f),
                        normal = new Vector4(0f,-1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // bottom 22
                    {
                        position = new Vector4(1.0f, -1.0f, 1.0f, 1.0f),
                        normal = new Vector4(0f,-1f,0f,1f),
                        texCoord0 = new Vector2(0f, 0f),
                        color = new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
                    },
                    new MeshObject.VertexDataStruct // bottom 23
                    {
                        position = new Vector4(1.0f, -1.0f, -1.0f, 1.0f),
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
            null)
        {
            _score = 0;
            _camera = camera;
            _fishingrod = fishingrod;
        }

        public void Gravity(float deltaTime)
        {
            if (!_isGround)
            {
                _fallTime += 1.0f;
                Vector4 gravity = new Vector4(0f, -_gravityForce * _mass, 0f, 1.0f);
                Vector4 acceleration = gravity / _mass;
                Vector4 velocity = acceleration * _fallTime * deltaTime;

                base._position.Y += _jumpForce + velocity.Y;
                _camera._position.Y += _jumpForce + velocity.Y;

                if(base.Position.Y < (this.GetBoundingBox().Max.Y - this.GetBoundingBox().Min.Y) / 2)
                {
                    base._position.Y = (this.GetBoundingBox().Max.Y - this.GetBoundingBox().Min.Y) / 2;
                    _camera._position.Y = this.GetBoundingBox().Max.Y;
                }
            }
            else
            {
                _fallTime = 0.0f;
                _isGround = true;
            }
        }

        public void Jump()
        {
            if(_isGround)
            {
                _isGround = false;
                _position += new Vector4(0.0f, _jumpForce, 0.0f, 1.0f);
            }
        }

        public override void MoveBy(float x, float y, float z)
        {
            Vector4 prevPosition = Position;
            Vector3 fisPrevPos = (Vector3)_fishingrod.Position;

            y = 0;
            Vector3 moveVector = new Vector3(x * _speed, y, z * _speed);
            base.MoveBy(moveVector);
            _camera.MoveBy(moveVector);
            _fishingrod.MoveBy(moveVector);

            if(hasIntersection())
            {
                MoveTo((Vector3)prevPosition);
                _camera.MoveTo(prevPosition.X, _camera.Position.Y, prevPosition.Z);
                _fishingrod.MoveTo(fisPrevPos);
            }
        }

        public void RotationBy(float yaw, float pitch)
        {
            YawBy(yaw);
            _camera.YawBy(yaw);

            if(yaw != 0) _fishingrod.RotateAroundPosition(Position, yaw);

            //-89.9 и 45 градусов (ограничение в радианах)
            if(pitch < 0 && _camera.Pitch > -1.55f)
            {
                _camera.Pitch += pitch;
            }
            else if(pitch > 0 && _camera.Pitch < 0.78f)
            {
                _camera.Pitch += pitch;
            }
        }

        public void AddScore(int score)
        {
            _score += score;
        }

        public Vector3 GetPlayerPositionUpDown()
        {
            Matrix rotation = Matrix.RotationYawPitchRoll(_yaw, _pitch, _roll);
            return Vector3.TransformNormal(Vector3.UnitZ, rotation);
        }

        public Vector3 GetPlayerPositionLeftRight()
        {
            Matrix rotation = Matrix.RotationYawPitchRoll(_yaw, _pitch, _roll);
            return Vector3.TransformNormal(Vector3.UnitX, rotation);
        }

        private bool hasIntersection()
        {
            foreach(var obj in ObjectsStorage.Objects)
            {
                if(Position.X >= obj.GetBoundingBox().Min.X - _actionRadius && Position.X <= obj.GetBoundingBox().Max.X + _actionRadius &&
                    Position.Z >= obj.GetBoundingBox().Min.Z - _actionRadius && Position.Z <= obj.GetBoundingBox().Max.Z + _actionRadius)
                {
                    if (obj.Name == "Ground")
                    {
                        _isGround = true;
                        continue;
                    }
                    Console.WriteLine(obj.Name + $"{obj.GetBoundingBox().ToString()}");
                    return true;
                }
            }

            return false;
        }

        public override void Render(Matrix viewMatrix, Matrix projectionMatrix)
        {
            /*base.Render(viewMatrix, projectionMatrix);*/
            _fishingrod.Render(viewMatrix, projectionMatrix);
        }
    }
}
