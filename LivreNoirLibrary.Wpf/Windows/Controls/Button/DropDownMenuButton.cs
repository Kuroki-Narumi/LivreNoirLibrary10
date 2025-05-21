using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Input;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    [TemplatePart(Name = PART_Button, Type = typeof(System.Windows.Controls.Primitives.ToggleButton))]
    [ContentProperty(nameof(Content))]
    public class DropDownMenuButton : DropDownBase, IOptionMark
    {
        public const string PART_Button = nameof(PART_Button);

        static DropDownMenuButton()
        {
            PropertyUtils.OverrideDefaultStyleKey<DropDownMenuButton>();
        }

        public static readonly DependencyProperty IsOptionMarkVisibleProperty = IOptionMark.IsOptionMarkVisibleProperty.AddOwner(typeof(DropDownMenuButton));
        public static readonly DependencyProperty CornerRadiusProperty = Border.CornerRadiusProperty.AddOwner(typeof(DropDownMenuButton));
        public static readonly DependencyProperty DropDownContentProperty = PropertyUtils.Register<object>(typeof(DropDownMenuButton));

        public static readonly DependencyProperty CommandParameterProperty = ButtonBase.CommandParameterProperty.AddOwner(typeof(DropDownMenuButton));
        public static readonly DependencyProperty CommandProperty = ButtonBase.CommandProperty.AddOwner<ICommand>(typeof(DropDownMenuButton), null, OnCommandChanged);
        public static readonly DependencyProperty CommandTargetProperty = ButtonBase.CommandTargetProperty.AddOwner(typeof(DropDownMenuButton));
        public static readonly DependencyProperty KeyGestureTextProperty = IIconButton.KeyGestureTextProperty.AddOwner(typeof(DropDownMenuButton));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(KeyGestureTextProperty, (e.NewValue as RoutedCommand)?.GetKeyGestureText());
        }

        public object? CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }
        public ICommand? Command { get => GetValue(CommandProperty) as ICommand; set => SetValue(CommandProperty, value); }
        public IInputElement? CommandTarget { get => GetValue(CommandTargetProperty) as IInputElement; set => SetValue(CommandTargetProperty, value); }
        public string? KeyGestureText { get => GetValue(KeyGestureTextProperty) as string; set => SetValue(KeyGestureTextProperty, value); }

        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

        public object? DropDownContent { get => GetValue(DropDownContentProperty); set => SetValue(DropDownContentProperty, value); }
        public bool IsOptionMarkVisible { get => (bool)GetValue(IsOptionMarkVisibleProperty); set => SetValue(IsOptionMarkVisibleProperty, value); }

        protected ButtonBase? _button;

        public override void OnApplyTemplate()
        {
            if (_button is not null)
            {
                _button.Click -= OnClick_Button;
            }
            base.OnApplyTemplate();
            _button = GetTemplateChild(PART_Button) as ButtonBase;
            if (_button is not null)
            {
                _button.Click += OnClick_Button;
            }
        }

        private void OnClick_Button(object? sender, RoutedEventArgs e)
        {
            if (IsDropDownOpen && (!AutoOpen && !IsMouseCaptured))
            {
                IsDropDownOpen = false;
            }
            else
            {
                RaiseClickEvent();
            }
        }
    }
}
