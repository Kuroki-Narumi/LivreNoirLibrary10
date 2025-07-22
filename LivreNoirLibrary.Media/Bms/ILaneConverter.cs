using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using static LivreNoirLibrary.Media.Bms.KeyIndexes;

namespace LivreNoirLibrary.Media.Bms
{
    public interface ILaneConverter
    {
        public int Convert(int lane);
        public int ConvertBack(int index);

        public static ILaneConverter GetAuto(KeyType keyType)
        {
            return keyType.Type switch
            {
                ChartType.Beat => LaneConverter.BmsLaneConverter,
                ChartType.Popn => keyType.Keys is > 9 ? LaneConverter.Pms18LaneConverter
                                : keyType.Keys is 5 ? LaneConverter.Pms5LaneConverter
                                : LaneConverter.Pms9LaneConverter,
                _ => DefaultLaneConverter.Instance,
            };
        }
    }

    public class DefaultLaneConverter : ILaneConverter
    {
        public static readonly DefaultLaneConverter Instance = new();

        public int Convert(int lane) => Math.Max(lane, 0);
        public int ConvertBack(int index) => index;
    }

    public class LaneConverter : ILaneConverter
    {
        protected readonly Dictionary<int, int> _converter;
        protected readonly Dictionary<int, int> _back_converter;

        public LaneConverter(Dictionary<int, int> dictionary)
        {
            _converter = dictionary;
            _back_converter = _converter.Invert();
        }

        public int Convert(int lane) => _converter.TryGetValue(lane, out var index) ? index : Math.Max(lane, 0);
        public int ConvertBack(int index) => _back_converter.TryGetValue(index, out var lane) ? lane: index;

        public static readonly LaneConverter BmsLaneConverter = new(new()
        {
            {Beat_1P_1,  1},
            {Beat_1P_2,  2},
            {Beat_1P_3,  3},
            {Beat_1P_4,  4},
            {Beat_1P_5,  5},
            {Beat_1P_6,  6},
            {Beat_1P_7,  7},
            {Beat_1P_Scratch,  8},
            {Beat_2P_1,  9},
            {Beat_2P_2, 10},
            {Beat_2P_3, 11},
            {Beat_2P_4, 12},
            {Beat_2P_5, 13},
            {Beat_2P_6, 14},
            {Beat_2P_7, 15},
            {Beat_2P_Scratch, 16},

            {Beat_1P_FootPedal, 17},
            {Beat_2P_FootPedal, 18},
        });

        public static readonly LaneConverter Pms9LaneConverter = new(new()
        {
            {Pop_1, 1},
            {Pop_2, 2},
            {Pop_3, 3},
            {Pop_4, 4},
            {Pop_5, 5},
            {Pop_6, 6},
            {Pop_7, 7},
            {Pop_8, 8},
            {Pop_9, 9},
        });

        public static readonly LaneConverter Pms5LaneConverter = new(new()
        {
            {Pop_3, 1},
            {Pop_4, 2},
            {Pop_5, 3},
            {Pop_6, 4},
            {Pop_7, 5},
        });

        public static readonly LaneConverter Pms18LaneConverter = new(new()
        {
            {Pop_1P_1,  1},
            {Pop_1P_2,  2},
            {Pop_1P_3,  3},
            {Pop_1P_4,  4},
            {Pop_1P_5,  5},
            {Pop_1P_6,  6},
            {Pop_1P_7,  7},
            {Pop_1P_8,  8},
            {Pop_1P_9,  9},
            {Pop_2P_1, 10},
            {Pop_2P_2, 11},
            {Pop_2P_3, 12},
            {Pop_2P_4, 13},
            {Pop_2P_5, 14},
            {Pop_2P_6, 15},
            {Pop_2P_7, 16},
            {Pop_2P_8, 17},
            {Pop_2P_9, 18},
        });
    }
}
