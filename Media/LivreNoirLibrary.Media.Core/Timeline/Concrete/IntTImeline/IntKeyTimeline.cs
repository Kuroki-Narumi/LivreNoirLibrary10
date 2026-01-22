using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public partial class IntKeyTimeline<TKey, TValue> : XYSingleTimelineBase<TKey, int, TValue, Operator_int> where TKey : struct
    {
    }
}