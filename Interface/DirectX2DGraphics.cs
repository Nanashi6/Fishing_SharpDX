using Fishing_SharpDX.Graphics;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using SharpDX.WIC;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishing_SharpDX.Interface
{
    public class DirectX2DGraphics : IDisposable
    {
        private DirectX3DGraphics _directX3DGraphics;

        private Factory _factory;
        public Factory Factory { get => _factory; }

        private SharpDX.DirectWrite.Factory _writeFactory;
        public SharpDX.DirectWrite.Factory WriteFactory { get => _writeFactory; }


        private ImagingFactory _imagingFactory;
        public ImagingFactory ImagingFactory { get => _imagingFactory; }


        private RenderTargetProperties _renderTargetProperties;

        private RenderTarget _renderTarget;
        public RenderTarget RenderTarget { get => _renderTarget; }


        private RawRectangleF _renderTargetClientRectangle;
        public RawRectangleF RenderTargetClientRectangle { get => _renderTargetClientRectangle; }

        private List<SharpDX.DirectWrite.TextFormat> _textFormats;
        public List<SharpDX.DirectWrite.TextFormat> TextFormats { get => _textFormats; }

        private List<RawColor4> _solidColorBrushesColors;
        private List<SolidColorBrush> _solidColorBrushes;
        public List<SolidColorBrush> SolidColorBrushes { get => _solidColorBrushes; }

        private List<BitmapFrameDecode> _decodedFirstFrameOfBitmaps;
        private List<SharpDX.Direct2D1.Bitmap> _bitmaps;
        public List<SharpDX.Direct2D1.Bitmap> Bitmaps { get => _bitmaps; }


        public DirectX2DGraphics(DirectX3DGraphics directX3DGraphics)
        {
            _directX3DGraphics = directX3DGraphics;

            _directX3DGraphics.Resizing += BeforeResizeSwapChain;
            _directX3DGraphics.Resized += AfterResizeSwapChain;

            _factory = new Factory();
            _writeFactory = new SharpDX.DirectWrite.Factory();
            _imagingFactory = new ImagingFactory();

            _renderTargetProperties.DpiX = 0;
            _renderTargetProperties.DpiY = 0;
            _renderTargetProperties.MinLevel = FeatureLevel.Level_10;
            _renderTargetProperties.PixelFormat = new SharpDX.Direct2D1.PixelFormat(
                SharpDX.DXGI.Format.Unknown,
                AlphaMode.Premultiplied);
            _renderTargetProperties.Type = RenderTargetType.Hardware;
            _renderTargetProperties.Usage = RenderTargetUsage.None;

            _textFormats = new List<SharpDX.DirectWrite.TextFormat>(4);
            _solidColorBrushesColors = new List<RawColor4>(4);
            _solidColorBrushes = new List<SolidColorBrush>(4);
            _decodedFirstFrameOfBitmaps = new List<BitmapFrameDecode>(4);
            _bitmaps = new List<SharpDX.Direct2D1.Bitmap>(4);
        }

        public void BeforeResizeSwapChain(object sender, EventArgs e)
        {
            DisposeBitmaps();
            DisposeSolidColorBrushes();
            Utilities.Dispose(ref _renderTarget);
        }

        private void CreateBitmap(BitmapFrameDecode decodedFirstFrameOfBitmap)
        {
            FormatConverter imageFormatConverter = new FormatConverter(_imagingFactory);
            imageFormatConverter.Initialize(
                decodedFirstFrameOfBitmap,
                SharpDX.WIC.PixelFormat.Format32bppPRGBA,
                BitmapDitherType.Ordered4x4, null, 0.0, BitmapPaletteType.Custom);
            SharpDX.Direct2D1.Bitmap bitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(_renderTarget, imageFormatConverter);
            _bitmaps.Add(bitmap);
            Utilities.Dispose(ref imageFormatConverter);
        }

        public void AfterResizeSwapChain(object sender, EventArgs e)
        {
            SharpDX.DXGI.Surface surface = _directX3DGraphics.BackBuffer.QueryInterface<SharpDX.DXGI.Surface>();
            _renderTarget = new RenderTarget(_factory, surface, _renderTargetProperties);
            Utilities.Dispose(ref surface);
            _renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            _renderTarget.TextAntialiasMode = TextAntialiasMode.Cleartype;

            _renderTargetClientRectangle.Left = 0;
            _renderTargetClientRectangle.Top = 0;
            _renderTargetClientRectangle.Right = _renderTarget.Size.Width;
            _renderTargetClientRectangle.Bottom = _renderTarget.Size.Height;

            if (_solidColorBrushesColors.Count > 0)
                for (int i = 0; i <= _solidColorBrushesColors.Count - 1; ++i)
                    _solidColorBrushes.Add(new SolidColorBrush(_renderTarget, _solidColorBrushesColors[i]));

            if (_decodedFirstFrameOfBitmaps.Count > 0)
                for (int i = 0; i <= _decodedFirstFrameOfBitmaps.Count - 1; ++i)
                    CreateBitmap(_decodedFirstFrameOfBitmaps[i]);
        }

        public int NewTextFormat(string fontFamilyName, SharpDX.DirectWrite.FontWeight fontWeight,
            SharpDX.DirectWrite.FontStyle fontStyle, SharpDX.DirectWrite.FontStretch fontStretch, float fontSize,
            SharpDX.DirectWrite.TextAlignment textAlignment, SharpDX.DirectWrite.ParagraphAlignment paragraphAlignment)
        {
            SharpDX.DirectWrite.TextFormat textFormat = new SharpDX.DirectWrite.TextFormat(_writeFactory, fontFamilyName, fontWeight,
                fontStyle, fontStretch, fontSize);
            textFormat.TextAlignment = textAlignment;
            textFormat.ParagraphAlignment = paragraphAlignment;
            _textFormats.Add(textFormat);
            return _textFormats.Count - 1;
        }

        public int NewSolidColorBrush(RawColor4 color)
        {
            _solidColorBrushesColors.Add(color);
            int index = _solidColorBrushesColors.Count - 1;
            if (null != _renderTarget)
                _solidColorBrushes.Add(new SolidColorBrush(_renderTarget, color));
            return index;
        }

        public int LoadBitmapFromFile(string imageFileName)
        {
            BitmapDecoder decoder = new BitmapDecoder(_imagingFactory, imageFileName, DecodeOptions.CacheOnDemand);
            BitmapFrameDecode bitmapFirstFrame = decoder.GetFrame(0);
            _decodedFirstFrameOfBitmaps.Add(bitmapFirstFrame);
            int index = _decodedFirstFrameOfBitmaps.Count - 1;
            if (null != _renderTarget)
                CreateBitmap(bitmapFirstFrame);

            Utilities.Dispose(ref decoder);

            return index;
        }

        public void BeginDraw()
        {
            _renderTarget.BeginDraw();
        }

        public void DrawText(string text, int textFormatIndex, RawRectangleF layoutRectangle, int brushIndex)
        {
            _renderTarget.Transform = Matrix3x2.Identity;
            _renderTarget.DrawText(text, _textFormats[textFormatIndex], layoutRectangle, _solidColorBrushes[brushIndex]);
        }

        public void DrawBitmap(int bitmapIndex, Matrix3x2 transformMatrix, float opacity,
            SharpDX.Direct2D1.BitmapInterpolationMode interpolationMode)
        {
            _renderTarget.Transform = transformMatrix;
            _renderTarget.DrawBitmap(_bitmaps[bitmapIndex], opacity, interpolationMode);
        }

        public void EndDraw()
        {
            _renderTarget.EndDraw();
        }

        public void DisposeSolidColorBrushes()
        {
            for (int i = _solidColorBrushes.Count - 1; i >= 0; i--)
            {
                SolidColorBrush brush = _solidColorBrushes[i];
                _solidColorBrushes.RemoveAt(i);
                Utilities.Dispose(ref brush);
            }
        }

        public void DisposeBitmaps()
        {
            for (int i = _bitmaps.Count - 1; i >= 0; i--)
            {
                SharpDX.Direct2D1.Bitmap bitmap = _bitmaps[i];
                _bitmaps.RemoveAt(i);
                Utilities.Dispose(ref bitmap);
            }
        }

        public void DrawEcllipse(Vector2 center, float radius, int brushIndex)
        {
            Ellipse ellipse = new Ellipse(center, radius, radius);
            _renderTarget.FillEllipse(ellipse, _solidColorBrushes[brushIndex]);
        }

        public void DrawRectangle(RawRectangleF rectangle, int brushIndex)
        {
            _renderTarget.FillRectangle(rectangle, _solidColorBrushes[brushIndex]);
        }

        public void Dispose()
        {
            DisposeBitmaps();
            for (int i = _decodedFirstFrameOfBitmaps.Count - 1; i >= 0; i--)
            {
                BitmapFrameDecode bitmapFirstFrame = _decodedFirstFrameOfBitmaps[i];
                _decodedFirstFrameOfBitmaps.RemoveAt(i);
                Utilities.Dispose(ref bitmapFirstFrame);
            }
            DisposeSolidColorBrushes();
            for (int i = _textFormats.Count - 1; i >= 0; i--)
            {
                SharpDX.DirectWrite.TextFormat textFormat = _textFormats[i];
                _textFormats.RemoveAt(i);
                Utilities.Dispose(ref textFormat);
            }
            Utilities.Dispose(ref _imagingFactory);
            Utilities.Dispose(ref _writeFactory);
            Utilities.Dispose(ref _factory);
        }
    }
}
