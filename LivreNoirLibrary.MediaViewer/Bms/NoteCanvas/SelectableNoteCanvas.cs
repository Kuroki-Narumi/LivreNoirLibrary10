using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public abstract partial class SelectableNoteCanvas : NoteCanvasBase<NoteRect>
    {
        public const bool DefaultIsSelectedVisible = true;

        [DependencyProperty]
        private bool _isSelectedVisible = DefaultIsSelectedVisible;

        private void OnIsSelectedVisibleChanged()
        {
            InvalidateVisual();
        }
    }
}
