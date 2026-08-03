using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public abstract class StatisticsItemBase : IClear
    {
        public int Count { get; set; }
        public int ValidCount { get; set; }

        protected readonly Dictionary<Order, int> _order = [];
        protected readonly Dictionary<Result, int> _result = [];
        protected readonly Dictionary<Order, int> _win = [];
        protected readonly Dictionary<Order, int> _win_den = [];

        public int First { get => _order.GetValueOrDefault(Order.First); set => _order[Order.First] = value; }
        public int Second { get => _order.GetValueOrDefault(Order.Second); set => _order[Order.Second] = value; }
        public int CFirst { get => _order.GetValueOrDefault(Order.CFirst); set => _order[Order.CFirst] = value; }
        public int CSecond { get => _order.GetValueOrDefault(Order.CSecond); set => _order[Order.CSecond] = value; }

        public int Lose { get => _result.GetValueOrDefault(Result.Lose); set => _result[Result.Lose] = value; }
        public int Win { get => _result.GetValueOrDefault(Result.Win); set => _result[Result.Win] = value; }
        public int Draw { get => _result.GetValueOrDefault(Result.Draw); set => _result[Result.Draw] = value; }
        public int DiscLose { get => _result.GetValueOrDefault(Result.DiscLose); set => _result[Result.DiscLose] = value; }
        public int DiscWin { get => _result.GetValueOrDefault(Result.DiscWin); set => _result[Result.DiscWin] = value; }

        public int WinLike => Win + DiscWin;

        public int FirstWin { get => _win.GetValueOrDefault(Order.First); set => _win[Order.First] = value; }
        public int SecondWin { get => _win.GetValueOrDefault(Order.Second); set => _win[Order.Second] = value; }
        public int CFirstWin { get => _win.GetValueOrDefault(Order.CFirst); set => _win[Order.CFirst] = value; }
        public int CSecondWin { get => _win.GetValueOrDefault(Order.CSecond); set => _win[Order.CSecond] = value; }

        public double CountRatio { get; set; }
        public string CountRatioText => GetRatioText(CountRatio);

        public double FirstRatio => GetRatio(First, Count);
        public double SecondRatio => GetRatio(Second, Count);
        public double CFirstRatio => GetRatio(CFirst, Count);
        public double CSecondRatio => GetRatio(CSecond, Count);

        public double LoseRatio => GetRatio(Lose, ValidCount);
        public double WinRatio => GetRatio(Win, ValidCount);
        public double DrawRatio => GetRatio(Draw, ValidCount);
        public double DiscLoseRatio => GetRatio(DiscLose, Count);
        public double DiscWinRatio => GetRatio(DiscWin, Count);
        public double WinLikeRatio => GetRatio(WinLike, Count);

        public double FirstWinRatio => GetRatio(FirstWin, _win_den.GetValueOrDefault(Order.First));
        public double SecondWinRatio => GetRatio(SecondWin, _win_den.GetValueOrDefault(Order.Second));
        public double CFirstWinRatio => GetRatio(CFirstWin, _win_den.GetValueOrDefault(Order.CFirst));
        public double CSecondWinRatio => GetRatio(CSecondWin, _win_den.GetValueOrDefault(Order.CSecond));

        public string FirstRatioText => GetRatioText(FirstRatio);
        public string SecondRatioText => GetRatioText(SecondRatio);
        public string CFirstRatioText => GetRatioText(CFirstRatio);
        public string CSecondRatioText => GetRatioText(CSecondRatio);

        public string LoseRatioText => GetRatioText(LoseRatio);
        public string WinRatioText => GetRatioText(WinRatio);
        public string DrawRatioText => GetRatioText(DrawRatio);
        public string DiscLoseRatioText => GetRatioText(DiscLoseRatio);
        public string DiscWinRatioText => GetRatioText(DiscWinRatio);
        public string WinLikeRatioText => GetRatioText(WinLikeRatio);

        public string FirstWinRatioText => GetRatioText(FirstWinRatio);
        public string SecondWinRatioText => GetRatioText(SecondWinRatio);
        public string CFirstWinRatioText => GetRatioText(CFirstWinRatio);
        public string CSecondWinRatioText => GetRatioText(CSecondWinRatio);

        public virtual void Clear()
        {
            Count = 0;
            ValidCount = 0;
            _order.Clear();
            _result.Clear();
            _win.Clear();
            _win_den.Clear();
            CountRatio = 0;
        }

        public void Append(DuelLog item)
        {
            Count++;
            var result = item.Result;
            var order = item.Order;
            if (result is not (Result.DiscLose or Result.DiscWin))
            {
                ValidCount++;
                _win_den[order] = _win_den.GetValueOrDefault(order) + 1;
            }
            _order[order] = _order.GetValueOrDefault(order) + 1;
            _result[result] = _result.GetValueOrDefault(result) + 1;
            if (result is Result.Win)
            {
                _win[order] = _win.GetValueOrDefault(order) + 1;
            }
        }

        public static double GetRatio(int value, int total) => value is <= 0 || total is <= 0 ? 0 : value * 100.0 / total;
        public static string GetRatioText(double value) => value is <= 0 ? "" : value is >= 100 ? "100" : value.ToString("0.0");

        public virtual void AppendLine(StringBuilder sb)
        {
            sb.Append(Count);
            sb.Append('\t');
            sb.Append(CountRatioText);
            sb.Append('\t');
            sb.Append(Win);
            sb.Append('\t');
            sb.Append(WinRatioText);
            sb.Append('\t');
            sb.Append(Lose);
            sb.Append('\t');
            sb.Append(LoseRatioText);
            sb.Append('\t');
            sb.Append(Draw);
            sb.Append('\t');
            sb.Append(DrawRatioText);
            sb.Append('\t');
            sb.Append(DiscWin);
            sb.Append('\t');
            sb.Append(DiscWinRatioText);
            sb.Append('\t');
            sb.Append(DiscLose);
            sb.Append('\t');
            sb.Append(DiscLoseRatioText);
            sb.Append('\t');
            sb.Append(WinLike);
            sb.Append('\t');
            sb.Append(WinLikeRatioText);
            sb.Append('\t');
            sb.Append(First);
            sb.Append('\t');
            sb.Append(FirstRatioText);
            sb.Append('\t');
            sb.Append(Second);
            sb.Append('\t');
            sb.Append(SecondRatioText);
            sb.Append('\t');
            sb.Append(CFirst);
            sb.Append('\t');
            sb.Append(CFirstRatioText);
            sb.Append('\t');
            sb.Append(CSecond);
            sb.Append('\t');
            sb.Append(CSecondRatioText);
            sb.Append('\t');
            sb.Append(FirstWin);
            sb.Append('\t');
            sb.Append(FirstWinRatioText);
            sb.Append('\t');
            sb.Append(SecondWin);
            sb.Append('\t');
            sb.Append(SecondWinRatioText);
            sb.Append('\t');
            sb.Append(CFirstWin);
            sb.Append('\t');
            sb.Append(CFirstWinRatioText);
            sb.Append('\t');
            sb.Append(CSecondWin);
            sb.Append('\t');
            sb.Append(CSecondWinRatioText);
            sb.AppendLine();
        }

        public IEnumerable<(int, double)> EnumerateIndexAndValue()
        {
            var i = 1;
            yield return (i++, Count);
            yield return (i++, CountRatio);
            yield return (i++, Win);
            yield return (i++, WinRatio);
            yield return (i++, Lose);
            yield return (i++, LoseRatio);
            yield return (i++, Draw);
            yield return (i++, DrawRatio);
            yield return (i++, DiscWin);
            yield return (i++, DiscWinRatio);
            yield return (i++, DiscLose);
            yield return (i++, DiscLoseRatio);
            yield return (i++, WinLike);
            yield return (i++, WinLikeRatio);
            yield return (i++, First);
            yield return (i++, FirstRatio);
            yield return (i++, Second);
            yield return (i++, SecondRatio);
            yield return (i++, CFirst);
            yield return (i++, CFirstRatio);
            yield return (i++, CSecond);
            yield return (i++, CSecondRatio);
            yield return (i++, FirstWin);
            yield return (i++, FirstWinRatio);
            yield return (i++, SecondWin);
            yield return (i++, SecondWinRatio);
            yield return (i++, CFirstWin);
            yield return (i++, CFirstWinRatio);
            yield return (i++, CSecondWin);
            yield return (i++, CSecondWinRatio);
        }
    }
}
