using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Sandbox
{
    public partial class TestClass2 : ObservableObjectBase
    {
        [JsonIgnore]
        [JsonPropertyName("name")]
        [TypeConverter(typeof(string))]
        [ObservableProperty]
        private string? _name;
    }
}
