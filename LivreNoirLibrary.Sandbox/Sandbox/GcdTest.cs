using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Sandbox
{
    internal static class GcdTest
    {
        public static long CoprimeCount(long n)
        {
            var result = 0L;
            for (var a = 1; a <= n; a++)
            {
                result += Totient(a);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long Totient(long a)
        {
            var result = 0L;
            for (var n = 1; n <= a; n++)
            {
                if (GCD(a, n) is 1)
                {
                    result++;
                }
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long GCD(long a, long b)
        {
            while (b is not 0)
            {
                (a, b) = (b, a % b);
            }
            return a;
        }
    }
}
