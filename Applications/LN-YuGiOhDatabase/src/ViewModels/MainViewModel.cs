using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.YuGiOh.MasterDuel;
using LivreNoirLibrary.Windows.YuGiOh.Controls;
using LivreNoirLibrary.YuGiOh.Search;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using System.Windows;
using System.IO;
using LivreNoirLibrary.YuGiOh.Inspect;

namespace LivreNoir.YuGiOhDatabase
{
    public class MainViewModel : AppSettingsBase
    {
        public const string AppName = "YuGiOh";
        public const string Filename = "SettingsV2";

        public static MainViewModel Instance { get; } = Load<MainViewModel>(AppName, Filename);
        public static void Save() => Instance.SaveInstance(AppName, Filename);

        public double WindowLeft { get; set => SetValue(ref field, value); } = double.NaN;
        public double WindowTop { get; set => SetValue(ref field, value); } = double.NaN;
        public Version? Version { get; set => SetValue(ref field, value); }
        public bool CheckUpdate { get; set => SetValue(ref field, value); } = true;

        public List<CardSearchConditionsPreset> CardSearchPresets { get; set => SetValue(ref field, value); } = [];
        public List<CardSortOptionsPreset> CardSortPresets { get; set => SetValue(ref field, value); } = [];
        public Dictionary<int, int> CardNameLBPositions { get; set => SetValue(ref field, value); } = [];
        public HandTestParams HandTestParams { get; set => SetValue(ref field, value); } = new();
        public DuelLog? LastEditedDuelLog { get; set => SetValue(ref field, value); }
        public DuelLogSearchConditions? DuelLogSearchConditions { get; set => SetValue(ref field, value); }

        [JsonIgnore]
        public CardPool CardPool { get; } = new();
        [JsonIgnore]
        public Regulation Regulation { get; } = new();

        [JsonIgnore]
        public CardList OriginalCards { get; } = [];
        [JsonIgnore]
        public Deck Deck { get; } = new();

        [JsonIgnore]
        public HandConditionsCollection HandInspectConditions { get; } = new();

        [JsonIgnore]
        public SortedCardList TrapMonsters { get; } = [];
        [JsonIgnore]
        public TokenCollection Tokens { get; } = new();

        [JsonIgnore]
        public ObservableList<DuelLog> DuelLogs { get; } = [];
        [JsonIgnore]
        public ObservableList<DeckTag> DeckTags { get; } = [];

        [JsonIgnore]
        public UnitDatabaseViewModel Database { get; } = new();

        public MainViewModel()
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            CardPool.LoadFile(CardPool.ResourceFilePath);
            var provider = CardPool.Cards;
            Regulation.LoadFile(GetFilePath(AppName, nameof(Regulation)), provider);
            if (Json.TryOpen<Card[]>(GetFilePath(AppName, nameof(OriginalCards)), out var ary1))
            {
                OriginalCards.AddRange(ary1);
            }

            Deck.LoadFile(GetFilePath(AppName, nameof(Deck)), provider);
            var path = GetFilePath(AppName, nameof(HandInspectConditions));
            if (File.Exists(path))
            {
                HandInspectConditions.LoadFile(path, provider);
            }
            else
            {
                HandInspectConditions.LoadFile(GetFilePath(AppName, "InspectCondition"), provider);
            }

            if (Json.TryOpen<DuelLog[]>(GetFilePath(AppName, nameof(DuelLogs)), out var ary2))
            {
                DuelLogs.AddRange(ary2);
            }
            if (Json.TryOpen<DeckTag[]>(GetFilePath(AppName, nameof(DeckTags)), out var ary3))
            {
                DeckTags.AddRange(ary3);
            }

            CardSearchWindow.LoadPreset(CardSearchPresets);
            CardSortWindow.LoadPreset(CardSortPresets);
        }

        protected override void OnSave()
        {
            base.OnSave();
            //Json.Save(GetFilePath(AppName, nameof(Regulation)), Regulation);
            //Json.Save(GetFilePath(AppName, nameof(OriginalCards)), OriginalCards);
            //Json.Save(GetFilePath(AppName, nameof(Deck)), Deck);
            Json.Save(GetFilePath(AppName, nameof(LivreNoirLibrary.YuGiOh.Serializable.HandInspectConditions<>)), HandInspectConditions);
            //Json.Save(GetFilePath(AppName, nameof(DuelLogs)), DuelLogs);
            //Json.Save(GetFilePath(AppName, nameof(DeckTags)), DeckTags);

            CardSearchWindow.SavePreset(CardSearchPresets);
            CardSortWindow.SavePreset(CardSortPresets);
        }
    }
}
