using System;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BmsViewModel
    {
        private readonly BmsInfo _info = new();

        public BmsInfo GetInfo()
        {
            _info.Refresh(_root, _currentData, _selection);
            return _info;
        }
    }
}
