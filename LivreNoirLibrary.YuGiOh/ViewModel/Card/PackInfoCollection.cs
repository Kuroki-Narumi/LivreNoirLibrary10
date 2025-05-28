using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public class PackInfoCollection() : ICollection<PackInfo>
    {
        private readonly Dictionary<string, PackInfo> _list = [];
        private int _ocg_count;
        private int _tcg_count;

        public string GetNumber(string pid) => _list.TryGetValue(pid, out var info) ? info.Number : "";

        public bool IsReadOnly => false;
        public int Count => _list.Count;
        public int OcgCount => _ocg_count;
        public int TcgCount => _tcg_count;

        public PackInfoCollection(List<Serializable.PackInfo>? source) : this()
        {
            if (source is not null)
            {
                foreach (var info in CollectionsMarshal.AsSpan(source))
                {
                    PackInfo item = new(info);
                    _list.TryAdd(item.ProductId, item);
                }
            }
        }

        public void Clear()
        {
            _date_checked = false;
            _list.Clear();
            _ocg_count = 0;
            _tcg_count = 0;
        }

        public void Add(PackInfo item)
        {
            var id = item.ProductId;
            if (_list.TryGetValue(id, out var current))
            {
                if (!string.IsNullOrEmpty(item.Number))
                {
                    current.Number = item.Number;
                }
            }
            else
            {
                _list.Add(id, item);
            }
            _date_checked = false;
        }

        public bool Remove(PackInfo item)
        {
            _date_checked = false;
            return _list.Remove(item.ProductId);
        }

        public bool Remove(string pid)
        {
            _date_checked = false;
            return _list.Remove(pid);
        }

        public bool Contains(PackInfo item) => _list.ContainsKey(item.ProductId);
        public void CopyTo(PackInfo[] array, int arrayIndex) => _list.Values.CopyTo(array, arrayIndex);

        public IEnumerator<PackInfo> GetEnumerator()
        {
            foreach (var (_, item) in _list)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private readonly Dictionary<LocaleType, (DateTime Frist, DateTime Last)> _dates = [];
        private bool _date_checked;
        private bool _ocg;
        private bool _tcg;
        private void CheckDate()
        {
            if (!_date_checked)
            {
                _dates.Clear();
                _ocg_count = 0;
                _tcg_count = 0;
                _date_checked = true;
                DateTime first_ocg, first_tcg, last_ocg, last_tcg;
                first_ocg = first_tcg = DateTime.MaxValue;
                last_ocg = last_tcg = DateTime.MinValue;
                foreach (var (_, item) in _list)
                {
                    var date = item.Date;
                    if (item.IsTcg())
                    {
                        _tcg_count++;
                        _tcg = true;
                        if (date < first_tcg)
                        {
                            first_tcg = date;
                        }
                        if (date > last_tcg)
                        {
                            last_tcg = date;
                        }
                    }
                    else
                    {
                        _ocg_count++;
                        _ocg = true;
                        if (date < first_ocg)
                        {
                            first_ocg = date;
                        }
                        if (date > last_ocg)
                        {
                            last_ocg = date;
                        }
                    }
                }

                var first = first_ocg > first_tcg ? first_tcg : first_ocg;
                var last = last_ocg > last_tcg ? last_ocg : last_tcg;
                _dates.Add(LocaleType.Ocg, (first_ocg, last_ocg));
                _dates.Add(LocaleType.Tcg, (first_tcg, last_tcg));
                _dates.Add(LocaleType.None, (first, last));
                _dates.Add(LocaleType.Both, (first, last));
            }
        }

        public bool ContainsOcg()
        {
            CheckDate();
            return _ocg;
        }

        public bool ContainsTcg()
        {
            CheckDate();
            return _tcg;
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
            return _ocg ? _dates[LocaleType.Ocg].Frist : GetPadding(ascending);
        }

        public DateTime GetLastDateOcg(bool ascending)
        {
            CheckDate();
            return _ocg ? _dates[LocaleType.Ocg].Last : GetPadding(ascending);
        }

        public DateTime GetFirstDateTcg(bool ascending)
        {
            CheckDate();
            return _tcg ? _dates[LocaleType.Tcg].Last : GetPadding(ascending);
        }

        public DateTime GetLastDateTcg(bool ascending)
        {
            CheckDate();
            return _tcg ? _dates[LocaleType.Tcg].Last : GetPadding(ascending);
        }
    }
}
