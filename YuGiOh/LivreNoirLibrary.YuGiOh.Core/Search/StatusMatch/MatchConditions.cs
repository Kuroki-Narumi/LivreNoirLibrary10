using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public partial class MatchConditions() : ObservableObjectBase
    {
        public bool Attribute { get; set => SetValue(ref field, value, OnStatusFlagChanged); }
        public bool MonsterType { get; set => SetValue(ref field, value, OnStatusFlagChanged); }
        public bool Level { get; set => SetValue(ref field, value, OnStatusFlagChanged); }
        public bool Atk { get; set => SetValue(ref field, value, OnStatusFlagChanged); }
        public bool Def { get; set => SetValue(ref field, value, OnStatusFlagChanged); }
        public bool AtkDef { get; set => SetValue(ref field, value, OnStatusFlagChanged); }

        public int Count { get; set => SetValue(ref field, value); }
        public int CountMax { get; private set => SetValue(ref field, value); }
        public bool AllowsGreater { get; set => SetValue(ref field, value); }

        public bool ExceptSelf { get; set => SetValue(ref field, value); }
        public bool Candidate_Main { get; set => SetValue(ref field, value); }
        public bool Target_Main { get; set => SetValue(ref field, value); }

        public MatchConditions(MatchConditions source) : this()
        {
            CopyFrom(source);
        }

        public override string ToString() => $"Attr={Attribute}, MT={MonsterType}, Lv={Level}, Atk={Atk}, Def={Def}, AtkDef={AtkDef}, Count={Count}";

        private void OnStatusFlagChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                CountMax++;
                if (Count < CountMax)
                {
                    Count++;
                }
            }
            else
            {
                CountMax--;
                Count = Math.Min(Count, CountMax);
            }
        }

        public IEnumerable<Card> EnumerateMatches(Card card, IEnumerable<Card> targets)
        {
            var attr = Attribute;
            var mtype = MonsterType;
            var level = Level;
            var atk = Atk;
            var def = Def;
            var atkDef = AtkDef;
            var exceptLink = level || def || atkDef;
            var cand_main = Candidate_Main;
            var tgt_main = Target_Main;
            var exceptSelf = ExceptSelf;
            var requiredCount = Count;
            Func<int, int, bool> predicate = AllowsGreater ? (a, b) => a >= b : (a, b) => a == b;
            foreach (var target in targets)
            {
                var ct2 = target.CardType;
                if (!ct2.IsMonster() ||
                    // 「同名カードを除外」かつ対象が同名カード
                    (exceptSelf && target.Name == card.Name) ||
                    // 「メインモンスター」かつ対象がエクストラモンスター
                    (tgt_main && ct2.IsExtraDeck()) ||
                    // 「レベル」「守備力」「攻+守」が1つ以上有効かつ対象がリンクモンスター
                    (exceptLink && ct2.IsLink()) ||
                    // 「攻+守」が有効かつ対象の攻守いずれかが「?」
                    (atkDef && (target.Atk < 0 || target.Def < 0)))
                {
                    continue;
                }
                var count = 0;
                if (attr && target.Attribute == card.Attribute)
                {
                    count++;
                }
                if (mtype && target.MonsterType == card.MonsterType)
                {
                    count++;
                }
                if (level && target.Level == card.Level)
                {
                    count++;
                }
                if (atk && target.Atk == card.Atk)
                {
                    count++;
                }
                if (def && target.Def == card.Def)
                {
                    count++;
                }
                if (atkDef && target.Atk + target.Def == card.Atk + card.Def)
                {
                    count++;
                }
                if (predicate(count, requiredCount))
                {
                    yield return target;
                }
            }
        }

        public void BuildMatchList(List<Card> source, List<MatchCard> targets, Func<MatchCard> factory, ProgressReporter? p = null, CancellationToken c = default)
        {
            p?.ReportInitial("Building match list...");
            targets.Clear();

            var attr = Attribute;
            var mtype = MonsterType;
            var level = Level;
            var atk = Atk;
            var def = Def;
            var atkDef = AtkDef;
            var exceptLink = level || def || atkDef;
            var cand_main = Candidate_Main;
            var tgt_main = Target_Main;
            var exceptSelf = ExceptSelf;
            var requiredCount = Count;
            Func<int, int, bool> predicate = AllowsGreater ? (a, b) => a >= b : (a, b) => a == b;

            var i = 0;
            var sourceSpan = source.AsSpan();
            var maxCount = source.Count;
            foreach (var card in sourceSpan)
            {
                p?.Report($"{++i}/{maxCount}", i, maxCount);
                c.ThrowIfCancellationRequested();

                var ct = card.CardType;
                if (!ct.IsMonster() ||
                    // 「メインモンスター」かつ対象がエクストラモンスター
                    (cand_main && ct.IsExtraDeck()) ||
                    // 「レベル」「守備力」「攻+守」が1つ以上有効かつ対象がリンクモンスター
                    (exceptLink && ct.IsLink()) ||
                    // 「攻+守」が有効かつ対象の攻守いずれかが「?」
                    (atkDef && (card.Atk < 0 || card.Def < 0)))
                {
                    continue;
                }
                var match = 0;
                foreach (var target in sourceSpan)
                {
                    var ct2 = target.CardType;
                    if (!ct2.IsMonster() ||
                        // 「同名カードを除外」かつ対象が同名カード
                        (exceptSelf && target.Name == card.Name) ||
                        // 「メインモンスター」かつ対象がエクストラモンスター
                        (tgt_main && ct2.IsExtraDeck()) ||
                        // 「レベル」「守備力」「攻+守」が1つ以上有効かつ対象がリンクモンスター
                        (exceptLink && ct2.IsLink()) ||
                        // 「攻+守」が有効かつ対象の攻守いずれかが「?」
                        (atkDef && (target.Atk < 0 || target.Def < 0)))
                    {
                        continue;
                    }
                    var count = 0;
                    if (attr && target.Attribute == card.Attribute)
                    {
                        count++;
                    }
                    if (mtype && target.MonsterType == card.MonsterType)
                    {
                        count++;
                    }
                    if (level && target.Level == card.Level)
                    {
                        count++;
                    }
                    if (atk && target.Atk == card.Atk)
                    {
                        count++;
                    }
                    if (def && target.Def == card.Def)
                    {
                        count++;
                    }
                    if (atkDef && target.Atk + target.Def == card.Atk + card.Def)
                    {
                        count++;
                    }
                    if (predicate(count, requiredCount))
                    {
                        match++;
                    }
                }
                if (match > 0)
                {
                    var mc = factory();
                    mc.ThisCard = card;
                    mc.MatchCount = match;
                    targets.Add(mc);
                }
            }
        }
    }
}
