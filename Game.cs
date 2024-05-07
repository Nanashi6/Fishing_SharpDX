using Fishing_SharpDX.Graphics.Light;
using Fishing_SharpDX.Graphics;
using Fishing_SharpDX.Helpers;
using Fishing_SharpDX.Interface;
using Fishing_SharpDX.Objects;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DirectInput;
using SharpDX.WIC;
using SharpDX.Windows;
using SharpDX;
using System;
using SharpDX.DXGI;

using Fishing_SharpDX.Objects.Player;
using Plane = Fishing_SharpDX.Objects.Nature.Plane;
using Fishing_SharpDX.Objects.Nature;
using Fishing_SharpDX.Enums;

namespace Fishing_SharpDX
{
    public class Game
    {
        RenderForm _renderForm;
        DirectX3DGraphics _directX3DGraphics;

        #region Objects

        Player _player;
        Plane _ground;
        Plane _water;
        Rock _rock1;
        Tree _tree1;

        #endregion

        #region Materials

        Material _groundMaterial;
        Material _waterMaterial;
        Material _rockMaterial;
        Material _treeMaterial;

        #endregion

        #region Textures

        ImagingFactory _imagingFactory;

        #endregion

        #region Lights

        Illumination _illumination;

        LightSource _directionalLight;

        #endregion

        #region Interfaces

        DirectX2DGraphics _directX2DGraphics;
        HUD _hud;

        #endregion


        // Objects
        Camera _camera;
        Renderer _renderer;

        // Helpers
        TimeHelper _timeHelper;
        Input _input;

        private bool _firstRun = true;

        //hud
        private bool _isPressI = false;
        private bool _isDrawNotebook = false;

        private bool _isDrawBitingCondition = false;
        private bool _isDrawCenterMessage = false;
        private float _timeBitingConditionDuration = 2.5f;
        private float _timeCenterMessageDuration = 2.5f;
        private float _timeBitingCondition = 0.0f;
        private float _timeCenterMessage = 0.0f;

        public Game()
        {
            _imagingFactory = new ImagingFactory();

            _renderForm = new RenderForm();
            _renderForm.UserResized += RenderFormResizedCallback;
            _directX3DGraphics = new DirectX3DGraphics(_renderForm);
            _renderer = new Renderer(_directX3DGraphics);
            _renderer.CreateConstantBuffers();

            _camera = new Camera(new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

            _directionalLight = new LightSource();
            _directionalLight.Color = new Vector4(0f, 1f, 1f, 1f);
            _directionalLight.Direction = new Vector4(0f, -1f, 0f, 1f);
            _directionalLight.Position = new Vector4(0f, 2f, 0f, 1f);
            /*_directionalLight.SpotAngle = (float)Math.PI / 6.0f;*/
            _directionalLight.LightSourceType = (int)LightSource.LightType.DirectionalLight;

            _illumination = new Illumination(_camera.Position,
                                            new Vector4(0.3f, 0.3f, 0.3f, 1f),
                                            new LightSource[] { _directionalLight }
                                            );

            _player = new Player(_directX3DGraphics, _renderer, new Vector4(0.0f, 0.0f, 0.0f, 0.0f), _camera);

            Texture groundTex = LoadTextureFromFile("Textures/ground.jpg", _renderer.AnisotropicSampler);
            _groundMaterial = new Material("GroundMaterial",
                new Vector4(0.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.07568f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.07568f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.07568f, 1.0f),
                32f, true, groundTex);
            _ground = new Plane("Ground", _directX3DGraphics, _renderer, new Vector4(0f, 0f, 0f, 1f), _groundMaterial, 10);

            Texture waterTex = LoadTextureFromFile("Textures/water.jpg", _renderer.AnisotropicSampler);
            _waterMaterial = new Material("WaterMaterial",
                new Vector4(0.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.5f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.5f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.5f, 1.0f),
                32f, true, waterTex);
            _water = new Plane("Water", _directX3DGraphics, _renderer, new Vector4(0f, 0f, 20f, 1f), _waterMaterial, 10);

            _rockMaterial = new Material("RockMaterial",
                new Vector4(0.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.5f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.5f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.5f, 1.0f),
                32f, false, waterTex);
            _rock1 = new Rock("Rock1", _directX3DGraphics, _renderer, new Vector4(0, 0.25f, 0, 1), _rockMaterial);

            _treeMaterial = new Material("TreeMaterial",
                new Vector4(0.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.5f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.5f, 1.0f),
                new Vector4(0.07568f, 0.61424f, 0.5f, 1.0f),
                32f, false, waterTex);
            _tree1 = new Tree("Tree1", _directX3DGraphics, _renderer, new Vector4(1, 1f, 5, 1), _treeMaterial);

            _input = new Input(_renderForm.Handle);
            _timeHelper = new TimeHelper();

            _directX2DGraphics = new DirectX2DGraphics(_directX3DGraphics);
            _hud = new HUD(_directX2DGraphics);
        }

        private void RenderLoopCallback()
        {
            if (_firstRun)
            {
                RenderFormResizedCallback(this, EventArgs.Empty);
                _firstRun = false;
            }
            _timeHelper.Update();
            _renderForm.Text = "FPS: " + _timeHelper.FPS.ToString();
            //test

            _input.Update();
            _camera.Yaw += _input.GetMouseDeltaX() * 0.01f;
            _camera.Pitch += _input.GetMouseDeltaY() * 0.01f;

            KeyUpdate();
            MouseUpdate();
            _player.Gravity(_timeHelper.DeltaT);

            Matrix viewMatrix = _camera.GetViewMatrix();
            Matrix projectionMatrix = _camera.GetProjectionMatrix();
            _renderer.BeginRender();

            _illumination.EyePosition = _camera.Position;
            _renderer.UpdateIlluminationProperties(_illumination);

            _ground.Render(viewMatrix, projectionMatrix);
            _water.Render(viewMatrix, projectionMatrix);

            _rock1.Render(viewMatrix, projectionMatrix);
            _tree1.Render(viewMatrix, projectionMatrix);

            RenderHUD();

            _renderer.EndRender();
        }
        public void RenderFormResizedCallback(object sender, EventArgs args)
        {
            _directX3DGraphics.Resize();
            _camera.Aspect = _renderForm.ClientSize.Width / (float)_renderForm.ClientSize.Height;
            _hud.Resize(_renderForm.Height, _renderForm.Width);
        }

        public void Run()
        {
            RenderLoop.Run(_renderForm, RenderLoopCallback);
        }

        public void KeyUpdate()
        {
            Vector3 direction = Vector3.Zero;
            if (_input.IsKeyPressed(Key.W))
            {
                direction += _player.GetPlayerPositionUpDown();
            }
            if (_input.IsKeyPressed(Key.S))
            {
                direction -= _player.GetPlayerPositionUpDown();
            }
            if (_input.IsKeyPressed(Key.D))
            {
                direction += _player.GetPlayerPositionLeftRight();
            }
            if (_input.IsKeyPressed(Key.A))
            {
                direction -= _player.GetPlayerPositionLeftRight();
            }
            if (_input.IsKeyPressed(Key.Space))
            {
                _player.Jump();
            }

            if (_input.IsKeyPressed(Key.I))
            {
                if (!_isPressI)
                {
                    _isDrawNotebook = !_isDrawNotebook;
                }

                _isPressI = true;
            }
            else
                _isPressI = false;

            if (_input.IsKeyPressed(Key.I))
            {
                _isDrawCenterMessage = true;
            }

            _player.MoveBy(direction.X, direction.Y, direction.Z);
        }

        public void MouseUpdate()
        {
            float yaw = _input.GetMouseDeltaX() * 0.001f;
            float pitch = _input.GetMouseDeltaY() * 0.001f;

            _player.RotationBy(yaw, pitch);
        }

        public void RenderHUD()
        {
            _hud.DrawScore(_player.Score);
            if (_isDrawBitingCondition)
            {
                if (_timeBitingConditionDuration > _timeBitingCondition)
                {
                    _hud.DrawBitingCondition(FishingStatus.Pecks);
                    _timeBitingCondition += _timeHelper.DeltaT;
                }
                else
                {
                    _isDrawBitingCondition = false;
                    _timeBitingCondition = 0;
                }
            }

            if (_isDrawCenterMessage)
            {
                if (_timeCenterMessageDuration > _timeCenterMessage)
                {
                    _hud.DrawCenterMessage(new Fish("Fish", _directX3DGraphics, _renderer, Vector4.Zero, null));
                    _timeCenterMessage += _timeHelper.DeltaT;
                }
                else
                {
                    _isDrawCenterMessage = false;
                    _timeCenterMessage = 0;
                }
            }

            if (_isDrawNotebook)
            {
               _hud.DrawNotebook();
            }
        }

        public Texture LoadTextureFromFile(string fileName, SamplerState samplerState)
        {
            BitmapDecoder decoder = new BitmapDecoder(_imagingFactory, fileName, DecodeOptions.CacheOnDemand);
            BitmapFrameDecode bitmapFirstFrame = decoder.GetFrame(0);

            Utilities.Dispose(ref decoder);

            FormatConverter imageFormatConverter = new FormatConverter(_imagingFactory);
            imageFormatConverter.Initialize(bitmapFirstFrame, PixelFormat.Format32bppRGBA, BitmapDitherType.None, null, 0.0, BitmapPaletteType.Custom);
            int stride = imageFormatConverter.Size.Width * 4;
            DataStream buffer = new DataStream(imageFormatConverter.Size.Height * stride, true, true);
            imageFormatConverter.CopyPixels(stride, buffer);

            int width = imageFormatConverter.Size.Width;
            int height = imageFormatConverter.Size.Height;

            Texture2DDescription textureDescription = new Texture2DDescription()
            {
                Width = width,
                Height = height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.R8G8B8A8_UNorm,
                SampleDescription = _directX3DGraphics.SampleDescription,
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };
            Texture2D textureObject = new Texture2D(_directX3DGraphics.Device, textureDescription, new DataRectangle(buffer.DataPointer, stride));

            ShaderResourceViewDescription shaderResourceViewDescription = new ShaderResourceViewDescription()
            {
                Dimension = ShaderResourceViewDimension.Texture2D,
                Format = Format.R8G8B8A8_UNorm,
                Texture2D = new ShaderResourceViewDescription.Texture2DResource
                {
                    MostDetailedMip = 0,
                    MipLevels = -1
                }
            };
            ShaderResourceView shaderResourceView = new ShaderResourceView(_directX3DGraphics.Device, textureObject, shaderResourceViewDescription);

            Utilities.Dispose(ref imageFormatConverter);

            return new Texture(textureObject, shaderResourceView, width, height, samplerState);
        }

        public void Dispose()
        {
            _input.Dispose();
            _renderForm.Dispose();
            _directX2DGraphics.Dispose();
            _directX3DGraphics.Dispose();
        }
    }
}
