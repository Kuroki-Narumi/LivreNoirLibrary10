using HtmlAgilityPack;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LivreNoirLibrary.YuGiOh.Scraping
{
    public static class CardPack
    {
        public static async Task<SortedSet<int>> GetCardList(CardPackCollection? target = null, ProgressReporter? p = null, CancellationToken c = default)
        {
            p?.ReportInitial("checking card pack", "creating pack list...");

            List<(string, string, DateTime)> packs = [];
            c.ThrowIfCancellationRequested();

            await GetPackList(target, false, packs, c);

            await GetPackList(target, true, packs, c);

            packs.Reverse();
            var cardIds = await GetCardIds(target, packs, p, c);
            target?.NotifyCollectionReset();
            return cardIds;
        }

        const string Id_UpdateList = "update_list";
        const string T_Body = "t_body";
        const string Class_Time = "time";
        const string Class_Main = "main";

        private static async ValueTask GetPackList(CardPackCollection? target, bool tcg, List<(string, string, DateTime)> packs, CancellationToken c)
        {
            c.ThrowIfCancellationRequested();
            var now = tcg ? DateTime.UtcNow : DateTime.Now;
            var url = Url.PackList(tcg);
            var document = await Html.CreateDocument(url, c);
            var invalidDate = _invalidDate;
            var dateFallback = _dateFallback;
            if (document?.GetElementbyId(Id_UpdateList) is { } list && list.SelectSingleNode(Tags.Div, klass:T_Body) is { } div)
            {
                foreach (var child in div.ChildNodes)
                {
                    c.ThrowIfCancellationRequested();
                    var validCount = 0;
                    DateTime time = default;
                    string name = "";
                    string pid = "";
                    if (child.SelectSingleNode(Tags.Div, klass: Class_Main) is { } mainDiv)
                    {
                        if (mainDiv.SelectSingleNode(Tags.P) is { } p)
                        {
                            name = p.InnerText;
                            validCount++;
                        }
                        if (mainDiv.SelectSingleNode(Tags.Input) is { } input)
                        {
                            var value = input.Attributes["value"].Value;
                            if (Url.TryGetPackId(value, out var pidSpan))
                            {
                                pid = Data.CardPack.EnsureTcgSuffix(pidSpan, tcg);
                                validCount++;
                            }
                        }
                    }
                    if (child.SelectSingleNode(Tags.Div, klass: Class_Time) is { } timeDiv &&
                        DateTime.TryParse(timeDiv.InnerText, out time))
                    {
                        validCount++;
                        if (time == invalidDate)
                        {
                            dateFallback.TryGetValue(pid, out time);
                        }
                    }
                    if (validCount is 3 && time <= now && (target is null || !target.Contains(pid)))
                    {
                        packs.Add((pid, name, time));
                    }
                }
            }
        }

        const string Id_CardList = "card_list";

        private static async ValueTask<SortedSet<int>> GetCardIds(CardPackCollection? target, List<(string, string, DateTime)> packs, ProgressReporter? p, CancellationToken c)
        {
            SortedSet<int> cids = [];
            var count = packs.Count;
            for (var i = 0; i < count; i++)
            {
                c.ThrowIfCancellationRequested();
                var (pid, name, date) = packs[i];
                p?.Report($"{name} ({i + 1}/{count})", i, count);

                var url = Url.Pack(pid);
                var document = await Html.CreateDocument(url, c);
                if (document?.GetElementbyId(Id_CardList) is { } div)
                {
                    var valid = false;
                    foreach (var node in div.EnumerateNodes(Tags.Input, klass: "cid"))
                    {
                        if (int.TryParse(node.Attributes["value"]?.Value, out var cid))
                        {
                            cids.Add(cid);
                            valid = true;
                        }
                    }
                    if (valid)
                    {
                        target?.AddWithoutNotify(new() { ProductId = pid, Name = name, Date = date });
                    }
                }
            }
            return cids;
        }

        public static bool TryGetFallbackDate(string pid, out DateTime date)
        {
            return _dateFallback.TryGetValue(pid, out date);
        }

        private static readonly DateTime _invalidDate = new(1990, 1, 1);
        private static readonly Dictionary<string, DateTime> _dateFallback = new()
        {
            { "1111108004e", new(2012, 4, 1) },
            { "1111108003e", new(2012, 3, 1) },
            { "1111108000e", new(2012, 1, 1) },
            { "1111107010e", new(2011, 12, 1) },
            { "1111107009e", new(2011, 12, 1) },
            { "1111107008e", new(2011, 9, 1) },
            { "1111107005e", new(2011, 6, 1) },
            { "1111107004e", new(2011, 6, 1) },
            { "1111107003e", new(2011, 4, 1) },
            { "1111107001e", new(2011, 3, 1) },
            { "1111107000e", new(2011, 1, 1) },
            { "1111106013e", new(2010, 12, 1) },
            { "1111106012e", new(2010, 12, 1) },
            { "1111106011e", new(2010, 11, 1) },
            { "1111106009e", new(2010, 9, 1) },
            { "1111106008e", new(2010, 7, 1) },
            { "1111106006e", new(2010, 5, 1) },
            { "1111106005e", new(2010, 5, 1) },
            { "1111106003e", new(2010, 3, 1) },
            { "1111106002e", new(2010, 3, 1) },
            { "1111106000e", new(2010, 1, 1) },
            { "1111105006e", new(2009, 11, 1) },
            { "1111105005e", new(2009, 9, 1) },
            { "1111105003e", new(2009, 5, 1) },
            { "1111105002e", new(2009, 3, 1) },
            { "1111105001e", new(2009, 2, 1) },
            { "1111105000e", new(2009, 1, 1) },
            { "1111104006e", new(2008, 9, 1) },
            { "1111104004e", new(2008, 5, 1) },
            { "1111104003e", new(2008, 5, 1) },
            { "1111104002e", new(2008, 3, 1) },
            { "1111104000e", new(2008, 1, 1) },
            { "1111103005e", new(2007, 11, 1) },
            { "1111103004e", new(2007, 9, 1) },
            { "1111103002e", new(2007, 6, 1) },
            { "1111103000e", new(2007, 1, 1) },
            { "1111102005e", new(2006, 12, 1) },
            { "1111102004e", new(2006, 11, 1) },
            { "1111102003e", new(2006, 9, 1) },
            { "1111102000e", new(2006, 1, 1) },
            { "1111101000e", new(2005, 1, 1) },
            { "1121203001e", new(2004, 5, 1) },
            { "1121203000e", new(2004, 4, 1) },
            { "1121202005e", new(2003, 10, 1) },
            { "1121202002e", new(2003, 5, 1) },
            { "1121202001e", new(2003, 4, 1) },
        };
    }
}