using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class DuelLog : ObservableObjectBase
    {
        public const string TagNone = "(none)";
        public const string Trail = "...";

        [JsonPropertyName(JsonPropertyNames.Date)]
        [JsonConverter(typeof(NoSecondsDateJsonConverter))]
        public DateTime DateTime { get; set => SetValue(ref field, value); }

        [JsonPropertyName(JsonPropertyNames.Log_User)]
        public SortedSet<string> UserTags { get; set => SetValue(ref field, value); } = [];

        [JsonPropertyName(JsonPropertyNames.Log_Opponent)]
        public SortedSet<string> OpponentTags { get; set => SetValue(ref field, value); } = [];

        [JsonPropertyName(JsonPropertyNames.Log_Rank)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Rank Rank { get; set => SetValue(ref field, value); }
        [JsonPropertyName(JsonPropertyNames.Log_Order)]
        public Order Order { get; set => SetValue(ref field, value); }
        [JsonPropertyName(JsonPropertyNames.Log_Result)]
        public Result Result { get; set => SetValue(ref field, value); }
        [JsonPropertyName(JsonPropertyNames.Log_Turn)]
        public int Turn { get; set => SetValue(ref field, value); }

        [JsonIgnore]
        public string UserTagText => GetTagText(UserTags, TagNone);
        [JsonIgnore]
        public string OpponentTagText => GetTagText(OpponentTags, TagNone);

        [JsonPropertyName(JsonPropertyNames.Log_InitialHand)]
        public ObservableList<int> InitialHand { get; set => SetValue(ref field, value); } = [];
        [JsonPropertyName(JsonPropertyNames.Log_AdditionalHand)]
        public ObservableList<int> AdditionalHand { get; set => SetValue(ref field, value); } = [];

        [JsonPropertyName(JsonPropertyNames.Note)]
        public string Note { get; set => SetValue(ref field, value); } = "";

        public void RenameTag(string oldName, string newName)
        {
            if (UserTags.Remove(oldName))
            {
                UserTags.Add(newName);
            }
            if (OpponentTags.Remove(oldName))
            {
                OpponentTags.Add(newName);
            }
            this.NotifyPropertyChanged(nameof(UserTags));
            this.NotifyPropertyChanged(nameof(UserTagText));
            this.NotifyPropertyChanged(nameof(OpponentTags));
            this.NotifyPropertyChanged(nameof(OpponentTagText));
        }

        public void SetUserTags(IEnumerable<string> source) => SetTags(UserTags, source, nameof(UserTags), nameof(UserTagText));
        public void SetOpponentTags(IEnumerable<string> source) => SetTags(OpponentTags, source, nameof(OpponentTags), nameof(OpponentTagText));

        private void SetTags(SortedSet<string> tags, IEnumerable<string> source, params ReadOnlySpan<string> propertyNames)
        {
            tags.Clear();
            tags.UnionWith(source);
            foreach (var prop in propertyNames)
            {
                this.NotifyPropertyChanged(prop);
            }
        }

        public static string GetTagText(IEnumerable<string> tags, string ifnone, ReadOnlySpan<char> separator=",", int lengthLimit = 512)
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
    }
}
