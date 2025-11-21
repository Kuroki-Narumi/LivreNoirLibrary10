using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly unsafe struct LnBitmapData(void* pointer, int width, int height)
    {
        public readonly uint* Pointer = (uint*)pointer;
        public readonly int Width = width;
        public readonly int Height = height;

        public int PixelSize => Width * Height;
        public int Stride => Width * 4;
        public bool IsValid => Pointer is not null && Width is > 0 && Height > 0;

        public uint* Offset(int y) => Pointer + y * Width;
        public uint* Offset(int x, int y) => Pointer + y * Width + x;

        public void Deconstruct(out uint* pointer, out int width, out int height)
        {
            pointer = Pointer;
            width = Width;
            height = Height;
        }

        public Enumerator EnumerateLines(in Rectangle rect) => new(this, rect);

        public unsafe struct Enumerator
        {
            private readonly int _stride;
            private readonly int _height;
            private uint* _pointer;
            private int _y = 0;

            internal Enumerator(LnBitmapData bitmap)
            {
                _pointer = bitmap.Pointer;
                _height = bitmap.Height;
            }

            internal Enumerator(LnBitmapData bitmap, in Rectangle rect)
            {
                _pointer = bitmap.Offset(rect.X, rect.Y);
                _stride = bitmap.Width;
                _height = rect.Height;
            }

            public readonly uint* Current => _pointer;

            public bool MoveNext()
            {
                if (_y is 0)
                {
                    _y++;
                    return true;
                }
                else if (_y < _height)
                {
                    _pointer += _stride;
                    _y++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public readonly Enumerator GetEnumerator() => this;
        }
    }
}
