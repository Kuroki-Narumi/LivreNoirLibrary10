using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh
{
    [JsonConverter(typeof(AttributeJsonConverter))]
    public enum Attribute
    {
        None,
        Light,  // 光
        Dark,   // 闇
        Water,  // 水
        Fire,   // 炎
        Earth,  // 地
        Wind,   // 風
        Divine, // 神
    }
}
