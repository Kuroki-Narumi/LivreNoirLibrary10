using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public class FollowOwnerWindow : Window
    {
        /// <inheritdoc cref="Window.Owner"/>
        public new Window? Owner
        {
            get => base.Owner;
            set
            {
                if (base.Owner is Window current)
                {
                    current.LocationChanged -= OnOwnerLocationChanged;
                }
                base.Owner = value;
                if (value is not null)
                {
                    _owner_x = value.Left;
                    _owner_y = value.Top;
                    value.LocationChanged += OnOwnerLocationChanged;
                }
            }
        }

        private double _owner_x;
        private double _owner_y;

        private void OnOwnerLocationChanged(object? sender, EventArgs e)
        {
            var owner = base.Owner!;
            var x = owner.Left;
            var y = owner.Top;
            Left += x - _owner_x;
            Top += y - _owner_y;
            _owner_x = x;
            _owner_y = y;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (base.Owner is Window owner)
            {
                owner.LocationChanged -= OnOwnerLocationChanged;
            }
            base.OnClosed(e);
        }
    }
}
