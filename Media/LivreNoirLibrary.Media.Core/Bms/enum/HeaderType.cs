
namespace LivreNoirLibrary.Media.Bms
{
    public enum HeaderType : byte
    {
        Unknown = 0,
        Player, Genre, Title, SubTitle, Artist, SubArtist,
        Bpm, PlayLevel, Difficulty, Rank, Total,
        StageFile, Banner, BackBmp, 
        Preview, LnObj, LnMode, DefExRank, Comment, Base,
    }
}
