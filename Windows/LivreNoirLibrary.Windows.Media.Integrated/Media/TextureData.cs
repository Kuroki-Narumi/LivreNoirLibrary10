
namespace LivreNoirLibrary.Media
{
    public readonly record struct TextureData(string SourcePath, int X, int Y, int Width, int Height, int DivX, int DivY, double LoopPeriod)
    {
        public bool IsConstantSource => DivX * DivY is <= 1;
    }
}
