using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    public static class VectorUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector<T> CreateFilling<T>(params ReadOnlySpan<T> values)
            where T : unmanaged
        {
            var valueLength = values.Length;
            // 空のスパン
            if (valueLength is 0)
            {
                return default;
            }
            // 1要素の場合は専用処理がある
            if (valueLength is 1)
            {
                return Vector.Create(values[0]);
            }
            var count = Vector<T>.Count;
            // ソースのほうが長い場合はそのまま作成
            if (valueLength >= count)
            {
                return Vector.Create(values);
            }
            // ベクトル作成用のバッファ
            var span = (stackalloc T[count]);
            values.CopyTo(span);
            return Vector.Create(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector<T> CreateRepeating<T>(params ReadOnlySpan<T> values)
            where T : unmanaged
        {
            var valueLength = values.Length;
            // 空のスパン
            if (valueLength is 0)
            {
                return default;
            }
            // 1要素の場合は専用処理がある
            if (valueLength is 1)
            {
                return Vector.Create(values[0]);
            }
            var count = Vector<T>.Count;
            // ソースのほうが長い場合はそのまま作成
            if (valueLength >= count)
            {
                return Vector.Create(values);
            }
            // ベクトル作成用のバッファ
            var span = (stackalloc T[count]);
            var offset = 0;
            for (; count >= valueLength; offset += valueLength, count -= valueLength)
            {
                values[..valueLength].CopyTo(span[offset..]);
            }
            if (count is > 0)
            {
                values[..count].CopyTo(span[offset..]);
            }
            return Vector.Create(span);
        }
    }
}
