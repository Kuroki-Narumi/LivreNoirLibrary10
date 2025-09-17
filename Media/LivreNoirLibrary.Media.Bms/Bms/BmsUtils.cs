using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsUtils
    {
        private static readonly HashSet<HeaderType> _doubleHeaderTypes = [
            HeaderType.Player, HeaderType.Bpm, HeaderType.Rank, HeaderType.Total, HeaderType.LnObj, HeaderType.LnMode, HeaderType.DefExRank
            ];
        public static bool IsNumberHeader(HeaderType type) => _doubleHeaderTypes.Contains(type);

        /// <summary>
        /// Returns a base36 string from a <see cref="Channel"/>.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToBased(Channel channel) => ((long)channel).ToBased(BasedNumber.StandardRadix, 2);

        /// <summary>
        /// Returns a <see cref="Channel"/> from a.base36 string
        /// </summary>
        /// <returns></returns>
        public static Channel ToChannel(string index)
        {
            if (index.TryParseToInt(BasedNumber.StandardRadix, out var value))
            {
                return (Channel)value;
            }
            return 0;
        }

        public static ushort ToInt(string index, int radix) => (ushort)index.ParseToInt(radix);
        public static string ToBased(int index, int radix) => index.ToBased(radix, 2);

        public static double CalcTotal(int notes)
        {
            var total = Math.Truncate(760.5 * notes / (notes + 650));
            return Math.Max(260.0, total);
        }

        public static string GetBarText(this int number) => string.Format(Constants.BarTextFormat, number);

        public static readonly Rational StopUnit = new(Constants.DefaultBarLength, Constants.StopResolution);
        public static Rational GetStopLength(Rational count) => count * StopUnit;
        public static decimal ConvertBackStopLength(Rational length) => (decimal)(length / StopUnit);

        public static bool IsBgm(this Channel channel) => channel is Channel.Bgm;
        public static bool IsBar(this Channel channel) => channel is Channel.Bar;
        public static bool IsBpm(this Channel channel) => channel is Channel.Bpm_Base or Channel.Bpm;
        public static bool IsExt(this Channel channel) => channel is Channel.Ext;
        public static bool IsSoundChannel(this Channel channel) => IsBgm(channel) || IsVisible(channel) || IsInvisible(channel) || IsLong(channel) || IsMine(channel);

        public static NoteType GetNoteType(this Channel channel)
        {
            return IsInvisible(channel) ? NoteType.Invisible
                : IsMine(channel) ? NoteType.Mine
                : NoteType.Normal;
        }

        public static short GetLane(this Channel channel)
        {
            return IsVisible(channel) ? channel - Channel.P1_Visible
                : IsInvisible(channel) ? channel - Channel.P1_Invisible
                : IsLong(channel) ? channel - Channel.P1_Long
                : channel - Channel.P1_Mine;
        }

        public static Channel GetChannel(this NoteType type, short lane)
        {
            return type switch
            {
                NoteType.Mine => Channel.P1_Mine + lane,
                NoteType.Invisible => Channel.P1_Invisible + lane,
                _ => lane is > 0 ? Channel.P1_Visible + lane : Channel.Bgm
            };
        }

        public static bool IsHex(this Channel channel) => _hex.Contains(channel);
        private static readonly SortedSet<Channel> _hex = 
            [
                Channel.Bpm_Base,
                Channel.Opacity_Base,
                Channel.Opacity_Layer1,
                Channel.Opacity_Layer2,
                Channel.Bgm_Volume,
                Channel.Key_Volume,
            ];

        public static bool IsDecimal(this Channel channel) => _decimal.Contains(channel);
        private static readonly SortedSet<Channel> _decimal =
            [
                Channel.Bpm_Base,
                Channel.Bpm,
                Channel.Scroll,
                Channel.Speed,
            ];

        public static bool IsRational(this Channel channel) => channel is Channel.Stop;

        public static bool IsDefChannel(this Channel channel) => _def.Contains(channel);
        private static readonly SortedSet<Channel> _def =
        [
            Channel.Bga_Base,
            Channel.Bga_Layer1,
            Channel.Bga_Layer2,
            Channel.Bga_Poor,
            Channel.Text,
            Channel.ExRank,
            Channel.Argb_Base,
            Channel.Argb_Layer1,
            Channel.Argb_Layer2,
            Channel.Argb_Poor,
            Channel.SwBga,
            Channel.ChangeOption,
        ];

        public static readonly Channel[] BgaChannelList =
        [
            Channel.Bga_Base,
            Channel.Bga_Layer1,
            Channel.Bga_Layer2,
            Channel.Bga_Poor,
        ];
        public static bool IsBga(this Channel channel) => _bga.Contains(channel);
        private static readonly SortedSet<Channel> _bga = [.. BgaChannelList];

        public static bool IsExtendedBga(this Channel channel) => channel is Channel.SwBga or Channel.Bga_Layer2;

        public static bool IsArgb(this Channel channel) => _argb.Contains(channel);
        private static readonly SortedSet<Channel> _argb =
        [
            Channel.Argb_Base,
            Channel.Argb_Layer1,
            Channel.Argb_Layer2,
            Channel.Argb_Poor,
        ];

        private static bool IsP(Channel channel, Channel start) => channel - start is >= 0 and < 36;

        public static bool IsVisible(this Channel channel) => IsP(channel, Channel.P1_Visible) || IsP(channel, Channel.P2_Visible);
        public static bool IsInvisible(this Channel channel) => IsP(channel, Channel.P1_Invisible) || IsP(channel, Channel.P2_Invisible);
        public static bool IsLong(this Channel channel) => IsP(channel, Channel.P1_Long) || IsP(channel, Channel.P2_Long);
        public static bool IsMine(this Channel channel) => IsP(channel, Channel.P1_Mine) || IsP(channel, Channel.P2_Mine);

        public static bool IsP1(this Channel channel)
        {
            return IsP(channel, Channel.P1_Visible) || IsP(channel, Channel.P1_Invisible) || IsP(channel, Channel.P1_Long) || IsP(channel, Channel.P1_Mine);
        }

        public static bool IsP2(this Channel channel)
        {
            return IsP(channel, Channel.P2_Visible) || IsP(channel, Channel.P2_Invisible) || IsP(channel, Channel.P2_Long) || IsP(channel, Channel.P2_Mine);
        }

        public static bool IsNamed(this Channel channel) => _channel_set.Contains(channel);
        public static bool IsConductor(this Channel channel) => _conductor_set.Contains(channel);
        public static bool IsReserved(this Channel channel) => _reserved_set.Contains(channel);

        public static readonly Channel[] NamedChannelList = [
            Channel.Bpm, Channel.Stop, Channel.Scroll, Channel.Speed,
            Channel.Ext,
            Channel.Bga_Base, Channel.Bga_Layer1, Channel.Bga_Layer2, Channel.Bga_Poor,
            Channel.SwBga, Channel.Text, Channel.ExRank, Channel.ChangeOption,
            Channel.Opacity_Base, Channel.Opacity_Layer1, Channel.Opacity_Layer2, Channel.Opacity_Poor,
            Channel.Argb_Base, Channel.Argb_Layer1, Channel.Argb_Layer2, Channel.Argb_Poor,
            Channel.Bgm_Volume, Channel.Key_Volume
            ];
        private static readonly SortedSet<Channel> _channel_set = [.. NamedChannelList];
        private static readonly SortedSet<Channel> _conductor_set = [Channel.Bpm, Channel.Stop, Channel.Scroll, Channel.Speed];
        private static readonly SortedSet<Channel> _reserved_set = CreateReservedSet();
        private static SortedSet<Channel> CreateReservedSet()
        {
            SortedSet<Channel> set = [
                Channel.None,
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
            Add(Channel.P1_Visible);
            Add(Channel.P1_Invisible);
            Add(Channel.P1_Long);
            Add(Channel.P1_Mine);
            return set;
        }

        public static DefType GetDefType(this Channel channel)
        {
            if (IsBga(channel))
            {
                return DefType.Bmp;
            }
            else if (IsArgb(channel))
            {
                return DefType.Argb;
            }
            else
            {
                return channel switch
                {
                    Channel.ExRank => DefType.ExRank,
                    Channel.Text => DefType.Text,
                    Channel.SwBga => DefType.SwBga,
                    Channel.ChangeOption => DefType.ChangeOption,
                    _ => 0,
                };
            }
        }

        public static bool IsKeyLane(int lane) => lane is > 0 and < Constants.MaxKeyLane;
        public static bool IsBgmLane(int lane) => lane is <= 0;

        public static string GetLaneName(this int lane)
        {
            return lane switch
            {
                <= 0 => $"bgm-{1 - lane}",
                < Constants.MaxKeyLane => $"key-{lane}",
                _ => $"lane-{lane}(invalid)"
            };
        }

        public static bool IsConductor(this DefType type) => type is >= DefType.Bpm and <= DefType.Speed;
    }
}
