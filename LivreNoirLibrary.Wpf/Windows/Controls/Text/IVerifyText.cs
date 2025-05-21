using System;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IVerifyText
    {
        public event VerifyTextEventHandler? Verify;

        public string? Text { get; set; }
        public bool IsTextValid { get; }
    }
}
