using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Inspect;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract partial class HandInspectEditor_Base : FileEditorBase<HandInspectHistoryData>
    {
        [DependencyProperty]
        private HandConditionsCollection? _conditions;
        [DependencyProperty]
        private ICardProvider? _cardProvider;

        protected virtual void OnConditionsChanged(HandConditionsCollection? value)
        {
            this.ClearHistory();
        }
        
        protected sealed override HandInspectHistoryData GetHistoryData() => new(Conditions);
        protected sealed override void ProcessNew() => Conditions?.Clear();
        protected sealed override bool ProcessOpen(string path) => Conditions is { } conds && conds.LoadFile(path, CardProvider);
        protected sealed override void ProcessSave(string path)
        {
            if (Conditions is { } conds)
            {
                Json.Save(path, conds, true);
            }
        }
    }
}
