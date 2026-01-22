using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public partial class DecimalKeyTimeline<TKey, TValue> : XYSingleTimelineBase<TKey, decimal, TValue, Operator_decimal> where TKey : struct
    {
    }
}