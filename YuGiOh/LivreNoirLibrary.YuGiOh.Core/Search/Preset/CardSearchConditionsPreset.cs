
namespace LivreNoirLibrary.YuGiOh.Search
{
    public class CardSearchConditionsPreset : ConditionsPreset<CardSearchConditions>
    {
        public override void Copy(CardSearchConditions from, CardSearchConditions to)
        {
            CopyHashSet(from.CardTypes, to.CardTypes);
            CopyHashSet(from.Limits, to.Limits);
            CopyHashSet(from.MonsterTypes, to.MonsterTypes);
            to.StatusFlags = from.StatusFlags;
            to.Abilities = from.Abilities;
            to.AbilitiesExcept = from.AbilitiesExcept;
            CopyHashSet(from.Levels, to.Levels);
            to.Atk.CopyFrom(from.Atk);
            to.Def.CopyFrom(from.Def);
            CopyHashSet(from.PendulumScales, to.PendulumScales);
            to.LinkMarkers = from.LinkMarkers;
            to.StatusExpression = from.StatusExpression;
            to.OcgState = from.OcgState;
            to.TcgState = from.TcgState;
            to.FirstDate.CopyFrom(from.FirstDate);
            to.LastDate.CopyFrom(from.LastDate);
            to.DateLocale = from.DateLocale;
            to.TextLength.CopyFrom(from.TextLength);
            to.PTextLength.CopyFrom(from.PTextLength);
            to.TextFlags = from.TextFlags;
        }
    }
}
