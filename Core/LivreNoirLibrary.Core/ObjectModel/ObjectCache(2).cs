using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.ObjectModel
{
    public class ObjectCache<T, TFactoryArg>(Func<TFactoryArg, T> factory) : IClear
        where T : IClear
        where TFactoryArg : allows ref struct
    {
        private readonly List<T> _data = [];
        private readonly Func<TFactoryArg, T> _factory = factory;
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

        public T GetNext(TFactoryArg arg)
        {
            var index = _index;
            try
            {
                var data = _data;
                var factory = _factory;
                while (index >= data.Count)
                {
                    data.Add(factory(arg));
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
