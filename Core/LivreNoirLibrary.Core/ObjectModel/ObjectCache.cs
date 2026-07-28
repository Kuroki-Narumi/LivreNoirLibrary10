using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.ObjectModel
{
    public class ObjectCache<T>(Func<T> factory) : IClear
        where T : IClear
    {
        private readonly List<T> _data = [];
        private readonly Func<T> _factory = factory;
        private int _index;

        protected ReadOnlySpan<T> ActiveElements => _data.AsSpan(0, _index);

        public void Clear()
        {
            foreach (var data in _data.AsSpan())
            {
                data.Clear();
            }
            _index = 0;
        }

        public T GetNext()
        {
            var index = _index;
            try
            {
                var data = _data;
                var factory = _factory;
                while (index >= data.Count)
                {
                    data.Add(factory());
                }
                return data[index];
            }
            finally
            {
                _index = index + 1;
            }
        }
    }
}
