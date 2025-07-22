using System;
using System.Collections.Generic;
using System.Text.Json;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bmxon
{
    public class FlowInfo : ObjectBase
    {
        public IFlowIndexProvider? IndexProvider { get; set; }
        public Dictionary<int, FlowBranch> Branches { get; set; } = [];
        public FlowBranch? Default { get; set; }
    }

    public interface IFlowIndexProvider : IJsonWriter
    {
        public int GetIndex();
    }

    public class FixedFlowIndexProvider(int index) : IFlowIndexProvider
    {
        public int Index { get; set; } = index;
        int IFlowIndexProvider.GetIndex() => Index;

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options) => writer.WriteNumberValue(Index);
    }

    public class RandomFlowIndexProvider(int min, int max, RandomBase? random) : IFlowIndexProvider
    {
        public int Minimum { get; set; } = min;
        public int Maximum { get; set; } = max;
        public RandomBase? Random { get; set; } = random;

        int IFlowIndexProvider.GetIndex() => Random is not null ? Random.Next(Minimum, Maximum) : System.Random.Shared.Next(Minimum, Maximum);

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
            => writer.WriteStringValue($"rand({(Minimum is 0 ? Maximum : $"{Minimum},{Maximum}")})");
    }
}
