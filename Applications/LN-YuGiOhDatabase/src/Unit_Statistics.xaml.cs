using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.Windows.YuGiOh;

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// Unit_Statistics.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_Statistics : UserControl
    {
        public Unit_Statistics()
        {
            InitializeComponent();
            this.RegisterCommand(YgoCommands.RefreshItems, Executed_Refresh);
        }

        private void Executed_Refresh(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            RefreshMonsterLike();
        }

        public static void RefreshMonsterLike()
        {
            var t0 = Stopwatch.GetTimestamp();
            var vm = MainViewModel.Instance;
            MonsterLike.ParseMonsterLikeCards(vm.CardPool.Cards, vm.TrapMonsters, vm.Tokens);
            Console.WriteLine($"Refresh MonsterLike in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms, TrapMonsters={vm.TrapMonsters.Count}, Tokens={vm.Tokens.Count}");
        }
    }
}
