using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// CardSortWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CardSortWindow : Window
    {
        public static ObservableList<CardSortOptionsPreset> Presets { get; } = [];
        public static event EventHandler<CardSortOptionsPreset?>? DefaultPresetChanged;

        public static void LoadPreset(IEnumerable<CardSortOptionsPreset> presets)
        {
            Presets.Clear();
            Presets.AddRange(presets);
        }

        public static void SavePreset(List<CardSortOptionsPreset> target)
        {
            target.Clear();
            target.AddRange(Presets.AsSpan());
        }

        public event EventHandler? Sort;

        private ICardSort? _target;

        public SortOptionViewModel[] ViewModels { get; }

        public CardSortWindow(ICardSort target)
        {
            ViewModels = [new("Sort1"), new("Sort2"), new("Sort3"), new("Sort4")];
            DataContext = this;
            InitializeComponent();

            this.RegisterCommand(Commands.Insert, Executed_PresetAdd);
            this.RegisterCommand(Commands.MoveUp, Executed_PresetMoveUp, ListView_Preset.CanExecute_MoveUp);
            this.RegisterCommand(Commands.MoveDown, Executed_PresetMoveDown, ListView_Preset.CanExecute_MoveDown);
            this.RegisterCommand(Commands.Delete, Executed_PresetDelete, ListView_Preset.CanExecute_Item);
            this.RegisterCommand(Commands.Save, Executed_PresetOverwrite, ListView_Preset.CanExecute_Item);

            this.RegisterCommand(PresetPresenterBase.DefaultChangedCommand, Executed_DefaultChanged);

            Setup(target);
        }

        public void Setup(ICardSort target)
        {
            _target = target;
            CopyFrom(target.CardSortOptions);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void CopyFrom(List<CardSortOption> options)
        {
            var vms = ViewModels;
            var vmCount = vms.Length;
            var count = Math.Min(options.Count, vmCount);
            for (var i = 0; i < count; i++)
            {
                vms[i].CopyFrom(options[i]);
            }
            if (count < vmCount)
            {
                for (var i = count; i < vmCount; i++)
                {
                    vms[i].Clear();
                }
            }
        }

        private void OnClick_Exec(object sender, RoutedEventArgs e)
        {
            if (_target is { CardSortOptions: { } options })
            {
                CopyTo(options);
                Sort?.Invoke(this, EventArgs.Empty);
            }
            Close();
            e.Handled = true;
        }

        private void CopyTo(List<CardSortOption> options)
        {
            options.Clear();
            foreach (var vm in ViewModels)
            {
                var option = vm.GetOption();
                if (option.Key is not SortKey.None)
                {
                    options.Add(option);
                }
            }
        }

        private void OnClick_Clear(object sender, RoutedEventArgs e)
        {
            if (_target is { DefaultCardSortOptions: { } options })
            {
                CopyFrom(options);
            }
            else
            {
                foreach (var vm in ViewModels)
                {
                    vm.Clear();
                }
            }
            e.Handled = true;
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            _target = null;
            Close();
            e.Handled = true;
        }

        private static bool IsCommandFromPreset(ExecutedRoutedEventArgs e)
        {
            return (e.OriginalSource as DependencyObject).TryGetAncestor<DropDownMenuButton>(out _);
        }

        private void Executed_PresetAdd(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsCommandFromPreset(e))
            {
                CardSortOptionsPreset item = new();
                CopyTo(item.Conditions);
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
            if (IsCommandFromPreset(e) && ListView_Preset.SelectedItem is CardSortOptionsPreset p)
            {
                CopyTo(p.Conditions);
                e.Handled = true;
            }
        }

        private void OnApplyPreset(object sender, RoutedEventArgs<CardSortOptionsPreset> e)
        {
            CopyFrom(e.Value.Conditions);
            e.Handled = true;
        }

        private void Executed_DefaultChanged(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is CardSortOptionsPreset preset)
            {
                DefaultPresetChanged?.Invoke(this, preset.IsDefault ? preset : null);
            }
        }
    }
}
