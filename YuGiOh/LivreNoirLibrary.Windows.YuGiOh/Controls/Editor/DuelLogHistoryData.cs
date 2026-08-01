using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.MasterDuel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class DuelLogHistoryData(ICollection<DuelLog>? data) : CompressionHistoryData<ICollection<DuelLog>>(data), IHistoryData<DuelLogHistoryData>
    {
        private int _selectedIndex = -1;

        public bool IsSelectionStored { get; private set; }

        public bool EqualsAll(DuelLogHistoryData other) => base.EqualsAll(other);

        public void StoreSelection(ReadOnlySpan<ListBox> listViews)
        {
            if (listViews.Length > 0)
            {
                var lv = listViews[0];
                if (lv.SelectedItem is DuelLog item && lv.ItemsSource is IList<DuelLog> list)
                {
                    _selectedIndex = list.IndexOf(item);
                }
                IsSelectionStored = true;
            }
        }

        public void RestoreSelection(ReadOnlySpan<ListBox> listViews)
        {
            if (listViews.Length > 0)
            {
                var lv = listViews[0];
                if (lv.ItemsSource is IList<DuelLog> list && (uint)_selectedIndex < (uint)list.Count)
                {
                    var item = list[_selectedIndex];
                    lv.SelectedItem = item;
                }
            }
        }

        protected override void Dump(Stream stream, ICollection<DuelLog> source)
        {
            Json.Dump(stream, source);
        }

        protected override void Load(Stream stream, ICollection<DuelLog> target, object? state)
        {
            var source = Json.Load<DuelLog[]>(stream);
            LoadData(target, source);
        }

        public static void LoadData(ICollection<DuelLog> target, DuelLog[] source)
        {
            target.Clear();
            switch (target)
            {
                case List<DuelLog> list:
                    list.AddRange(source);
                    break;
                case ObservableList<DuelLog> list:
                    list.AddRange(source);
                    break;
                default:
                    foreach (var item in source)
                    {
                        target.Add(item);
                    }
                    break;
            }
        }
    }
}
