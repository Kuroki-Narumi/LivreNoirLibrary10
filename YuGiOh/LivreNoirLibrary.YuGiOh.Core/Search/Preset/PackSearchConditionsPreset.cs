
namespace LivreNoirLibrary.YuGiOh.Search
{
    public class PackSearchConditionsPreset : ConditionsPreset<PackSearchConditions>
    {
        public override void Copy(PackSearchConditions from, PackSearchConditions to)
        {
            to.TextFlags = from.TextFlags;
            to.CardCount.CopyFrom(from.CardCount);
            to.Date.CopyFrom(from.Date);
            to.DateLocale = from.DateLocale;
        }
    }
}
