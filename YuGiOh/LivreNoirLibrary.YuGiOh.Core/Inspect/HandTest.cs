using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace LivreNoirLibrary.YuGiOh.Inspect
{
    using CondsDic = Dictionary<int, List<HandConditions>>;

    public class HandTest
    {
        private static readonly CondsDic _conditionGroups = [];
        private static readonly List<int> _deckSource = [];
        private static readonly List<DrawSourceType> _dsSource = [];
        private static readonly List<int> _handBuffer = [];
        private static readonly List<HandConditions> _matchBuffer = [];

        public static void Run(IIdEnumerable deck, HandConditionsCollection conditions, HandTestParams @params, HandTestResult result, ProgressReporter? p = null, CancellationToken c = default)
        {
            var groups = _conditionGroups;
            var deckSource = _deckSource;
            var dsSource = _dsSource;
            var handBuffer = _handBuffer;
            var matchBuffer = _matchBuffer;
            try
            {
                p?.ReportInitial("Hand Test", "initializing...");

                conditions.Prepare(groups);
                result.Clear();

                c.ThrowIfCancellationRequested();

                deckSource.AddRange(deck.IdEnumerable);
                deckSource.Sort();
                var deckSpan = deckSource.AsSpan();
                CheckDrawSource(dsSource, deckSpan, @params);
                var handCount = Math.Min(deckSpan.Length, @params.NumberOfHand);
                var dsSpan = dsSource.AsSpan();

                HandTestState state = new(deckSource);
                var repeat = @params.RepeatCount;
                for (var i = 0; i < repeat; i++)
                {
                    if ((i % 1000) is 0)
                    {
                        p?.Report($"{i} / {repeat}", i, repeat);
                    }
                    c.ThrowIfCancellationRequested();

                    // デッキをシャッフル
                    deckSpan.ShuffleItems();
                    // セットアップ
                    state.Setup(handCount, dsSpan);

                    Judge(state, groups, handBuffer, matchBuffer);

                    foreach (var cond in state.Matched.AsSpan())
                    {
                        cond.Count++;
                    }
                    result.AddValue(state.Value1, state.Value2);

                    state.Clear();
                }

                p?.Report("finalizing...", repeat, repeat);
                result.EndInit(conditions);
            }
            finally
            {
                groups.Clear();
                deckSource.Clear();
                dsSource.Clear();
                handBuffer.Clear();
                matchBuffer.Clear();
            }
        }

        private static readonly HashSet<DrawSourceType> _containingDs = [];
        private static void CheckDrawSource(List<DrawSourceType> ds, ReadOnlySpan<int> deck, HandTestParams @params)
        {
            var generic = SpecialCards.DrawSource;
            var named = SpecialCards.NamedDrawSource;
            var containing = _containingDs;
            containing.Clear();
            foreach (var id in deck)
            {
                if (generic.Contains(id))
                {
                    containing.Add(DrawSourceType.Other);
                }
                else if (named.Contains(id))
                {
                    containing.Add((DrawSourceType)id);
                }
            }
            foreach (var type in @params.DrawSourcePriority.AsSpan())
            {
                if (containing.Contains(type))
                {
                    ds.Add(type);
                }
            }
        }

        private static bool CalculateValue(HandTestState state, CondsDic conds, List<int> handBuffer, List<HandConditions> matchBuffer)
        {
            try
            {
                double v1 = 0, v2 = 0;
                var hand = state.Hand.AsSpan();
                foreach (var (_, list) in conds)
                {
                    foreach (var item in list.AsSpan())
                    {
                        if (item.IsMatch(hand, handBuffer))
                        {
                            v1 += item.Value1;
                            v2 += item.Value2;
                            matchBuffer.Add(item);
                            break;
                        }
                    }
                }
                return state.Update(v1, v2, matchBuffer);
            }
            finally
            {
                matchBuffer.Clear();
            }
        }

        private static bool Judge(HandTestState state, CondsDic conds, List<int> handBuffer, List<HandConditions> matchBuffer)
        {
            // まずは引いた手札そのままで評価
            var improved = CalculateValue(state, conds, handBuffer, matchBuffer);

            var ds = state.DrawSource;
            var count = ds.Count;
            var funcs = UpdateFuncs;
            // ドローソースの確認
            while (count > 0)
            {
                foreach (var type in ds.AsSpan())
                {
                    if (funcs.TryGetValue(type, out var func) && func(state, conds, handBuffer, matchBuffer))
                    {
                        break;
                    }
                }
                if (count == ds.Count)
                {
                    break;
                }
                count = ds.Count;
            }

            return improved;
        }

        private delegate bool UpdateFunc(HandTestState state, CondsDic conds, List<int> handBuffer, List<HandConditions> matchBuffer);
        private static Dictionary<DrawSourceType, UpdateFunc> UpdateFuncs { get; } = new()
        {
            [DrawSourceType.Other] = ApplyDrawSource,
            [DrawSourceType.GoKen] = ApplyGoKen,
            [DrawSourceType.GoDon] = ApplyGoDon,
            [DrawSourceType.GoKin] = ApplyGoKin,
            [DrawSourceType.KinKen] = ApplyKinKen,
        };

        private static void RemoveMain1(List<int> hand) => hand.RemoveAll(SpecialCards.Main1.Contains);

        private static bool ApplyDrawSource(HandTestState state, CondsDic conds, List<int> handBuffer, List<HandConditions> matchBuffer)
        {
            var hand = state.Hand;
            var ds = state.DrawSource;
            var applied = false;
            while (state.DeckRemain > 0)
            {
                var modified = false;
                foreach (var id in SpecialCards.DrawSource)
                {
                    var index = hand.IndexOf(id);
                    if (index >= 0)
                    {
                        hand.RemoveAt(index);
                        state.Draw(1);
                        ds.Remove(DrawSourceType.GoKin);
                        ds.Remove(DrawSourceType.KinKen);
                        RemoveMain1(hand);
                        modified = true;
                        applied = true;
                    }
                }
                if (!modified)
                {
                    break;
                }
            }
            if (applied)
            {
                Judge(state, conds, handBuffer, matchBuffer);
                return true;
            }
            return false;
        }

        private static bool ApplyGoDon(HandTestState state, CondsDic conds, List<int> handBuffer, List<HandConditions> matchBuffer)
        {
            var hand = state.Hand;
            var index = hand.IndexOf(SpecialCards.GoDon);
            if (state.DeckRemain >= 12 && index >= 0)
            {
                hand.RemoveAt(index);
                state.DeckIndex += 10;
                state.Draw(2);
                var ds = state.DrawSource;
                ds.Remove(DrawSourceType.GoDon);
                ds.Remove(DrawSourceType.GoKin);
                ds.Remove(DrawSourceType.KinKen);
                RemoveMain1(hand);
                Judge(state, conds, handBuffer, matchBuffer);
                return true;
            }
            return false;
        }

        private static bool ApplyGoKin(HandTestState state, CondsDic conds, List<int> handBuffer, List<HandConditions> matchBuffer)
        {
            var hand = state.Hand;
            var index = hand.IndexOf(SpecialCards.GoKin);
            if (state.DeckRemain >= 2 && index >= 0)
            {
                hand.RemoveAt(index);
                state.Draw(2);
                var ds = state.DrawSource;
                ds.Remove(DrawSourceType.Other);
                ds.Remove(DrawSourceType.GoDon);
                ds.Remove(DrawSourceType.GoKin);
                ds.Remove(DrawSourceType.KinKen);
                RemoveMain1(hand);
                Judge(state, conds, handBuffer, matchBuffer);
                return true;
            }
            return false;
        }

        private static bool ApplyKen(int count, HandTestState state, CondsDic conds, List<int> handBuffer, List<HandConditions> matchBuffer)
        {
            var hand = state.Hand;
            var deck = state.Deck;
            RemoveMain1(hand);
            var nextCards = (stackalloc int[count]);
            // デッキの上から指定された枚数めくる
            deck.AsSpan(state.DeckIndex, count).CopyTo(nextCards);
            // めくったカードは破棄する(デッキの一番下に戻す=破棄とみなす)
            state.DeckIndex += count;
            var main1 = SpecialCards.Main1;
            HandTestState? applied = null;
            foreach (var id in nextCards)
            {
                if (!main1.Contains(id))
                {
                    var newState = state.Clone();
                    newState.Hand.Add(id);
                    if (Judge(newState, conds, handBuffer, matchBuffer))
                    {
                        applied = newState;
                        state.Update(newState.Value1, newState.Value2, newState.Matched);
                    }
                }
            }
            if (applied is not null)
            {
                state.CopyFrom(applied);
            }
            return true;
        }

        private static bool ApplyGoKen(HandTestState state, CondsDic conds, List<int> handBuffer, List<HandConditions> matchBuffer)
        {
            var hand = state.Hand;
            var index = hand.IndexOf(SpecialCards.GoKen);
            if (state.DeckRemain >= 3 && index >= 0)
            {
                hand.RemoveAt(index);
                var dc = state.DrawSource;
                dc.Remove(DrawSourceType.GoKen);
                return ApplyKen(3, state, conds, handBuffer, matchBuffer);
            }
            return false;
        }

        private static bool ApplyKinKen(HandTestState state, CondsDic conds, List<int> handBuffer, List<HandConditions> matchBuffer)
        {
            var hand = state.Hand;
            var index = hand.IndexOf(SpecialCards.KinKen);
            if (state.DeckRemain >= 6 && index >= 0)
            {
                hand.RemoveAt(index);
                var dc = state.DrawSource;
                dc.Remove(DrawSourceType.Other);
                dc.Remove(DrawSourceType.GoDon);
                dc.Remove(DrawSourceType.KinKen);
                return ApplyKen(6, state, conds, handBuffer, matchBuffer);
            }
            return false;
        }
    }
}
