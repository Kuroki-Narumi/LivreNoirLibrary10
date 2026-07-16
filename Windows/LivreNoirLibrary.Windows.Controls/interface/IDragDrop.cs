using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IDragDrop
    {
        WeakReference<object> DragSource { get; }
        Point DragStartPoint { get; set; }

        bool HandleMouseButtonEvent(object sender, MouseButtonEventArgs e) => false;
        void BuildDataObject(DataObject obj, object sender) { }
        bool CanDrop(IDataObject obj) => true;
        bool HandleDrop(IDataObject obj, object sender) => true;
    }

    public static class IDragDropExtensions
    {
        public const string DataObjectType_ProcessGuid = "LivreNoirLibrary.ProcessGuid";

        public static readonly byte[] ProcessGuid = Guid.NewGuid().ToByteArray();

        public static void IDragDrop_PreviewMouseLeftButtonDown<T>(this T owner, object sender, MouseButtonEventArgs e)
            where T : IInputElement, IDragDrop
        {
            owner.DragSource.SetTarget(sender);
            owner.DragStartPoint = e.GetPosition(owner);
            if (owner.HandleMouseButtonEvent(sender, e))
            {
                e.Handled = true;
            }
        }

        public static void IDragDrop_PreviewMouseMove<T>(this T owner, object sender, MouseEventArgs e)
            where T : DependencyObject, IInputElement, IDragDrop
        {
            if (owner.DragSource.TryGetTarget(out var target) && target == sender && 
                e.LeftButton is MouseButtonState.Pressed)
            {
                var pos1 = owner.DragStartPoint;
                var pos2 = e.GetPosition(owner);
                if (Math.Abs(pos1.X - pos2.X) < SystemParameters.MinimumHorizontalDragDistance &&
                    Math.Abs(pos1.Y - pos2.Y) < SystemParameters.MinimumVerticalDragDistance)
                {
                    return;
                }
                owner.DragSource.SetTarget(null!);
                e.Handled = true;
                var obj = new DataObject();
                obj.SetData(DataObjectType_ProcessGuid, ProcessGuid);
                owner.BuildDataObject(obj, sender);
                DragDrop.DoDragDrop(owner, obj, DragDropEffects.Move);
            }
        }

        public static void IDragDrop_DragOver(this IDragDrop owner, object sender, DragEventArgs e)
        {
            var data = e.Data;
            if (IsSameProcessGuid(data) && owner.CanDrop(data))
            {
                e.Handled = true;
                e.Effects = DragDropEffects.Move;
            }
        }

        public static void IDragDrop_Drop(this IDragDrop owner, object sender, DragEventArgs e)
        {
            var data = e.Data;
            if (IsSameProcessGuid(data) && owner.HandleDrop(data, sender))
            {
                e.Handled = true;
            }
        }

        private static bool IsSameProcessGuid(IDataObject data)
        {
            try
            {
                return data.GetDataPresent(DataObjectType_ProcessGuid) &&
                       data.GetData(DataObjectType_ProcessGuid) is byte[] guid &&
                       guid.SequenceEqual(ProcessGuid);
            }
            catch
            {
                return false;
            }
        }
    }
}
