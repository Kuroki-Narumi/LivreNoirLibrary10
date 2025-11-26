using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsUtils
    {
        /// <summary>
        /// Returns a base36 string from a <see cref="Channel"/>.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToBased(this Channel channel) => ((long)channel).ToBased(BmsConstants.Base_Default, 2);

        /// <summary>
        /// Returns a <see cref="Channel"/> from a base36 string
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Channel ToChannel(ReadOnlySpan<char> span)
        {
            return BasedNumber.TryParseToInt(span, BmsConstants.Base_Default, out var value) ? (Channel)value : 0;
        }

        public static string ToBased(int index, int radix) => index.ToBased(radix, 2);

        public static double CalcTotal(int notes)
        {
            var total = Math.Truncate(760.5 * notes / (notes + 650));
            return Math.Max(260.0, total);
        }

        public static string GetBarText(this int number) => string.Format(BmsConstants.BarTextFormat, number);

        public static bool IsConductor(this Channel channel) => _conductor.Contains(channel);

        private static bool IsP(Channel channel, Channel start) => channel - start is >= 0 and < 72;
        public static bool IsBgm(this Channel channel) => channel is Channel.Bgm or >= Channel.Bgm_Start and <= Channel.Bgm_End;
        public static bool IsVisible(this Channel channel) => IsP(channel, Channel.Visible_Start);
        public static bool IsInvisible(this Channel channel) => IsP(channel, Channel.Invisible_Start);
        public static bool IsLong(this Channel channel) => IsP(channel, Channel.Long_Start);
        public static bool IsMine(this Channel channel) => IsP(channel, Channel.Mine_Start);
        public static bool IsKey(this Channel channel) => IsVisible(channel) || IsInvisible(channel) || IsLong(channel) || IsMine(channel);
        public static bool IsSoundLane(this Channel channel) => IsKey(channel) || IsBgm(channel);

        public static bool TryGetLane(this Channel channel, out int lane)
        {
            switch (channel)
            {
                case >= Channel.Visible_Start and <= Channel.Visible_End:
                    lane = channel - Channel.Visible_Start;
                    return true;
                case >= Channel.Invisible_Start and <= Channel.Invisible_End:
                    lane = channel - Channel.Invisible_Start;
                    return true;
                case >= Channel.Long_Start and <= Channel.Long_End:
                    lane = channel - Channel.Long_Start;
                    return true;
                case >= Channel.Mine_Start and <= Channel.Mine_End:
                    lane = channel - Channel.Mine_Start;
                    return true;
                case >= Channel.Bgm_Start and <= Channel.Bgm_End:
                    lane = Channel.Bgm_Start - channel;
                    return true;
                default:
                    lane = int.MaxValue;
                    return false;
            }
        }

        public static bool TryGetChannel(this int lane, out Channel channel)
        {
            switch (lane)
            {
                case <= 0:
                    channel = Channel.Bgm_Start - (short)lane;
                    return true;
                case < 72:
                    channel = (short)lane + Channel.Visible_Start;
                    return true;
                default:
                    channel = Channel.None;
                    return false;
            }
        }

        public static bool IsBga(this Channel channel) => _bga.Contains(channel);
        public static bool IsArgb(this Channel channel) => _argb.Contains(channel);
        public static bool IsOpacity(this Channel channel) => channel is >= Channel.Opacity_Base and <= Channel.Opacity_Poor;
        public static Channel ToBga(this Channel channel) => channel switch
        {
            Channel.Bga_Base or Channel.Argb_Base or Channel.Opacity_Base => Channel.Bga_Base,
            Channel.Bga_Layer1 or Channel.Argb_Layer1 or Channel.Opacity_Layer1 => Channel.Bga_Layer1,
            Channel.Bga_Layer2 or Channel.Argb_Layer2 or Channel.Opacity_Layer2 => Channel.Bga_Layer2,
            Channel.Bga_Poor or Channel.Argb_Poor or Channel.Opacity_Poor => Channel.Bga_Poor,
            _ => Channel.None,
        };

        public static bool IsReserved(this Channel channel) => _reserved.Contains(channel);
        public static bool IsNamed(this Channel channel) => _named.Contains(channel);

        public static (Channel, NoteType) Split(this Channel channel) => channel switch
        {
            Channel.Bgm => (Channel.Bgm_Start, NoteType.Normal),
            >= Channel.Invisible_Start and <= Channel.Invisible_End => (channel - Channel.Invisible_Start + Channel.Visible_Start, NoteType.Invisible),
            >= Channel.Long_Start and <= Channel.Long_End => (channel - Channel.Long_Start + Channel.Visible_Start, NoteType.LongEnd),
            >= Channel.Mine_Start and <= Channel.Mine_End => (channel - Channel.Mine_Start + Channel.Mine_End, NoteType.Mine),
            _ => (channel, NoteType.Normal),
        };

        public static Channel ToInvisible(this Channel channel) => channel - Channel.Visible_Start + Channel.Invisible_Start;
        public static Channel ToLong(this Channel channel) => channel - Channel.Visible_Start + Channel.Long_Start;
        public static Channel ToMine(this Channel channel) => channel - Channel.Visible_Start + Channel.Mine_Start;

        public static Channel Merge(Channel channel, NoteType type) => channel switch
        {
            >= Channel.Visible_Start and <= Channel.Visible_End => type switch
            {
                NoteType.Invisible => ToInvisible(channel),
                NoteType.LongEnd => ToLong(channel),
                NoteType.Mine => ToMine(channel),
                _ => channel,
            },
            >= Channel.Bgm_Start and <= Channel.Bgm_End => Channel.Bgm,
            _ => channel,
        };

        public static string GetChannelName(this Channel channel) => channel switch
        {
            >= Channel.Visible_Start and <= Channel.Visible_End => $"key-{channel - Channel.Visible_Start}",
            >= Channel.Invisible_Start and <= Channel.Invisible_End => $"key-{channel - Channel.Invisible_Start}(Invisible)",
            >= Channel.Long_Start and <= Channel.Long_End => $"key-{channel - Channel.Long_Start}(Long End)",
            >= Channel.Mine_Start and <= Channel.Mine_End => $"key-{channel - Channel.Mine_Start}(Mine)",
            >= Channel.Bgm_Start and <= Channel.Bgm_End => $"bgm-{channel - Channel.Bgm_Start}",
            _ => GetChannelName_Named(channel),
        };
        private static string GetChannelName_Named(Channel channel) => IsNamed(channel) ? channel.ToString() : $"ch-{ToBased(channel)}";

        public static bool IsWavDef(this Channel channel) => IsBgm(channel) || IsVisible(channel) || IsInvisible(channel) || IsLong(channel);
        public static bool IsDefValue(this Channel channel) => IsWavDef(channel) || _defTypes.ContainsKey(channel);
        public static bool IsHexValue(this Channel channel) => _hex.Contains(channel) || IsMine(channel);

        public static bool TryGetDefType(this Channel channel, out DefType type)
        {
            if (IsWavDef(channel))
            {
                type = DefType.Wav;
                return true;
            }
            return _defTypes.TryGetValue(channel, out type);
        }

        public static bool IsConductor(this DefType type) => type is >= DefType.Bpm and <= DefType.Speed;
        public static bool NeedsBpmDef(this double value) => value != double.Truncate(value) || value is <= 0 || value is > 255;

        public static Rational ToInnerOffset(this double value) => Rational.ConvertBySBT(value, BmsConstants.MaxInnerResolution);

        private static readonly SortedSet<Channel> _hex = 
        [
            Channel.Bpm_Base,
            Channel.Opacity_Base,
            Channel.Opacity_Layer1,
            Channel.Opacity_Layer2,
            Channel.Opacity_Poor,
            Channel.Bgm_Volume,
            Channel.Key_Volume,
        ];

        private static readonly SortedSet<Channel> _conductor =
        [
            Channel.Bpm,
            Channel.Stop,
            Channel.Scroll,
            Channel.Speed,
        ];

        private static readonly SortedSet<Channel> _bga =
        [
            Channel.Bga_Base,
            Channel.Bga_Layer1,
            Channel.Bga_Layer2,
            Channel.Bga_Poor,
        ];

        private static readonly SortedSet<Channel> _argb =
        [
            Channel.Argb_Base,
            Channel.Argb_Layer1,
            Channel.Argb_Layer2,
            Channel.Argb_Poor,
        ];

        private static readonly SortedSet<Channel> _reserved = CreateReservedSet();
        private static SortedSet<Channel> CreateReservedSet()
        {
            SortedSet<Channel> set = [
                Channel.Bgm,
                Channel.Bar,
                Channel.Bpm_Base,
                ];
            void Add(Channel start)
            {
                for (short i = 0; i is < 72; i++)
                {
                    set.Add(start + i);
                }
            }
            Add(Channel.Invisible_Start);
            Add(Channel.Long_Start);
            Add(Channel.Mine_Start);
            return set;
        }

        private static readonly SortedSet<Channel> _named =
        [
            Channel.Bpm, Channel.Stop, Channel.Scroll, Channel.Speed,
            Channel.Ext,
            Channel.Bga_Base, Channel.Bga_Layer1, Channel.Bga_Layer2, Channel.Bga_Poor,
            Channel.SwBga, Channel.Text, Channel.ExRank, Channel.ChangeOption,
            Channel.Opacity_Base, Channel.Opacity_Layer1, Channel.Opacity_Layer2, Channel.Opacity_Poor,
            Channel.Argb_Base, Channel.Argb_Layer1, Channel.Argb_Layer2, Channel.Argb_Poor,
            Channel.Bgm_Volume, Channel.Key_Volume
        ];

        private static readonly Dictionary<Channel, DefType> _defTypes = CreateDefTypes();
        private static Dictionary<Channel, DefType> CreateDefTypes()
        {
            Dictionary<Channel, DefType> dic = [];
            foreach (var ch in _bga)
            {
                dic[ch] = DefType.Bmp;
            }
            foreach (var ch in _argb)
            {
                dic[ch] = DefType.Argb;
            }
            dic[Channel.Bpm] = DefType.Bpm;
            dic[Channel.Stop] = DefType.Stop;
            dic[Channel.Scroll] = DefType.Scroll;
            dic[Channel.Speed] = DefType.Speed;
            dic[Channel.ExRank] = DefType.ExRank;
            dic[Channel.Text] = DefType.Text;
            dic[Channel.SwBga] = DefType.SwBga;
            dic[Channel.ChangeOption] = DefType.ChangeOption;
            return dic;
        }
    }
}
