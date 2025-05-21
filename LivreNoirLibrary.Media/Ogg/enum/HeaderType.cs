using System;

namespace LivreNoirLibrary.Media.Ogg
{
    [Flags]
    public enum HeaderType : byte
    {
        None = 0,
        Continued = 1,
        BeginningOfStream = 2,
        EndOfStream = 4,
    }

    public static class HeaderTypeFlagExtension
    {
        public static bool IsContinued(this HeaderType flag) => (flag & HeaderType.Continued) is not 0;
        public static bool IsBeginningOfStream(this HeaderType flag) => (flag & HeaderType.BeginningOfStream) is not 0;
        public static bool IsEndOfStream(this HeaderType flag) => (flag & HeaderType.EndOfStream) is not 0;
    }
}
