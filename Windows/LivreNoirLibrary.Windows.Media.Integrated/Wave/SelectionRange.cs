using System;

namespace LivreNoirLibrary.Windows.Media
{
    public readonly struct WaveSelection
    {
        public readonly long Left;
        public readonly long Right;
        public long Length => Right - Left;

        public WaveSelection(long left, long right)
        {
            if (left > right)
            {
                (left, right) = (right, left);
            }
            Left = left;
            Right = right;
        }

        public void Deconstruct(out long left, out long right)
        {
            left = Left;
            right = Right;
        }
    }
}
