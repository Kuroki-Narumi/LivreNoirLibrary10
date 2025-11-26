using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public class MergeOptions : ObservableObjectBase
    {
        public bool Type_Conductor { get; set => SetValue(ref field, value); }
        public bool Type_Meta { get; set => SetValue(ref field, value); }
        public bool Type_Sound { get; set => SetValue(ref field, value); }
        public bool AvoidDuplication { get; set => SetValue(ref field, value); } = true;
        public int DefStart { get; set => SetValue(ref field, value); }
        public bool AbsolutePosition { get; set => SetValue(ref field, value); }
    }
}
