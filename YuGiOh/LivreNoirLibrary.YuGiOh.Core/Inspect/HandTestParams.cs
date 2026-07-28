using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Inspect
{
    public class HandTestParams : ObservableObjectBase
    {
        public const int DefaultRepeat = 100000;
        public const int DefaultHand = 5;

        public static readonly DrawSourceType[] DefaultPriority =
        [
            DrawSourceType.GoKin,
            DrawSourceType.KinKen,
            DrawSourceType.Other,
            DrawSourceType.GoDon,
            DrawSourceType.GoKen,
        ];

        public int RepeatCount { get; set => SetValue(ref field, value); } = DefaultRepeat;
        public int NumberOfHand { get; set => SetValue(ref field, value); } = DefaultHand;
        public ObservableList<DrawSourceType> DrawSourcePriority { get; set => SetValue(ref field, value); } = [.. DefaultPriority];
    }
}
