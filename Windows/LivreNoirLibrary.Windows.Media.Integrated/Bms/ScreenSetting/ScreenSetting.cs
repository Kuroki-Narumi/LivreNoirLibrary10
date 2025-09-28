using System;
using System.Text.Json.Serialization;
using Dr = System.Drawing;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public abstract class ScreenSetting : ObservableObjectBase
    {
        public int Width { get; set => SetValue(ref field, value); } = 1280;
        public int Height { get; set => SetValue(ref field, value); } = 720;

    }
}
