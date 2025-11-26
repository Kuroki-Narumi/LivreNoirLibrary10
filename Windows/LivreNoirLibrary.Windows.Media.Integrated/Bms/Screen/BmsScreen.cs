using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.ViewModels;
using LivreNoirLibrary.Windows.Controls.Bms.Elements;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BmsScreen : FrameworkElement, IVariableProvider
    {
        public const int DefaultWidth = 1920;
        public const int DefaultHeight = 1080;

        [DependencyProperty]
        private Skin? _skin;
        [DependencyProperty]
        private string? _bmsPath;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _isBmsReady;
        [DependencyProperty]
        private double _highSpeed = 2.5;
        [DependencyProperty]
        private FixedHighSpeedMode _fixedHighSpeedMode = FixedHighSpeedMode.MainTimeBpm;
        [DependencyProperty(SetterScope = Scope.Private)]
        private double _fixedHighSpeed = 1.0;

        public string? Directory => Path.GetDirectoryName(_bmsPath);
        public SimpleBmsViewModel ViewModel { get; } = new();
        public Dictionary<string, string> Options { get; } = [];
        public Dictionary<string, string> Variables { get; } = [];

        private WriteableBitmap _bitmap;
        private Rect _bitmapRect;
        private bool _needEnsureBitmap;
        private bool _needRender;
        private readonly FloatBitmap _buffer1 = new(0, 0);
        private readonly UnmanagedArray<float> _buffer2 = new();

        private readonly List<ScreenElement> _children = [];
        private readonly BmsTimer _timer = new();
        private readonly TimingList _timingList = new();
        private readonly TextureCache _textureCache = new();
        private readonly MediaCache _mediaCache = new();
        private readonly NoteElementCollection _notes = new();
        private readonly BgaParams _bga = new();

        public BmsScreen()
        {
            _bitmap = CreateBitmap(DefaultWidth, DefaultHeight);
            _needEnsureBitmap = true;
            ClipToBounds = true;
        }

        public bool TryGetOption(string key, [MaybeNullWhen(false)] out string value) => Options.TryGetValue(key, out value);
        public bool TryGetVariable(string key, [MaybeNullWhen(false)] out string value) => Variables.TryGetValue(key, out value);

        private void OnSkinChanged(Skin? value)
        {
            _textureCache.Clear();
            var children = _children;
            children.Clear();
            if (value is not null)
            {
                var (w, h) = value.BaseSize;
                Width = w;
                Height = h;
                _needEnsureBitmap = true;
                foreach (var child in value.Children)
                {
                    if (child is SkinElement element)
                    {
                        AppendChild(children, element);
                    }
                }
            }
            InvalidateVisual();
        }

        static void AppendChild(List<ScreenElement> children, SkinElement element)
        {
            ScreenElement? e = null;
            switch (element)
            {
                case Group g:
                    GroupElement group = new(g);
                    e = group;
                    foreach (var gchild in g.Children.AsSpan())
                    {
                        if (gchild is SkinElement gelement)
                        {
                            AppendChild(group.Children, gelement);
                        }
                    }
                    break;
                case Image i:
                    e = new ImageElement(i);
                    break;
                case Bga b:
                    e = new BgaElement(b);
                    break;
                case NoteArea n:
                    e = new NoteAreaElement(n);
                    break;
            }
            if (e is not null)
            {
                children.Add(e);
            }
        }

        public void DetermineExpressions()
        {
            if (_skin is { } skin)
            {
                foreach (var element in _children.AsSpan())
                {
                    element.DetermineExpressions(skin, this);
                }
            }
        }

        private void OnBmsPathChanged(string? value)
        {
            _mediaCache.Clear();
            if (File.Exists(value))
            {
                var vm = ViewModel;
                try
                {
                    vm.Data = BmsData.Open(value);
                    var basePath = Directory!;
                    var cache = _textureCache;
                    cache.Set(Texture.Key_StageFile, vm.StageFile, basePath);
                    cache.Set(Texture.Key_Banner, vm.Banner, basePath);
                    cache.Set(Texture.Key_BackBmp, vm.BackBmp, basePath);
                    cache.Set(Texture.Key_Bmp00, vm.GetDefValue(DefType.Bmp, 0), basePath);
                    IsBmsReady = true;
                    return;
                }
                catch (Exception ex)
                {
                    ExConsole.Write(ex);
                    vm.Data.Clear();
                }
            }
            IsBmsReady = false;
        }

        public void SetupPlay(bool autoPlay)
        {
            var timer = _timer;
            timer.Remove(TimerId.Play_LoadingStart);
            timer.Remove(TimerId.Play_LoadingFinished);
            timer.Remove(TimerId.Play_MusicStart);
            timer.Remove(TimerId.Play_Miss);
            timer.Remove(TimerId.Play_FullCombo);
            timer.Set(TimerId.Scene_Start, 0);
            _bga.Setup();
            if (_isBmsReady)
            {
                var timing = _timingList;
                timing.Load(ViewModel, Directory!, autoPlay);
                _notes.Setup(timing);
                FixedHighSpeed = _fixedHighSpeedMode switch
                {
                    FixedHighSpeedMode.MinBpm => 60 / timing.MinTempo,
                    FixedHighSpeedMode.MaxBpm => 60 / timing.MaxTempo,
                    FixedHighSpeedMode.MainBpm => 60 / timing.MainTempo,
                    FixedHighSpeedMode.MainTimeBpm => 60 / timing.MainTimeTempo,
                    _ => 1,
                };
            }
        }

        public void StartLoading(double time) => _timer.Set(TimerId.Play_LoadingStart, time);
        public void FinishLoading(double time) => _timer.Set(TimerId.Play_LoadingFinished, time);
        public void StartMusic(double time) => _timer.Set(TimerId.Play_MusicStart, time);

        public void Update(double time)
        {
            UpdateArgs args = new(_timer, time, _timingList, _textureCache, _mediaCache, _notes, _bga, _highSpeed * _fixedHighSpeed);
            _notes.Update(args);
            _bga.Update(args);
            
            foreach (var child in _children.AsSpan())
            {
                child.Update(args);
            }
            _needRender = true;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            EnsureRender();
            base.OnRender(drawingContext);
            drawingContext.DrawImage(_bitmap, _bitmapRect);
        }

        private WriteableBitmap CreateBitmap(int width, int height)
        {
            if (_bitmap is null || _bitmap.PixelWidth < width || _bitmap.PixelHeight < height)
            {
                _bitmap = Bitmap.Create(width, height);
                _bitmapRect = new(0, 0, width, height);
            }
            return _bitmap;
        }

        private void EnsureRender()
        {
            if (_skin is not { } skin)
            {
                return;
            }
            var (w, h) = skin.BaseSize;
            // 描画範囲の検証
            if (_needEnsureBitmap)
            {
                CreateBitmap(w, h);
                _needEnsureBitmap = false;
                _needRender = true;
            }
            // 描画
            if (_needRender)
            {
                using (var p = _bitmap.BeginWrite())
                {
                    p.Fill(_skin!.Background);
                    var buffer1 = _buffer1;
                    var buffer2 = _buffer2;
                    foreach (var child in _children.AsSpan())
                    {
                        child.Render(p, buffer1, buffer2);
                    }
                }
                _needRender = false;
            }
        }

        public unsafe void CopyPixels(Span<byte> destination, int destWidth)
        {
            EnsureRender();
            var (w, h) = _skin!.BaseSize;
            fixed (byte* buffer = destination)
            {
                _bitmap.CopyPixels(new(0, 0, w, h), (nint)buffer, destination.Length, destWidth * 4);
            }
        }
    }
}
