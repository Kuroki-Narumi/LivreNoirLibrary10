using System;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public static partial class ICardExtensions
    {
        public static string NameWithBracket(this ICard obj) => $"《{obj.Name}》";

        public static string RemoveBracket(this string name) => Regex_Bracket.Replace(name, "$1");

        [GeneratedRegex(@"^《(.+)》$")]
        private static partial Regex Regex_Bracket { get; }

        extension(ICard card)
        {
            public bool IsMonster() => card.CardType.IsMonster();
            public bool IsSpell() => card.CardType.IsSpell();
            public bool IsTrap() => card.CardType.IsTrap();
            public bool IsMainDeck() => card.CardType.IsMainDeck();
            public bool IsMainDeckMonster() => card.CardType.IsMainDeckMonster();
            public bool IsExtraDeck() => card.CardType.IsExtraDeck();

            public bool IsMainMonster() => card.CardType.IsMainMonster();
            public bool IsFusion() => card.CardType.IsFusion();
            public bool IsRitual() => card.CardType.IsRitual();
            public bool IsSynchro() => card.CardType.IsSynchro();
            public bool IsXyz() => card.CardType.IsXyz();
            public bool IsLink() => card.CardType.IsLink();
            public bool IsToken() => card.CardType.IsToken();
            public bool HasLevel() => card.CardType.HasLevel();
            public bool HasDef() => card.CardType.HasDef();

            public bool IsPendulum() => card.Ability.IsPendulum();
            public bool IsTuner() => card.Ability.IsTuner();
            public bool IsSpecualSummon() => card.Ability.IsSpecualSummon();
            public bool IsFlip() => card.Ability.IsFlip();
            public bool IsSpirit() => card.Ability.IsSpirit();
            public bool IsUnion() => card.Ability.IsUnion();
            public bool IsGemini() => card.Ability.IsGemini();
            public bool IsToon() => card.Ability.IsToon();

            public bool IsOcgReleased() => card.PackInfo.ContainsOcg;
            public bool IsTcgReleased() => card.PackInfo.ContainsTcg;

            public LinkDirection GetLinkDirections() => card.IsLink() ? (LinkDirection)card.Def : 0;

            public void SetLinkDirections(LinkDirection direction)
            {
                if (card.ThisCard is { } c)
                {
                    c.CardType = CardType.Link_Monster;
                    c.Def = Math.Clamp((int)direction, 0, 255);
                    c.Level = direction.GetCount();
                    c.Ability &= ~(Ability.Pendulum | Ability.Flip | Ability.Gemini);
                }
            }

            public Media.CardIconType GetFrameType() => Media.Icons.GetIconType(card);
        }
    }
}