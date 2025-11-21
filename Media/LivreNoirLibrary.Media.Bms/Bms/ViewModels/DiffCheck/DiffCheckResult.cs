using System;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class DiffCheckResult() : DiffResultBase(FlowAddress.Empty)
    {
        public int Radix { get; set; }
        public string? LeftName { get; set; }
        public string? RightName { get; set; }
    }
}
