using System;
using System.Drawing;

namespace LivreNoirLibrary.Benchmark
{
    public class JsonTest
    {
        public static void Test()
        {
            var obj = new TestClass()
            {
                Point = new(0.1f, 0.2f)
            };
            var json = Text.Json.GetJsonText(obj);
            var obj2 = Text.Json.Parse<TestClass>(json);
            Console.WriteLine(json);
            Console.WriteLine(obj2?.Point);
            obj2 = Text.Json.Parse<TestClass>(@"{""Hoge"":""piyo""}");
            Console.WriteLine(obj2?.Point);
        }

        public class TestClass
        {
            public PointF Point { get; set; } = new(0.5f, 0.5f);
        }
    }
}
