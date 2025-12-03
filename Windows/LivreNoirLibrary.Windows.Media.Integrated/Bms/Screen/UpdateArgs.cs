using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public readonly struct UpdateArgs
    {
        public readonly BmsTimer Timer;
        public readonly double AbsoluteTime;
        public readonly TimingList Timings;
        public readonly TextureCache Textures;
        public readonly MediaCache Media;
        public readonly NoteElementCollection Notes;
        public readonly BgaSource Bga;
        public readonly double HighSpeed;

        public UpdateArgs(BmsTimer timer, double absoluteTime, TimingList timings, TextureCache textures, MediaCache media, NoteElementCollection notes, BgaSource bga, double highSpeed)
        {
            Timer = timer;
            AbsoluteTime = absoluteTime;
            Timings = timings;
            Textures = textures;
            Media = media;
            Notes = notes;
            Bga = bga;
            HighSpeed = highSpeed;
        }
    }
}