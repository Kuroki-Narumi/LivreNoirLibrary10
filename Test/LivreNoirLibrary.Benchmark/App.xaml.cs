using BenchmarkDotNet.Running;
using LivreNoirLibrary.Collections;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Numerics;
using System.Runtime.Intrinsics;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.Benchmark
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //PointerTest.Run();

            BenchmarkRunner.Run<ColorTest>();
            ColorTest.Validate();

            //JsonTest.Test();
            //BenchmarkRunner.Run<VectorTest>();
            //BenchmarkRunner.Run<Lanczos3Test>();
            //Lanczos3Test.Check();

            /*
            BenchmarkRunner.Run<VectorTest2>();

            var test1 = new VectorTest2();
            var test2 = new VectorTest2();
            test1.Setup();
            test2.Setup();
            Console.WriteLine($"initial state is equal: {test1._vectors.SequenceEqual(test2._vectors)}");
            test1.Manual();
            test2.ShuffleNative();
            Console.WriteLine($"process is equal: {test1._results.SequenceEqual(test2._results)}");
            */

            //BenchmarkRunner.Run<DoubleRectTest>();
            //DoubleRectTest.Validate();

            //BenchmarkRunner.Run<IfTest>();
            //IfTest.Validate();

            /*
            var random = Random.Shared;
            var t0 = Stopwatch.GetTimestamp();
            for (var i = 0L; ; i++)
            {
                var value = random.NextDouble();

                var (num, den) = Rational.Rationalize(value);

                if ((i & 32767) is 0)
                {
                    var t = Stopwatch.GetElapsedTime(t0);
                    Console.Write($"i={i}, period={t.TotalMicroseconds / i:F10}us/op\r");
                }
            }
            //*/

            //BenchmarkRunner.Run<ShuffleTest>();
            //ShuffleTest.Validate();

            //BenchmarkRunner.Run<BiQuadFilterTest>();
            //BiQuadFilterTest.Validate();

            //BenchmarkRunner.Run<RationalEnumeration>();

            /*
            const int maxDen = 10;
            const double estimatedCount = 3.0 * maxDen * maxDen / (Math.PI * Math.PI);
            Console.WriteLine($"maxDen = {maxDen}, estimated count = {estimatedCount}");
            foreach (var (num, den) in Rational.EnumerateZeroToOne(maxDen))
            {
                Console.WriteLine($"{num}/{den}");
                var value = (double)num / (double)den;
                var (rn, rd) = Rational.Rationalize(value, maxDen);
                if (num != rn || den != rd)
                {
                    Console.WriteLine($"Mismatch: {num}/{den} = {value}, re = {rn}/{rd}");
                }
            }
            Console.WriteLine("Finished.");
            //*/

            /*
            // 357686312646216567629137
            const long origNum = 629137;
            const long origDen = 1209600;
            const long offset = 999;
            var maxMaxDen = long.MaxValue;

            var period = origDen / 100;
            for (var i = origNum + 1; i <= origDen; i++)
            {
                var maxDen = FindMaxDen(origNum, i, offset);
                maxMaxDen = Math.Min(maxMaxDen, maxDen);
                if (i % period == 0)
                {
                    Console.WriteLine($"... processing {i} / {origDen}");
                }
            }
            Console.WriteLine($"maxMaxDen = {maxMaxDen}");

            static long FindMaxDen(long origNum, long origDen, double offset)
            {
                // reduct
                var gcd = origNum.GCD(origDen);
                origNum /= gcd;
                origDen /= gcd;

                var a = offset + (double)origNum / (double)origDen;
                var b = a - offset;
                var lower = 1L;
                var upper = Rational.DoubleDenominatorLimit;
                while (lower <= upper)
                {
                    var n = lower + (upper - lower) / 2;
                    var (num, den) = Rational.Rationalize(b, n);
                    if (num == origNum && den == origDen)
                    {
                        lower = n + 1;
                    }
                    else
                    {
                        upper = n - 1;
                    }
                }
                if (upper > 1)
                {
                    var (n2, d2) = Rational.Rationalize(b, upper);
                    if (n2 != origNum || d2 != origDen)
                    {
                        Console.WriteLine($"orig={origNum}/{origDen}, decValue={b}, maxDen={upper}, Rationalized={n2}/{d2}");
                    }
                }
                return upper;
            }
            //*/

            /*
            var source = "あ・いう・・え/お";
            var reg = new Regex("[・/]");
            foreach (var range in reg.EnumerateSplits(source))
            {
                Console.WriteLine(source.AsSpan()[range]);
            }
            var ranges = (stackalloc Range[6]);
            var count = source.AsSpan().Split(ranges, "・/", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine(count);

            //*/
        }
    }
}
