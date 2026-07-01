using HtmlAgilityPack;
using LivreNoirLibrary.ObjectModel;
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

        public static async Task<Dictionary<int, List<int>>> GetLists(bool tcg, ProgressReporter p, CancellationToken c)
        {
            p.Report("updating regulation", "");

            c.ThrowIfCancellationRequested();
            var url = Url.Regulation(tcg);
            var document = await Html.CreateDocument(url, c);
            c.ThrowIfCancellationRequested();

            Dictionary<int, List<int>> result = [];

            var list = await GetList(document?.GetElementbyId(Id_Forbidden), LimitCount.Forbidden, p, c);
            c.ThrowIfCancellationRequested();
            result[LimitCount.Forbidden] = list;

            list = await GetList(document?.GetElementbyId(Id_Limited), LimitCount.Limit1, p, c);
            c.ThrowIfCancellationRequested();
            result[LimitCount.Limit1] = list;

            list = await GetList(document?.GetElementbyId(Id_SemiLimited), LimitCount.Limit2, p, c);
            c.ThrowIfCancellationRequested();
            result[LimitCount.Limit2] = list;

            return result;
        }

        private static async ValueTask<List<int>> GetList(HtmlNode? node, int limitCount, ProgressReporter p, CancellationToken c)
        {
            p.Report($"updating limit-{limitCount}", limitCount, 3);
            c.ThrowIfCancellationRequested();

            List<int> result = [];
            foreach (var input in node.EnumerateNodes(Tags.Input, klass: Class_LinkValue))
            {
                c.ThrowIfCancellationRequested();
                if (Url.TryGetCardId(input.Attributes["value"]?.Value, out var cid))
                {
                    result.Add(cid);
                }
            }
            return result;
        }
    }
}
