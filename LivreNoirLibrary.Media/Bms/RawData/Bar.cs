using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms.RawData
{
    public class Bar
    {
        public decimal Length { get; set; } = 1;
        public List<ChannelData> Channels { get; set; } = [];
        public List<ChannelData> Bgms { get; set; } = [];

        public bool IsEmpty()
        {
            if (Length != 1)
            {
                return false;
            }
            foreach (var line in CollectionsMarshal.AsSpan(Channels))
            {
                if (!line.IsEmpty())
                {
                    return false;
                }
            }
            foreach (var line in CollectionsMarshal.AsSpan(Bgms))
            {
                if (!line.IsEmpty())
                {
                    return false;
                }
            }
            return true;
        }

        public ChannelData GetChannel(Channel ch)
        {
            int index = Channels.FindIndex(c => c.Channel == ch);
            if (index == -1)
            {
                index = Channels.Count;
                Channels.Add(ChannelData.Empty(ch));
            }
            return Channels[index];
        }

        public static ChannelData EmptyBgm() => ChannelData.Empty(Channel.Bgm);

        public ChannelData GetBgm(int index)
        {
            var bgms = Bgms;
            while (bgms.Count <= index)
            {
                bgms.Add(EmptyBgm());
            }
            return bgms[index];
        }

        public void Set(Channel channel, string list, int radix, int? replace = null)
        {
            switch (channel)
            {
                case Channel.Bgm:
                    SetBgm(ChannelData.Create(channel, list, radix), replace);
                    break;
                case Channel.Bar:
                    if (decimal.TryParse(list, out decimal l))
                    {
                        Length = l;
                    }
                    break;
                default:
                    SetChannel(ChannelData.Create(channel, list, radix), replace is not null);
                    break;
            }
        }

        public void Set(ChannelData data, int? replace = null)
        {
            switch (data.Channel)
            {
                case Channel.Bgm:
                    SetBgm(data, replace);
                    break;
                default:
                    SetChannel(data, replace is not null);
                    break;
            }
        }

        public void SetBgm(ChannelData data, int? index = null)
        {
            var bgms = Bgms;
            if (index is int i)
            {
                while (bgms.Count < i)
                {
                    bgms.Add(EmptyBgm());
                }
            }
            bgms.Add(data);
        }

        public void SetChannel(ChannelData data, bool replace = true)
        {
            if (replace)
            {
                var index = Channels.FindIndex(c => c.Channel == data.Channel);
                if (index >= 0)
                {
                    Channels[index] = data;
                    return;
                }
            }
            Channels.Add(data);
        }

        public void Compact()
        {
            foreach (var line in CollectionsMarshal.AsSpan(Channels))
            {
                line.Compact();
            }
            foreach (var line in CollectionsMarshal.AsSpan(Bgms))
            {
                line.Compact();
            }
        }

        public const string DumpFmt = $"{Constants.BarTextFormat}{{1}}:{{2}}";

        internal void Dump(BmsTextWriter writer, int number, int radix)
        {
            foreach (var line in CollectionsMarshal.AsSpan(Bgms))
            {
                DumpData(writer, number, line, radix);
            }
            if (Length != 1)
            {
                writer.Dump(DumpFmt, number, BmsUtils.ToBased(Channel.Bar), Length);
            }
            Channels.Sort();
            foreach (var line in CollectionsMarshal.AsSpan(Channels))
            {
                DumpData(writer, number, line, radix);
            }
        }

        private static void DumpData(BmsTextWriter writer, int number, ChannelData data, int radix)
        {
            writer.Dump(DumpFmt, number, BmsUtils.ToBased(data.Channel), data.GetDataString(radix));
        }

        public void Merge(Bar bar, int bgmOffset)
        {
            var ch = Channels;
            var sbgm = bar.Bgms;
            var sch = bar.Channels;
            var bgms = Bgms;
            var required = sbgm.Count + bgmOffset;
            while (bgms.Count < required)
            {
                bgms.Add(EmptyBgm());
            }
            for (var i = 0; i < sbgm.Count; i++)
            {
                if (sbgm[i] is ChannelData src)
                {
                    bgms[i + bgmOffset] = src.Clone();
                }
            }
            for (var i = 0; i < sch.Count; i++)
            {
                var src = sch[i];
                var index = ch.FindIndex(c => c.Channel == src.Channel && c.CanMerge(src));
                if (index is >= 0)
                {
                    ch[index].Merge(src);
                }
                else
                {
                    ch.Add(src.Clone());
                }
            }
        }
    }
}
