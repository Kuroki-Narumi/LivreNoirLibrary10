using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.YuGiOh;
using LivreNoirLibrary.Windows.YuGiOh.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using S = LivreNoirLibrary.YuGiOh.Scraping;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System;
using System.ComponentModel;
using LivreNoirLibrary.Windows.YuGiOh.Converters;

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IUpdateCheck, IProgressReporter
    {
        public static MainViewModel ViewModel => MainViewModel.Instance;

        bool IUpdateCheck.CheckUpdate { get => ViewModel.CheckUpdate; set => ViewModel.CheckUpdate = value; }
        string IUpdateCheck.VersionUrl => "";
        string IUpdateCheck.UpdaterLocation => MainViewModel.AppName;

        UIElement IProgressReporter.MainElement => Grid_Main;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get; set; }
        Dispatcher IProgressReporter.Dispatcher => Dispatcher;

        public MainWindow()
        {
            DataContext = ViewModel;
            InitializeResources();
            InitializeComponent();
            this.SetDispatcher(CheckUpdate_Auto);

            this.RegisterCommand(YgoCommands.UpdateDatabase, Executed_UpdateDatabase);
            this.RegisterCommand(YgoCommands.LoadOcgRegulation, Executed_LoadOcgRegulation);
            this.RegisterCommand(YgoCommands.LoadTcgRegulation, Executed_LoadTcgRegulation);
        }

        private void InitializeResources()
        {
            if (double.IsFinite(ViewModel.WindowLeft))
            {
                Left = ViewModel.WindowLeft;
            }
            if (double.IsFinite(ViewModel.WindowTop))
            {
                Top = ViewModel.WindowTop;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel.WindowLeft = Left;
            ViewModel.WindowTop = Top;
            //MainViewModel.Save();
            base.OnClosing(e);
        }

        private void CheckUpdate_Auto() => CheckUpdate(false);

        private void CheckUpdate(bool force)
        {
            var current = UpdateInfo.GetCurrentVersion();
            var previous = ViewModel.Version;
            if (previous is not null && previous < current)
            {
                TextBlock_Info.Text = $"{Vocab.Current.Message_UpdateComplete} ({current.ToStringAuto()})";
                Area_UpdateInfo.Open(true);
            }
            ViewModel.Version = current;
            _ = UpdateChecker.CheckUpdate(this, force);
        }

        bool IUpdateCheck.NotifyNewVersion(Version version)
        {
            var message = string.Format(Vocab.Current.Message_UpdateAvailable, version.ToStringAuto());
            return this.ShowMessage_YesNo(message, MessageBoxImage.Information) is MessageBoxResult.Yes; 
        }

        void IUpdateCheck.NotifyNoUpdate()
        {
            TextBlock_Info.Text = Vocab.Current.Message_NoUpdate;
            Area_UpdateInfo.Open(true);
        }

        private void OnClick_OfficialDatabase(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.ShellOpen(S.Url.BaseUrl);
        }

        private void OnClick_Help(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Area_Help.Open(true);
        }

        private void OnClick_Help_Manual(object sender, RoutedEventArgs e)
        {

        }

        private void OnClick_Help_Update(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            CheckUpdate(true);
        }

        private void OnClick_Info_OK(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Area_UpdateInfo.Close();
        }

        private void Executed_UpdateDatabase(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            this.StartTask(asyncProcess: UpdateDatabase, isAbortable: false);
        }

        private async Task UpdateDatabase(ProgressReporter p, CancellationToken c)
        {
            var database = ViewModel.CardPool;
            var ids = await S.CardPack.GetCardList(database.Packs, p, c);
            await S.Card.UpdateAllCards(ids, database.Cards, database.Packs, p, c);
            database.SaveJson(CardPool.ResourceFilePath);

            await Dispatcher.BeginInvoke(() =>
            {
                TextBlock_Info.Text = ids.Count is 0
                    ? Vocab.Current.Message_NoUpdate
                    : string.Format(Vocab.Current.Message_CardUpdateComplete, ids.Count);
                Area_UpdateInfo.Open(true);
            });
        }

        private bool _regulation_tcg;

        private void Executed_LoadOcgRegulation(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            _regulation_tcg = false;
            this.StartTask(asyncProcess: UpdateRegulation, isAbortable: false);
        }

        private void Executed_LoadTcgRegulation(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            _regulation_tcg = true;
            this.StartTask(asyncProcess: UpdateRegulation, isAbortable: false);
        }

        private async Task UpdateRegulation(ProgressReporter p, CancellationToken c)
        {
            await S.Regulation.Update(ViewModel.Regulation, ViewModel.CardPool.Cards, _regulation_tcg, p, c);
            Unit_Database.OnRegulationLoad();
        }
    }
}