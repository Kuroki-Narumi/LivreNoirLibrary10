using LivreNoirLibrary.Media.VectorGraphics;
using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.YuGiOh.Data
{
    partial class Card
    {
        public int LimitCount { get; set => SetValue(ref field, value, [nameof(ActualLimitCount), nameof(LimitText), nameof(LimitIcon)]); } = YuGiOh.LimitCount.Unlimited;
        public int ActualLimitCount => Unusable ? YuGiOh.LimitCount.Unusable : LimitCount;
        public string LimitText => Vocab.GetLimitText(ActualLimitCount);
        public ElementGroup? LimitIcon => Media.Icons.GetLimitIcon(ActualLimitCount);

        public static bool TryGetCard(object? obj, ICardProvider? provider, [MaybeNullWhen(false)] out Card card)
        {
            card = obj switch
            {
                Card c => c,
                ICard c => c.ThisCard,
                ICardId c => provider?.GetOrDefault(c.Id),
                _ => null,
            };
            return card is not null;
        }
    }
}
