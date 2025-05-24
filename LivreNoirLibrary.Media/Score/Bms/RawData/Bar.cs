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
            for (int i = 0; i < Channels.Count; i++)
            {
                if (!Channels[i].IsEmpty())
                {
                    return false;
                }
            }
            for (int i = 0; i < Bgms.Count; i++)
            {
                if (!Bgms[i].IsEmpty())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Set bar length by time signature format. (4/4 etc.)
        /// </summary>
        /// <param name="num">Numerator of the signature.</param>
        /// <param name="den">Denominator of the signature represented by an exponent of 2.</param>
        public void SetSignature(int num, int den)
        {
            Length = num / (decimal)Math.Pow(2, den);
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
        public void EnsureBgm(int index)
        {
            var bgms = Bgms;
            while (bgms.Count <= index)
            {
                bgms.Add(EmptyBgm());
            }
        }

        public ChannelData GetBgm(int index)
        {
            EnsureBgm(index);
            return Bgms[index];
        }

        public void Set(Channel channel, string list, int radix, int replace = -1)
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
                    SetChannel(ChannelData.Create(channel, list, radix), replace is >= 0);
                    break;
            }
        }

        public void Set(ChannelData data, int replace = -1)
        {
            switch (data.Channel)
            {
                case Channel.Bgm:
                    SetBgm(data, replace);
                    break;
                default:
                    SetChannel(data, replace is >= 0);
                    break;
            }
        }

        public void SetBgm(ChannelData data, int index = -1)
        {
            if (index is >= 0)
            {
                EnsureBgm(index);
                Bgms[index] = data;
            }
            else
            {
                Bgms.Add(data);
            }
        }

        public void SetChannel(ChannelData data, bool replace = true)
        {
            if (replace)
            {
                int index = Channels.FindIndex(c => c.Channel == data.Channel);
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
            for (int i = 0; i < Bgms.Count; i++)
            {
                Bgms[i]?.Compact();
            }
            for (int i = 0; i < Channels.Count; i++)
            {
                Channels[i]?.Compact();
            }
        }

        public const string DumpFmt = $"{Constants.BarTextFormat}{{1}}:{{2}}";

        internal void Dump(BmsTextWriter writer, int number, int radix)
        {
            if (Length != 1)
            {
                writer.Dump(DumpFmt, number, BmsUtils.ToBased(Channel.Bar), Length);
            }
            for (int i = 0; i < Bgms.Count; i++)
            {
                if (Bgms[i] is ChannelData cd)
                {
                    DumpData(writer, number, cd, radix);
                }
                else
                {
                    DumpData(writer, number, ChannelData.Empty(Channel.Bgm), radix);
                }
            }
            Channels.Sort();
            for (int i = 0; i < Channels.Count; i++)
            {
                DumpData(writer, number, Channels[i], radix);
            }
        }

        private static void DumpData(BmsTextWriter writer, int number, ChannelData data, int radix)
        {
            if (data is not null)
            {
                writer.Dump(DumpFmt, number, BmsUtils.ToBased(data.Channel), data.GetDataString(radix));
            }
        }

        public void Merge(Bar bar, int bgmOffset)
        {
            var ch = Channels;
            var bgms = Bgms;
            var srcBgms = bar.Bgms;
            var srcChannels = bar.Channels;
            EnsureBgm(srcBgms.Count + bgmOffset - 1);
            for (var i = 0; i < srcBgms.Count; i++)
            {
                if (srcBgms[i] is ChannelData src)
                {
                    bgms[i + bgmOffset] = src.Clone();
                }
            }
            foreach (var src in CollectionsMarshal.AsSpan(srcChannels))
            {
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
