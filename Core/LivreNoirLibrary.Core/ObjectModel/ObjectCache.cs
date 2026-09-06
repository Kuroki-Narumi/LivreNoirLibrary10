using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.ObjectModel
{
    public class ObjectCache<T>(Func<T> factory, Action<T>? initializeFunc = null) : IClear
        where T : IClear
    {
        private readonly List<T> _data = [];
        private readonly Func<T> _factory = factory;
        private readonly Action<T>? _initializeFunc = initializeFunc;
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
                var item = data[index];
                _initializeFunc?.Invoke(item);
                return item;
            }
            finally
            {
                _index = index + 1;
            }
        }
    }
}
