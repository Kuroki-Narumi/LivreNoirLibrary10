using LivreNoirLibrary.Numerics;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BaseData : ObjectBase, IBmsData
    {
        public BaseData? Parent { get; internal set; }
        public HeaderCollection Headers { get; } = new();
        public DefListCollection DefLists { get; } = [];
        public BarLengthCollection Bars { get; } = [];
        public Timeline Timeline { get; } = [];
        public List<FlowContainer> Flows { get; } = [];

        IBmsData? IBmsData.Parent => Parent;
        IHeaderCollection IBmsData.Headers => Headers;
        IDefListCollection IBmsData.DefLists => DefLists;
        ITimeline IBmsData.Timeline => Timeline;

        public void Clear()
        {
            Headers.Clear();
            DefLists.Clear();
            Bars.Clear();
            Timeline.Clear();
            Flows.Clear();
        }

        public IEnumerable<BaseData> EachData()
        {
            yield return this;
            foreach (var flow in Flows)
            {
                foreach (var data in flow.EnumerateBranches())
                {
                    yield return data;
                }
            }
        }

        IEnumerable<IBmsData> IBmsData.EachData()
        {
            yield return this;
            foreach (var flow in Flows)
            {
                foreach (var data in flow.EnumerateBranches())
                {
                    yield return data;
                }
            }
        }

        public void ClearBarLength()
        {
            Bars.Clear();
            ClearBarLengthCache(0);
        }

        public Rational GetBarLength(int number)
        {
            if (Bars.TryGetValue(number, out var value))
            {
                return value;
            }
            else if (Parent is { } parent)
            {
                return Parent.GetBarLength(number);
            }
            return Constants.DefaultBarLengthR;
        }

        public void SetBarLength(int number, Rational value)
        {
            var defaultValue = Parent is { } p ? p.GetBarLength(number) : Constants.DefaultBarLengthR;
            if (value == defaultValue)
            {
                Bars.Remove(number);
            }
            else
            {
                Bars.Set(number, value);
            }
            ClearBarLengthCache(number);
        }

        public Rational GetHead(int number) => GetHead(number, this);
        public Rational GetAbsolutePosition(BarPosition position) => GetAbsolutePosition(position, this);
        public BarPosition GetBarPosition(Rational absolutePosition) => GetBarPosition(absolutePosition, this);
        public IEnumerable<BarInfo> EnumerateBars(int first, int last) => EnumerateBars(first, last, this);

        public void InsertBar(int number, Rational value)
        {
            foreach (var data in EachData())
            {
                data.Bars.Insert(number, value);
                data.Timeline.InsertBar(number);
            }
            ClearBarLengthCache(number);
        }

        public void DeleteBar(int number, int count)
        {
            foreach (var data in EachData())
            {
                data.Bars.Delete(number);
                data.Timeline.DeleteBar(number, count);
            }
            ClearBarLengthCache(number);
        }

        internal virtual void ClearBarLengthCache(int number) => Parent?.ClearBarLengthCache(number);
        internal virtual Rational GetHead(int number, IBarPositionProvider provider) => Parent!.GetHead(number, provider);
        internal virtual Rational GetAbsolutePosition(BarPosition position, IBarPositionProvider provider) => Parent!.GetAbsolutePosition(position, provider);
        internal virtual BarPosition GetBarPosition(Rational absolutePosition, IBarPositionProvider provider) => Parent!.GetBarPosition(absolutePosition, provider);
        internal virtual IEnumerable<BarInfo> EnumerateBars(int first, int last, IBarPositionProvider provider) => Parent!.EnumerateBars(first, last, provider);

        public void Merge(BaseData data)
        {
            Note += data.Note;
            Headers.Merge(data.Headers);
            DefLists.Merge(data.DefLists);
            Bars.Merge(data.Bars);
            data.Timeline.CopyTo(Timeline);
        }

        public void DumpHeader(BmsTextWriter writer)
        {
            var radix = writer.Radix;
            if (Headers.HasValue || radix is not Constants.Base_Default)
            {
                Headers.Dump(writer);
            }
            if (DefLists.HasValue)
            {
                DefLists.Dump(writer);
            }
        }

        internal void DumpMain(BinaryWriter writer)
        {
            WriteNote(writer);
            Headers.Dump(writer);
            DefLists.Dump(writer);
            Bars.Dump(writer);
            Timeline.Dump(writer);
            writer.Write(Flows.Count);
            foreach (var flow in CollectionsMarshal.AsSpan(Flows))
            {
                flow.Dump(writer);
            }
        }

        internal void LoadMain(BinaryReader reader)
        {
            Note = ReadNote(reader);
            Headers.ProcessLoad(reader);
            DefLists.ProcessLoad(reader);
            Bars.ProcessLoad(reader);
            Timeline.ReplaceBy(reader);
            Flows.Clear();
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                Flows.Add(FlowContainer.Load(reader, this));
            }
        }
    }
}
