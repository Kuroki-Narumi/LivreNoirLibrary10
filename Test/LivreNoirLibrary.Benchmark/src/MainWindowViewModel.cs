using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace LivreNoirLibrary.Benchmark
{
    public class MainWindowViewModel : ObservableObjectBase
    {
        public float FloatValue { get; set => SetValue(ref field, value); } = 3f;
        public float FloatDelta { get; set => SetValue(ref field, value); } = 0.01f;

        [MethodImpl(MethodImplOptions.NoOptimization)]
        public void FloatApplyDelta(bool isAdd, int repeat = 1)
        {
            var oldValue = FloatValue;
            var newValue = oldValue;
            var delta = FloatDelta;
            if (isAdd)
            {
                for (var i = 0; i < repeat; i++)
                {
                    newValue += delta;
                }
            }
            else
            {
                for (var i = 0; i < repeat; i++)
                {
                    newValue -= delta;
                }
            }
            Console.WriteLine($"{oldValue:E23} {(isAdd ? "+" : "-")} {delta:E23} {(repeat > 1 ? $"* {repeat}" : "")} = {newValue:E23}");
            FloatValue = newValue;
        }
    }
}
