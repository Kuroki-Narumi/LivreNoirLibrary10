using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IAssemblePreviewOptions : IAssembleCoreOptions, IAssemblePlaysLongEndOptions
    {
        public BarPosition PreviewStart { get; }
        public double PreviewFadeIn { get; }
        public double PreviewBody { get; }
        public double PreviewFadeOut { get; }
    }
}
