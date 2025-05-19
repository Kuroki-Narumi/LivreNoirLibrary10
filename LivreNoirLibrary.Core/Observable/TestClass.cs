using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LivreNoirLibrary.Observable;

namespace LivreNoirLibrary.Observable
{
    public partial class TestClass : ObservableObjectBase
    {

        [ObservableProperty(Related = [nameof(UpperName)])]
        private string _name = "";

        [ObservableProperty]
        private int _number;

        [ObservableProperty(SetterScope = Scope.Protected)]
        private int _value;

        public string UpperName => _name.ToUpper();

        private static int ValidateNumber(int value) => Math.Clamp(value, 0, 100);
    }
}
