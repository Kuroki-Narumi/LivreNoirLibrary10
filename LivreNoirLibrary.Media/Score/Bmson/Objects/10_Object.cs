using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bmson
{
    public abstract record Object
    {
        [JsonPropertyName("y")]
        public long Y { get; set; }
    }
}
