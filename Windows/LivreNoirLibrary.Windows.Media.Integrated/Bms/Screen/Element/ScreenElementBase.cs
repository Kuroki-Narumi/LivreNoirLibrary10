using System;
using System.Windows;
using LivreNoirLibrary.Windows.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class ScreenElementBase : FrameworkElement, IScreenElement
    {
        public SkinElement SkinElement { get; }
        public ScreenElementViewModel ViewModel { get; }

        public ScreenElementBase(SkinElement element)
        {
            SkinElement = element;
            ViewModel = this.CreateVideModel(element);
        }
    }
}
