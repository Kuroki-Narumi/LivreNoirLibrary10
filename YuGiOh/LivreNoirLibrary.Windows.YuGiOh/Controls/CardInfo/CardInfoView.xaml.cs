using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// CardInfoView.xaml の相互作用ロジック
    /// </summary>
    public partial class CardInfoView : UserControl
    {
        public const double DefaultPackListHeight = 104;
        public const double ExpandedPackListHeight = 224;

        [DependencyProperty]
        private Card? _source;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _canDetach;

        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _isMonster;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _isLink;
        [DependencyProperty(SetterScope = Scope.Private)]
        private LinkDirection _linkMarker;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _isPendulum;
        [DependencyProperty(SetterScope = Scope.Private)]
        private VocabData? _levelName;

        public CardInfoView()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void OnSourceChanged(Card? value)
        {
            if (value is not null)
            {
                IsMonster = value.IsMonster();
                IsPendulum = value.IsPendulum();
                if (value.IsLink())
                {
                    IsLink = true;
                    LevelName = Vocab.Current.CInfo.Link;
                    LinkMarker = value.GetLinkDirections();
                }
                else
                {
                    IsLink = false;
                    LevelName = value.IsXyz() ? Vocab.Current.CInfo.Rank : Vocab.Current.CInfo.Level;
                    LinkMarker = 0;
                }
            }
            else
            {
                IsMonster = false;
                IsLink = false;
                IsPendulum = false;
            }
        }

        private void OnClick_DB1(object sender, RoutedEventArgs e)
        {
            if (_source is { } card)
            {
                this.RaiseCardLinkClicked(card.Id, false);
            }
        }

        private void OnClick_DB2(object sender, RoutedEventArgs e)
        {
            if (_source is { } card)
            {
                this.RaiseCardLinkClicked(card.Id, true);
            }
        }

        private void OnClick_Pack(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkContentElement { DataContext: PackInfo info })
            {
                this.RaisePackLinkClicked(info.ProductId);
            }
        }

        private void OnClick_RelatedText(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkContentElement { DataContext: string text })
            {
                this.RaiseRelatedTextClicked(text);
            }
        }

        private void OnClick_Copy(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement { Tag: TextBox {Text: string text } })
            {
                if (!string.IsNullOrEmpty(text))
                {
                    try
                    {
                        Clipboard.SetText(text);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void OnClick_Detach(object sender, RoutedEventArgs e)
        {
            if (Source is { } card)
            {
                RaiseEvent(new RoutedEventArgs<Card>(card, DetachEvent, this));
            }
        }
    }
}
