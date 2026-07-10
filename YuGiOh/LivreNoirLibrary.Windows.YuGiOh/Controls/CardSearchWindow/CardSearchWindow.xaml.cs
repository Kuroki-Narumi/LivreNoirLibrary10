using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Search;
using LivreNoirLibrary.Windows.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class CardSearchWindow : Window, IToggleButtonContainer
    {
        public static ObservableList<CardSearchConditionsPreset> Presets { get; } = [new() { Name = "TestPreset" }];
        public static CardSearchConditionsPreset? DefaultPreset { get; private set; }

        public static void LoadPreset(IEnumerable<CardSearchConditionsPreset> presets)
        {
            var list = Presets;
            list.ClearWithoutNotify();
            CardSearchConditionsPreset? @default = null;
            foreach (var preset in presets)
            {
                list.AddWithoutNotify(preset);
                if (preset.IsDefault)
                {
                    @default = preset;
                }
            }
            DefaultPreset = @default;
        }

        public static void SavePreset(List<CardSearchConditionsPreset> target)
        {
            target.Clear();
            target.AddRange(Presets.AsSpan());
        }

        public event EventHandler? Search;

        private CardSearchConditions? _conditions;

        public CardSearchConditionsViewModel ViewModel { get; } = new();

        bool IToggleButtonContainer.MousePressed { get; set; }
        bool IToggleButtonContainer.MouseToggleState { get; set; }

        public CardSearchWindow()
        {
            DataContext = ViewModel;
            InitializeComponent();
            this.InitializeIToggleButtonContainer();
            CreateDateContextMenu(DatePicker_FirstSince);
            CreateDateContextMenu(DatePicker_FirstUntil);
            CreateDateContextMenu(DatePicker_LastSince);
            CreateDateContextMenu(DatePicker_LastUntil);

            this.RegisterCommand(Commands.Insert, Executed_PresetAdd);
            this.RegisterCommand(Commands.MoveUp, Executed_PresetMoveUp, ListView_Preset.CanExecute_MoveUp);
            this.RegisterCommand(Commands.MoveDown, Executed_PresetMoveDown, ListView_Preset.CanExecute_MoveDown);
            this.RegisterCommand(Commands.Delete, Executed_PresetDelete, ListView_Preset.CanExecute_Item);
            this.RegisterCommand(Commands.Save, Executed_PresetOverwrite, ListView_Preset.CanExecute_Item);
        }

        public void Setup(CardSearchConditions conditions)
        {
            _conditions = conditions;
            ViewModel.CopyFrom(conditions);
            TextBox_Expression.Text = ViewModel.StatusExpression.Expression;
            TextBox_Search.Text = ViewModel.SearchText;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void TabControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as TabControl)?.ChangeByWheel(e);
        }

        private void OnClick_Search(object sender, RoutedEventArgs e)
        {
            if (_conditions is { } cond)
            {
                ViewModel.CopyTo(cond);
                Search?.Invoke(this, EventArgs.Empty);
            }
            Close();
            e.Handled = true;
        }

        private void OnClick_Clear(object sender, RoutedEventArgs e)
        {
            ViewModel.Clear();
            e.Handled = true;
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            _conditions = null;
            Close();
            e.Handled = true;
        }

        private void RadioContainer_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as Panel)?.ChangeRadioButtonByWheel(e);
        }

        private void DatePicker_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is DatePicker { SelectedDate: DateTime t } d)
            {
                if (e.Delta > 0)
                {
                    t += TimeSpan.FromDays(1);
                }
                else
                {
                    t -= TimeSpan.FromDays(1);
                }
                d.SelectedDate = t;
                e.Handled = true;
            }
        }

        private bool Expression_Verify(string text)
        {
            ViewModel.StatusExpression.Expression = text;
            return ViewModel.StatusExpression.IsValid;
        }

        private bool SearchText_Verify(string text)
        {
            ViewModel.SearchText = text;
            return ViewModel.IsTextValid;
        }

        internal static void CreateDateContextMenu(DatePicker picker)
        {
            ContextMenu menu = new();
            var items = menu.Items;
            var periods = Utils.Periods.AsSpan();
            var i = 0;
            foreach (var date in periods)
            {
                items.Add(CreateMenuItem(picker, $"第{++i}期({date:yyyy-MM-dd})", date));
            }
            items.Add(CreateMenuItem(picker, "現在", default));
            picker.ContextMenu = menu;
        }

        private static MenuItem CreateMenuItem(DatePicker picker, string header, DateTime date)
        {
            MenuItem item = new() { Header = header };
            item.Click += (s, e) => picker.SelectedDate = (date == default ? DateTime.Now : date);
            return item;
        }

        private static bool IsCommandFromPreset(ExecutedRoutedEventArgs e)
        {
            return (e.OriginalSource as DependencyObject).TryGetAncestor<DropDownMenuButton>(out _);
        }

        private void Executed_PresetAdd(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsCommandFromPreset(e))
            {
                CardSearchConditionsPreset item = new();
                ViewModel.CopyTo(item.Conditions, false);
                ListView_Preset.OnExecuted_Insert(Presets, item, e);
            }
        }

        private void Executed_PresetMoveUp(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsCommandFromPreset(e))
            {
                ListView_Preset.OnExecuted_MoveUp(Presets, e);
            }
        }

        private void Executed_PresetMoveDown(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsCommandFromPreset(e))
            {
                ListView_Preset.OnExecuted_MoveDown(Presets, e);
            }
        }

        private void Executed_PresetDelete(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsCommandFromPreset(e))
            {
                ListView_Preset.OnExecuted_Delete(Presets, e);
            }
        }

        private void Executed_PresetOverwrite(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsCommandFromPreset(e) && ListView_Preset.SelectedItem is CardSearchConditionsPreset p)
            {
                ViewModel.CopyTo(p.Conditions, false);
                e.Handled = true;
            }
        }

        private void OnApplyPreset(object sender, RoutedEventArgs<CardSearchConditionsPreset> e)
        {
            ViewModel.CopyFrom(e.Value.Conditions, false);
            e.Handled = true;
        }
    }
}
