using LivreNoirLibrary.Debug;
using System;

namespace LivreNoirLibrary.GameOfLife
{
    public class FieldState
    {
        private readonly Dictionary<Position, int> _effectCount = [];

        public SortedSet<Position> LivingCells { get; } = [];

        public void UpdateEffect()
        {
            var dic = _effectCount;
            dic.Clear();
            foreach (var (x, y) in LivingCells)
            {
                AddEffect(dic, new(x - 1, y - 1));
                AddEffect(dic, new(x, y - 1));
                AddEffect(dic, new(x + 1, y - 1));
                AddEffect(dic, new(x - 1, y));
                dic.TryAdd(new(x, y), 0);
                AddEffect(dic, new(x + 1, y));
                AddEffect(dic, new(x - 1, y + 1));
                AddEffect(dic, new(x, y + 1));
                AddEffect(dic, new(x + 1, y + 1));
            }

            static void AddEffect(Dictionary<Position, int> dic, Position position)
            {
                if (dic.TryGetValue(position, out var count))
                {
                    dic[position] = count + 1;
                }
                else
                {
                    dic[position] = 1;
                }
            }
        }

        public void UpdateCells()
        {
            var cells = LivingCells;
            foreach (var (position, count) in _effectCount)
            {
                switch (count)
                {
                    case 3:
                        cells.Add(position);
                        break;
                    case < 2 or > 3:
                        cells.Remove(position);
                        break;
                }
            }
        }
    }
}