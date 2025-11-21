using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IBmsData : IClear
    {
        ChartType ChartType { get; set; }
        int LnObj { get; set; }
        IBmsDataUnit Root { get; }

        IBmsDataUnit GetBranchData(FlowBranch branch);
        bool TryGetBranch(FlowAddress address, [MaybeNullWhen(false)] out IFlowContainer flow, [MaybeNullWhen(false)] out IBmsDataUnit data);
        bool InsulateBranch(FlowBranch branch);

        void WriteHistoryData(Stream stream);
        void ReadHistoryData(Stream stream);
    }
}
