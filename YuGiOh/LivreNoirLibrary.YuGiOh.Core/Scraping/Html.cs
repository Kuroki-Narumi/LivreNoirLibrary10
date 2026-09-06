using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.Scraping
{
    public static class Html
    {
        public static int RetryCount { get; set; } = 5;

        public static async Task<string> GetString(string url, CancellationToken c = default)
        {
            for (var count = 0; ; count++)
            {
                try
                {
                    var text = await HttpClientPool.Instance.GetStringAsync(url, c);
                    return text;
                }
                catch (HttpRequestException)
                {
                    if (count >= RetryCount)
                    {
                        throw;
                    }
                    Thread.Sleep(10);
                }
            }
        }

        public static async Task<HtmlDocument?> CreateDocument(string url, CancellationToken c = default)
        {
            var t0 = Stopwatch.GetTimestamp();
            Console.WriteLine($"document create from {url}");
            var text = await GetString(url, c);
            Console.WriteLine($"  got html in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");
            t0 = Stopwatch.GetTimestamp();
            var document = new HtmlDocument();
            document.LoadHtml(text);
            Console.WriteLine($"  document created in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");
            return document;
        }

        public static string ToHtmlDecoded(this string? text) => string.IsNullOrEmpty(text) ? "" : HttpUtility.HtmlDecode(text.Trim());

        public static HtmlNode? SelectSingleNode(this HtmlNode? node, string? tag = null, string? klass = null, string? id = null)
        {
            return node?.SelectSingleNode(GetXPath(tag, klass, id));
        }

        public static HtmlNodeCollection? SelectNodes(this HtmlNode? node, string? tag = null, string? klass = null, string? id = null)
        {
            return node?.SelectNodes(GetXPath(tag, klass, id));
        }

        public static IEnumerable<HtmlNode> EnumerateNodes(this HtmlNode? node, string? tag = null, string? klass = null, string? id = null)
        {
            if (SelectNodes(node, tag, klass, id) is { } nodes)
            {
                return nodes;
            }
            return [];
        }

        private static readonly Dictionary<(string?, string?, string?), string> _xpath = [];

        private static string GetXPath(string? tag, string? klass, string? id)
        {
            var key = (tag, klass, id);
            if (_xpath.TryGetValue(key, out var result))
            {
                return result;
            }
            using var o = ObjectPool.RentStringBuilder(out var sb);
            sb.Append(".//");
            if (string.IsNullOrEmpty(tag))
            {
                sb.Append('*');
            }
            else
            {
                sb.Append(tag);
            }
            var k = !string.IsNullOrEmpty(klass);
            var i = !string.IsNullOrEmpty(id);
            if (k || i)
            {
                sb.Append('[');
                if (k)
                {
                    sb.Append($"@class='{klass}'");
                }
                if (k && i)
                {
                    sb.Append(" and ");
                }
                if (i)
                {
                    sb.Append($"@id='{id}'");
                }
                sb.Append(']');
            }
            result = sb.ToString();
            _xpath[key] = result;
            return result;
        }
    }
}
