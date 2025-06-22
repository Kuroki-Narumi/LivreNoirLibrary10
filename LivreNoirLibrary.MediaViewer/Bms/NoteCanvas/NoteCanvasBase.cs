using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public abstract partial class NoteCanvasBase : BmsCanvasBase
    {
        [DependencyProperty(AffectsRender = true)]
        private Color _mineColor = Colors.Note_Mine;
        [DependencyProperty(AffectsRender = true)]
        private Color _longEndColor = Colors.Note_LongEnd;
        [DependencyProperty(AffectsRender = true)]
        private Color _invalidColor = Colors.Note_Invalid;
        [DependencyProperty(AffectsRender = true)]
        private Color _selectedColor = Colors.Selected;
        [DependencyProperty(AffectsRender = true)]
        private Color _selectedLongColor = Colors.SelectedLong;
    }
}
