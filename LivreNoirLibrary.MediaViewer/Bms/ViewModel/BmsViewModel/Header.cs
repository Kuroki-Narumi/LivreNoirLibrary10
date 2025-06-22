using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public delegate void HeaderChangedEventHandler(BmsViewModel sender, string? newValue);

    public partial class BmsViewModel
    {
        public const string DefaultLnObjText = "(LN Ch)";

        public event HeaderChangedEventHandler? DifficultyChanged;
        public event HeaderChangedEventHandler? StageFileChanged;
        public event HeaderChangedEventHandler? BannerChanged;
        public event HeaderChangedEventHandler? BackBmpChanged;
        public event HeaderChangedEventHandler? PreviewChanged;

        [ObservableProperty]
        private PlayerType _player = Constants.DefaultPlayer;
        [ObservableProperty]
        private string? _genre;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultGenre;
        [ObservableProperty]
        private string? _title;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultTitle;
        [ObservableProperty]
        private string? _subTitle;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultSubTitle;
        [ObservableProperty]
        private string? _artist;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultArtist;
        [ObservableProperty]
        private string? _subArtist;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultSubArtist;
        [ObservableProperty]
        private string? _bpm;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultBpm;
        [ObservableProperty]
        private string? _playLevel;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultPlayLevel;
        [ObservableProperty]
        private string? _difficulty;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultDifficulty;
        [ObservableProperty]
        private Rank _rank = Constants.DefaultRank;
        [ObservableProperty]
        private string? _total;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultTotal;
        [ObservableProperty]
        private string? _stageFile;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultStageFile;
        [ObservableProperty]
        private string? _banner;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultBanner;
        [ObservableProperty]
        private string? _backBmp;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultBackBmp;
        [ObservableProperty]
        private string? _preview;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultPreview;
        [ObservableProperty]
        private string? _lnObj;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultLnObj;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultDefaultLnObj;
        [ObservableProperty]
        private LongNoteMode _lnMode = Constants.DefaultLnMode;
        [ObservableProperty]
        private string? _exRank;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultExRank = Constants.DefaultExRank.ToString();
        [ObservableProperty]
        private string? _comment;
        [ObservableProperty(SetterScope = Scope.Private)]
        private string? _defaultComment;
        [ObservableProperty(SetterScope = Scope.Private)]
        private int _base = Constants.Base_Default;
        [ObservableProperty(SetterScope = Scope.Private)]
        private int _maxDefIndex = Constants.DefMax_Default;
        private readonly ObservableList<HeaderItem> _subHeaders = [];
        private bool _headerUpdating;

        public IList<HeaderItem> SubHeaders => _subHeaders;

        private void RefreshHeaders(BaseData source)
        {
            _headerUpdating = true;
            Player = source.Player;
            (Genre, DefaultGenre) = GetString(source, HeaderType.Genre);
            (Title, DefaultTitle) = GetString(source, HeaderType.Title);
            (SubTitle, DefaultSubTitle) = GetString(source, HeaderType.SubTitle);
            (Artist, DefaultArtist) = GetString(source, HeaderType.Artist);
            (SubArtist, DefaultSubArtist) = GetString(source, HeaderType.SubArtist);
            (Bpm, DefaultBpm) = GetString(source, HeaderType.Bpm, Constants.DefaultBpm);
            (PlayLevel, DefaultPlayLevel) = GetString(source, HeaderType.PlayLevel, Constants.DefaultLevel);
            (Difficulty, DefaultDifficulty) = GetString(source, HeaderType.Difficulty);
            Rank = source.Rank;
            (Total, DefaultTotal) = GetString(source, HeaderType.Total, 0d);
            (StageFile, DefaultStageFile) = GetString(source, HeaderType.StageFile);
            (Banner, DefaultBanner) = GetString(source, HeaderType.Banner);
            (BackBmp, DefaultBackBmp) = GetString(source, HeaderType.BackBmp);
            (Preview, DefaultPreview) = GetString(source, HeaderType.Preview);
            (LnObj, DefaultLnObj) = GetString(source, HeaderType.LnObj, DefaultLnObjText);
            LnMode = source.LnMode;
            (ExRank, DefaultExRank) = GetString(source, HeaderType.DefExRank, Constants.DefaultExRank);
            (Comment, DefaultComment) = GetString(source, HeaderType.Comment);
            Base = source.Base;
            MaxDefIndex = source.MaxDefIndex;
            var sub = _subHeaders;
            sub.ClearWithoutNotify();
            foreach (var header in CollectionsMarshal.AsSpan(source.Headers.SubHeaders))
            {
                sub.AddWithoutNotify(new(header));
            }
            sub.NotifyCollectionReset();
            _headerUpdating = false;
        }

        private void ProcessHeaderChange(Action action)
        {
            if (!_headerUpdating)
            {
                action();
                this.OnEdit(true);
            }
        }

        private static (string?, string?) GetString(BaseData source, HeaderType type, object? defaultValue = null)
        {
            var h = source.Headers;
            return (h.Get(type), h.GetParent(type) ?? (defaultValue?.ToString()));
        }

        private void OnPlayerChanged(PlayerType value) => ProcessHeaderChange(() => _currentData.Player = value);
        private void OnGenreChanged(string? value) => ProcessHeaderChange(() => _currentData.Genre = value);
        private void OnTitleChanged(string? value) => ProcessHeaderChange(() => _currentData.Title = value);
        private void OnSubTitleChanged(string? value) => ProcessHeaderChange(() => _currentData.SubTitle = value);
        private void OnArtistChanged(string? value) => ProcessHeaderChange(() => _currentData.Artist = value);
        private void OnSubArtistChanged(string? value) => ProcessHeaderChange(() => _currentData.SubArtist = value);
        private void OnBpmChanged(string? value) => ProcessHeaderChange(() => _currentData.Headers.Set(HeaderType.Bpm, value));
        private void OnPlayLevelChanged(string? value) => ProcessHeaderChange(() => _currentData.Headers.Set(HeaderType.PlayLevel, value));
        private void OnDifficultyChanged(string? value)
        {
            ProcessHeaderChange(() => _currentData.Headers.Set(HeaderType.Difficulty, value));
            DifficultyChanged?.Invoke(this, value);
        }
        private void OnRankChanged(Rank value) => ProcessHeaderChange(() => _currentData.Rank = value);
        private void OnTotalChanged(string? value) => ProcessHeaderChange(() => _currentData.Headers.Set(HeaderType.Total, value));
        private void OnStageFileChanged(string? value)
        {
            ProcessHeaderChange(() => _currentData.Headers.Set(HeaderType.StageFile, value));
            StageFileChanged?.Invoke(this, value);
        }
        private void OnBannerChanged(string? value)
        {
            ProcessHeaderChange(() => _currentData.Headers.Set(HeaderType.Banner, value));
            BannerChanged?.Invoke(this, value);
        }
        private void OnBackBmpChanged(string? value)
        {
            ProcessHeaderChange(() => _currentData.Headers.Set(HeaderType.BackBmp, value));
            BackBmpChanged?.Invoke(this, value);
        }
        private void OnPreviewChanged(string? value)
        {
            ProcessHeaderChange(() => _currentData.Headers.Set(HeaderType.Preview, value));
            PreviewChanged?.Invoke(this, value);
        }

        public void InsertHeader(int index, string key, string value)
        {
            ProcessHeaderChange(() =>
            {
                Header h = new(key, value);
                HeaderItem item = new(h);
                _currentData.Headers.SubHeaders.Insert(index, h);
                _subHeaders.Insert(index, item);
            });
        }

        public void RemoveHeader(int index)
        {
            ProcessHeaderChange(() =>
            {
                _currentData.Headers.SubHeaders.RemoveAt(index);
                _subHeaders.RemoveAt(index);
            });
        }

        public void ReplaceHeader(int index, string key, string value)
        {
            ProcessHeaderChange(() =>
            {
                var item = _subHeaders[index];
                item.Key = key;
                item.Value = value;
            });
        }

        public void MoveDownHeader(int index)
        {
            ProcessHeaderChange(() =>
            {
                _currentData.Headers.SubHeaders.MoveDown(index);
                _subHeaders.MoveDown(index);
            });
        }

        public void MoveUpHeader(int index)
        {
            ProcessHeaderChange(() =>
            {
                _currentData.Headers.SubHeaders.MoveUp(index);
                _subHeaders.MoveUp(index);
            });
        }
    }
}
