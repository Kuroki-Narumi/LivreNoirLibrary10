using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract partial class CenteringPanelChildBase : ContentControl, ICenteringPanelChild
    {
        public event DecidedEventHandler? Decided;
        public event EventHandler? Closed;

        protected virtual UIElement? FirstElement => null;

        [DependencyProperty(SetterScope = ObjectModel.Scope.Protected)]
        private bool _canDecide = true;

        protected CenteringPanel? _parent;

        public CenteringPanelChildBase()
        {
            _canDecide = (bool)CanDecideProperty.GetMetadata(GetType()).DefaultValue;
            this.RegisterCommand(Commands.Decide, OnExecuted_Decide, CanExecute_Decide);
            this.RegisterCommand(Commands.AltDecide, OnExecuted_AltDecide, CanExecute_Decide);
            this.RegisterCommand(Commands.Cancel, OnExecuted_Cancel);
        }

        void ICenteringPanelChild.ReserveFocus()
        {
            this.SetDispatcher(() => FirstElement?.Focus());
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            if (_parent is not null)
            {
                _parent.Opened -= OnOpenedPrivate;
                _parent.Closed -= OnClosedPrivate;
            }
            if (this.FindAncestor(out _parent))
            {
                _parent.Opened += OnOpenedPrivate;
                _parent.Closed += OnClosedPrivate;
            }
        }

        private void OnOpenedPrivate(object sender, RoutedEventArgs e)
        {
            if (Visibility is Visibility.Visible)
            {
                OnOpened();
            }
        }

        private void OnClosedPrivate(object sender, RoutedEventArgs e)
        {
            if (Visibility is Visibility.Visible)
            {
                OnClosed();
                Closed?.Invoke(this, EventArgs.Empty);
            }
        }

        protected void InvokeDecided(bool alt = false)
        {
            Decided?.Invoke(this, alt ? DecidedEventArgs.AltDecide : DecidedEventArgs.Decide);
        }

        private void CanExecute_Decide(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = CanDecide;

        private void OnExecuted_Decide(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (CanDecide)
            {
                ProcessDecide();
                InvokeDecided(false);
            }
        }

        private void OnExecuted_AltDecide(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (CanDecide)
            {
                ProcessAltDecide();
                InvokeDecided(true);
            }
        }

        private void OnExecuted_Cancel(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            ProcessCancel();
        }

        public void Close() => _parent?.Close();

        protected virtual void ProcessDecide() => Close();
        protected virtual void ProcessAltDecide() => ProcessDecide();
        protected virtual void ProcessCancel() => Close();

        protected virtual void OnOpened() { }
        protected virtual void OnClosed() { }
    }
}
