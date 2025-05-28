using System;
using System.Collections.Generic;
using System.Linq;

namespace LivreNoirLibrary.YuGiOh
{
    using DuelLog;

    public static partial class Enums
    {
        public static CardType[] CardTypes { get; } = [.. EnumCardType(true, true, true)];
        public static MonsterType[] MonsterTypes { get; } = [.. EnumMonsterType(true)];
        public static Attribute[] Attributes { get; } = [.. EnumAttribute(true)];
        public static Ability[] Abilities { get; } = [Ability.Pendulum, Ability.Tuner, Ability.SpecialSummon, Ability.Flip, Ability.Spirit, Ability.Union, Ability.Gemini, Ability.Toon];
        public static LinkDirection[] LinkDirections { get; } = Enum.GetValues<LinkDirection>();
        public static Rank[] Ranks { get; } = Enum.GetValues<Rank>();
        public static Order[] Orders { get; } = [Order.First, Order.Second, Order.CFirst, Order.CSecond];
        public static Result[] Results { get; } = Enum.GetValues<Result>();

        public static bool IsMonster(this CardType type) => (type & CardType.Type_Filter) is 0;
        public static bool IsSpell(this CardType type) => (type & CardType.Type_Filter) is CardType.Normal_Spell;
        public static bool IsTrap(this CardType type) => (type & CardType.Type_Filter) is CardType.Normal_Trap;
        public static bool IsMainDeck(this CardType type) => type is CardType.Main_Monster or CardType.Ritual_Monster or >= CardType.Normal_Spell;

        public static bool IsMainMonster(this CardType type) => type is CardType.Main_Monster;
        public static bool IsFusion(this CardType type) => type is CardType.Fusion_Monster;
        public static bool IsRitual(this CardType type) => type is CardType.Ritual_Monster;
        public static bool IsSynchro(this CardType type) => type is CardType.Synchro_Monster;
        public static bool IsXyz(this CardType type) => type is CardType.Xyz_Monster;
        public static bool IsLink(this CardType type) => type is CardType.Link_Monster;
        public static bool IsToken(this CardType type) => type is CardType.Token;
        public static bool HasLevel(this CardType type) => type is (>= CardType.Main_Monster and <= CardType.Synchro_Monster) or CardType.Token;
        public static bool HasDef(this CardType type) => type is >= CardType.Main_Monster and <= CardType.Xyz_Monster;
        public static bool IsMainDeckMonster(this CardType type) => type is CardType.Main_Monster or CardType.Ritual_Monster;
        public static bool IsExtraDeck(this CardType type) => type is CardType.Fusion_Monster or CardType.Synchro_Monster or CardType.Xyz_Monster or CardType.Link_Monster;

        public static bool IsPendulum(this Ability ability) => (ability & Ability.Pendulum) is not 0;
        public static bool IsTuner(this Ability ability) => (ability & Ability.Tuner) is not 0;
        public static bool IsSpecualSummon(this Ability ability) => (ability & Ability.SpecialSummon) is not 0;
        public static bool IsFlip(this Ability ability) => (ability & Ability.Flip) is not 0;
        public static bool IsSpirit(this Ability ability) => (ability & Ability.Spirit) is not 0;
        public static bool IsUnion(this Ability ability) => (ability & Ability.Union) is not 0;
        public static bool IsGemini(this Ability ability) => (ability & Ability.Gemini) is not 0;
        public static bool IsToon(this Ability ability) => (ability & Ability.Toon) is not 0;

        public static CardType Next(this CardType t) => t switch
        {
            CardType.Link_Monster => CardType.Normal_Spell,
            CardType.Ritual_Spell => CardType.Normal_Trap,
            >= CardType.Counter_Trap => CardType.Counter_Trap,
            _ => t + 1
        };

        public static CardType Previous(this CardType t) => t switch
        {
            <= CardType.Main_Monster => CardType.Main_Monster,
            CardType.Normal_Spell => CardType.Link_Monster,
            CardType.Normal_Trap => CardType.Ritual_Spell,
            _ => t - 1
        };

        public static Attribute Next(this Attribute t) => t is >= Attribute.Divine ? Attribute.Divine : t + 1;
        public static Attribute Previous(this Attribute t) => t is <= Attribute.Light ? Attribute.Light : t - 1;
        public static MonsterType Next(this MonsterType t) => t is >= MonsterType.CreatorGod ? MonsterType.CreatorGod : t + 1;
        public static MonsterType Previous(this MonsterType t) => t is <= MonsterType.Spellcaster ? MonsterType.Spellcaster : t - 1;

        public static int GetCount(this LinkDirection dir)
        {
            int result = 0;
            foreach (var d in LinkDirections)
            {
                if ((d & dir) is not 0)
                {
                    result++;
                }
            }
            return result;
        }

        public static bool IsValid(this LinkDirection dir)
        {
            return (uint)dir <= 255u;
        }

        public static IEnumerable<CardType> EnumCardType(bool monster = true, bool spell = true, bool trap = true)
        {
            if (monster)
            {
                for (var t = CardType.Main_Monster; t <= CardType.Link_Monster; t++)
                {
                    yield return t;
                }
            }
            if (spell)
            {
                for (var t = CardType.Normal_Spell; t <= CardType.Ritual_Spell; t++)
                {
                    yield return t;
                }
            }
            if (trap)
            {
                for (var t = CardType.Normal_Trap; t <= CardType.Counter_Trap; t++)
                {
                    yield return t;
                }
            }
        }

        public static IEnumerable<MonsterType> EnumMonsterType(bool includesSpecial = true)
        {
            var end = includesSpecial ? MonsterType.CreatorGod : MonsterType.DivineBeast - 1;
            for (var t = MonsterType.Spellcaster; t <= end; t++)
            {
                yield return t;
            }
        }

        public static IEnumerable<Attribute> EnumAttribute(bool includesSpecial = true)
        {
            var end = includesSpecial ? Attribute.Divine : Attribute.Wind;
            for (var a = Attribute.Light; a <= end; a++)
            {
                yield return a;
            }
        }

        public static IEnumerable<LinkDirection> EnumAllLinkDirection()
        {
            var max = (LinkDirection)255;
            for (var d = (LinkDirection)1; d <= max; d++)
            {
                yield return d;
            }
        }

    }
}
