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

            //BenchmarkRunner.Run<ColorTest>();

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
            */

            //BenchmarkRunner.Run<ShuffleTest>();
            //ShuffleTest.Validate();

            BenchmarkRunner.Run<BiQuadFilterTest>();
            BiQuadFilterTest.Validate();
        }
    }
}
