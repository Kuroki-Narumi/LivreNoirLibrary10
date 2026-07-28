using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Text.Convert;
using LivreNoirLibrary.Windows.Converters;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.YuGiOh.Converters;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class DeckRecipeView : Control
    {
        public const double DashLength1 = 3;
        public const double DashLength2 = 1;

        static DeckRecipeView()
        {
            PropertyUtils.OverrideDefaultStyleKey<DeckRecipeView>();
        }

        [DependencyProperty]
        private Deck? _deck;
        [DependencyProperty]
        private IDictionary<int, int>? _lineBreaks;
        [DependencyProperty]
        private GridLength _headerHeight;
        [DependencyProperty]
        private double _contentHeight;
        [DependencyProperty]
        private GridLength _nameWidth1;
        [DependencyProperty]
        private GridLength _nameWidth2;
        [DependencyProperty]
        private GridLength _nameWidth3;
        [DependencyProperty]
        private GridLength _nameWidth4;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _isExtraDeckVisible;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _isSideDeckVisible;
        [DependencyProperty]
        private Brush? _selectedBackground = Brushes.White;
        [DependencyProperty]
        private Color _mainBorderColor = Colors.Black;
        [DependencyProperty]
        private Color _subBorderColor = Colors.White;
        [DependencyProperty(SetterScope = Scope.Private)]
        private SolidColorBrush? _mainBorderBrush;
        [DependencyProperty(SetterScope = Scope.Private)]
        private SolidColorBrush? _subBorderBrush;
        [DependencyProperty(SetterScope = Scope.Private)]
        private DrawingBrush? _horizontalDashBrush;
        [DependencyProperty(SetterScope = Scope.Private)]
        private DrawingBrush? _verticalDashBrush;
        [DependencyProperty]
        private FontWeight? _numberFontWeight;

        private bool _needRefresh;
        private StackPanel? _mainDeck1;
        private StackPanel? _mainDeck2;
        private StackPanel? _extraDeck;
        private StackPanel? _sideDeck;
        private bool _textChanging;

        public DeckRecipeView()
        {
            SolidColorBrush main = new();
            SetBinding(MainBorderColorProperty, new Binding(nameof(SolidColorBrush.Color)) { Mode = BindingMode.OneWayToSource, Source = main });
            MainBorderBrush = main;

            SolidColorBrush sub = new();
            SetBinding(SubBorderColorProperty, new Binding(nameof(SolidColorBrush.Color)) { Mode = BindingMode.OneWayToSource, Source = sub });
            SubBorderBrush = sub;

            HorizontalDashBrush = CreateHorizontalDashBrush(main, sub);
            VerticalDashBrush = CreateVerticalDashBrush(main, sub);
        }

        private static DrawingBrush CreateHorizontalDashBrush(SolidColorBrush main, SolidColorBrush sub)
        {
            DrawingGroup group = new();
            RenderOptions.SetEdgeMode(group, EdgeMode.Aliased);

            group.Children.Add(new GeometryDrawing()
            {
                Brush = main,
                Geometry = MediaUtils.CreateRectGeometry(new(0, 0, DashLength1, 1)),
            });
            group.Children.Add(new GeometryDrawing()
            {
                Brush = sub,
                Geometry = MediaUtils.CreateRectGeometry(new(DashLength1, 0, DashLength2, 1)),
            });

            return new DrawingBrush(group)
            {
                Viewport = new(0, 0, DashLength1 + DashLength2, 1),
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
            };
        }

        private static DrawingBrush CreateVerticalDashBrush(SolidColorBrush main, SolidColorBrush sub)
        {
            DrawingGroup group = new();
            RenderOptions.SetEdgeMode(group, EdgeMode.Aliased);

            group.Children.Add(new GeometryDrawing()
            {
                Brush = main,
                Geometry = MediaUtils.CreateRectGeometry(new(0, 0, 1, DashLength1)),
            });
            group.Children.Add(new GeometryDrawing()
            {
                Brush = sub,
                Geometry = MediaUtils.CreateRectGeometry(new(0, DashLength1, 1, DashLength2)),
            });

            return new DrawingBrush(group)
            {
                Viewport = new(0, 0, 1, DashLength1 + DashLength2),
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mainDeck1 = GetTemplateChild("MainDeck1") as StackPanel;
            _mainDeck2 = GetTemplateChild("MainDeck2") as StackPanel;
            _extraDeck = GetTemplateChild("ExtraDeck") as StackPanel;
            _sideDeck = GetTemplateChild("SideDeck") as StackPanel;
        }

        private void OnDeckChanged(Deck? oldValue, Deck? newValue)
        {
            oldValue?.MainDeck.CollectionChanged -= Deck_CollectionChanged;
            oldValue?.ExtraDeck.CollectionChanged -= Deck_CollectionChanged;
            oldValue?.SideDeck.CollectionChanged -= Deck_CollectionChanged;

            newValue?.MainDeck.CollectionChanged += Deck_CollectionChanged;
            newValue?.ExtraDeck.CollectionChanged += Deck_CollectionChanged;
            newValue?.SideDeck.CollectionChanged += Deck_CollectionChanged;

            ReserveRefresh();
        }

        private void Deck_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ReserveRefresh();
        }

        public void ReserveRefresh()
        {
            _needRefresh = true;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_needRefresh)
            {
                Refresh();
            }
            base.OnRender(drawingContext);
        }

        const string Format_CountFirst = "{0}{1}{2}";
        const string Format_NameFirst = "{2}{1}{0}";

        public string GetText(bool isNameFirst, bool withBracket, string separator)
        {
            if (Deck is not { } deck)
            {
                return "";
            }
            var format = isNameFirst ? Format_NameFirst : Format_CountFirst;
            using var o = ObjectPool.RentStringBuilder(out var sb);
            Append(Vocab.Current.Deck_Main, deck.MainDeck, sb, format, withBracket, separator);
            Append(Vocab.Current.Deck_Extra, deck.ExtraDeck, sb, format, withBracket, separator);
            Append(Vocab.Current.Deck_Side, deck.SideDeck, sb, format, withBracket, separator);
            return sb.ToString();

            static void Append(VocabData header, DeckCardList list, StringBuilder sb, string format, bool withBracket, string separator)
            {
                if (list.UniqueCount is 0)
                {
                    return;
                }
                sb.AppendLine(header.Value);
                foreach (var item in list.AsSpan())
                {
                    var c = item.ThisCard;
                    sb.AppendFormat(format, item.Count, separator, withBracket ? c.NameWithBracket() : c.Name);
                    sb.AppendLine();
                }
                sb.AppendLine();
            }
        }

        private void Refresh()
        {
            _needRefresh = false;
            _mainDeck1?.Children.Clear();
            _mainDeck2?.Children.Clear();
            _extraDeck?.Children.Clear();
            _sideDeck?.Children.Clear();
            if (Deck is not { } deck)
            {
                return;
            }
            _textChanging = true;

            var borderId = 0;
            if (_mainDeck1 is { } main1 && _mainDeck2 is { } main2)
            {
                var span = deck.MainDeck.AsSpan();
                var center = (span.Length + 1) / 2;
                var id = 0;
                for (var i = 0; i < center; i++)
                {
                    if (id is not 0)
                    {
                        AddBorder(main1, ref borderId);
                    }
                    AddChild(main1, span[i], nameof(NameWidth1), ref id);
                }
                id = 0;
                for (var i = center; i < span.Length; i++)
                {
                    if (id is not 0)
                    {
                        AddBorder(main2, ref borderId);
                    }
                    AddChild(main2, span[i], nameof(NameWidth2), ref id);
                }
            }

            IsExtraDeckVisible = deck.ExtraDeck.UniqueCount > 0;
            if (_extraDeck is { } extra)
            {
                var id = 0;
                foreach (var item in deck.ExtraDeck.AsSpan())
                {
                    if (id is not 0)
                    {
                        AddBorder(extra, ref borderId);
                    }
                    AddChild(extra, item, nameof(NameWidth3), ref id);
                }
            }

            IsSideDeckVisible = deck.SideDeck.UniqueCount > 0;
            if (_sideDeck is { } side)
            {
                var id = 0;
                foreach (var item in deck.SideDeck.AsSpan())
                {
                    if (id is not 0)
                    {
                        AddBorder(side, ref borderId);
                    }
                    AddChild(side, item, nameof(NameWidth4), ref id);
                }
            }

            _textChanging = false;
        }

        void AddBorder(StackPanel target, ref int cacheId)
        {
            var list = _lineCache;
            while (cacheId >= list.Count)
            {
                Border border = new()
                {
                    Height = 1,
                };
                border.SetBinding(Border.BackgroundProperty, new Binding(nameof(HorizontalDashBrush)) { Source = this });
                list.Add(border);
            }
            target.Children.Add(list[cacheId]);
            cacheId++;
        }

        void AddChild(StackPanel target, CountedCard card, string widthPropName, ref int cacheId)
        {
            var list = _cache.GetOrAdd(widthPropName);
            while (cacheId >= list.Count)
            {
                list.Add(new(this, widthPropName));
            }
            var cache = list[cacheId];
            cacheId++;

            cache.Background.Background = MediaUtils.GetBrush(LivreNoirLibrary.YuGiOh.Media.Icons.GetFrameBrush(card.GetFrameType()));
            cache.NumberText.Text = $"{card.Count}";
            cache.NameText.Tag = card.ThisCard;
            cache.NameText.Text = BuildCardNameText(card.ThisCard);
            target.Children.Add(cache.Root);
        }

        private string BuildCardNameText(Card card)
        {
            if (LineBreaks is not { } dic)
            {
                return card.Name;
            }
            var name = card.Name.AsSpan();
            var len = name.Length;
            var index = -1;
            if (dic.TryGetValue(card.Id, out var i) && i > 0 && i < len)
            {
                index = i;
                len++;
            }
            return string.Create(len, new CardNameState(name, index), ProcessBuildCardNameText);
        }

        private readonly ref struct CardNameState(ReadOnlySpan<char> source, int lbIndex)
        {
            public readonly ReadOnlySpan<char> Source = source;
            public readonly int LbIndex = lbIndex;
        }

        private static void ProcessBuildCardNameText(Span<char> target, CardNameState state)
        {
            var source = state.Source;
            var index = state.LbIndex;
            var targetIndex = 0;
            for (var i = 0; i < source.Length; i++, targetIndex++)
            {
                if (i == index)
                {
                    target[targetIndex] = '\n';
                    targetIndex++;
                }
                target[targetIndex] = source[i].ToHalf();
            }
        }

        private readonly struct ReverseTextConverter : IStringConverter
        {
            public int GetMaxCharCount(ReadOnlySpan<char> span) => span.Length;

            public bool TryGetChar(ReadOnlySpan<char> span, ref int spanIndex, out char c)
            {
                for (; spanIndex < span.Length; spanIndex++)
                {
                    c = span[spanIndex].ToHalf();
                    if (c is not ('\r' or '\n'))
                    {
                        return true;
                    }
                }
                c = default;
                return false;
            }
        }

        private static readonly ReverseTextConverter _converter = new();
        private static readonly ConvertingStringComparer<ReverseTextConverter> _comparer = new(_converter);

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_textChanging && sender is TextBox { Text: { } text, Tag: Card card } t)
            {
                _textChanging = true;
                try
                {
                    if (LineBreaks is not { } dic)
                    {
                        t.Text = card.Name;
                        return;
                    }

                    if (_comparer.Equals(text, card.Name))
                    {
                        var idx1 = text.IndexOf('\r');
                        var idx2 = text.IndexOf('\n');
                        var idx = idx1 is < 0 ? idx2 : (idx2 is < 0 ? idx1 : Math.Min(idx1, idx2));
                        if (idx > 0)
                        {
                            dic[card.Id] = idx;
                        }
                        else
                        {
                            dic.Remove(card.Id);
                        }
                    }
                    else
                    {
                        var i = t.CaretIndex;
                        foreach (var change in e.Changes)
                        {
                            i -= change.AddedLength;
                            i += change.RemovedLength;
                        }
                        t.Text = BuildCardNameText(card);
                        t.CaretIndex = i;
                    }
                }
                finally
                {
                    _textChanging = false;
                }
            }
        }

        private readonly List<Border> _lineCache = [];
        private readonly Dictionary<string, List<ElementCache>> _cache = [];

        private class ElementCache
        {
            public Border Root { get; }
            public Border Background { get; }
            public TextBlock NumberText { get; }
            public TextBox NameText { get; }

            public ElementCache(DeckRecipeView owner, string widthPropName)
            {
                Grid grid = new();
                var cols = grid.ColumnDefinitions;

                cols.Add(new() { Width = GridLength.Auto, SharedSizeGroup = "Label" });
                cols.Add(new() { Width = new(1) });
                ColumnDefinition col = new();
                col.SetBinding(ColumnDefinition.WidthProperty, new Binding(widthPropName) { Source = owner });
                cols.Add(col);

                var children = grid.Children;

                Border border = new()
                {
                    Opacity = 0.25
                };
                Grid.SetColumnSpan(border, 3);
                children.Add(border);
                Background = border;

                TextBlock text = new();
                text.SetBinding(TextBlock.FontWeightProperty, new Binding(nameof(NumberFontWeight)) { Source = owner });
                children.Add(text);
                NumberText = text;

                TextBox textBox = new();
                textBox.TextChanged += owner.TextBox_OnTextChanged;
                NameText = textBox;
                Viewbox box = new()
                {
                    Child = textBox,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    StretchDirection = StretchDirection.DownOnly,
                    Margin = new(4, 0, 4, 0)
                };
                Grid.SetColumn(box, 2);
                children.Add(box);

                border = new()
                {
                    Child = grid,
                };
                border.SetBinding(HeightProperty, new Binding(nameof(ContentHeight)) { Source = owner });
                border.SetBinding(BackgroundProperty, new Binding(nameof(Background)) { Source = owner });
                Root = border;
            }
        }
    }
}
