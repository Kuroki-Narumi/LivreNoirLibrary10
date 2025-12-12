using System;
using System.Windows;
using System.Windows.Media;
using Colors = LivreNoirLibrary.Windows.Media.Bms.Colors;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public abstract partial class NoteCanvasBase : BmsCanvasBase, INoteRectContainer
    {
        public const int DefaultHeadHeight = 10;

        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private int _headHeight = DefaultHeadHeight;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private bool _displaysValueText = true;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private Color _mineColor = Colors.Note_Mine;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private Color _longEndColor = Colors.Note_LongEnd;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private Color _selectedColor = Colors.Selected;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private Color _selectedLongColor = Colors.SelectedLong;
        [DependencyProperty(BindsTwoWayByDefault = true, AffectsRender = true)]
        private Color _invalidColor = Colors.Note_Invalid;

        public Rect GetLogicalRect(in Rect visualRect)
        {
            return new(visualRect.X, GetAbsolutePosition(visualRect.Bottom + _headHeight), visualRect.Width, GetAbsolutePosition(visualRect.Top));
        }
    }
}
