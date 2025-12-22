using LivreNoirLibrary.Text;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bms.Play
{
    //[JsonConverter(typeof(JudgeDefinitionCollectionJsonConverter))]
    public class JudgeDefinitionCollection
    {
        internal readonly JudgeDefinition[] _definitions;

        public JudgeDefinition ThroughJudge { get; }

        public JudgeDefinitionCollection(JudgeDefinition through, params ReadOnlySpan<JudgeDefinition> judges)
        {
            ThroughJudge = through;
            _definitions = [.. judges];
            Array.Sort(_definitions);
        }

        public bool TryGetJudge(double error, out JudgeDefinition judge)
        {
            foreach (var j in _definitions)
            {
                if (j.BeforeMargin >= -error && j.AfterMargin <= error)
                {
                    judge = j;
                    return true;
                }
            }
            judge = default;
            return false;
        }
    }
    /*
    public class JudgeDefinitionCollectionJsonConverter : JsonConverter<JudgeDefinitionCollection>
    {
        public const int MaxJudgeCount = 10;
        public const string PropertyName_Definitions = "Definitions";

        public override JudgeDefinitionCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.StartObject)
            {
                var defs = (stackalloc JudgeDefinition[MaxJudgeCount]);
                var defsCount = 0;
                JudgeDefinition through = default;
                using var document = JsonDocument.ParseValue(ref reader);
                foreach (var property in document.RootElement.EnumerateObject())
                {
                    var v = property.Value;
                    switch (property.Name)
                    {
                        case nameof(JudgeDefinitionCollection.ThroughJudge):
                            through = v.Deserialize<JudgeDefinition>();
                            break;
                        case PropertyName_Definitions:
                            if (v.ValueKind is not JsonValueKind.Array)
                            {
                                goto OnError;
                            }
                            defsCount = Math.Min(v.GetArrayLength(), MaxJudgeCount);
                            var i = 0;
                            foreach (var item in v.EnumerateArray())
                            {
                                defs[i] = item.Deserialize<JudgeDefinition>();
                                ++i;
                            }
                            break;
                    }
                }
                return new(through, defs[..defsCount]);
            }
        OnError:
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, JudgeDefinitionCollection value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(JudgeDefinitionCollection.ThroughJudge));
            JsonSerializer.Serialize(writer, value.ThroughJudge, options);
            writer.WriteArrayIfNotNull(PropertyName_Definitions, value._definitions, options);
            writer.WriteEndObject();
        }
    }
    */
}
