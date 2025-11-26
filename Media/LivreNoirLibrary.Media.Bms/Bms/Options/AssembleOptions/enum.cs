using System;

namespace LivreNoirLibrary.Media.Bms
{
    public enum AssembleMode
    {
        Entire,
        Selection,
        Preview,
    }

    public enum RandomProvideMode
    {
        Auto,
        Seed,
        Manual,
        Ignore,
    }

    public enum AssembleReplaceMode
    {
        None,
        Selection,
        All,
    }

    public enum NormalizeMode
    {
        None,
        Peak,
        Rms,
        Lufs,
    }
}