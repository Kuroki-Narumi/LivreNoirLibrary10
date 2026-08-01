using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.MasterDuel;
using LivreNoirLibrary.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls.DuelLogStatistics
{
    public class CheckableItem : NullableCheckableObject, IGridComboItem
    {
        public required IVocabData Name { get; init; }
        public int Row { get; init; }
        public int Column { get; init; }
        public virtual Brush? Background => AltBackgroundComboItem.GetBackground(Row, Column);

        private readonly List<CheckableItem> _children = []; 
        private bool _updating;

        public void SetChildren(ReadOnlySpan<CheckableItem> children)
        {
            _children.AddRange(children);
            foreach (var child in children)
            {
                child.IsCheckedChanged += Child_IsCheckedChanged;
            }
        }

        private void Child_IsCheckedChanged(object? sender, bool? value)
        {
            if (!_updating)
            {
                _updating = true;
                var allChecked = true;
                var allUnchecked = true;
                foreach (var child in _children.AsSpan())
                {
                    switch (child.IsChecked)
                    {
                        case true:
                            allUnchecked = false;
                            break;
                        case false:
                            allChecked = false;
                            break;
                        default:
                            allChecked = false;
                            allUnchecked = false;
                            break;
                    }
                    if (!(allChecked || allUnchecked))
                    {
                        break;
                    }
                }
                IsChecked = allChecked ? true : allUnchecked ? false : null;
                _updating = false;
            }
        }

        protected override void OnIsCheckedChanged(bool? oldValue, bool? newValue)
        {
            if (!_updating && newValue is { } v)
            {
                _updating = true;
                foreach (var child in _children)
                {
                    child.IsChecked = v;
                }
                _updating = false;
            }
        }
    }
}
