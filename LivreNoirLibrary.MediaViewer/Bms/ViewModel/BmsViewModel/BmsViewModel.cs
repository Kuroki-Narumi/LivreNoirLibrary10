using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BmsViewModel : ObservableObjectBase
    {
        [ObservableProperty]
        internal BmsData _root;
        [ObservableProperty(SetterScope = Scope.Private)]
        private BaseData _currentData;

        public BmsViewModel()
        {
            _history = new(this);
            _root = BmsData.Create();
            _currentData = _root;
        }

        private void OnRootChanged(BmsData value)
        {
            _history.Initialize();
            ClearStack();
            CurrentData = value;
        }

        private void OnCurrentDataChanged(BaseData value)
        {
            _selection.Clear();
            ForceInherit();
            RefreshHeaders(value);
            RefreshBars(value);
            RefreshDefList(value);
            RefreshNotes(value);
        }
    }
}
