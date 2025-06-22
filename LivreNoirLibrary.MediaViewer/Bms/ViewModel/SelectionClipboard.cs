using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Media.Bms;
using static LivreNoirLibrary.Media.Bms.BmsUtils;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public static partial class SelectionClipboard
    {
        public const string DataObjectType = "LNL.Bms Selection";
        public const string ObjectType_Bmse = "BMSE ClipBoard Object Data Format";

        private static readonly Lock _lock = new();
        private static readonly MemoryStream _stream = new(32768);

        public static bool Exists()
        {
            try
            {
                return Clipboard.ContainsData(DataObjectType) || (Clipboard.ContainsText() && Clipboard.GetText().StartsWith(ObjectType_Bmse));
            }
            catch (COMException)
            {
                return false;
            }
        }

        public static bool Set(Selection selection, BaseData? data)
        {
            lock (_lock)
            {
                try
                {
                    var ms = _stream;
                    ms.SetLength(0);
                    using BinaryWriter writer = new(ms, Encoding.UTF8, true);
                    selection.Dump(writer);
                    var buffer = ms.ToArray();
                    DataObject obj = new(DataObjectType, buffer);
                    if (data is not null)
                    {
                        obj.SetText(GetBmseData(data, selection));
                    }
                    Clipboard.SetDataObject(obj);
                    return true;
                }
                catch (COMException)
                {
                    return false;
                }
            }
        }

        public static bool Get(BaseData? data, [MaybeNullWhen(false)]out Selection selection)
        {
            try
            {
                if (Clipboard.GetDataObject() is DataObject obj)
                {
                    if (obj.GetData(DataObjectType) is byte[] buffer)
                    {
                        using MemoryStream ms = new(buffer);
                        using BinaryReader reader = new(ms, Encoding.UTF8, true);
                        selection = Selection.Load(reader);
                        return true;
                    }
                    else if (data is not null)
                    {
                        return FromBmseData(obj.GetText(), data, out selection);
                    }
                }
            }
            catch (COMException)
            {
            }
            selection = null;
            return false;
        }

        public static string GetBmseData(BaseData data, Selection selection)
        {
            StringBuilder sb = new();
            sb.AppendLine(ObjectType_Bmse);
            static string ToBased(int val) => BasedIndex.ToBased(val, 36, 2);
            var offset = selection.GetFirstBarHead();
            var lnobj = data.LnObj;
            foreach (var (pos, item) in selection)
            {
                var note = item.Note;
                var lane = note.Lane;
                if (note.IsBgm())
                {
                    // bgm
                    sb.Append('1');
                    // bgm lane index
                    sb.Append(ToBased(1 - lane));
                    // attr normal
                    sb.Append('0');
                }
                else
                {
                    // not bgm
                    sb.Append('0');
                    if (note.IsKey())
                    {
                        // lane index
                        sb.Append(ToBased((int)NoteType.Normal.GetChannel(lane)));
                        if (note.IsInvisible())
                        {
                            sb.Append('1'); // attr invisible
                        }
                        else if (note.IsMine())
                        {
                            sb.Append('3'); // attr mine?
                        }
                        else
                        {
                            sb.Append('0'); // attr normal
                        }
                    }
                    else
                    {
                        // channel index
                        sb.Append(ToBased((int)note.Type.GetChannel(lane)));
                        // attr normal
                        sb.Append('0');
                    }
                }
                // position
                var p = (uint)ConvertBackStopLength(pos - offset);
                sb.Append($"{p:D7}");
                // id
                if (note.IsStop())
                {
                    sb.Append(ConvertBackStopLength(note.Value));
                }
                if (note.IsDecimal() || note.IsRational())
                {
                    sb.Append((decimal)note.Value);
                }
                else if (note.IsLongEnd())
                {
                    sb.Append(lnobj);
                }
                else
                {
                    sb.Append(note.Id);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static bool FromBmseData(string text, BaseData data, [MaybeNullWhen(false)]out Selection selection)
        {
            var list = text.SplitLines();
            var regex = Regex_BmseClipboard;
            var lnobj = data.LnObj;
            HashSet<int> lastNotes = [];
            if (list.Length is > 1 && list[0] is ObjectType_Bmse)
            {
                selection = [];
                List<BmseClipboadData> dataList = [];
                foreach (var line in list[1..])
                {
                    var match = regex.Match(line);
                    if (match.Success)
                    {
                        dataList.Add(new(match));
                    }
                }
                dataList.Sort();
                foreach (var item in CollectionsMarshal.AsSpan(dataList))
                {
                    var position = item.Position;
                    var ch = item.Channel;
                    var type = ch.GetNoteType();
                    var lane = ch.GetLane();
                    var attr = item.Attr;
                    var value = item.Value;
                    Note note = lane switch
                    {
                        TempoLane => Note.Tempo(value),
                        StopLane => Note.Stop(GetStopLength(value)),
                        ScrollLane => Note.Scroll(value),
                        SpeedLane => Note.Speed(value),
                        _ => new(type, lane, (int)value)
                    };
                    if (item.IsBgm)
                    {
                        note.Type = NoteType.Normal;
                        note.Lane = 1 - (int)ch;
                    }
                    else if (attr is '1')
                    {
                        note.Type = NoteType.Invisible;
                    }
                    else if (attr is '2')
                    {
                        if (lastNotes.Remove(lane))
                        {
                            note.Type = NoteType.LongEnd;
                        }
                        else
                        {
                            note.Type = NoteType.Normal;
                            lastNotes.Add(lane);
                        }
                    }
                    else if (note.IsKey() && note.Id == lnobj)
                    {
                        note.Type = NoteType.LongEnd;
                        note.Id = 0;
                    }
                    selection.Add(new(0, position), position, note);
                }
                return true;
            }
            selection = null;
            return false;
        }

        [GeneratedRegex(@"(?<head>\d)(?<ch>[0-9a-zA-Z]{2})(?<attr>\d)(?<pos>\d{7})(?<value>.+)")]
        private static partial Regex Regex_BmseClipboard { get; }

        private readonly struct BmseClipboadData(Match match) : IComparable<BmseClipboadData>
        {
            public bool IsBgm { get; } = match.Groups["head"].Value is "1";
            public Channel Channel { get; } = (Channel)match.Groups["ch"].Value.ParseToInt(36);
            public char Attr { get; } = match.Groups["attr"].Value[0];
            public Rational Position { get; } = GetStopLength(Rational.Parse(match.Groups["pos"].Value));
            public Rational Value { get; } = Rational.Parse(match.Groups["value"].Value);

            public int CompareTo(BmseClipboadData other) => Position.CompareTo(other.Position);
        }
    }
}
