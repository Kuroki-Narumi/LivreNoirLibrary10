using LivreNoirLibrary.YuGiOh.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.YuGiOh.Search;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class DuelLogSearchConditions : ObservableObjectBase
    {
        public static DateTime DefaultDateStart { get; } = new(2022, 1, 18);
        public static DateTime DefaultDateEnd { get; } = new(DateTime.Now.Year + 1, 12, 31, 23, 59, 59);
        public static NumberRange DefaultTurn { get; } = new(0, 99, true, false);

        [JsonConverter(typeof(NoSecondsDateJsonConverter))]
        public DateTime DateStart { get; set => SetValue(ref field, value); } = DefaultDateStart;
        [JsonIgnore]
        public DateTime DateEnd { get; set => SetValue(ref field, value); } = DefaultDateEnd;

        public SortedSet<string> UserTags { get; set; } = [];
        public MatchType UserTagMatchType { get; set => SetValue(ref field, value); }

        public SortedSet<string> OpponentTags { get; set; } = [];
        public MatchType OpponentTagMatchType { get; set => SetValue(ref field, value); }

        public HashSet<Rank> Ranks { get; set; } = [];
        public HashSet<Order> Orders { get; set; } = [];
        public HashSet<Result> Results { get; set; } = [];
        public NumberRange Turn { get; set; } = new(DefaultTurn);

        public void Clear()
        {
            DateStart = DefaultDateStart;
            DateEnd = DefaultDateEnd;
            UserTags.Clear();
            UserTagMatchType = 0;
            OpponentTags.Clear();
            OpponentTagMatchType = 0;
            Ranks.Clear();
            Orders.Clear();
            Results.Clear();
            Turn.CopyFrom(DefaultTurn);
        }

        public void SetUserTags(IEnumerable<string> value)
        {
            UserTags.Clear();
            UserTags.UnionWith(value);
        }

        public void SetOpponentTags(IEnumerable<string> value)
        {
            OpponentTags.Clear();
            OpponentTags.UnionWith(value);
        }

        public bool IsMatch(DuelLog log)
        {
            var dt = log.DateTime;
            if (dt < DateStart || dt > DateEnd) return false;
            if (SearchUtils.NotMatch(Ranks, log.Rank)) return false;
            if (SearchUtils.NotMatch(Orders, log.Order)) return false;
            if (SearchUtils.NotMatch(Results, log.Result)) return false;
            if (SearchUtils.NotMatch(Turn, log.Turn)) return false;
            if (!SearchUtils.IsMatch(log.UserTags, UserTags, UserTagMatchType)) return false;
            if (!SearchUtils.IsMatch(log.OpponentTags, OpponentTags, OpponentTagMatchType)) return false;
            return true;
        }
    }
}
