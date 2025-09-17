using System;

namespace LivreNoirLibrary.ObjectModel
{
    public delegate bool Predicate<T1, T2>(T1 obj1, T2 obj2);
    public delegate bool Predicate<T1, T2, T3>(T1 obj1, T2 obj2, T3 obj3);
}
