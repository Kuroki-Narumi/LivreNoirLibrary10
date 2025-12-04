using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public readonly struct UpdateArgs(
        BmsTimer timer, 
        double absoluteTime, 
        TimingList timings, 
        TextureCache textures, 
        MediaCache media, 
        NoteElementCollection notes, 
        BgaSource bga, 
        JudgeInfo judge,
        double highSpeed)
    {
        public readonly BmsTimer Timer = timer;
        public readonly double AbsoluteTime = absoluteTime;
        public readonly TimingList Timings = timings;
        public readonly TextureCache Textures = textures;
        public readonly MediaCache Media = media;
        public readonly NoteElementCollection Notes = notes;
        public readonly BgaSource Bga = bga;
        public readonly JudgeInfo Judge = judge;
        public readonly double HighSpeed = highSpeed;
    }
}