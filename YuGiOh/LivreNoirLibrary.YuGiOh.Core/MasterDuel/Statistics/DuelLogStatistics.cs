using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class DuelLogStatistics : IObservableObject
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableList<DuelLog> Logs { get; } = [];
        public OpponentStatisticsCollection DeckTagSet { get; } = [];
        public OpponentStatisticsCollection DeckTagSingle { get; } = [];
        public HandStatisticsCollection InitialHand { get; } = [];
        public HandStatisticsCollection TotalHand { get; } = [];

        private readonly HashSet<int> _handBuffer = [];

        public void BeginInit()
        {
            Logs.ClearWithoutNotify();
            DeckTagSet.Clear();
            DeckTagSingle.Clear();
            InitialHand.Clear();
            TotalHand.Clear();
        }

        public void Update(ICardProvider? cardProvider, IEnumerable<DuelLog> logSource, DuelLogSearchConditions cond)
        {
            var logs = Logs;
            var tags = DeckTagSet;
            var singleTags = DeckTagSingle;
            var initialHand = InitialHand;
            var totalHand = TotalHand;
            var handBuffer = _handBuffer;
            var count = 0;
            foreach (var log in logSource)
            {
                if (cond.IsMatch(log))
                {
                    count++;
                    logs.AddWithoutNotify(log);
                    tags.Total.Append(log);
                    singleTags.Total.Append(log);
                    tags.Append(log.OpponentTagText, log);
                    foreach (var tag in log._opponentTags)
                    {
                        singleTags.Append(tag, log);
                    }
                    AppendHands(log, cardProvider, log._initialHand, handBuffer, initialHand, totalHand);
                    AppendHands(log, cardProvider, log._additionalHand, handBuffer, totalHand);
                    handBuffer.Clear();
                }
            }

            tags.UpdateRatio(count);
            singleTags.UpdateRatio(count);
            initialHand.UpdateRatio(count);
            totalHand.UpdateRatio(count);

            static void AppendHands(DuelLog log, ICardProvider? provider, List<int> ids, HashSet<int> handBuffer, params ReadOnlySpan<HandStatisticsCollection> targets)
            {
                foreach (var id in ids.AsSpan())
                {
                    if (handBuffer.Add(id) && Card.TryGetCard(id, provider, out var card))
                    {
                        foreach (var target in targets)
                        {
                            target.Append(card, log);
                        }
                    }
                }
            }
        }

        public void EndInit()
        {
            Logs.NotifyCollectionReset();
            DeckTagSet.NotifyCollectionReset();
            DeckTagSingle.NotifyCollectionReset();
            InitialHand.NotifyCollectionReset();
            TotalHand.NotifyCollectionReset();
        }

        public void RaisePropertyChanged(object sender, PropertyChangedEventArgs e) => PropertyChanged?.Invoke(sender, e);
    }
}
