using LivreNoirLibrary.YuGiOh.MasterDuel;
using System;
using System.Collections.Generic;
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

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// Unit_Statistics.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_DuelLog : UserControl
    {
        public Unit_DuelLog()
        {
            DataContext = MainViewModel.Instance;
            InitializeComponent();
        }

        private void OnTagNameChanged(object sender, LivreNoirLibrary.Windows.YuGiOh.Controls.TagNameChangedEventArgs e)
        {
            MainViewModel.Instance.DuelLogs.RenameTag(e.OldName, e.NewName);
        }
    }
}
