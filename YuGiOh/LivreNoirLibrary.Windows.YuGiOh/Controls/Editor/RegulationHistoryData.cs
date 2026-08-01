using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using SReg = LivreNoirLibrary.YuGiOh.Serializable.Regulation;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class RegulationHistoryData(Regulation? data) : HistoryDataWithSelectionBase(5), IHistoryData<RegulationHistoryData>
    {
        private readonly SReg? _data = data?.ToSerializable();

        public bool EqualsAll(RegulationHistoryData other)
        {
            var data1 = _data;
            var data2 = other._data;
            if (data1 is null)
            {
                return data2 is null;
            }
            if (data2 is null)
            {
                return false;
            }
            return Equals(data1.Forbidden, data2.Forbidden) &&
                   Equals(data1.Limit1, data2.Limit1) &&
                   Equals(data1.Limit2, data2.Limit2) &&
                   Equals(data1.Specified, data2.Specified);

            static bool Equals(List<int>? left, List<int>? right)
            {
                if (left is null)
                {
                    return right is null;
                }
                if (right is null)
                {
                    return false;
                }
                return left.EqualsAll(right);
            }
        }

        public void ConvertBack(Regulation? target, ReadOnlySpan<ListBox> listViews, ICardProvider? provider)
        {
            if (target is not null && _data is { } data)
            {
                target.Load(data, provider);
                RestoreSelection(listViews);
            }
        }
    }
}
