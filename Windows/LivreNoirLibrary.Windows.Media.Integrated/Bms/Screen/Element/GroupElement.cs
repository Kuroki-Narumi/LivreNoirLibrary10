using System;
using System.Windows.Controls;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class GroupElement : Canvas, IScreenElement
    {
        public SkinElement SkinElement { get; }
        public ScreenElementViewModel ViewModel { get; }

        public GroupElement(Group source)
        {
            SkinElement = source;
            ViewModel = this.CreateVideModel(source);
        }

        public void Update(BmsTimer timer, long absoluteTick)
        {
            if (ViewModel.Update(timer, absoluteTick))
            {
                InvalidateVisual();
            }
        }
    }
}
