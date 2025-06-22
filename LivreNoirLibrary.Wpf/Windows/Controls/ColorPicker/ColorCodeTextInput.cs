using System;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public class ColorCodeTextInput : EditableTextBlock
    {
        protected override bool VerifyProtected(string? text) => ColorUtils.IsValidColorCode(text);

        protected override void ApplyText(string? text)
        {
            var oldValue = Text?.ToColor();
            Text = text;
            this.RaiseModifiedEvent(oldValue != Text?.ToColor());
        }
    }
}
