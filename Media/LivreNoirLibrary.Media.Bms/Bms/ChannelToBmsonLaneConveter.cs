using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class DefaultLaneConverter : IChannelToBmsonLaneConverter
    {
        public static readonly DefaultLaneConverter Instance = new();

        public static bool TryConvertStatic(Channel channel, out int lane)
        {
            if (BmsUtils.TryGetLane(channel, out lane))
            {
                lane = Math.Max(lane, 0);
                return true;
            }
            return false;
        }

        public bool TryConvert(Channel channel, out int lane) => TryConvertStatic(channel, out lane);
    }

    public class ChannelToBmsonLaneConverter : IChannelToBmsonLaneConverter
    {
        public static IChannelToBmsonLaneConverter GetAuto(ChartType chartType, int keys)
        {
            return chartType switch
            {
                ChartType.Beat => BmsLaneConverter,
                ChartType.Popn => keys is > 9 ? Pms18LaneConverter
                                : keys is 5 ? Pms5LaneConverter
                                : Pms9LaneConverter,
                _ => DefaultLaneConverter.Instance,
            };
        }

        protected readonly Dictionary<Channel, int> _dic = [];

        public ChannelToBmsonLaneConverter(ReadOnlySpan<(Channel, int)> lanes)
        {
            var conv = _dic;
            foreach (var (ch, lane) in lanes)
            {
                conv[ch] = lane;
            }
        }

        public bool TryConvert(Channel channel, out int lane)
        {
            if (channel is <= 0)
            {
                lane = 0;
                return true;
            }
            return _dic.TryGetValue(channel, out lane);
        }

        public static readonly ChannelToBmsonLaneConverter BmsLaneConverter = new(
        [
            (Channel.Beat_1P_1,  1),
            (Channel.Beat_1P_2,  2),
            (Channel.Beat_1P_3,  3),
            (Channel.Beat_1P_4,  4),
            (Channel.Beat_1P_5,  5),
            (Channel.Beat_1P_6,  6),
            (Channel.Beat_1P_7,  7),
            (Channel.Beat_1P_Scratch,  8),
            (Channel.Beat_2P_1,  9),
            (Channel.Beat_2P_2, 10),
            (Channel.Beat_2P_3, 11),
            (Channel.Beat_2P_4, 12),
            (Channel.Beat_2P_5, 13),
            (Channel.Beat_2P_6, 14),
            (Channel.Beat_2P_7, 15),
            (Channel.Beat_2P_Scratch, 16),

            (Channel.Beat_1P_Ext, 17),
            (Channel.Beat_2P_Ext, 18),
        ]);

        public static readonly ChannelToBmsonLaneConverter Pms9LaneConverter = new(
        [
            (Channel.Popn_1, 1),
            (Channel.Popn_2, 2),
            (Channel.Popn_3, 3),
            (Channel.Popn_4, 4),
            (Channel.Popn_5, 5),
            (Channel.Popn_6, 6),
            (Channel.Popn_7, 7),
            (Channel.Popn_8, 8),
            (Channel.Popn_9, 9),
        ]);

        public static readonly ChannelToBmsonLaneConverter Pms5LaneConverter = new(
        [
            (Channel.Popn_3, 1),
            (Channel.Popn_4, 2),
            (Channel.Popn_5, 3),
            (Channel.Popn_6, 4),
            (Channel.Popn_7, 5),
        ]);

        public static readonly ChannelToBmsonLaneConverter Pms18LaneConverter = new(
        [
            (Channel.Popn_1P_1,  1),
            (Channel.Popn_1P_2,  2),
            (Channel.Popn_1P_3,  3),
            (Channel.Popn_1P_4,  4),
            (Channel.Popn_1P_5,  5),
            (Channel.Popn_1P_6,  6),
            (Channel.Popn_1P_7,  7),
            (Channel.Popn_1P_8,  8),
            (Channel.Popn_1P_9,  9),
            (Channel.Popn_2P_1, 10),
            (Channel.Popn_2P_2, 11),
            (Channel.Popn_2P_3, 12),
            (Channel.Popn_2P_4, 13),
            (Channel.Popn_2P_5, 14),
            (Channel.Popn_2P_6, 15),
            (Channel.Popn_2P_7, 16),
            (Channel.Popn_2P_8, 17),
            (Channel.Popn_2P_9, 18),
        ]);
    }
}