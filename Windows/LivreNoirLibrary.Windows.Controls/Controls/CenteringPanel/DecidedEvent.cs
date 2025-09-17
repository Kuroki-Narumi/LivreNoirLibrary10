using System;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate void DecidedEventHandler(object sender, DecidedEventArgs e);

    public class DecidedEventArgs : EventArgs
    {
        public static readonly DecidedEventArgs Decide = new(false);
        public static readonly DecidedEventArgs AltDecide = new(true);

        public bool Alt { get; }

        private DecidedEventArgs(bool alt) { Alt = alt; }
    }
}
