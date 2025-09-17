
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Wave
{
    public partial class Relation
    {
        public readonly struct Channel(Analysis.Channel ch1, Analysis.Channel ch2)
        {
            public const int DataSize = 3;

            public float WaveForm { get; } = LinearAlgebra.R2(ch1.Data, ch2.Data);
            public float RMS { get; } = LinearAlgebra.R2(ch1.MeanSquare, ch2.MeanSquare);
            public float Centroid { get; } = LinearAlgebra.R2(ch1.Centroids, ch2.Centroids);

            public override string ToString()
            {
                return $"{{RMS={RMS}, WaveForm={WaveForm}, Centroid={Centroid}}}";
            }
        }
    }
}
