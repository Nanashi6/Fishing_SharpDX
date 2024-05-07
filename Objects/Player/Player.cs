using Fishing_SharpDX.Graphics;
using SharpDX;

namespace Fishing_SharpDX.Objects.Player
{
    public class Player : MeshObject
    {
        private Camera _camera;
        private float _mass = 5f;
        private float _jumpForce = 0.2f;
        private float _gravityForce = 1f;
        private bool _isGround = true;
        private float _fallTime = 0.0f;
        private int _score;
        public int Score { get { return _score; } }
        public Player(DirectX3DGraphics directX3DGraphics, Renderer renderer, Vector4 position, Camera camera)
        : base("Player", directX3DGraphics, renderer, Vector4.Zero, 
              new VertexDataStruct[]
              {  
				            //front
				            new VertexDataStruct
                            {
                                position = new Vector4(position.X - 3f, position.Y - 3f, position.Z + 3f, 1.0f), //bottom left
                            },
                            new VertexDataStruct
                            {
                                position = new Vector4(position.X + 3f, position.Y - 3f, position.Z + 3f, 1.0f), //bottom right
                            },
                            new VertexDataStruct
                            {
                                position = new Vector4(position.X - 3f, position.Y + 3f, position.Z + 3f, 1.0f), //top left
                            },
                              new VertexDataStruct
                            {
                                position = new Vector4(position.X + 3f, position.Y + 3f, position.Z + 3f, 1.0f), //top right
                            },
				
				            //back
				            new VertexDataStruct
                            {
                                position = new Vector4(position.X - 3f, position.Y - 3f, position.Z - 3f, 1.0f), //bottom left
                            },
                            new VertexDataStruct
                            {
                                position = new Vector4(position.X + 3f, position.Y - 3f, position.Z - 3f, 1.0f), //bottom right
                            },
                            new VertexDataStruct
                            {
                                position = new Vector4(position.X - 3f, position.Y + 3f, position.Z - 3f, 1.0f), //top left
                            },
                             new VertexDataStruct
                            {
                                position = new Vector4(position.X + 3f, position.Y + 3f, position.Z - 3f, 1.0f), //top right
                            },
              }, 
              new uint[]
               {
                        //front 
                        0, 1, 2,
                        1, 3, 2,

                        //back
                        4, 6, 5,
                        5, 6, 7,

                        //left
                        4, 5, 0,
                        5, 1, 0,

                        //right
                        2, 3, 6,
                        3, 7, 6,

                        //top
                        2, 6, 0,
                        6, 4, 0,

                        //bottom 
                        1, 5, 3,
                        5, 7, 3,
            }, null)
        {
            _score = 0;
            _camera = camera;
        }

        public void Gravity(float deltaTime)
        {
            //!_isGround
            if (base.Position.Y > 0)
            {
                _fallTime += 1.0f;
                Vector4 gravity = new Vector4(0f, -_gravityForce * _mass, 0f, 1.0f);
                Vector4 acceleration = gravity / _mass;
                Vector4 velocity = acceleration * _fallTime * deltaTime;

                base._position.Y += _jumpForce + velocity.Y;
                _camera._position.Y += _jumpForce + velocity.Y;

                if(base.Position.Y < 0)
                {
                    base._position.Y = 0f;
                    _camera._position.Y = 0f;
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
            y = 0;
            base.MoveBy(x, y, z);
            _camera.MoveBy(x, y, z);
        }

        public void RotationBy(float yaw, float pitch)
        {
            base.Yaw += yaw;
            _camera.Yaw += yaw;

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
    }
}
