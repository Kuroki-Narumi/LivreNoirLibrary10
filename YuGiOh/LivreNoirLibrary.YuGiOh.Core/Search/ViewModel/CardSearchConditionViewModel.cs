using System;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class CardSearchConditionsViewModel : TextSearchConditionsViewModel
    {
        bool _cardType_monster_changing;
        bool _cardType_spell_changing;
        bool _cardType_trap_changing;
        int _cardType_monster_count;
        int _cardType_spell_count;
        int _cardType_trap_count;

        public bool? CardType_Monster
        {
            get;
            set
            {
                if (SetValue(ref field, value) && !_cardType_monster_changing)
                {
                    switch (value)
                    {
                        case true:
                            CardType_MainMonster =
                            CardType_FusionMonster =
                            CardType_RitualMonster =
                            CardType_SynchroMonster =
                            CardType_XyzMonster =
                            CardType_LinkMonster = true;
                            break;
                        case false:
                            CardType_MainMonster =
                            CardType_FusionMonster =
                            CardType_RitualMonster =
                            CardType_SynchroMonster =
                            CardType_XyzMonster =
                            CardType_LinkMonster = false;
                            break;
                    }
                }
            }
        } = false;

        public bool? CardType_Spell
        {
            get;
            set
            {
                if (SetValue(ref field, value) && !_cardType_spell_changing)
                {
                    switch (value)
                    {
                        case true:
                            CardType_NormalSpell =
                            CardType_FieldSpell =
                            CardType_EquipSpell =
                            CardType_ContinuousSpell =
                            CardType_QuickSpell =
                            CardType_RitualSpell = true;
                            break;
                        case false:
                            CardType_NormalSpell =
                            CardType_FieldSpell =
                            CardType_EquipSpell =
                            CardType_ContinuousSpell =
                            CardType_QuickSpell =
                            CardType_RitualSpell = false;
                            break;
                    }
                }
            }
        } = false;

        public bool? CardType_Trap
        {
            get;
            set
            {
                if (SetValue(ref field, value) && !_cardType_trap_changing)
                {
                    switch (value)
                    {
                        case true:
                            CardType_NormalTrap =
                            CardType_ContinuousTrap =
                            CardType_CounterTrap = true;
                            break;
                        case false:
                            CardType_NormalTrap =
                            CardType_ContinuousTrap =
                            CardType_CounterTrap = false;
                            break;
                    }
                }
            }
        } = false;

        public bool CardType_MainMonster { get; set => SetValue(ref field, value, OnCardTypeChanged_Monster); }
        public bool CardType_FusionMonster { get; set => SetValue(ref field, value, OnCardTypeChanged_Monster); }
        public bool CardType_RitualMonster { get; set => SetValue(ref field, value, OnCardTypeChanged_Monster); }
        public bool CardType_SynchroMonster { get; set => SetValue(ref field, value, OnCardTypeChanged_Monster); }
        public bool CardType_XyzMonster { get; set => SetValue(ref field, value, OnCardTypeChanged_Monster); }
        public bool CardType_LinkMonster { get; set => SetValue(ref field, value, OnCardTypeChanged_Monster); }

        public bool CardType_NormalSpell { get; set => SetValue(ref field, value, OnCardTypeChanged_Spell); }
        public bool CardType_FieldSpell { get; set => SetValue(ref field, value, OnCardTypeChanged_Spell); }
        public bool CardType_EquipSpell { get; set => SetValue(ref field, value, OnCardTypeChanged_Spell); }
        public bool CardType_ContinuousSpell { get; set => SetValue(ref field, value, OnCardTypeChanged_Spell); }
        public bool CardType_QuickSpell { get; set => SetValue(ref field, value, OnCardTypeChanged_Spell); }
        public bool CardType_RitualSpell { get; set => SetValue(ref field, value, OnCardTypeChanged_Spell); }

        public bool CardType_NormalTrap { get; set => SetValue(ref field, value, OnCardTypeChanged_Trap); }
        public bool CardType_ContinuousTrap { get; set => SetValue(ref field, value, OnCardTypeChanged_Trap); }
        public bool CardType_CounterTrap { get; set => SetValue(ref field, value, OnCardTypeChanged_Trap); }

        private void OnCardTypeChanged_Monster(bool _, bool newValue)
        {
            _cardType_monster_count += newValue ? 1 : -1;
            _cardType_monster_changing = true;
            CardType_Monster = _cardType_monster_count switch
            {
                0 => false,
                6 => true,
                _ => null,
            };
            _cardType_monster_changing = false;
        }

        private void OnCardTypeChanged_Spell(bool _, bool newValue)
        {
            _cardType_spell_count += newValue ? 1 : -1;
            _cardType_spell_changing = true;
            CardType_Spell = _cardType_spell_count switch
            {
                0 => false,
                6 => true,
                _ => null,
            };
            _cardType_spell_changing = false;
        }

        private void OnCardTypeChanged_Trap(bool _, bool newValue)
        {
            _cardType_trap_count += newValue ? 1 : -1;
            _cardType_trap_changing = true;
            CardType_Trap = _cardType_trap_count switch
            {
                0 => false,
                3 => true,
                _ => null,
            };
            _cardType_trap_changing = false;
        }

        public bool Limit_Forbidden { get; set => SetValue(ref field, value); }
        public bool Limit_Limit1 { get; set => SetValue(ref field, value); }
        public bool Limit_Limit2 { get; set => SetValue(ref field, value); }
        public bool Limit_Unlimited { get; set => SetValue(ref field, value); }
        public bool Limit_Unusable { get; set => SetValue(ref field, value); }
        public bool Limit_Specified { get; set => SetValue(ref field, value); }

        public bool Attribute_Light { get; set => SetValue(ref field, value); }
        public bool Attribute_Dark { get; set => SetValue(ref field, value); }
        public bool Attribute_Water { get; set => SetValue(ref field, value); }
        public bool Attribute_Fire { get; set => SetValue(ref field, value); }
        public bool Attribute_Earth { get; set => SetValue(ref field, value); }
        public bool Attribute_Wind { get; set => SetValue(ref field, value); }
        public bool Attribute_Divine { get; set => SetValue(ref field, value); }

        public bool MonsterType_Spellcaster { get; set => SetValue(ref field, value); }
        public bool MonsterType_Dragon { get; set => SetValue(ref field, value); }
        public bool MonsterType_Zombie { get; set => SetValue(ref field, value); }
        public bool MonsterType_Warrior { get; set => SetValue(ref field, value); }
        public bool MonsterType_BeastWarrior { get; set => SetValue(ref field, value); }
        public bool MonsterType_Beast { get; set => SetValue(ref field, value); }
        public bool MonsterType_WingedBeast { get; set => SetValue(ref field, value); }
        public bool MonsterType_Machine { get; set => SetValue(ref field, value); }
        public bool MonsterType_Fiend { get; set => SetValue(ref field, value); }
        public bool MonsterType_Fairy { get; set => SetValue(ref field, value); }
        public bool MonsterType_Insect { get; set => SetValue(ref field, value); }
        public bool MonsterType_Dinosaur { get; set => SetValue(ref field, value); }
        public bool MonsterType_Reptile { get; set => SetValue(ref field, value); }
        public bool MonsterType_Fish { get; set => SetValue(ref field, value); }
        public bool MonsterType_SeaSerpent { get; set => SetValue(ref field, value); }
        public bool MonsterType_Aqua { get; set => SetValue(ref field, value); }
        public bool MonsterType_Pyro { get; set => SetValue(ref field, value); }
        public bool MonsterType_Thunder { get; set => SetValue(ref field, value); }
        public bool MonsterType_Rock { get; set => SetValue(ref field, value); }
        public bool MonsterType_Plant { get; set => SetValue(ref field, value); }
        public bool MonsterType_Psychic { get; set => SetValue(ref field, value); }
        public bool MonsterType_Wyrm { get; set => SetValue(ref field, value); }
        public bool MonsterType_Cyberse { get; set => SetValue(ref field, value); }
        public bool MonsterType_Illusion { get; set => SetValue(ref field, value); }
        public bool MonsterType_DivineBeast { get; set => SetValue(ref field, value); }
        public bool MonsterType_CreatorGod { get; set => SetValue(ref field, value); }

        public bool Status_Normal { get; set => SetValue(ref field, value); }
        public bool Status_Effect { get; set => SetValue(ref field, value); }

        public bool Ability_Or { get; set => SetValue(ref field, value); }
        public bool Ability_And { get; set => SetValue(ref field, value); }
        public bool Ability_SpecialSummon { get; set => SetValue(ref field, value); }
        public bool Ability_Pendulum { get; set => SetValue(ref field, value); }
        public bool Ability_Toon { get; set => SetValue(ref field, value); }
        public bool Ability_Gemini { get; set => SetValue(ref field, value); }
        public bool Ability_Union { get; set => SetValue(ref field, value); }
        public bool Ability_Spirit { get; set => SetValue(ref field, value); }
        public bool Ability_Tuner { get; set => SetValue(ref field, value); }
        public bool Ability_Flip { get; set => SetValue(ref field, value); }

        public bool AbilityEx_SpecialSummon { get; set => SetValue(ref field, value); }
        public bool AbilityEx_Pendulum { get; set => SetValue(ref field, value); }
        public bool AbilityEx_Toon { get; set => SetValue(ref field, value); }
        public bool AbilityEx_Gemini { get; set => SetValue(ref field, value); }
        public bool AbilityEx_Union { get; set => SetValue(ref field, value); }
        public bool AbilityEx_Spirit { get; set => SetValue(ref field, value); }
        public bool AbilityEx_Tuner { get; set => SetValue(ref field, value); }
        public bool AbilityEx_Flip { get; set => SetValue(ref field, value); }

        public bool Level_0 { get; set => SetValue(ref field, value); }
        public bool Level_1 { get; set => SetValue(ref field, value); }
        public bool Level_2 { get; set => SetValue(ref field, value); }
        public bool Level_3 { get; set => SetValue(ref field, value); }
        public bool Level_4 { get; set => SetValue(ref field, value); }
        public bool Level_5 { get; set => SetValue(ref field, value); }
        public bool Level_6 { get; set => SetValue(ref field, value); }
        public bool Level_7 { get; set => SetValue(ref field, value); }
        public bool Level_8 { get; set => SetValue(ref field, value); }
        public bool Level_9 { get; set => SetValue(ref field, value); }
        public bool Level_10 { get; set => SetValue(ref field, value); }
        public bool Level_11 { get; set => SetValue(ref field, value); }
        public bool Level_12 { get; set => SetValue(ref field, value); }
        public bool Level_13 { get; set => SetValue(ref field, value); }

        public NumberRange Atk { get; } = new(-1, 5000, false, false);
        public NumberRange Def { get; } = new(-1, 5000, false, false);

        public bool PScale_0 { get; set => SetValue(ref field, value); }
        public bool PScale_1 { get; set => SetValue(ref field, value); }
        public bool PScale_2 { get; set => SetValue(ref field, value); }
        public bool PScale_3 { get; set => SetValue(ref field, value); }
        public bool PScale_4 { get; set => SetValue(ref field, value); }
        public bool PScale_5 { get; set => SetValue(ref field, value); }
        public bool PScale_6 { get; set => SetValue(ref field, value); }
        public bool PScale_7 { get; set => SetValue(ref field, value); }
        public bool PScale_8 { get; set => SetValue(ref field, value); }
        public bool PScale_9 { get; set => SetValue(ref field, value); }
        public bool PScale_10 { get; set => SetValue(ref field, value); }
        public bool PScale_11 { get; set => SetValue(ref field, value); }
        public bool PScale_12 { get; set => SetValue(ref field, value); }
        public bool PScale_13 { get; set => SetValue(ref field, value); }

        public bool LinkMarker_Or { get; set => SetValue(ref field, value); }
        public bool LinkMarker_And { get; set => SetValue(ref field, value); }
        public LinkDirection LinkMarkers { get; set => SetValue(ref field, value); }

        public bool IsStatusExpressionEnabled { get; set => SetValue(ref field, value); }
        public StatusExpression StatusExpression { get; } = new();

        public bool Locale_Any { get; set => SetValue(ref field, value); }
        public bool Locale_OcgExists { get; set => SetValue(ref field, value); }
        public bool Locale_OnlyOcg { get; set => SetValue(ref field, value); }
        public bool Locale_TcgExists { get; set => SetValue(ref field, value); }
        public bool Locale_OnlyTcg { get; set => SetValue(ref field, value); }
        public bool Locale_Both { get; set => SetValue(ref field, value); }

        public DateRange FirstDate { get; } = new();
        public DateRange LastDate { get; } = new();
        public bool Date_Ocg { get; set => SetValue(ref field, value); }
        public bool Date_Tcg { get; set => SetValue(ref field, value); }

        public NumberRange TextLength { get; } = new(0, 999, false, false);
        public NumberRange PTextLength { get; } = new(0, 999, false, false);

        public void CopyFrom(CardSearchConditions conditions)
        {
            var ctype = conditions.CardTypes;
            CardType_MainMonster = ctype.Contains(CardType.Main_Monster);
            CardType_FusionMonster = ctype.Contains(CardType.Fusion_Monster);
            CardType_RitualMonster = ctype.Contains(CardType.Ritual_Monster);
            CardType_SynchroMonster = ctype.Contains(CardType.Synchro_Monster);
            CardType_XyzMonster = ctype.Contains(CardType.Xyz_Monster);
            CardType_LinkMonster = ctype.Contains(CardType.Link_Monster);
            CardType_NormalSpell = ctype.Contains(CardType.Normal_Spell);
            CardType_FieldSpell = ctype.Contains(CardType.Field_Spell);
            CardType_EquipSpell = ctype.Contains(CardType.Equip_Spell);
            CardType_ContinuousSpell = ctype.Contains(CardType.Continuous_Spell);
            CardType_QuickSpell = ctype.Contains(CardType.Quick_Spell);
            CardType_RitualSpell = ctype.Contains(CardType.Ritual_Spell);
            CardType_NormalTrap = ctype.Contains(CardType.Normal_Trap);
            CardType_ContinuousTrap = ctype.Contains(CardType.Continuous_Trap);
            CardType_CounterTrap = ctype.Contains(CardType.Counter_Trap);

            var limits = conditions.Limits;
            Limit_Forbidden = limits.Contains(LimitCount.Forbidden);
            Limit_Limit1 = limits.Contains(LimitCount.Limit1);
            Limit_Limit2 = limits.Contains(LimitCount.Limit2);
            Limit_Unlimited = limits.Contains(LimitCount.Unlimited);
            Limit_Unusable = limits.Contains(LimitCount.Unusable);
            Limit_Specified = limits.Contains(LimitCount.Specified);

            var attr = conditions.Attributes;
            Attribute_Light = attr.Contains(Attribute.Light);
            Attribute_Dark = attr.Contains(Attribute.Dark);
            Attribute_Water = attr.Contains(Attribute.Water);
            Attribute_Fire = attr.Contains(Attribute.Fire);
            Attribute_Earth = attr.Contains(Attribute.Earth);
            Attribute_Wind = attr.Contains(Attribute.Wind);
            Attribute_Divine = attr.Contains(Attribute.Divine);

            var mtype = conditions.MonsterTypes;
            MonsterType_Spellcaster = mtype.Contains(MonsterType.Spellcaster);
            MonsterType_Dragon = mtype.Contains(MonsterType.Dragon);
            MonsterType_Zombie = mtype.Contains(MonsterType.Zombie);
            MonsterType_Warrior = mtype.Contains(MonsterType.Warrior);
            MonsterType_BeastWarrior = mtype.Contains(MonsterType.BeastWarrior);
            MonsterType_Beast = mtype.Contains(MonsterType.Beast);
            MonsterType_WingedBeast = mtype.Contains(MonsterType.WingedBeast);
            MonsterType_Machine = mtype.Contains(MonsterType.Machine);
            MonsterType_Fiend = mtype.Contains(MonsterType.Fiend);
            MonsterType_Fairy = mtype.Contains(MonsterType.Fairy);
            MonsterType_Insect = mtype.Contains(MonsterType.Insect);
            MonsterType_Dinosaur = mtype.Contains(MonsterType.Dinosaur);
            MonsterType_Reptile = mtype.Contains(MonsterType.Reptile);
            MonsterType_Fish = mtype.Contains(MonsterType.Fish);
            MonsterType_SeaSerpent = mtype.Contains(MonsterType.SeaSerpent);
            MonsterType_Aqua = mtype.Contains(MonsterType.Aqua);
            MonsterType_Pyro = mtype.Contains(MonsterType.Pyro);
            MonsterType_Thunder = mtype.Contains(MonsterType.Thunder);
            MonsterType_Rock = mtype.Contains(MonsterType.Rock);
            MonsterType_Plant = mtype.Contains(MonsterType.Plant);
            MonsterType_Psychic = mtype.Contains(MonsterType.Psychic);
            MonsterType_Wyrm = mtype.Contains(MonsterType.Wyrm);
            MonsterType_Cyberse = mtype.Contains(MonsterType.Cyberse);
            MonsterType_Illusion = mtype.Contains(MonsterType.Illusion);
            MonsterType_DivineBeast = mtype.Contains(MonsterType.DivineBeast);
            MonsterType_CreatorGod = mtype.Contains(MonsterType.CreatorGod);

            var stFlag = conditions.StatusFlags;
            Status_Normal = (stFlag & StatusFlags.Normal) is not 0;
            Status_Effect = (stFlag & StatusFlags.Effect) is not 0;

            var abiOr = (stFlag & StatusFlags.AbilityPerf) is 0;
            Ability_Or = abiOr;
            Ability_And = !abiOr;
            var abi = conditions.Abilities;
            Ability_SpecialSummon = (abi & Ability.SpecialSummon) is not 0;
            Ability_Pendulum = (abi & Ability.Pendulum) is not 0;
            Ability_Toon = (abi & Ability.Toon) is not 0;
            Ability_Gemini = (abi & Ability.Gemini) is not 0;
            Ability_Union = (abi & Ability.Union) is not 0;
            Ability_Spirit = (abi & Ability.Spirit) is not 0;
            Ability_Tuner = (abi & Ability.Tuner) is not 0;
            Ability_Flip = (abi & Ability.Flip) is not 0;
            abi = conditions.AbilitiesExcept;
            AbilityEx_SpecialSummon = (abi & Ability.SpecialSummon) is not 0;
            AbilityEx_Pendulum = (abi & Ability.Pendulum) is not 0;
            AbilityEx_Toon = (abi & Ability.Toon) is not 0;
            AbilityEx_Gemini = (abi & Ability.Gemini) is not 0;
            AbilityEx_Union = (abi & Ability.Union) is not 0;
            AbilityEx_Spirit = (abi & Ability.Spirit) is not 0;
            AbilityEx_Tuner = (abi & Ability.Tuner) is not 0;
            AbilityEx_Flip = (abi & Ability.Flip) is not 0;

            var level = conditions.Levels;
            Level_0 = level.Contains(0);
            Level_1 = level.Contains(1);
            Level_2 = level.Contains(2);
            Level_3 = level.Contains(3);
            Level_4 = level.Contains(4);
            Level_5 = level.Contains(5);
            Level_6 = level.Contains(6);
            Level_7 = level.Contains(7);
            Level_8 = level.Contains(8);
            Level_9 = level.Contains(9);
            Level_10 = level.Contains(10);
            Level_11 = level.Contains(11);
            Level_12 = level.Contains(12);
            Level_13 = level.Contains(13);

            Atk.CopyFrom(conditions.Atk);
            Def.CopyFrom(conditions.Def);

            var scale = conditions.PendulumScales;
            PScale_0 = scale.Contains(0);
            PScale_1 = scale.Contains(1);
            PScale_2 = scale.Contains(2);
            PScale_3 = scale.Contains(3);
            PScale_4 = scale.Contains(4);
            PScale_5 = scale.Contains(5);
            PScale_6 = scale.Contains(6);
            PScale_7 = scale.Contains(7);
            PScale_8 = scale.Contains(8);
            PScale_9 = scale.Contains(9);
            PScale_10 = scale.Contains(10);
            PScale_11 = scale.Contains(11);
            PScale_12 = scale.Contains(12);
            PScale_13 = scale.Contains(13);

            var linkOr = (stFlag & StatusFlags.LinkMarkerPerf) is 0;
            LinkMarker_Or = linkOr;
            LinkMarker_And = !linkOr;
            LinkMarkers = conditions.LinkMarkers;

            IsStatusExpressionEnabled = (stFlag & StatusFlags.StatusExpression) is not 0;
            StatusExpression.Expression = conditions.StatusExpression;

            var locale = (int)conditions.OcgState + (int)conditions.TcgState * 3;
            Locale_Any = locale is 0; // Any & Any
            Locale_OcgExists = locale is 1; // Released & Any
            Locale_OnlyOcg = locale is 6 or 7; // (Any | Released) & Unreleased
            Locale_TcgExists = locale is 3; // Any & Released
            Locale_OnlyTcg = locale is 2 or 5; // Unreleased & (Any | Released)
            Locale_Both = locale is 4; // Released & Released

            FirstDate.CopyFrom(conditions.FirstDate);
            LastDate.CopyFrom(conditions.LastDate);
            var dateLocale = conditions.DateLocale;
            Date_Ocg = (dateLocale & LocaleType.Ocg) is not 0;
            Date_Tcg = (dateLocale & LocaleType.Tcg) is not 0;

            TextLength.CopyFrom(conditions.TextLength);
            PTextLength.CopyFrom(conditions.PTextLength);

            SearchText = conditions.SearchText;
            SetTextFlags(conditions.TextFlags);
        }

        public void CopyTo(CardSearchConditions conditions)
        {
            var ctype = conditions.CardTypes;
            ctype.Clear();
            if (CardType_MainMonster) ctype.Add(CardType.Main_Monster);
            if (CardType_FusionMonster) ctype.Add(CardType.Fusion_Monster);
            if (CardType_RitualMonster) ctype.Add(CardType.Ritual_Monster);
            if (CardType_SynchroMonster) ctype.Add(CardType.Synchro_Monster);
            if (CardType_XyzMonster) ctype.Add(CardType.Xyz_Monster);
            if (CardType_LinkMonster) ctype.Add(CardType.Link_Monster);
            if (CardType_NormalSpell) ctype.Add(CardType.Normal_Spell);
            if (CardType_FieldSpell) ctype.Add(CardType.Field_Spell);
            if (CardType_EquipSpell) ctype.Add(CardType.Equip_Spell);
            if (CardType_ContinuousSpell) ctype.Add(CardType.Continuous_Spell);
            if (CardType_QuickSpell) ctype.Add(CardType.Quick_Spell);
            if (CardType_RitualSpell) ctype.Add(CardType.Ritual_Spell);
            if (CardType_NormalTrap) ctype.Add(CardType.Normal_Trap);
            if (CardType_ContinuousTrap) ctype.Add(CardType.Continuous_Trap);
            if (CardType_CounterTrap) ctype.Add(CardType.Counter_Trap);

            var limits = conditions.Limits;
            limits.Clear();
            if (Limit_Forbidden) limits.Add(LimitCount.Forbidden);
            if (Limit_Limit1) limits.Add(LimitCount.Limit1);
            if (Limit_Limit2) limits.Add(LimitCount.Limit2);
            if (Limit_Unlimited) limits.Add(LimitCount.Unlimited);
            if (Limit_Unusable) limits.Add(LimitCount.Unusable);
            if (Limit_Specified) limits.Add(LimitCount.Specified);

            var attr = conditions.Attributes;
            attr.Clear();
            if (Attribute_Light) attr.Add(Attribute.Light);
            if (Attribute_Dark) attr.Add(Attribute.Dark);
            if (Attribute_Water) attr.Add(Attribute.Water);
            if (Attribute_Fire) attr.Add(Attribute.Fire);
            if (Attribute_Earth) attr.Add(Attribute.Earth);
            if (Attribute_Wind) attr.Add(Attribute.Wind);
            if (Attribute_Divine) attr.Add(Attribute.Divine);

            var mtype = conditions.MonsterTypes;
            mtype.Clear();
            if (MonsterType_Spellcaster) mtype.Add(MonsterType.Spellcaster);
            if (MonsterType_Dragon) mtype.Add(MonsterType.Dragon);
            if (MonsterType_Zombie) mtype.Add(MonsterType.Zombie);
            if (MonsterType_Warrior) mtype.Add(MonsterType.Warrior);
            if (MonsterType_BeastWarrior) mtype.Add(MonsterType.BeastWarrior);
            if (MonsterType_Beast) mtype.Add(MonsterType.Beast);
            if (MonsterType_WingedBeast) mtype.Add(MonsterType.WingedBeast);
            if (MonsterType_Machine) mtype.Add(MonsterType.Machine);
            if (MonsterType_Fiend) mtype.Add(MonsterType.Fiend);
            if (MonsterType_Fairy) mtype.Add(MonsterType.Fairy);
            if (MonsterType_Insect) mtype.Add(MonsterType.Insect);
            if (MonsterType_Dinosaur) mtype.Add(MonsterType.Dinosaur);
            if (MonsterType_Reptile) mtype.Add(MonsterType.Reptile);
            if (MonsterType_Fish) mtype.Add(MonsterType.Fish);
            if (MonsterType_SeaSerpent) mtype.Add(MonsterType.SeaSerpent);
            if (MonsterType_Aqua) mtype.Add(MonsterType.Aqua);
            if (MonsterType_Pyro) mtype.Add(MonsterType.Pyro);
            if (MonsterType_Thunder) mtype.Add(MonsterType.Thunder);
            if (MonsterType_Rock) mtype.Add(MonsterType.Rock);
            if (MonsterType_Plant) mtype.Add(MonsterType.Plant);
            if (MonsterType_Psychic) mtype.Add(MonsterType.Psychic);
            if (MonsterType_Wyrm) mtype.Add(MonsterType.Wyrm);
            if (MonsterType_Cyberse) mtype.Add(MonsterType.Cyberse);
            if (MonsterType_Illusion) mtype.Add(MonsterType.Illusion);
            if (MonsterType_DivineBeast) mtype.Add(MonsterType.DivineBeast);
            if (MonsterType_CreatorGod) mtype.Add(MonsterType.CreatorGod);

            var sFlags = StatusFlags.None;
            if (Status_Normal) sFlags |= StatusFlags.Normal;
            if (Status_Effect) sFlags |= StatusFlags.Effect;
            if (Ability_And) sFlags |= StatusFlags.AbilityPerf;
            if (LinkMarker_And) sFlags |= StatusFlags.LinkMarkerPerf;
            if (IsStatusExpressionEnabled) sFlags |= StatusFlags.StatusExpression;
            conditions.StatusFlags = sFlags;

            var abi = Ability.Normal;
            if (Ability_SpecialSummon) abi |= Ability.SpecialSummon;
            if (Ability_Pendulum) abi |= Ability.Pendulum;
            if (Ability_Toon) abi |= Ability.Toon;
            if (Ability_Gemini) abi |= Ability.Gemini;
            if (Ability_Union) abi |= Ability.Union;
            if (Ability_Spirit) abi |= Ability.Spirit;
            if (Ability_Tuner) abi |= Ability.Tuner;
            if (Ability_Flip) abi |= Ability.Flip;
            conditions.Abilities = abi;
            abi = Ability.Normal;
            if (AbilityEx_SpecialSummon) abi |= Ability.SpecialSummon;
            if (AbilityEx_Pendulum) abi |= Ability.Pendulum;
            if (AbilityEx_Toon) abi |= Ability.Toon;
            if (AbilityEx_Gemini) abi |= Ability.Gemini;
            if (AbilityEx_Union) abi |= Ability.Union;
            if (AbilityEx_Spirit) abi |= Ability.Spirit;
            if (AbilityEx_Tuner) abi |= Ability.Tuner;
            if (AbilityEx_Flip) abi |= Ability.Flip;
            conditions.AbilitiesExcept = abi;

            var level = conditions.Levels;
            level.Clear();
            if (Level_0) level.Add(0);
            if (Level_1) level.Add(1);
            if (Level_2) level.Add(2);
            if (Level_3) level.Add(3);
            if (Level_4) level.Add(4);
            if (Level_5) level.Add(5);
            if (Level_6) level.Add(6);
            if (Level_7) level.Add(7);
            if (Level_8) level.Add(8);
            if (Level_9) level.Add(9);
            if (Level_10) level.Add(10);
            if (Level_11) level.Add(11);
            if (Level_12) level.Add(12);
            if (Level_13) level.Add(13);

            conditions.Atk.CopyFrom(Atk);
            conditions.Def.CopyFrom(Def);

            var scale = conditions.PendulumScales;
            scale.Clear();
            if (PScale_0) scale.Add(0);
            if (PScale_1) scale.Add(1);
            if (PScale_2) scale.Add(2);
            if (PScale_3) scale.Add(3);
            if (PScale_4) scale.Add(4);
            if (PScale_5) scale.Add(5);
            if (PScale_6) scale.Add(6);
            if (PScale_7) scale.Add(7);
            if (PScale_8) scale.Add(8);
            if (PScale_9) scale.Add(9);
            if (PScale_10) scale.Add(10);
            if (PScale_11) scale.Add(11);
            if (PScale_12) scale.Add(12);
            if (PScale_13) scale.Add(13);

            conditions.LinkMarkers = LinkMarkers;

            conditions.StatusExpression = StatusExpression.Expression ?? "";

            conditions.OcgState = Locale_OnlyTcg ? LocaleState.Unreleased :
                ((Locale_OcgExists || Locale_Both) ? LocaleState.Released : LocaleState.Any);
            conditions.TcgState = Locale_OnlyOcg ? LocaleState.Unreleased :
                ((Locale_TcgExists || Locale_Both) ? LocaleState.Released : LocaleState.Any);

            conditions.FirstDate.CopyFrom(FirstDate);
            conditions.LastDate.CopyFrom(LastDate);
            var locale = LocaleType.None;
            if (Date_Ocg) locale |= LocaleType.Ocg;
            if (Date_Tcg) locale |= LocaleType.Tcg;
            conditions.DateLocale = locale;

            conditions.TextLength.CopyFrom(TextLength);
            conditions.PTextLength.CopyFrom(PTextLength);

            conditions.SearchText = SearchText ?? "";
            conditions.TextFlags = GetTextFlags();
        }
    }
}
