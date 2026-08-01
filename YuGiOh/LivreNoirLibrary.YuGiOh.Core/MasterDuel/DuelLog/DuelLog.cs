using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class DuelLog : ObservableObjectBase
    {
        public const string TagNone = "(none)";
        public const string Trail = "...";

        internal readonly SortedSet<string> _userTags = [];
        internal readonly SortedSet<string> _opponentTags = [];
        internal readonly List<int> _initialHand = [];
        internal readonly List<int> _additionalHand = [];

        [JsonPropertyName(JsonPropertyNames.Date)]
        [JsonConverter(typeof(NoSecondsDateJsonConverter))]
        public DateTime DateTime { get; set => SetValue(ref field, value); } = DateTime.Now;

        [JsonPropertyName(JsonPropertyNames.Log_User)]
        public IEnumerable<string> UserTags { get => _userTags; set => SetTags(_userTags, value, nameof(UserTagText)); }

        [JsonPropertyName(JsonPropertyNames.Log_Opponent)]
        public IEnumerable<string> OpponentTags { get => _opponentTags; set => SetTags(_opponentTags, value, nameof(OpponentTagText)); }

        [JsonPropertyName(JsonPropertyNames.Log_Rank)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Rank Rank { get; set => SetValue(ref field, value); }

        [JsonPropertyName(JsonPropertyNames.Log_Order)]
        public Order Order { get; set => SetValue(ref field, value, [nameof(OrderText)]); }

        [JsonPropertyName(JsonPropertyNames.Log_Result)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Result Result { get; set => SetValue(ref field, value, [nameof(ResultText)]); }

        [JsonPropertyName(JsonPropertyNames.Log_Turn)]
        public int Turn { get; set => SetValue(ref field, value); }

        [JsonPropertyName(JsonPropertyNames.Note)]
        public string Note { get; set => SetValue(ref field, value); } = "";

        [JsonPropertyName(JsonPropertyNames.Log_InitialHand)]
        public IEnumerable<int> InitialHand { get => _initialHand; set => SetHand(_initialHand, value); }

        [JsonPropertyName(JsonPropertyNames.Log_AdditionalHand)]
        public IEnumerable<int> AdditionalHand { get => _additionalHand; set => SetHand(_additionalHand, value); }

        [JsonIgnore]
        public string OrderText => Vocab.GetName(Order);
        [JsonIgnore]
        public string ResultText => Vocab.GetName(Result);
        [JsonIgnore]
        public string UserTagText => GetTagText(_userTags, TagNone);
        [JsonIgnore]
        public string OpponentTagText => GetTagText(_opponentTags, TagNone);

        private void SetTags(SortedSet<string> field, IEnumerable<string> value, string tagTextPropName, [CallerMemberName]string propName = "")
        {
            field.Clear();
            field.UnionWith(value);
            this.NotifyPropertyChanged(propName);
            this.NotifyPropertyChanged(tagTextPropName);
        }

        private void SetHand(List<int> field, IEnumerable<int> value, [CallerMemberName]string propName = "")
        {
            field.Clear();
            field.AddRange(value);
            this.NotifyPropertyChanged(propName);
        }

        public void RenameTag(string? oldName, string? newName)
        {
            if (string.IsNullOrEmpty(oldName))
            {
                return;
            }
            if (_userTags.Remove(oldName))
            {
                if (!string.IsNullOrEmpty(newName))
                {
                    _userTags.Add(newName);
                }
                this.NotifyPropertyChanged(nameof(UserTags));
                this.NotifyPropertyChanged(nameof(UserTagText));
            }
            if (_opponentTags.Remove(oldName))
            {
                if (!string.IsNullOrEmpty(newName))
                {
                    _opponentTags.Add(newName);
                }
                this.NotifyPropertyChanged(nameof(OpponentTags));
                this.NotifyPropertyChanged(nameof(OpponentTagText));
            }
        }

        public static string GetTagText(IEnumerable<string> tags, string ifnone, string separator = ", ", int lengthLimit = 512)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(lengthLimit, 1);
            var buffer = StringBuffer.Get();
            var count = 0;
            var index = 0;
            foreach (var tag in tags)
            {
                if (count is not 0)
                {
                    separator.CopyTo(buffer[index..]);
                    index += separator.Length;
                }
                tag.CopyTo(buffer[index..]);
                index += tag.Length;
                count++;
                if (index >= lengthLimit)
                {
                    Trail.CopyTo(buffer[lengthLimit..]);
                    return new(buffer[..(lengthLimit + Trail.Length)]);
                }
            }
            return count is 0 ? ifnone : new(buffer[..index]);
        }

        public void CopyFrom(DuelLog source)
        {
            DateTime = source.DateTime;
            UserTags = source.UserTags;
            OpponentTags = source.OpponentTags;
            Rank = source.Rank;
            Order = source.Order;
            Result = source.Result;
            Turn = source.Turn;
            InitialHand = source.InitialHand;
            AdditionalHand = source.AdditionalHand;
            Note = source.Note;
        }
    }
}
