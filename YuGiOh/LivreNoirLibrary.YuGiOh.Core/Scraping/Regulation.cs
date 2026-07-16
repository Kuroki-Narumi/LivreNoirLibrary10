using HtmlAgilityPack;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LivreNoirLibrary.YuGiOh.Scraping
{
    public static class Regulation
    {
        const string Id_Forbidden = "list_forbidden";
        const string Id_Limited = "list_limited";
        const string Id_SemiLimited = "list_semi_limited";

        const string Class_LinkValue = "link_value";

        public static async Task Update(Data.Regulation target, ICardProvider provider, bool tcg, ProgressReporter p, CancellationToken c)
        {
            p.Report("updating regulation", "");

            c.ThrowIfCancellationRequested();
            var url = Url.Regulation(tcg);
            var document = await Html.CreateDocument(url, c);
            c.ThrowIfCancellationRequested();

            List<int> list = [];
            target.ClearLimit(LimitCount.Forbidden);
            target.ClearLimit(LimitCount.Limit1);
            target.ClearLimit(LimitCount.Limit2);

            await GetList(list, document?.GetElementbyId(Id_Forbidden), LimitCount.Forbidden, p, c);
            target.Set(list.AsSpan(), LimitCount.Forbidden, provider);
            c.ThrowIfCancellationRequested();

            await GetList(list, document?.GetElementbyId(Id_Limited), LimitCount.Limit1, p, c);
            target.Set(list.AsSpan(), LimitCount.Limit1, provider);
            c.ThrowIfCancellationRequested();

            await GetList(list, document?.GetElementbyId(Id_SemiLimited), LimitCount.Limit2, p, c);
            target.Set(list.AsSpan(), LimitCount.Limit2, provider);
            c.ThrowIfCancellationRequested();
        }

        private static async ValueTask GetList(List<int> list, HtmlNode? node, int limitCount, ProgressReporter p, CancellationToken c)
        {
            list.Clear();
            p.Report($"updating limit-{limitCount}", limitCount, 3);
            c.ThrowIfCancellationRequested();
            foreach (var input in node.EnumerateNodes(Tags.Input, klass: Class_LinkValue))
            {
                c.ThrowIfCancellationRequested();
                if (Url.TryGetCardId(input.Attributes["value"]?.Value, out var cid))
                {
                    list.Add(cid);
                }
            }
        }
    }
}
