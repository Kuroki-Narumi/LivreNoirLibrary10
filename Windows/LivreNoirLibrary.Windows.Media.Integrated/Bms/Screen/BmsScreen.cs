using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.Media.Bms.ViewModels;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls.Bms.Elements;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BmsScreen : FrameworkElement, IVariableProvider, IBmsScreen
    {
        public const int DefaultWidth = 1920;
        public const int DefaultHeight = 1080;

        [DependencyProperty]
        private Skin? _skin;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _hasSkinOptions;
        [DependencyProperty]
        private string? _bmsPath;
        [DependencyProperty(SetterScope = Scope.Private)]
        private string? _bmsDirectory;
        [DependencyProperty(SetterScope = Scope.Private)]
        private string? _bmsBaseName;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _isBmsReady;
        [DependencyProperty]
        private bool _showDebugText;
        [DependencyProperty(SetterScope = Scope.Private)]
        private string? _debugText;
        [DependencyProperty]
        private double _fadeOpacity;
        [DependencyProperty]
        private ISkinOptionProvider? _skinOptionProvider;

        public SimpleBmsViewModel ViewModel { get; } = new();
        public BmsPlayOptions PlayOptions { get; }
        public IBmsTimer Timer => _timer;
        public ScoreManager ScoreManager { get; }
        public ISkinRoot? SkinRoot => _skinRoot;
        public IDictionary<string, string>? SkinOptions { get; private set; }
        public Dictionary<string, string> BmsOptions { get; } = [];
        public Dictionary<string, string> Variables { get; } = [];
        public AudioComposer<string> AudioComposer { get; }
        public double FirstSoundTime => _timingList.FirstSoundTime;
        public double LastSoundTime => _timingList.LastSoundTime;

        private WriteableBitmap _bitmap;
        private Rect _bitmapRect;
        private bool _needEnsureBitmap;
        private bool _needRender;
        private readonly BmsTimer _timer = new();
        private SkinRoot _skinRoot = Elements.SkinRoot.Default;
        private readonly FloatBitmap _buffer = new(0, 0);

        private readonly List<ScreenElement> _children = [];
        private readonly CachedWaveBufferProvider _waveProvider = new();
        private readonly TimingList _timingList = new();
        private readonly TextureCache _textureCache = new();
        private readonly MediaCache _mediaCache = new();
        private readonly NoteElementCollection _notes = new();
        private readonly BgaSource _bga;

        private DebugItem _debugRoot;
        private readonly Dictionary<object, DebugItem> _debugDic = [];

        public BmsScreen(BmsPlayOptions options)
        {
            PlayOptions = options;
            _bitmap = CreateBitmap(DefaultWidth, DefaultHeight);
            _needEnsureBitmap = true;
            ClipToBounds = true;
            ScoreManager = new(PlayOptions);
            _bga = new(PlayOptions);
            AudioComposer = new(_waveProvider, _timingList.BgmTimeline);
        }

        public bool TryGetOption(string key, [MaybeNullWhen(false)] out string value) => BmsOptions.TryGetValue(key, out value);
        public bool TryGetVariable(string key, [MaybeNullWhen(false)] out string value) => Variables.TryGetValue(key, out value);

        private void OnSkinChanged(Skin? value)
        {
            DebugText = null;
            _debugDic.Clear();
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
                _skinRoot = Elements.SkinRoot.Create(value);
                _debugRoot = new(value.Name ?? value.GetType().Name, 0, 0);
                ExConsole.Write($"Skin \"{value.Name}\" loaded");
            }
            else
            {
                _skinRoot = Elements.SkinRoot.Default;
                _debugRoot = default;
            }
            ApplySkinOptions(value, SkinOptionProvider);
            InvalidateVisual();
        }

        private void OnSkinOptionProviderChanged(ISkinOptionProvider? provider) => ApplySkinOptions(_skin, provider);

        void ApplySkinOptions(Skin? skin, ISkinOptionProvider? provider)
        {
            if (skin is not null && skin.Options.Count is > 0)
            {
                HasSkinOptions = true;
                if ((SkinOptions = provider?.GetSkinOptions(skin)) is { } dic)
                {
                    foreach (var option in skin!.Options)
                    {
                        if (dic.TryGetValue(option.Key, out var optionValue))
                        {
                            option.SetValue(optionValue);
                        }
                    }
                }
            }
            else
            {
                HasSkinOptions = false;
                SkinOptions = null;
            }
        }

        void AppendChild(List<ScreenElement> children, SkinElement element, int indent = 1)
        {
            ScreenElement? e = element switch
            {
                Group g => new GroupElement(g),
                Image i => new ImageElement(i),
                Number n => new NumberElement(n),
                Bga b => new BgaElement(b),
                NoteArea n => new NoteAreaElement(n),
                Judge j => new JudgeElement(j),
                Media.Bms.SkinInfo.Text t => new TextElement(t),
                _ => null,
            };
            if (e is not null)
            {
                _debugDic[e] = new(element.Name, indent, 0);
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

        public bool OpenBms(string path)
        {
            BmsPath = path;
            return IsBmsReady;
        }

        private void OnBmsPathChanged(string? value)
        {
            _mediaCache.Clear();
            if (File.Exists(value))
            {
                var directory = Path.GetDirectoryName(value) ?? "";
                if (BmsDirectory != directory)
                {
                    _waveProvider.Clear();
                    _timingList.Directory = directory;
                    BmsDirectory = directory;
                }
                BmsBaseName = Path.GetFileName(value);
                var vm = ViewModel;
                try
                {
                    vm.Data = BmsData.Open(value);
                    BmsOptions.SetBmsOptions(vm);
                    Variables.SetBmsVariables(vm);
                    _textureCache.SetBmsTexture(vm, directory);
                    IsBmsReady = true;
                    return;
                }
                catch (Exception ex)
                {
                    ExConsole.Write(ex);
                    vm.Data.Clear();
                }
            }
            else
            {
                BmsDirectory = "";
                BmsBaseName = "";
            }
            IsBmsReady = false;
        }

        private void OnShowDebugTextChanged(bool value)
        {
            if (!value)
            {
                DebugText = null;
            }
        }

        public void DetermineExpressions()
        {
            if (_skin is { } skin)
            {
                _skinRoot.DetermineExpressions(this);
                _notes.DetermineExpressions(skin, this);
                foreach (var element in _children.AsSpan())
                {
                    element.DetermineExpressions(skin, this);
                }
            }
        }

        public void SetupAudio()
        {
            var op = PlayOptions;
            ScoreManager.Clear();
            AudioComposer.SetVolume(op.MasterVolume, (BgmTimeline.Tag_KeySound, op.KeyVolume), (BgmTimeline.Tag_BgmSound, op.BgmVolume));
            if (_isBmsReady)
            {
                var timing = _timingList;
                timing.Load(ViewModel);
                Variables.SetPlayInfos(timing);
                _notes.Setup(timing);
                op.UpdateHsCorrection(timing);
                op.GaugeGainBase = ViewModel.Total / timing.NoteCount;
                ScoreManager.MaxNoteCount = _timingList.NoteCount;
            }
        }

        public void SetupPlay(bool isAutoPlay)
        {
            DetermineExpressions();
            Timer.PrepareToPlay();
            _bga.Clear();

            SetupAudio();

            _debugRoot = _debugRoot.Reset();
            var debug = _debugDic;
            var keys = debug.Keys.ToArray();
            foreach (var key in keys)
            {
                debug[key] = debug[key].Reset();
            }
            DebugText = null;
        }

        public void Update(double time)
        {
            if (_skin is { } skin)
            {
                var options = PlayOptions;
                var timer = _timer;
                var timings = _timingList;
                timer.SetBeatTimer(time, timings, ViewModel);
                UpdateArgs args = new(skin, this, options, timer, time, timings, _textureCache, _mediaCache, _notes, _bga, ScoreManager);
                _notes.Update(args);
                _bga.Update(args);
                Variables.UpdateCurrentInfos(args);

                foreach (var child in _children.AsSpan())
                {
                    child.Update(args);
                }
                _needRender = true;
                InvalidateVisual();
            }
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
                    if (FadeOpacity is not 0)
                    {
                        var color = new LnColor(ColorUtils.GetByte((float)FadeOpacity), 0, 0, 0);
                        p.Blend(BlendMode.Alpha, color);
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

        void ConstructDebugText()
        {
            if (_showDebugText)
            {
                using var o = ObjectPool.Rent<StringBuilder>();
                var sb = o.Value;
                var total = _debugRoot.Time;
                AppendLine(sb, _debugRoot, total);
                foreach (var (_, item) in _debugDic)
                {
                    if (!string.IsNullOrEmpty(item.Name) && item.Time is not 0)
                    {
                        sb.AppendLine();
                        AppendLine(sb, item, total);
                    }
                }
                DebugText = sb.ToString();
            }
        }

        void AppendLine(StringBuilder sb, in DebugItem item, double total)
        {
            sb.Append(' ', item.Indent * 2);
            sb.Append(item.Name);
            sb.Append($" - {item.Time / total:P2}({item.Time:F3}ms)");
        }
    }
}
