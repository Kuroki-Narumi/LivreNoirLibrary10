using BenchmarkDotNet.Running;
using LivreNoirLibrary.Collections;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Numerics;
using System.Runtime.Intrinsics;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Text;

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

            RangeSet<int> set = [];
            void MatchTest(string text)
            {
                Console.WriteLine($"MatchTest text: \"{text}\"");
                if (BasedNumber.TryParseRangeSet(text, set, 10))
                {
                    Console.Write($"  Parse successed: ");
                    foreach (var range in set)
                    {
                        Console.Write($"{range}, ");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"  Parse failed!");
                }
            }

            ReadOnlySpan<string> texts = ["1 2 3 5 6 7", "1-6 3,4", "-9 4-15", "36-5 3 6 9", "1 5 2 4 3", "6 6 6 9..2"];
            foreach (var text in texts)
            {
                MatchTest(text);
            }
        }
    }
}
