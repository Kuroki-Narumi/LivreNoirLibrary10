using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public abstract partial class DefListViewBase : CtListView
    {
        [DependencyProperty]
        private string? _editText;
        [DependencyProperty]
        private string? _moveUpText;
        [DependencyProperty]
        private string? _moveDownText;
    }
}
