using System;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public class SortKeyVocab : VocabBase
    {
        public VocabData OpenBracket { get; }
        public VocabData CloseBracket { get; }

        public MergedVocabData NameLength { get; }
        public MergedVocabData RubyLength { get; }
        public MergedVocabData EnNameLength { get; }
        public MergedVocabData TextLength { get; }
        public MergedVocabData PTextLength { get; }
        public MergedVocabData FirstDateOcg { get; }
        public MergedVocabData FirstDateTcg { get; }
        public MergedVocabData LastDateOcg { get; }
        public MergedVocabData LastDateTcg { get; }
        public MergedVocabData PackInfoOcg { get; }
        public MergedVocabData PackInfoTcg { get; }

        public SortKeyVocab(Vocab parent)
        {
            OpenBracket = "(";
            CloseBracket = ")";

            NameLength = new([parent.CInfo.Name, parent.CharCount]);
            RubyLength = new([parent.CInfo.Ruby, parent.CharCount]);
            EnNameLength = new([parent.CInfo.EnName, parent.CharCount]);
            TextLength = new([parent.CInfo.Text, parent.CharCount]);
            PTextLength = new([parent.CInfo.PendulumText, parent.CharCount]);
            FirstDateOcg = new([parent.Publish_First, OpenBracket, parent.Ocg, CloseBracket]);
            FirstDateTcg = new([parent.Publish_First, OpenBracket, parent.Tcg, CloseBracket]);
            LastDateOcg = new([parent.Publish_Latest, OpenBracket, parent.Ocg, CloseBracket]);
            LastDateTcg = new([parent.Publish_Latest, OpenBracket, parent.Tcg, CloseBracket]);
            PackInfoOcg = new([parent.PublishDate, OpenBracket, parent.Ocg, CloseBracket]);
            PackInfoTcg = new([parent.PublishDate, OpenBracket, parent.Tcg, CloseBracket]);
        }
    }
}
