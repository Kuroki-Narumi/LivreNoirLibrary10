using HtmlAgilityPack;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LivreNoirLibrary.YuGiOh.Scraping
{
    public static class Deck
    {
        public const string DeckUrl = "https://www.db.yugioh-card.com/yugiohdb/member_deck.action?cgid=";

        public const string JS_CopyDeck = """
            (()=>{const d={};const r=/cid=(\d+)/;function a(j){const a=document.getElementById(j).getElementsByTagName("a");const l=[];for(let i=0;i<a.length;i++){l.push(Number(r.exec(a[i].href)[1]));}d[j]=l;}a("main");a("extra");a("side");console.log(d);return d;})();
            """;

        public static async Task Get(string url, Serializable.Deck target, ProgressReporter? p = null, CancellationToken c = default)
        {
            p?.ReportInitial($"load deck from {url}");
            var document = await Html.CreateDocument(url, c);
            c.ThrowIfCancellationRequested();
            if (document is not null)
            {
                GetPartialList(document, "main", target.MainDeck ??= [], c);
                GetPartialList(document, "extra", target.ExtraDeck ??= [], c);
                GetPartialList(document, "side", target.SideDeck ??= [], c);
            }
        }

        private static void GetPartialList(HtmlDocument document, string elementId, List<int> list, CancellationToken c)
        {
            c.ThrowIfCancellationRequested();
            if (document.GetElementbyId(elementId) is { } node)
            {
                foreach (var a in node.EnumerateNodes(Tags.A))
                {
                    if (Url.TryGetCardId(a.GetAttributeValue("href", ""), out var id))
                    {
                        list.Add(id);
                    }
                }
            }
        }

        const string JS_Monster = "mo";
        const string JS_Spell = "sp";
        const string JS_Trap = "tr";
        const string JS_Extra = "ex";
        const string JS_Side = "si";
        const string JS_MonsterCardId = "monsterCardId";
        const string JS_SpellCardId = "spellCardId";
        const string JS_TrapCardId = "trapCardId";
        const string JS_ExtraCardId = "extraCardId";
        const string JS_SideCardId = "sideCardId";

        public const string JS_Header = """
            (()=>{for(let i=1;i<=65;i++){document.getElementById(`monm_${i}`).value=null;document.getElementById(`monum_${i}`).value=null;document.getElementById(`trnm_${i}`).value=null;document.getElementById(`trnum_${i}`).value=null;document.getElementById(`spnm_${i}`).value=null;document.getElementById(`spnum_${i}`).value=null;}for(let i=1;i<=20;i++){document.getElementById(`exnm_${i}`).value=null;document.getElementById(`exnum_${i}`).value=null;document.getElementById(`sinm_${i}`).value=null;document.getElementById(`sinum_${i}`).value=null;}
            """;

        public const string JS_Loop1 = """
            .forEach(({{n,c}},i)=>{{document.getElementById(`{0}nm_${{i+1}}`).value=n;document.getElementById(`{0}num_${{i+1}}`).value=c;}});
            """;

        public const string JS_Loop2 = """
            for(let i=1;i<=65;i++){document.querySelectorAll(`input#card_id_${i}`).forEach(e=>{const am=a[e.name];e.value=am?am[i-1]:null;});}
            """;

        public const string JS_Footer = "})();";

        private readonly record struct MMItem(string n, int c);

        public static string CreateDeckBuildText(Data.Deck source)
        {
            Dictionary<string, List<MMItem>> srcs = [];
            Dictionary<string, List<int>> ids = [];

            foreach (var item in source.MainDeck.AsSpan())
            {
                var card = item.ThisCard;
                var count = item.Count;
                var (t1, t2) = card.BaseCardType switch
                {
                    CardType.Normal_Spell => (JS_Spell, JS_SpellCardId),
                    CardType.Normal_Trap => (JS_Trap, JS_TrapCardId),
                    _ => (JS_Monster, JS_MonsterCardId),
                };
                srcs.GetOrAdd(t1).Add(new(card.Name, count));
                ids.GetOrAdd(t2).Add(card.Id);
            }
            List<MMItem> src = [];
            List<int> id = [];
            if (source.ExtraDeck.Count > 0)
            {
                foreach (var item in source.ExtraDeck.AsSpan())
                {
                    var card = item.ThisCard;
                    src.Add(new(card.Name, item.Count));
                    id.Add(card.Id);
                }
                srcs[JS_Extra] = src;
                ids[JS_ExtraCardId] = id;
                src = [];
                id = [];
            }
            if (source.SideDeck.Count > 0)
            {
                foreach (var item in source.SideDeck.AsSpan())
                {
                    var card = item.ThisCard;
                    src.Add(new(card.Name, item.Count));
                    id.Add(card.Id);
                }
                srcs[JS_Side] = src;
                ids[JS_SideCardId] = id;
            }

            // JS文字列を作成
            using var o = ObjectPool.RentStringBuilder(out var sb);
            // 入力欄の初期化
            sb.Append(JS_Header);
            // カード名と枚数を入力欄に適用
            foreach (var (key, values) in srcs)
            {
                sb.Append(values.GetJsonText(false));
                sb.AppendFormat(JS_Loop1, key);
            }
            // カードIDを隠された入力欄に適用
            sb.Append("const a=");
            sb.Append(ids.GetJsonText(false));
            sb.Append(';');
            sb.Append(JS_Loop2);
            // 関数の実行
            sb.Append(JS_Footer);
            return sb.ToString();
        }
    }
}
