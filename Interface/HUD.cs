using Fishing_SharpDX.Enums;
using Fishing_SharpDX.Objects;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fishing_SharpDX.Interface
{
    public class HUD
    {
        private DirectX2DGraphics _directX2DGraphics;

        private float _height;
        private float _width;

        private int _textCenter;
        private int _textLeft;
        private int _textRight;

        private int _brushWhite;
        private int _brushGreen;
        private int _brushYellow;
        private int _brushRed;

        private int _brushBlackAlpha;

        public HUD(DirectX2DGraphics directX2DGraphics) 
        {
            _directX2DGraphics = directX2DGraphics;
            _textCenter = _directX2DGraphics.NewTextFormat("Arial", SharpDX.DirectWrite.FontWeight.Normal,
                         SharpDX.DirectWrite.FontStyle.Normal, SharpDX.DirectWrite.FontStretch.Normal, 24f,
                         SharpDX.DirectWrite.TextAlignment.Center, SharpDX.DirectWrite.ParagraphAlignment.Center);
            _textLeft = _directX2DGraphics.NewTextFormat("Arial", SharpDX.DirectWrite.FontWeight.Normal,
                         SharpDX.DirectWrite.FontStyle.Normal, SharpDX.DirectWrite.FontStretch.Normal, 24f,
                         SharpDX.DirectWrite.TextAlignment.Justified, SharpDX.DirectWrite.ParagraphAlignment.Center);
            _textRight = _directX2DGraphics.NewTextFormat("Arial", SharpDX.DirectWrite.FontWeight.Normal,
                         SharpDX.DirectWrite.FontStyle.Normal, SharpDX.DirectWrite.FontStretch.Normal, 24f,
                         SharpDX.DirectWrite.TextAlignment.Leading, SharpDX.DirectWrite.ParagraphAlignment.Center);

            _brushWhite = _directX2DGraphics.NewSolidColorBrush(new RawColor4(1.0f, 1.0f, 1.0f, 1.0f));
            _brushGreen = _directX2DGraphics.NewSolidColorBrush(new RawColor4(0.0f, 1.0f, 0.0f, 1.0f));
            _brushYellow = _directX2DGraphics.NewSolidColorBrush(new RawColor4(1.0f, 1.0f, 0.0f, 1.0f));
            _brushRed = _directX2DGraphics.NewSolidColorBrush(new RawColor4(1.0f, 0.0f, 0.0f, 1.0f));

            _brushBlackAlpha = _directX2DGraphics.NewSolidColorBrush(new RawColor4(0.0f, 0.0f, 0.0f, 1.0f));
        }

        public void DrawScore(int score)
        {
            RawRectangleF rect = new RawRectangleF(20, 0, _width, 50);

            _directX2DGraphics.BeginDraw();
            //_directX2DGraphics.DrawRectangle(rect, _brushRed);
            _directX2DGraphics.DrawText($"Score: {score}", _textLeft, rect, _brushWhite);
            _directX2DGraphics.EndDraw();
        }

        public void DrawBitingCondition(FishingStatus status)
        {
            RawRectangleF rect = new RawRectangleF(_width - 200, _height - 150, _width, _height);

            _directX2DGraphics.BeginDraw();
            //_directX2DGraphics.DrawRectangle(rect, _brushRed);
            switch (status)
            {
                case FishingStatus.Expectation:
                    _directX2DGraphics.DrawText($"Ожидание...", _textRight, rect, _brushWhite);
                    break;
                case FishingStatus.Pecks:
                    _directX2DGraphics.DrawText($"Клюет!", _textRight, rect, _brushGreen);
                    break;
                case FishingStatus.GotOff:
                    _directX2DGraphics.DrawText($"Сорвалась!", _textRight, rect, _brushRed);
                    break;
            }
            _directX2DGraphics.EndDraw();
        }

        public void DrawCenterMessage(Fish fish)
        {
            RawRectangleF rect = new RawRectangleF(_width / 2 - 500, _height / 2 - 200, _width / 2 + 500, _height / 2 + 200);

            _directX2DGraphics.BeginDraw();
            //_directX2DGraphics.DrawRectangle(rect, _brushRed);
            _directX2DGraphics.DrawText(fish.ToString(), _textCenter, rect, _brushGreen);
            _directX2DGraphics.EndDraw();
        }

        public void DrawNotebook()
        {
            RawRectangleF rect = new RawRectangleF(_width / 2 - 800, 50, _width / 2 + 800, _height - 50);
            _directX2DGraphics.BeginDraw();
            _directX2DGraphics.DrawRectangle(rect, _brushBlackAlpha);
            _directX2DGraphics.EndDraw();
        }

        public void Resize(float height, float width)
        {
            _height = height;
            _width = width;
        }
    }
}
