using System;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public class ModifyDependencyObject : DependencyObject
    {
        public EventHandler? Modified;

        protected void RaiseModified()
        {
            Modified?.Invoke(this, EventArgs.Empty);
        }
    }
}
