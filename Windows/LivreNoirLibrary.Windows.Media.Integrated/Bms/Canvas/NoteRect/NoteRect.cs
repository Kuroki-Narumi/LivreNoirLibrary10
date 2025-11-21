using System;
using System.Windows;
using System.Windows.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class NoteRect : NoteRectBase
    {
        public NoteViewModel ViewModel
        {
            get;
            set
            {
                field = value;
                UpdateVisual();
            }
        }

        private NoteType _noteType;

        public bool IsInvisibleNote => _noteType is NoteType.Invisible;

        public NoteRect(NoteViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public void UpdateVisual()
        {
            var vm = ViewModel;
            AbsolutePosition = vm.AbsolutePosition;
            NoteLength = vm.Length;
            IsVisibleLane = vm.IsVisibleLane;
            _noteType = vm.Note is { } n ? n.Type : NoteType.Invalid;
            UpdateHorizontal(vm.X, vm.Width);
        }

        public void Render(DrawingContext ctx, INoteRectContainer provider)
        {
            var vm = ViewModel;
            var x = X;
            var y = Y;
            var w = Width;
            var length = Length;
            var color = vm.Color;
            var isInvisible = false;
            switch (_noteType)
            {
                case NoteType.Invisible:
                    isInvisible = true;
                    break;
                case NoteType.LongEnd:
                    color = provider.LongEndColor;
                    break;
                case NoteType.Mine:
                    color = provider.MineColor;
                    break;
            }

            if (isInvisible)
            {
                ctx.PushOpacity(0.5);
            }

            var headHeight = provider.HeadHeight;
            // ロングボディ
            if (length > headHeight)
            {
                Rect rect = new(x + 3, y + headHeight, w - 5, length - headHeight);
                ctx.DrawRectangle(MediaUtils.GetBrush(IsSelected ? provider.SelectedLongColor : vm.LongBody), null, rect);
            }
            y += length;
            // 本体
            if (IsSelected)
            {
                DrawSelectedHead(ctx, y, headHeight, provider.SelectedColor);
            }
            else
            {
                DrawHead(ctx, y, headHeight, color);
            }
            // インデックス
            if (provider.DisplaysValueText)
            {
                DrawText(ctx, y, vm.ValueText);
            }
            if (isInvisible)
            {
                ctx.Pop();
            }
            if (vm.HasProblem)
            {
                ctx.DrawRectangle(null, MediaUtils.GetPen(provider.InvalidColor, 2), new(x, y, w, headHeight));
                DrawIcon(ctx, x - 16, y - 8, Icons.Caution);
            }
        }

        public void RenderSelectionMoving(DrawingContext ctx, INoteRectContainer provider)
        {
            var x = X;
            var y = Y;
            var w = Width;
            var length = Length;
            var isInvisible = _noteType is NoteType.Invisible;
            if (isInvisible)
            {
                ctx.PushOpacity(0.5);
            }
            var headHeight = provider.HeadHeight;
            // ロングボディ
            if (length > headHeight)
            {
                Rect rect = new(x + 3, y + headHeight, w - 5, length - headHeight);
                ctx.DrawRectangle(MediaUtils.GetBrush(provider.SelectedLongColor), null, rect);
            }
            y += length;
            DrawSelectedHead(ctx, y, headHeight, provider.SelectedColor);
            if (isInvisible)
            {
                ctx.Pop();
            }
        }

        private static void DrawIcon(DrawingContext ctx, double x, double y, Drawing icon)
        {
            Matrix mat = new();
            mat.Scale(0.5, 0.5);
            mat.Translate(x, y);
            ctx.PushTransform(new MatrixTransform(mat));
            ctx.DrawDrawing(icon);
            ctx.Pop();
        }

        public double GetOffsetY(double y, int headHeight) => y - Y - Length - headHeight / 2;

        public override string ToString() => ViewModel.ToString();
    }
}
