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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        private double _highSpeed = 1;
        [DependencyProperty]
        private FixedHighSpeedMode _fixedHighSpeedMode = FixedHighSpeedMode.MainTimeBpm;
        [DependencyProperty(SetterScope = Scope.Private)]
        private double _fixedHighSpeed = 1.0;
        [DependencyProperty(SetterScope = Scope.Private)]
        private string? _debugText;

        public string? Directory => Path.GetDirectoryName(_bmsPath);
        public SimpleBmsViewModel ViewModel { get; } = new();
        public Dictionary<string, string> Options { get; } = [];
        public Dictionary<string, string> Variables { get; } = [];

        private WriteableBitmap _bitmap;
        private Rect _bitmapRect;
        private bool _needEnsureBitmap;
        private bool _needRender;
        private readonly FloatBitmap _buffer = new(0, 0);

        private readonly List<ScreenElement> _children = [];
        private readonly BmsTimer _timer = new();
        private readonly TimingList _timingList = new();
        private readonly TextureCache _textureCache = new();
        private readonly MediaCache _mediaCache = new();
        private readonly NoteElementCollection _notes = new();
        private readonly BgaSource _bga = new();
        private readonly JudgeInfo _judge = new();

        private DebugItem _debugRoot;
        private readonly Dictionary<object, DebugItem> _debugDic = [];

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
            DebugText = null;
            _debugDic.Clear();
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
                _debugRoot = new(value.Name ?? value.GetType().Name, 0, 0);
                ExConsole.Write($"Skin \"{value.DisplayName}\" loaded");
            }
            else
            {
                _debugRoot = default;
            }
            InvalidateVisual();
        }

        void AppendChild(List<ScreenElement> children, SkinElement element, int indent = 1)
        {
            ScreenElement? e = element switch
            {
                Group g => new GroupElement(g),
                Image i => new ImageElement(i),
                Bga b => new BgaElement(b),
                NoteArea n => new NoteAreaElement(n),
                Judge j => new JudgeElement(j),
                Media.Bms.SkinInfo.Text t => new TextElement(t),
                _ => null,
            };
            if (e is not null)
            {
                _debugDic[e] = new(element.Name ?? element.GetType().Name, indent, 0);
                children.Add(e);
                if (e is GroupElement g)
                {
                    foreach (var gchild in g._source.Children.AsSpan())
                    {
                        if (gchild is SkinElement gelement)
                        {
                            AppendChild(g.Children, gelement, indent + 1);
                        }
                    }
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
                    Options.SetBmsOptions(vm);
                    Variables.SetBmsVariables(vm);
                    _textureCache.SetBmsTexture(vm, basePath);
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
            _judge.Clear();
            if (_isBmsReady)
            {
                var timing = _timingList;
                timing.Load(ViewModel, Directory!, autoPlay);
                _notes.Setup(timing);
                FixedHighSpeed = 240d / _fixedHighSpeedMode switch
                {
                    FixedHighSpeedMode.MinBpm => timing.MinTempo,
                    FixedHighSpeedMode.MaxBpm => timing.MaxTempo,
                    FixedHighSpeedMode.MainBpm => timing.MainTempo,
                    FixedHighSpeedMode.MainTimeBpm => timing.MainTimeTempo,
                    _ => 60,
                };
            }

            _debugRoot = _debugRoot.Reset();
            var debug = _debugDic;
            var keys = debug.Keys.ToArray();
            foreach (var key in keys)
            {
                debug[key] = debug[key].Reset();
            }
            DebugText = null;
        }

        public void StartLoading(double time) => _timer.Set(TimerId.Play_LoadingStart, time);
        public void FinishLoading(double time) => _timer.Set(TimerId.Play_LoadingFinished, time);
        public void StartMusic(double time) => _timer.Set(TimerId.Play_MusicStart, time);

        public void Update(double time)
        {
            UpdateArgs args = new(_timer, time, _timingList, _textureCache, _mediaCache, _notes, _bga, _judge, _highSpeed * _fixedHighSpeed);
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
                var t0 = System.Diagnostics.Stopwatch.GetTimestamp();
                using (var p = _bitmap.BeginWrite())
                {
                    p.Fill(_skin!.Background);
                    RenderArgs args = new(p, _buffer, _debugDic);
                    foreach (var child in _children.AsSpan())
                    {
                        child.Render(args);
                    }
                }
                var time = TimeUtils.Ticks2Milliseconds(System.Diagnostics.Stopwatch.GetTimestamp() - t0);
                _debugRoot += time;
                ConstructDebugText();
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

        readonly StringBuilder _debugBuilder = new();

        void ConstructDebugText()
        {
            var total = _debugRoot.Time;
            _debugBuilder.Clear();
            AppendLine(_debugRoot, total);
            foreach (var (_, item) in _debugDic)
            {
                if (item.Time is not 0)
                {
                    _debugBuilder.AppendLine();
                    AppendLine(item, total);
                }
            }
            DebugText = _debugBuilder.ToString();
        }

        void AppendLine(in DebugItem item, double total)
        {
            _debugBuilder.Append(' ', item.Indent * 2);
            _debugBuilder.Append(item.Name);
            _debugBuilder.Append($" - {item.Time / total:P2}({item.Time:F3}ms)");
        }
    }
}
