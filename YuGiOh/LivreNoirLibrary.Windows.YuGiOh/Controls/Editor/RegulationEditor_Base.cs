using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract partial class RegulationEditor_Base : FileEditorBase<RegulationHistoryData>
    {
        [DependencyProperty]
        private ICardProvider? _cardProvider;
        [DependencyProperty]
        private Regulation? _regulation;

        protected virtual void OnCardProviderChanged(ICardProvider? value) { }

        protected virtual void OnRegulationChanged(Regulation? value)
        {
            this.ClearHistory();
        }

        protected sealed override RegulationHistoryData GetHistoryData() => new(Regulation);
        protected sealed override void ProcessNew() => Regulation?.Clear();
        protected sealed override bool ProcessOpen(string path) => Regulation is { } r && r.LoadFile(path, CardProvider);
        protected sealed override void ProcessSave(string path) => Json.Save(path, Regulation);
    }
}
