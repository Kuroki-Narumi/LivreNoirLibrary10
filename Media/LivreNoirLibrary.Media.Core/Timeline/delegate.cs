using System;
using System.IO;

namespace LivreNoirLibrary.ObjectModel
{
    public delegate bool Predicate<T1, T2>(T1 obj1, T2 obj2);
    public delegate bool Predicate<T1, T2, T3>(T1 obj1, T2 obj2, T3 obj3);
}

namespace LivreNoirLibrary.Media
{
    public delegate void ValueWriter<T>(BinaryWriter writer, T value);
    public delegate T ValueReader<T>(BinaryReader reader);
    public delegate TValue ValueReader<TX, TValue>(BinaryReader reader, TX position);
    public delegate TValue ValueReader<TY, TX, TValue>(BinaryReader reader, TY key, TX position);
}