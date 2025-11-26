using LivreNoirLibrary.IO;
using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IBarLengthCollection : IBarLengthProvider<double>, IEnumerable<(int, double)>, ICount, IClear, IDumpable, ILoadable
    {
        bool TryGetValue(int number, out double value);
        bool Set(int number, double value);
        bool Remove(int number);

        void Insert(int number, int count);
        void Delete(int number, int count);
        void Merge(IBarLengthCollection source);

        void IDumpable.Dump(BinaryWriter writer)
        {
            var c = Count;
            writer.Write((ushort)c);
            foreach (var (number, value) in this)
            {
                writer.Write((short)number);
                writer.Write(value);
            }
        }

        void ILoadable.ProcessLoad(BinaryReader reader)
        {
            Clear();
            var count = (int)reader.ReadUInt16();
            for (var i = 0; i < count; i++)
            {
                var number = reader.ReadInt16();
                var value = reader.ReadDouble();
                Set(number, value);
            }
        }

        double IBarLengthProvider<double>.GetBarLength(int number) => TryGetValue(number, out var value) ? value : BmsConstants.DefaultBarLength;

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
