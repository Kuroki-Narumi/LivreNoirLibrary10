using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SM = System.Windows.Controls.SelectionMode;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public static class CardClipboard
    {
        public static void Set(ICard card)
        {
            try
            {
                DataObject obj = new();
                obj.SetText(card.NameWithBracket());
                obj.SetData(DataObjectTypes.CardClipboard, card.ThisCard.GetJsonBytes(false));
                Clipboard.SetDataObject(obj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static bool CanPaste()
        {
            try
            {
                return Clipboard.ContainsData(DataObjectTypes.CardClipboard);
            }
            catch { }
            return false;
        }

        public static bool TryGet([MaybeNullWhen(false)] out Card card)
        {
            try
            {
                if (Clipboard.ContainsData(DataObjectTypes.CardClipboard) && 
                    Clipboard.GetData(DataObjectTypes.CardClipboard) is byte[] bytes &&
                    Json.TryParse(bytes, out card))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            card = default;
            return false;
        }

        public static void RegisterCopy(ListBox element)
        {
            element.RegisterCommand(ApplicationCommands.Copy, element.OnExecuted_Copy, element.CanExecute_Item);
        }

        public static void OnExecuted_Copy(this ListBox element, object sender, ExecutedRoutedEventArgs e)
        {
            var card = element.SelectionMode switch
            {
                SM.Single => element.SelectedItem as ICard,
                _ => GetSelectedItem(element.SelectedItems)
            };
            if (card is not null)
            {
                Set(card);
                e.Handled = true;
            }
        }

        private static ICard? GetSelectedItem(IList list) => list.Count is 0 ? null : list[0] as ICard;

        public static void CanExecute_Paste(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = CanPaste();


        public static void CardListView_PreviewMouseLeftButtonDown(this UIElement obj, object sender, MouseButtonEventArgs e)
        {
            if (KeyInput.IsShiftDown() && sender is ListViewItem { DataContext: ICard c })
            {
                try
                {
                    Clipboard.SetText(c.NameWithBracket());
                }
                catch { }
            }
        }

        public static void CardListView_PreviewMouseLeftButtonDown_Alt(this UIElement obj, object sender, MouseButtonEventArgs e)
        {
            if (KeyInput.IsAltDown() && sender is ListViewItem { DataContext: ICard c })
            {
                try
                {
                    Clipboard.SetText(c.NameWithBracket());
                }
                catch { }
            }
        }
    }
}
