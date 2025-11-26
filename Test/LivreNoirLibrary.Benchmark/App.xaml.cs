using BenchmarkDotNet.Running;
using LivreNoirLibrary.Collections;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Numerics;
using System.Runtime.Intrinsics;
using LivreNoirLibrary.Numerics;

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
            //ColorTest.Check();
            //JsonTest.Test();
            //BenchmarkRunner.Run<VectorTest>();
            //BenchmarkRunner.Run<Lanczos3Test>();
            //Lanczos3Test.Check();

            BenchmarkRunner.Run<VectorTest2>();

            var test1 = new VectorTest2();
            var test2 = new VectorTest2();
            test1.Setup();
            test2.Setup();
            Console.WriteLine($"initial state is equal: {test1._vectors.SequenceEqual(test2._vectors)}");
            test1.Manual();
            test2.ShuffleNative();
            Console.WriteLine($"process is equal: {test1._results.SequenceEqual(test2._results)}");
        }
    }

}
