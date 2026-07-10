using System.Windows;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class SearchBar
    {
        public static readonly RoutedEvent RequestSearchEvent = Events.Register<SearchBar, RoutedEventHandler<string>>();
        public static readonly RoutedEvent RequestOpenSortEvent = Events.Register<SearchBar, RoutedEventHandler>();
        public static readonly RoutedEvent RequestOpenSearchEvent = Events.Register<SearchBar, RoutedEventHandler>();
        public static readonly RoutedEvent RequestClearEvent = Events.Register<SearchBar, RoutedEventHandler>();

        public event RoutedEventHandler<string>? RequestSearch { add => AddHandler(RequestSearchEvent, value); remove => RemoveHandler(RequestSearchEvent, value);  }

        private bool AddHandler(RoutedEvent ev, RoutedEventHandler? handler, ref int listenerCount)
        {
            if (handler is not null)
            {
                AddHandler(ev, handler);
                listenerCount++;
            }
            return listenerCount > 0;
        }

        private bool RemoveHandler(RoutedEvent ev, RoutedEventHandler? handler, ref int listenerCount)
        {
            if (handler is not null)
            {
                RemoveHandler(ev, handler);
                listenerCount--;
            }
            return listenerCount > 0;
        }

        private int _sortListenerCount;
        public event RoutedEventHandler? RequestOpenSort
        {
            add => CanSort = AddHandler(RequestOpenSortEvent, value, ref _sortListenerCount);
            remove => CanSort = RemoveHandler(RequestOpenSortEvent, value, ref _sortListenerCount);
        }

        private int _searchListenerCount;
        public event RoutedEventHandler? RequestOpenSearch
        {
            add => CanSearch = AddHandler(RequestOpenSearchEvent, value, ref _searchListenerCount);
            remove => CanSearch = RemoveHandler(RequestOpenSearchEvent, value, ref _searchListenerCount);
        }

        private int _clearListenerCount;
        public event RoutedEventHandler? RequestClear
        {
            add => CanClear = AddHandler(RequestClearEvent, value, ref _clearListenerCount);
            remove => CanClear = RemoveHandler(RequestClearEvent, value, ref _clearListenerCount);
        }
    }
}
