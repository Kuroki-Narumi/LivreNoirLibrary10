using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Sandbox
{
    public static class JsonTest
    {
        public static void Test()
        {
            var text = """
                {
                    "Value": 6
                }
                """;



            var c1 = Json.Parse<JsonTestClass1>(text);
            var c2 = Json.Parse<JsonTestClass2>(text);
        }
    }

    public class JsonTestClass1
    {
        public int Value { get; set; }
    }

    public class JsonTestClass2
    {
        public string? Value { get; set; }
    }
}
