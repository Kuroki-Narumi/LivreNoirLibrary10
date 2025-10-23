using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using LivreNoirLibrary.Windows.Controls.Bms;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public class BmsScreen : ObservableObjectBase, SkinInfo.IVariableProvider
    {
        public SkinInfo.Skin? Skin { get; private set => SetValue(ref field, value); }
        public string? BmsPath { get; private set => SetValue(ref field, value); }
        public BmsData? BmsData { get; private set => SetValue(ref field, value); }
        public string? Directory => Path.GetDirectoryName(BmsPath);

        public double HighSpeed { get; set => SetValue(ref field, value); } = 1;
        public Dictionary<string, string> Options { get; } = [];
        public Dictionary<string, string> Variables { get; } = [];

        public Canvas MainCanvas { get; } = new() { ClipToBounds = true };
        public BgaImageSource BgaImageSource => _bgaSource;

        private readonly BmsTextureCache _textureCache = new();
        private readonly BgaImageSource _bgaSource = new();
        private readonly List<GroupElement> _groupElements = [];
        private readonly List<ImageElement> _imageElements = [];
        private readonly List<BgaElement> _bgaElements = [];
        private readonly List<NoteAreaElement> _noteElements = [];

        private readonly BmsTimer _timer = new();
        private readonly NoteElementCollection _notes = new();
        private readonly TimingList _timingList = new();

        public void LoadSkin(SkinInfo.Skin? skin)
        {
            Skin = skin;
            _textureCache.Clear();
            var main = MainCanvas;
            main.Children.Clear();
            var groups = _groupElements;
            var images = _imageElements;
            var bgas = _bgaElements;
            var notes = _noteElements;
            groups.Clear();
            images.Clear();
            bgas.Clear();
            notes.Clear();
            if (skin is not null)
            {
                var (w, h) = skin.BaseSize;
                main.Width = w;
                main.Height = h;
                main.Background = MediaUtils.GetBrush(skin.Background.ToColor());
                void AppendChild(Canvas canvas, SkinInfo.SkinElement element, int depth = 0)
                {
                    ScreenElementViewModel e = new();
                    UIElement? c = null;
                    switch (element)
                    {
                        case SkinInfo.Group g:
                            GroupElement group = new(g);
                            groups.Add(group);
                            c = group;
                            foreach (var cc in g.Children)
                            {
                                if (cc is SkinInfo.SkinElement ee)
                                {
                                    AppendChild(group, ee, depth + 1);
                                }
                            }
                            break;
                        case SkinInfo.Image i:
                            ImageElement image = new(i);
                            images.Add(image);
                            c = image;
                            break;
                        case SkinInfo.Bga b:
                            BgaElement bga = new(b, _bgaSource);
                            bgas.Add(bga);
                            c = bga;
                            break;
                        case SkinInfo.NoteArea n:
                            NoteAreaElement note = new(n);
                            notes.Add(note);
                            c = note;
                            break;
                        default:
                            break;
                    }
                    if (c is not null)
                    {
                        canvas.Children.Add(c);
                    }
                }
                foreach (var child in skin.Children)
                {
                    if (child is SkinInfo.SkinElement e)
                    {
                        AppendChild(main, e);
                    }
                }
            }
        }

        public void DetermineExpressions()
        {
            if (Skin is { } skin)
            {
                foreach (var element in CollectionsMarshal.AsSpan(_groupElements))
                {
                    element.LoadDestination(skin, this);
                }
                foreach (var element in CollectionsMarshal.AsSpan(_imageElements))
                {
                    element.LoadDestination(skin, this);
                }
                foreach (var element in CollectionsMarshal.AsSpan(_bgaElements))
                {
                    element.LoadDestination(skin, this);
                }
                foreach (var element in CollectionsMarshal.AsSpan(_noteElements))
                {
                    element.LoadDestination(skin, this);
                }
            }
        }

        public bool OpenBms(string path)
        {
            try
            {
                var data = BmsData.Open(path);
                BmsPath = path;
                BmsData = data;
                _bgaSource.Clear();
                _textureCache.LoadBms(data, Directory!);
                return true;
            }
            catch (Exception ex)
            {
                ExConsole.Write(ex);
                BmsData = null;
                return false;
            }
        }
        
        public void SetupPlay(bool autoPlay)
        {
            _timer.Remove(TimerId.Play_LoadingStart);
            _timer.Remove(TimerId.Play_LoadingFinished);
            _timer.Remove(TimerId.Play_MusicStart);
            _timer.Remove(TimerId.Play_Miss);
            _timer.Remove(TimerId.Play_FullCombo);
            _timer.Set(TimerId.Scene_Start, 0);
            if (BmsData is { } data)
            {
                var timing = _timingList;
                timing.Load(data, Directory!, autoPlay);
                _notes.Setup(timing);
            }
        }

        public void Update(long absoluteTick)
        {
            var timer = _timer;
            foreach (var element in CollectionsMarshal.AsSpan(_groupElements))
            {
                element.Update(timer, absoluteTick);
            }
            var tc = _textureCache;
            foreach (var element in CollectionsMarshal.AsSpan(_imageElements))
            {
                element.Update(timer, absoluteTick, tc);
            }
            if (Skin is SkinInfo.PlaySkin)
            {
                _bgaSource.Update(_timingList, timer, absoluteTick);
                foreach (var element in CollectionsMarshal.AsSpan(_bgaElements))
                {
                    element.Update(timer, absoluteTick);
                }
                var hs = HighSpeed;
                _notes.Update(_timingList, timer, absoluteTick, hs);
                foreach (var element in CollectionsMarshal.AsSpan(_noteElements))
                {
                    element.Update(_notes, timer, absoluteTick, tc, hs);
                }
            }
        }

        public bool TryGetOption(string key, [MaybeNullWhen(false)] out string value) => Options.TryGetValue(key, out value);
        public bool TryGetVariable(string key, [MaybeNullWhen(false)] out string value) => Variables.TryGetValue(key, out value);

        public void StartLoading(long tick) => _timer.Set(TimerId.Play_LoadingStart, tick);
        public void FinishLoading(long tick) => _timer.Set(TimerId.Play_LoadingFinished, tick);
        public void StartMusic(long tick) => _timer.Set(TimerId.Play_MusicStart, tick);
    }
}
