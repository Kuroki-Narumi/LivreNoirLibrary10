using System;
using System.Windows;
using LivreNoirLibrary.Windows.Media.Bms;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public interface IScreenElement
    {
        SkinElement SkinElement { get; }
        ScreenElementViewModel ViewModel { get; }
    }

    public static class IScreenElementExtension
    {
        public static ScreenElementViewModel CreateVideModel<T>(this T element, SkinElement source)
            where T : FrameworkElement, IScreenElement
        {
            ScreenElementViewModel vm = new();
            vm.SetBinding(element);
            return vm;
        }

        public static void LoadDestination(this IScreenElement element, Skin skin, IVariableProvider? provider)
        {
            element.ViewModel.LoadDestination(skin, provider, element.SkinElement);
        }
    }
}
