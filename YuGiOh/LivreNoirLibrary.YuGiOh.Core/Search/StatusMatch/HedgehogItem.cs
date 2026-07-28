using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class HedgehogItem : IClear
    {
        public HedgehogKey Key { get; set; }
        public List<Card> NormalMonsters { get; } = [];
        public List<Card> EffectMonsters { get; } = [];

        public MonsterType MonsterType => Key.MonsterType;
        public Attribute Attribute => Key.Attribute;
        public int Level => Key.Level;

        public string MonsterTypeText => Vocab.GetName(Key.MonsterType);
        public string AttributeText => Vocab.GetName(Key.Attribute);
        public string AttrText => Vocab.GetShortName(Key.Attribute);
        public string LevelText => Vocab.GetStatusText(Key.Level);
        public int NormalCount => NormalMonsters.Count;
        public int EffectCount => EffectMonsters.Count;

        public HedgehogItem ThisCard => this;

        public void Clear()
        {
            NormalMonsters.Clear();
            EffectMonsters.Clear();
        }
    }
}
