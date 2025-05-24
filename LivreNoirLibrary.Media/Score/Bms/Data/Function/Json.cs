using System;
using System.Collections.Generic;
using System.Text.Json;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Numerics;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BaseData : IJsonWriter
    {
        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            if (Comments.Count > 0)
            {
                writer.WritePropertyName("comments");
                writer.WriteStartArray();
                for (int i = 0; i < Comments.Count; i++)
                {
                    writer.WriteStringValue(Comments[i]);
                }
                writer.WriteEndArray();
            }
            if (!Headers.IsEmpty())
            {
                writer.WritePropertyName("headers");
                Headers.WriteJson(writer, options);
            }
            if (DefLists.Count is > 0)
            {
                writer.WritePropertyName("definitions");
                DefLists.WriteJson(writer, Base);
            }
            foreach (var item in EnumBars())
            {
                writer.WritePropertyName(item.Bar.ToString());
                item.WriteJson(writer, options);
            }
            writer.WriteEndObject();
        }

        protected IEnumerable<EnumPositionData> EnumBars(Predicate<Note>? noteSelector = null)
        {
            noteSelector ??= n => true;
            var notes = Timeline.GetEnumerator();
            var exists = notes.MoveNext();

            var position = Rational.Zero;
            EnumPositionData data = new();
            for (int barNumber = 0; exists; barNumber++)
            {
                if (!Bars.TryGetValue(barNumber, out var length))
                {
                    length = Rational.Zero;
                }
                data.Init(barNumber, position, length);
                position += length;

                while (exists)
                {
                    var (pos, note) = notes.Current;
                    if (pos.Bar > barNumber) { break; }
                    var beat = pos.Beat;
                    if (note.IsTempo())
                    {
                        data.Tempo.Add((beat, note.Value));
                    }
                    else if (note.IsStop())
                    {
                        data.Stop.Add((beat, note.Value));
                    }
                    else if (note.IsScroll())
                    {
                        data.Scroll.Add((beat, note.Value));
                    }
                    else if (note.IsSpeed())
                    {
                        data.Speed.Add((beat, note.Value));
                    }
                    else if (noteSelector(note))
                    {
                        data.Notes.Add((beat, note));
                    }
                    exists = notes.MoveNext();
                }
                if (data.Exists())
                {
                    yield return data;
                }
            }
        }

        protected class EnumPositionData : IJsonWriter
        {
            public int Bar { get; private set; }
            public Rational Head { get; private set; }
            public Rational Length { get; private set; }

            public List<(Rational, Rational)> Tempo { get; } = [];
            public List<(Rational, Rational)> Stop { get; } = [];
            public List<(Rational, Rational)> Scroll { get; } = [];
            public List<(Rational, Rational)> Speed { get; } = [];
            public List<(Rational, Note)> Notes { get; } = [];

            public void Init(int barNumber, Rational head, Rational length)
            {
                Bar = barNumber;
                Head = head;
                Length = length;
                Tempo.Clear();
                Stop.Clear();
                Scroll.Clear();
                Speed.Clear();
                Notes.Clear();
            }

            public bool Exists() => !Length.IsZero() || Tempo.Count is > 0 || Stop.Count is > 0 || Scroll.Count is > 0 || Speed.Count is > 0 || Notes.Count is > 0;

            public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                if (Length != 0)
                {
                    writer.WriteString("length", Length.ToString());
                }
                var c = Tempo.Count;
                if (c is > 0)
                {
                    writer.WritePropertyName("tempo");
                    writer.WriteStartArray();
                    foreach (var (p, v) in CollectionsMarshal.AsSpan(Tempo))
                    {
                        writer.WriteString(p.ToString(), ((decimal)v).ToString());
                    }
                    writer.WriteEndArray();
                }
                c = Stop.Count;
                if (c is > 0)
                {
                    writer.WritePropertyName("stop");
                    writer.WriteStartArray();
                    foreach (var (p, v) in CollectionsMarshal.AsSpan(Stop))
                    {
                        writer.WriteString(p.ToString(), v.ToString());
                    }
                    writer.WriteEndArray();
                }
                c = Scroll.Count;
                if (c is > 0)
                {
                    writer.WritePropertyName("scroll");
                    writer.WriteStartArray();
                    foreach (var (p, v) in CollectionsMarshal.AsSpan(Scroll))
                    {
                        writer.WriteString(p.ToString(), ((decimal)v).ToString());
                    }
                    writer.WriteEndArray();
                }
                c = Speed.Count;
                if (c is > 0)
                {
                    writer.WritePropertyName("speed");
                    writer.WriteStartArray();
                    foreach (var (p, v) in CollectionsMarshal.AsSpan(Speed))
                    {
                        writer.WriteString(p.ToString(), ((decimal)v).ToString());
                    }
                    writer.WriteEndArray();
                }
                c = Notes.Count;
                if (c is > 0)
                {
                    writer.WritePropertyName("notes");
                    writer.WriteStartArray();
                    foreach (var (p, v) in CollectionsMarshal.AsSpan(Notes))
                    {
                        writer.WritePropertyName(p.ToString());
                        v.WriteJson(writer, options);
                    }
                    writer.WriteEndArray();
                }
                writer.WriteEndObject();
            }
        }
    }
}
