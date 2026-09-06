using LivreNoirLibrary.Collections;
using LivreNoirLibrary.YuGiOh.Search;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class NumbersFlagCollection : CheckableItemCollection<int, NumbersFlag>
    {
        protected override NumbersFlag CreateItem() => new();
        protected override int GetKey(NumbersFlag item) => item.Number;

        public void RefreshItems(IEnumerable<int> numbers) => RefreshItems(numbers, InitializeFlag);

        private static void InitializeFlag(NumbersFlag item, int number) => item.Number = number;

        public bool Contains(NumbersKey obj)
        {
            var set = CheckedItems;
            return set.Count is 0 || set.Contains(obj.Value1) || set.Contains(obj.Value2) || set.Contains(obj.Value3) || set.Contains(obj.Value4);
        }

        public bool IsMatch(NumbersKey obj, MatchType type)
        {
            var set = CheckedItems;
            var setCount = set.Count;
            if (setCount is 0)
            {
                return true;
            }
            var matchCount = 0;
            if (set.Contains(obj.Value1)) matchCount++;
            if (set.Contains(obj.Value2)) matchCount++;
            if (set.Contains(obj.Value3)) matchCount++;
            if (set.Contains(obj.Value4)) matchCount++;
            return type switch
            {
                MatchType.All => matchCount == setCount,
                MatchType.Minimum => matchCount is 4,
                MatchType.Perfect => matchCount is 4 && setCount is 4,
                _ => matchCount is > 0,
            };
        }
    }
}
