using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.MasterDuel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public class PackInfoVocab : VocabBase
    {
        public VocabData Id { get => GetData(); set => SetData(value); }
        public VocabData Name { get => GetData(); set => SetData(value); }
        public VocabData Date { get => GetData(); set => SetData(value); }
        public VocabData Include { get => GetData(); set => SetData(value); }

        public void LoadDefault()
        {
            SetData(nameof(Id), "ID");
            SetData(nameof(Name), "パック名");
            SetData(nameof(Date), "発売日");
            SetData(nameof(Include), "収録カード");
        }
    }
}
