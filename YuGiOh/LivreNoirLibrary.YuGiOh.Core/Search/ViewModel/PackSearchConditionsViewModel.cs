using System;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public class PackSearchConditionsViewModel : TextSearchConditionsViewModel
    {
        public NumberRange CardCount { get; } = new(0, 999, false, false);
        public DateRange Date { get; } = new();
        public bool Date_Ocg { get; set => SetValue(ref field, value); }
        public bool Date_Tcg { get; set => SetValue(ref field, value); }

        public void CopyFrom(PackSearchConditions conditions)
        {
            SearchText = conditions.SearchText;
            SetTextFlags(conditions.TextFlags);
            CardCount.CopyFrom(conditions.CardCount);
            Date.CopyFrom(conditions.Date);
            var dateLocale = conditions.DateLocale;
            Date_Ocg = (dateLocale & LocaleType.Ocg) is not 0;
            Date_Tcg = (dateLocale & LocaleType.Tcg) is not 0;
        }

        public void CopyTo(PackSearchConditions conditions)
        {
            conditions.SearchText = SearchText ?? "";
            conditions.TextFlags = GetTextFlags();
            conditions.CardCount.CopyFrom(CardCount);
            conditions.Date.CopyFrom(Date);
            var locale = LocaleType.None;
            if (Date_Ocg) locale |= LocaleType.Ocg;
            if (Date_Tcg) locale |= LocaleType.Tcg;
            conditions.DateLocale = locale;
        }
    }
}
