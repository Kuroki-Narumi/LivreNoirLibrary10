using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public partial class LongKeyTimeline<TKey, TValue> : XYSingleTimelineBase<TKey, long, TValue, Operator_long> where TKey : struct
    {
    }
}