using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.YuGiOh;
using LivreNoirLibrary.Windows.YuGiOh.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using S = LivreNoirLibrary.YuGiOh.Scraping;

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IUpdateCheck, IProgressReporter
    {
        public static MainViewModel ViewModel => MainViewModel.Instance;

        public const string ManualUrl = "https://livrenoir.web.fc2.com/apps/yugioh/";
        public const string HistoryUrl = $"{ManualUrl}history.html";

        bool IUpdateCheck.CheckUpdate { get => ViewModel.CheckUpdate; set => ViewModel.CheckUpdate = value; }
        string IUpdateCheck.VersionUrl => "https://dl.dropboxusercontent.com/scl/fi/nek8vhpd6gi9pohgetu6r/version.json?rlkey=truzpczvnqniqgwx3rvc5cuaq";
        string IUpdateCheck.UpdaterLocation => MainViewModel.AppName;

        UIElement IProgressReporter.MainElement => Grid_Main;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get; set; }
        Dispatcher IProgressReporter.Dispatcher => Dispatcher;

        private long _t0;

        public MainWindow()
        {
            _t0 = Stopwatch.GetTimestamp();
            DataContext = ViewModel;
            InitializeResources();
            var t0 = Stopwatch.GetTimestamp();
            InitializeComponent();
            Console.WriteLine($"MainWindow: InitializeComponent in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");
            this.SetDispatcher(CheckUpdate_Auto);

            YgoEvents.AddRequestOpenCardUrlHandler(this, OnRequestOpenCardUrl);
            YgoEvents.AddRequestOpenPackUrlHandler(this, OnRequestOpenPackUrl);

            this.RegisterCommand(YgoCommands.UpdateDatabase, Executed_UpdateDatabase);
            this.RegisterCommand(YgoCommands.LoadOcgRegulation, Executed_LoadOcgRegulation);
            this.RegisterCommand(YgoCommands.LoadTcgRegulation, Executed_LoadTcgRegulation);

            this.RegisterCommand(YgoCommands.DetachCardInfo, OnExecuted_DetachCardInfo, CanExecute_DetachCardInfo);
            this.RegisterCommand(YgoCommands.DetachCardList, OnExecuted_DetachCardList);
            this.RegisterCommand(YgoCommands.CardLink, CardInfoWindow_CardLink);
            this.RegisterCommand(YgoCommands.PackLink, CardInfoWindow_PackLink);
            this.RegisterCommand(YgoCommands.RelatedText, CardInfoWindow_RelatedText);
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
            ViewModel.EditingDuelLog = Unit_DuelLog.DuelLogEditor.EditingLog;
            MainViewModel.Save();
            base.OnClosing(e);
        }

        private void CheckUpdate_Auto()
        {
            if (ViewModel.EditingDuelLog is { } log)
            {
                Unit_DuelLog.DuelLogEditor.UpdateEditingLog(log);
            }
            CheckUpdate(false);
            Console.WriteLine($"MainWindow: Initialized in {Stopwatch.GetElapsedTime(_t0).TotalMilliseconds}ms");
        }

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
            e.Handled = true;
            this.ShellOpen(ManualUrl);
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
            if (ViewModel.CardPool.LastUpdate + TimeSpan.FromHours(1) < DateTime.Now)
            {
                this.StartTask(asyncProcess: UpdateDatabase, isAbortable: false);
            }
        }

        private async Task UpdateDatabase(ProgressReporter p, CancellationToken c)
        {
            var database = ViewModel.CardPool;
            var ids = await S.CardPack.GetCardList(database.Packs, p, c);
            await S.Card.UpdateAllCards(ids, database.Cards, database.Packs, p, c);
            database.LastUpdate = DateTime.Now;
            Json.Save(CardPool.ResourceFilePath, database);

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

        private void OnRequestOpenCardUrl(object sender, CardLinkClickedEventArgs e)
        {
            e.Handled = true;
            this.OpenUrl_Card(e.Id, e.IsTcg);
        }

        private void OnRequestOpenPackUrl(object sender, RoutedEventArgs<string> e)
        {
            e.Handled = true;
            this.OpenUrl_Pack(e.Value);
        }

        private void OnExecuted_DetachCardList(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            OpenCardListWindow();
        }

        private void CanExecute_DetachCardInfo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Card.TryGetCard(e.Parameter, ViewModel.CardPool.Cards, out _);
        }

        private void OnExecuted_DetachCardInfo(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (Card.TryGetCard(e.Parameter, ViewModel.CardPool.Cards, out var card))
            {
                var id = card.Id;
                if (_windowCache.TryGetValue(id, out var window))
                {
                    window.Activate();
                }
                else
                {
                    window = new(card, this);
                    window.RegisterCommand(YgoCommands.CardLink, CardInfoWindow_CardLink);
                    window.RegisterCommand(YgoCommands.PackLink, CardInfoWindow_PackLink);
                    window.RegisterCommand(YgoCommands.RelatedText, CardInfoWindow_RelatedText);
                    window.Closed += CardInfoWindow_Closed;
                    _windowCache.Add(id, window);
                    window.Show();
                }
            }
        }

        private readonly Dictionary<int, CardInfoWindow> _windowCache = [];

        private void CardInfoWindow_Closed(object? sender, EventArgs e)
        {
            if (sender is CardInfoWindow w)
            {
                _windowCache.Remove(w.ReferenceId);
            }
        }

        public void OpenCardListWindow()
        {
            CardListWindow window = new(this, ViewModel.CardPool.Cards);
            window.RegisterCommand(YgoCommands.PackLink, CardInfoWindow_PackLink);
            window.Show();
        }

        private void CardInfoWindow_CardLink(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is int id)
            {
                Activate();
                Tab_Database.IsSelected = true;
                Unit_Database.SelectCard(id);
            }
        }

        private void CardInfoWindow_PackLink(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is string pid)
            {
                Activate();
                Tab_Database.IsSelected = true;
                Unit_Database.SelectPack(pid);
            }
        }

        private void CardInfoWindow_RelatedText(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is string text)
            {
                Activate();
                Tab_Database.IsSelected = true;
                Unit_Database.SearchCard(text);
            }
        }
    }
}