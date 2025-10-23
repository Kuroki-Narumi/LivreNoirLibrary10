using LivreNoirLibrary.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LivreNoirLibrary.SandBox
{
    public class TestElement : FrameworkElement
    {
        public Dictionary<string, string> Dictionary { get; } = new()
        {
            ["a"] = "A",
            ["b"] = "B",
            ["c"] = "C",
            ["d"] = "D",
            ["e"] = "E",
        };

        public TestElement()
        {
            DataContext = this;
        }


    }
}
