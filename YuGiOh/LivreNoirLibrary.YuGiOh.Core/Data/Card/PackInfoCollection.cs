using LivreNoirLibrary.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class PackInfoCollection() : IEnumerable<PackInfo>
    {
        private readonly List<PackInfo> _ocgList = [];
        private readonly List<PackInfo> _tcgList = [];

        private readonly Dictionary<LocaleType, (DateTime Frist, DateTime Last)> _dates = [];
        private bool _needCheckDate;

        public int Count => OcgCount + TcgCount;
        public int OcgCount => _ocgList.Count;
        public bool ContainsOcg => _ocgList.Count is > 0;
        public int TcgCount => _tcgList.Count;
        public bool ContainsTcg => _tcgList.Count is > 0;

        public void Clear()
        {
            _needCheckDate = true;
            _ocgList.Clear();
            _tcgList.Clear();
        }

        public void Load(PackInfoCollection source)
        {
            Clear();
            _ocgList.AddRange(source._ocgList);
            _tcgList.AddRange(source._tcgList);
        }

        public void Add(PackInfo item)
        {
            if (item.IsTcg())
            {
                AddImpl(_tcgList, item);
            }
            else
            {
                AddImpl(_ocgList, item);
            }
            _needCheckDate = true;
        }

        private static void AddImpl(List<PackInfo> list, PackInfo info)
        {
            var index = list.BinarySearch(info);
            if (index is >= 0)
            {
                list[index] = info;
            }
            else
            {
                list.Insert(~index, info);
            }
        }

        public IEnumerator<PackInfo> GetEnumerator()
        {
            foreach (var item in _ocgList)
            {
                yield return item;
            }
            foreach (var item in _tcgList)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<PackFullInfo> EnumerateFullInfo()
        {
            var packs = CardPool.Instance.Packs;
            foreach (var info in this)
            {
                yield return new(info, packs);
            }
        }

        private void CheckDate()
        {
            if (_needCheckDate)
            {
                var (first_ocg, last_ocg) = UpdateDate(_ocgList);
                var (first_tcg, last_tcg) = UpdateDate(_tcgList);

                var first = first_ocg > first_tcg ? first_tcg : first_ocg;
                var last = last_ocg > last_tcg ? last_ocg : last_tcg;
                var dates = _dates;
                dates[LocaleType.Ocg] = (first_ocg, last_ocg);
                dates[LocaleType.Tcg] = (first_tcg, last_tcg);
                dates[LocaleType.None] = (first, last);
                dates[LocaleType.Both] = (first, last);

                _needCheckDate = false;
            }

            static (DateTime First, DateTime Last) UpdateDate(List<PackInfo> list)
            {
                var first = DateTime.MaxValue;
                var last = DateTime.MinValue;
                foreach (var item in list.AsSpan())
                {
                    var date = item.Date;
                    if (date < first)
                    {
                        first = date;
                    }
                    if (date > last)
                    {
                        last = date;
                    }
                }
                return (first, last);
            }
        }

        public (DateTime First, DateTime Last) GetDate(LocaleType type)
        {
            CheckDate();
            return _dates[type];
        }

        private static DateTime GetPadding(bool ascending) => ascending ? DateTime.MaxValue : DateTime.MinValue;

        public DateTime GetFirstDateOcg(bool ascending)
        {
            CheckDate();
            return ContainsOcg ? _dates[LocaleType.Ocg].Frist : GetPadding(ascending);
        }

        public DateTime GetLastDateOcg(bool ascending)
        {
            CheckDate();
            return ContainsOcg ? _dates[LocaleType.Ocg].Last : GetPadding(ascending);
        }

        public DateTime GetFirstDateTcg(bool ascending)
        {
            CheckDate();
            return ContainsTcg ? _dates[LocaleType.Tcg].Last : GetPadding(ascending);
        }

        public DateTime GetLastDateTcg(bool ascending)
        {
            CheckDate();
            return ContainsTcg ? _dates[LocaleType.Tcg].Last : GetPadding(ascending);
        }
    }
}
