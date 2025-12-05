using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public readonly struct UpdateArgs(
        Skin skin,
        IVariableProvider variableProvider,
        BmsTimer timer, 
        double absoluteTime, 
        TimingList timings, 
        TextureCache textures, 
        MediaCache media, 
        NoteElementCollection notes, 
        BgaSource bga, 
        ScoreInfo score,
        double highSpeed)
    {
        public readonly Skin Skin = skin;
        public readonly IVariableProvider VariableProvider = variableProvider;
        public readonly BmsTimer Timer = timer;
        public readonly double AbsoluteTime = absoluteTime;
        public readonly TimingList Timings = timings;
        public readonly TextureCache Textures = textures;
        public readonly MediaCache Media = media;
        public readonly NoteElementCollection Notes = notes;
        public readonly BgaSource Bga = bga;
        public readonly ScoreInfo Score = score;
        public readonly double HighSpeed = highSpeed;
    }
}