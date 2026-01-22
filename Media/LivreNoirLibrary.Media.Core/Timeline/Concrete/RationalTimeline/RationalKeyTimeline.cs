using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public partial class RationalKeyTimeline<TKey, TValue> : XYSingleTimelineBase<TKey, Rational, TValue, Operator_Rational> where TKey : struct
    {
    }
}