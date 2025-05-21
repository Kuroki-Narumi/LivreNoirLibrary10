using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.ObjectModel
{
    public abstract partial class JsonNode
    {
        public static JsonNode Open(string path)
        {
            using var file = File.OpenRead(path);
            return CreateNode(file);
        }

        public static byte[] CreateBuffer(object obj)
        {
            using MemoryStream ms = new();
            if (obj is IJsonWriter w)
            {
                w.WriteJson(ms);
            }
            else
            {
                Json.Dump(ms, obj, false);
            }
            ms.Position = 0;
            return CreateBuffer(ms);
        }

        public static byte[] CreateBuffer(Stream stream)
        {
            var buffer = new byte[stream.Length - stream.Position];
            _ = stream.Read(buffer);
            return buffer;
        }

        public static JsonNode Create(object obj) => CreateNode(CreateBuffer(obj));

        public static JsonNode CreateNode(Stream stream)
        {
            return CreateNode(CreateBuffer(stream));
        }

        public static JsonNode CreateNode(string text) => CreateNode(Encoding.UTF8.GetBytes(text));
        public static JsonNode CreateNode(byte[] utf8Text) => CreateNode(new ReadOnlySpan<byte>(utf8Text));
        public static JsonNode CreateNode(Memory<byte> utf8Text) => CreateNode((ReadOnlySpan<byte>)utf8Text.Span);
        public static JsonNode CreateNode(ReadOnlyMemory<byte> utf8Text) => CreateNode(utf8Text.Span);
        public static JsonNode CreateNode(ReadOnlySpan<byte> utf8Text)
        {
            Utf8JsonReader reader = new(utf8Text, ReaderOptions);
            return ParseNodes(ref reader);
        }

        private static readonly JsonReaderOptions ReaderOptions = new()
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip
        };

        private static JsonNode ParseNodes(ref Utf8JsonReader reader)
        {
            ParseState state = new(true);
            Stack<ParseState> stack = new();
            void Push(bool isArray)
            {
                stack.Push(state);
                state = state.Stack(isArray);
            }
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        Push(false);
                        break;
                    case JsonTokenType.StartArray:
                        Push(true);
                        break;
                    case JsonTokenType.EndObject:
                    case JsonTokenType.EndArray:
                        state = stack.Pop();
                        break;
                    case JsonTokenType.PropertyName:
                        state.SetKey(reader.GetString());
                        break;
                    case JsonTokenType.String:
                        state.Update(new JsonStringNode(reader.GetString() ?? ""));
                        break;
                    case JsonTokenType.Number:
                        state.Update(new JsonNumberNode(reader.GetDouble()));
                        break;
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        state.Update(new JsonBoolNode(reader.GetBoolean()));
                        break;
                    case JsonTokenType.Null:
                        state.Update(new JsonNullNode());
                        break;
                }
            }
            return state.First();
        }

        private class ParseState(bool isArray)
        {
            private readonly JsonCollectionNode _object = isArray ? new JsonArrayNode() : new JsonObjectNode();
            private readonly bool _isArray = isArray;
            private string? _key;

            public void Update(JsonNode value)
            {
                if (value is not null)
                {
                    if (_isArray)
                    {
                        value.Name = $"{_object.Children.Count}";
                    }
                    else if (_key is not null)
                    {
                        value.Name = _key;
                        _key = null;
                    }
                    _object.Children.Add(value);
                }
            }

            public JsonNode First()
            {
                return _object.Children[0];
            }

            public void SetKey(string? key)
            {
                _key = key;
            }

            public ParseState Stack(bool isArray)
            {
                ParseState state = new(isArray);
                Update(state._object);
                return state;
            }
        }
    }

    public abstract partial class JsonNode : ObservableObjectBase, IEnumerable<JsonNode>, INamedObject
    {
        [ObservableProperty]
        private string _name = "";
        [ObservableProperty(Related = [nameof(ValueString)])]
        private bool _isExpanded = false;

        public abstract string ValueString { get; }

        public abstract IEnumerator<JsonNode> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public partial class JsonValueNode : JsonNode
    {
        [ObservableProperty(Related = [nameof(ValueString)])]
        private object? _value;

        public override string ValueString => $"{Value}";

        public override IEnumerator<JsonNode> GetEnumerator()
        {
            yield return this;
        }
    }

    public class JsonCollectionNode : JsonNode
    {
        public ObservableCollection<JsonNode> Children { get; } = [];

        public void Clear()
        {
            Children.Clear();
        }

        public override string ValueString => $"<Collection count: {Children.Count}>";

        public override IEnumerator<JsonNode> GetEnumerator() => Children.GetEnumerator();
    }

    public class JsonNullNode : JsonValueNode
    {
        public override string ValueString => "null";

        public JsonNullNode() { Value = null; }
    }

    public class JsonBoolNode : JsonValueNode
    {
        public override string ValueString => Value is true ? "true" : "false";

        public JsonBoolNode(bool value) { Value = value; }
    }

    public class JsonNumberNode : JsonValueNode
    {
        public override string ValueString => $"{Value}";

        public JsonNumberNode(double value) { Value = value; }
    }

    public class JsonStringNode : JsonValueNode
    {
        public override string ValueString => $"\"{Value}\"";

        public JsonStringNode(string value) { Value = value; }
    }

    public class JsonArrayNode : JsonCollectionNode
    {
        public override string ValueString => IsExpanded ? "" : $"[ {Children.Count} ]";

        public void Add(JsonNode node)
        {
            Children.Add(node);
        }

        public JsonNode this[int index] => Children[index];
    }

    public class JsonObjectNode : JsonCollectionNode
    {
        public override string ValueString => IsExpanded ? "" : "{ ... }";

        public JsonNode this[string key]
        {
            get
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    if (Children[i].Name == key)
                    {
                        return Children[i];
                    }
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                value.Name = key;
                for (int i = 0; i < Children.Count; i++)
                {
                    if (Children[i].Name == key)
                    {
                        Children[i] = value;
                        return;
                    }
                }
                Children.Add(value);
            }
        }

        public void Sort(bool inherit = true)
        {
            List<JsonNode> list = [];
            for (int i = 0; i < Children.Count; i++)
            {
                if (inherit && Children[i] is JsonObjectNode on)
                {
                    on.Sort(inherit);
                }
                list.Add(Children[i]);
            }
            list.Sort((a, b) => a.Name.CompareTo(b.Name));
            Children.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                Children.Add(list[i]);
            }
        }
    }
}
