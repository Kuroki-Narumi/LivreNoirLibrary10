using System;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate bool VerifyTextEventHandler(string? text);

    public interface IVerifyText
    {
        event VerifyTextEventHandler? Verify;

        string? Text { get; set; }
        bool IsTextValid { get; }
    }
}
