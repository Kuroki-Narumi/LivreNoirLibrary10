
namespace LivreNoirLibrary.Media.Wave
{
    public readonly struct MarkerInfo(int index, string? name, long offset, long length)
    {
        public readonly int Index = index;
        public readonly string Name = name ?? "";
        public readonly long Offset = offset;
        public readonly long Length = length;

        public void Deconstruct(out int index, out string name, out long offset, out long length)
        {
            index = Index;
            name = Name;
            offset = Offset;
            length = Length;
        }
    }
}
