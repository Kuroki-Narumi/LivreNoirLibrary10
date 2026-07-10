using LivreNoirLibrary.YuGiOh.Data;
using System.Windows;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class CardInfoView
    {
        private int _detachListenerCount;

        public static readonly RoutedEvent DetachEvent = Events.Register<CardInfoView, RoutedEventHandler<Card>>();

        public event RoutedEventHandler<Card>? Detach
        {
            add
            {
                AddHandler(DetachEvent, value);
                if (++_detachListenerCount > 0)
                {
                    CanDetach = true;
                }
            }
            remove
            {
                RemoveHandler(DetachEvent, value);
                if (--_detachListenerCount <= 0)
                {
                    CanDetach = false;
                }
            }
        }
    }
}
