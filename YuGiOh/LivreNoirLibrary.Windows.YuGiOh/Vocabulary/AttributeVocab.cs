using LivreNoirLibrary.YuGiOh;

namespace LivreNoirLibrary.Windows.YuGiOh.Vocabulary
{
    public class AttributeVocab : VocabBase
    {
        public VocabData Light { get => GetData(); set => SetData(value); }
        public VocabData Dark { get => GetData(); set => SetData(value); }
        public VocabData Water { get => GetData(); set => SetData(value); }
        public VocabData Fire { get => GetData(); set => SetData(value); }
        public VocabData Earth { get => GetData(); set => SetData(value); }
        public VocabData Wind { get => GetData(); set => SetData(value); }
        public VocabData Divine { get => GetData(); set => SetData(value); }

        public VocabData Light_S { get => GetData(); set => SetData(value); }
        public VocabData Dark_S { get => GetData(); set => SetData(value); }
        public VocabData Water_S { get => GetData(); set => SetData(value); }
        public VocabData Fire_S { get => GetData(); set => SetData(value); }
        public VocabData Earth_S { get => GetData(); set => SetData(value); }
        public VocabData Wind_S { get => GetData(); set => SetData(value); }
        public VocabData Divine_S { get => GetData(); set => SetData(value); }

        public void LoadDefault()
        {
            foreach (var attr in EnumUtils.EnumerateAttributes(true))
            {
                SetData(attr.ToString(), attr.GetName());
                SetData($"{attr}_S", attr.GetShortName());
            }
        }
    }
}
