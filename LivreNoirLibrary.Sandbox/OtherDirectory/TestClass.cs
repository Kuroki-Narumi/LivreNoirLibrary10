using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;

namespace LivreNoirLibrary.Sandbox
{
    public partial class TestClass3 : System.Windows.DependencyObject
    {
        public const string DefaultName = "Name";

        [DependencyProperty]
        private string _name = DefaultName;

        [DependencyProperty]
        private int _number = 14;

        [DependencyProperty(SetterScope = Scope.Protected)]
        private string? _value;

        [DependencyProperty]
        private Rational _ratValue = Rational.One;

        public string UpperName => _name.ToUpper();

        private static int CoerceNumber(int value) => Math.Clamp(value, 0, 100);
    }
}
