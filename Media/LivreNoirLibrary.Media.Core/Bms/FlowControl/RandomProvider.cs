using System;

namespace LivreNoirLibrary.Media.Bms
{
    public delegate int RandomProvider(FlowAddress address, int max, string? message = null);
}
