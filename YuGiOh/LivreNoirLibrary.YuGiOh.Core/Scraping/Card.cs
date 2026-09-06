using HtmlAgilityPack;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LivreNoirLibrary.YuGiOh.Scraping
{
    public static partial class Card
    {
        const string Id_CardSet = "CardSet";
        const string Id_UpdateList = "update_list";

        public static async Task UpdateAllCards(ICollection<int> ids, CardDataCollection cards, CardPackCollection packs, ProgressReporter? p = null, CancellationToken c = default)
        {
            p?.ReportInitial("loading card data");
            var i = 0;
            var max = ids.Count;
            foreach (var id in ids)
            {
                i++;
                c.ThrowIfCancellationRequested();
                try
                {
                    var card = await GetCardInfo(id, p, c);
                    if (card is not null)
                    {
                        p?.Report($"{i}/{max}: {card.Name}", i, max);
                        card = cards.Add(card);
                        foreach (var info in card.PackInfo)
                        {
                            if (packs.TryGet(info.ProductId, out var pack))
                            {
                                pack.Add(new(card, info.Number));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }

        public static async Task<Data.Card?> GetCardInfo(int id, ProgressReporter? p = null, CancellationToken c = default)
        {
            var ocgUrl = Url.Card(id, false);
            var tcgUrl = Url.Card(id, true);

            c.ThrowIfCancellationRequested();
            var ocgDocument = await Html.CreateDocument(ocgUrl, c);

            c.ThrowIfCancellationRequested();
            var tcgDocument = await Html.CreateDocument(tcgUrl, c);

            c.ThrowIfCancellationRequested();
            // 該当カード無し(OCG)
            var content = ocgDocument?.GetElementbyId(Id_CardSet);
            var sourceDocument = ocgDocument;
            c.ThrowIfCancellationRequested();
            if (content is null)
            {
                content = tcgDocument?.GetElementbyId(Id_CardSet);
                sourceDocument = tcgDocument;
                c.ThrowIfCancellationRequested();
                // 該当カード無し(TCG)
                if (content is null)
                {
                    return null;
                }
            }

            Data.Card card = new() { Id = id };

            c.ThrowIfCancellationRequested();
            UpdateCardInfo(card, sourceDocument!, content);

            c.ThrowIfCancellationRequested();
            UpdatePackList(card, ocgDocument, false);

            c.ThrowIfCancellationRequested();
            UpdatePackList(card, tcgDocument, true);

            return card;
        }

        const string Id_CardName = "cardname";
        const string Class_Ruby = "ruby";
        const string Class_CardText = "CardText";
        const string Class_CardText_Pen = "CardText pen";
        const string Class_CardText_Note = "CardText note";

        const string Class_Text_LineBreak = "text_linebreak";

        const string Class_ItemBox = "item_box";
        const string Class_ItemBoxText = "item_box_text";
        const string Class_ItemBoxTitle = "item_box_title";
        const string Class_ItemBoxValue = "item_box_value";
        const string Class_ItemBoxTCenter = "item_box t_center";

        const string Class_TextTitle = "text_title";
        const string Class_Species = "species";

        const string Title_Atk = "ATK";
        const string Title_Def = "DEF";
        const string Title_Attribute = "attribute";
        const string Title_Link = "icon_img_set link";

        [GeneratedRegex("カードテキスト|Card Text")]
        private static partial Regex Regex_CardText { get; }

        [GeneratedRegex("公式のデュエルでは使用できません|Cannot be used in official Duels")]
        private static partial Regex Regex_Unusable { get; }

        [GeneratedRegex("効果|Icon")]
        private static partial Regex Regex_NoMonsterCard { get; }

        [GeneratedRegex("icon_rank|icon_level")]
        private static partial Regex Regex_Level { get; }

        [GeneratedRegex($@"(?<={Title_Link})\d+")]
        private static partial Regex Regex_Link { get; }

        [GeneratedRegex("[0-9]+")]
        private static partial Regex Regex_Number { get; }

        private static void UpdateCardInfo(Data.Card card, HtmlDocument document, HtmlNode cardset)
        {
            using var o1 = ObjectPool.RentStringBuilder(out var sb);
            using var o2 = ObjectPool.RentList<string>(out var spcList);
            var t0 = Stopwatch.GetTimestamp();

            // カード名
            if (document.GetElementbyId(Id_CardName) is { } div_cardname && 
                div_cardname.SelectSingleNode(Tags.H1) is { } h1_cardname)
            {
                sb.Clear();
                foreach (var node in h1_cardname.ChildNodes)
                {
                    switch (node.NodeType)
                    {
                        case HtmlNodeType.Text:
                            sb.Append(node.InnerText.ToHtmlDecoded());
                            break;
                        case HtmlNodeType.Element:
                            if (node.Name is Tags.Span)
                            {
                                if (node.HasClass(Class_Ruby))
                                {
                                    card.Ruby = node.InnerText.ToHtmlDecoded();
                                }
                                else
                                {
                                    card.EnName = node.InnerText.ToHtmlDecoded();
                                }
                            }
                            break;
                    }
                }
                // カード名の文字列は加工しない。全角記号等が含まれていてもそのまま採用する。
                card.Name = sb.ToString();
                // TCG限定カードはルビと英語名の欄が存在しない
                if (string.IsNullOrEmpty(card.Ruby) && string.IsNullOrEmpty(card.EnName))
                {
                    card.EnName = card.Name;
                }
            }
            
            foreach (var node in cardset.EnumerateNodes(Tags.Div, klass: Class_CardText))
            {
                // カードテキスト
                if (node.SelectSingleNode(Tags.Div, klass: Class_ItemBoxText) is { } textNode)
                {
                    if (textNode.SelectSingleNode(Tags.Div, klass: Class_TextTitle) is { } titleNode &&
                        Regex_CardText.IsMatch(titleNode.InnerText))
                    {
                        card.Text = BuildCardText(sb, textNode);
                        continue;
                    }
                }
                // モンスター以外
                else if (node.SelectSingleNode(Tags.Div, klass: Class_ItemBoxTCenter) is { } frameNode)
                {
                    if (TryGetTitleAndValue(frameNode, out var title, out var value) &&
                        Regex_NoMonsterCard.IsMatch(title))
                    {
                        card.CardType = Vocab.GetCardType(value);
                    }
                }
                // 基本情報
                else
                {
                    foreach (var node2 in node.EnumerateNodes(Tags.Div, klass: Class_ItemBox))
                    {
                        if (TryGetTitleAndValue(node2, out var title, out var value))
                        {
                            // 属性
                            if (title.Contains(Title_Attribute, StringComparison.Ordinal))
                            {
                                card.Attribute = Vocab.GetAttribute(value);
                                continue;
                            }
                            // レベル
                            if (Regex_Level.IsMatch(title))
                            {
                                card.Level = ParseInt(value);
                                continue;
                            }
                            // リンク
                            if (title.Contains(Title_Link, StringComparison.Ordinal))
                            {
                                card.CardType = CardType.Link_Monster;
                                ReadOnlySpan<char> span = [];
                                foreach (var match in Regex_Link.EnumerateMatches(title))
                                {
                                    span = title.Slice(match.Index, match.Length);
                                }
                                card.Level = span.Length;
                                LinkDirection dir = 0;
                                foreach (var d in span)
                                {
                                    switch (d)
                                    {
                                        case '1':  // 左下
                                            dir |= LinkDirection.LowerLeft;
                                            break;
                                        case '2':  // 下
                                            dir |= LinkDirection.Lower;
                                            break;
                                        case '3':  // 右下
                                            dir |= LinkDirection.LowerRight;
                                            break;
                                        case '4':  // 左
                                            dir |= LinkDirection.Left;
                                            break;
                                        case '6':  // 右
                                            dir |= LinkDirection.Right;
                                            break;
                                        case '7':  // 左上
                                            dir |= LinkDirection.UpperLeft;
                                            break;
                                        case '8':  // 上
                                            dir |= LinkDirection.Upper;
                                            break;
                                        case '9':  // 右上
                                            dir |= LinkDirection.UpperRight;
                                            break;
                                    }
                                }
                                card.Def = (int)dir;
                                continue;
                            }
                            // ATK
                            if (title.Contains(Title_Atk, StringComparison.Ordinal))
                            {
                                card.Atk = ParseInt(value);
                                continue;
                            }
                            // DEF
                            if (title.Contains(Title_Def, StringComparison.Ordinal))
                            {
                                if (!card.IsLink())
                                {
                                    card.Def = ParseInt(value);
                                }
                                continue;
                            }
                        }
                        // 種族/能力
                        else if (node2.SelectSingleNode(Tags.P, klass: Class_Species) is { } species)
                        {
                            spcList.Clear();
                            foreach (var child in species.ChildNodes)
                            {
                                if (child.Name is Tags.Span)
                                {
                                    var text = child.InnerText.AsSpan();
                                    var start = 0;
                                    for (var i = 0; i < text.Length; i++)
                                    {
                                        if (text[i] is '／')
                                        {
                                            if (start < i)
                                            {
                                                spcList.Add(text[start..i].Trim().ToString());
                                            }
                                            start = i + 1;
                                        }
                                    }
                                    if (start < text.Length)
                                    {
                                        spcList.Add(text[start..].ToString());
                                    }
                                }
                            }
                            // 種族
                            if (spcList.Count is > 0 && Vocab.TryGetMonsterType(spcList[0], out var mType))
                            {
                                card.MonsterType = mType;
                                spcList.RemoveAt(0);
                            }
                            // カードタイプ
                            card.CardType = CardType.Main_Monster;
                            if (spcList.Count is > 0 && Vocab.TryGetCardType(spcList[0], out var cType))
                            {
                                card.CardType = cType;
                                spcList.RemoveAt(0);
                            }
                            // 効果の有無
                            if (spcList.Remove(Vocab.Normal) || spcList.Remove(nameof(Vocab.Normal)))
                            {
                                card.HasEffect = false;
                            }
                            if (spcList.Remove(Vocab.Effect) || spcList.Remove(nameof(Vocab.Effect)))
                            {
                                card.HasEffect = true;
                            }
                            // その他の能力
                            card.Ability = Vocab.GetAbility(spcList);
                        }
                    }
                }
            }

            // 使用不可
            if (cardset.SelectSingleNode(Tags.Div, klass: Class_CardText_Note) is { } noteNode && Regex_Unusable.IsMatch(noteNode.InnerText))
            {
                card.Unusable = true;
            }
            // ペンデュラム
            if (cardset.SelectSingleNode(Tags.Div, klass: Class_CardText_Pen) is { } penNode)
            {
                card.Ability |= Ability.Pendulum;
                // Pスケール
                if (penNode.SelectSingleNode(Tags.Span, klass: Class_ItemBoxValue) is { } scaleNode)
                {
                    card.PendulumScale = ParseInt(scaleNode.InnerText);
                }
                // P効果
                if (penNode.SelectSingleNode(Tags.Div, klass: Class_ItemBoxText) is { } textNode)
                {
                    card.PendulumText = BuildCardText(sb, textNode);
                }
            }

            Console.WriteLine($" parsed 《{card.Name}》 in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");
        }

        private static bool TryGetTitleAndValue(HtmlNode node, [MaybeNullWhen(false)] out ReadOnlySpan<char> title, [MaybeNullWhen(false)] out ReadOnlySpan<char> value)
        {
            title = null;
            value = null;
            if (node.SelectSingleNode(Tags.Span, klass:Class_ItemBoxTitle) is { } t)
            {
                title = t.InnerHtml;
            }
            if (node.SelectSingleNode(Tags.Span, klass:Class_ItemBoxValue) is { } v)
            {
                value = v.InnerHtml;
            }
            return title.Length > 0 && value.Length > 0;
        }

        private static int ParseInt(ReadOnlySpan<char> span)
        {
            foreach (var match in Regex_Number.EnumerateMatches(span))
            {
                if (int.TryParse(span.Slice(match.Index, match.Length), out var value))
                {
                    return value;
                }
            }
            return -1;
        }

        private static string BuildCardText(StringBuilder sb, HtmlNode node)
        {
            var line = node.SelectSingleNode(Tags.Div, klass: Class_Text_LineBreak);
            var lineText = (line?.InnerText).ToHtmlDecoded().AsSpan();
            if (lineText.Length is 0)
            {
                return "";
            }
            sb.Length = 0;
            var isNewLine = true;
            var index = 0;
            var segmentStart = 0;
            var parenCount = 0;
            for (; index < lineText.Length; index++)
            {
                switch (lineText[index])
                {
                    case '●':
                        if (!isNewLine)
                        {
                            // ここまでのセクションを追加
                            sb.Append(lineText[segmentStart..index].ToHalf());
                            segmentStart = index;
                            // 記号の直前で改行する
                            sb.AppendLine();
                            isNewLine = true;
                        }
                        break;
                    case >= '①' and <= '⑨':
                        if (!isNewLine && index + 1 < lineText.Length && lineText[index + 1] is ':' or '：')
                        {
                            // ここまでのセクションを追加
                            sb.Append(lineText[segmentStart..index].ToHalf());
                            segmentStart = index;
                            // 記号の直前で改行する
                            sb.AppendLine();
                            isNewLine = true;
                        }
                        break;
                    case '「':
                        if (parenCount is 0 && index > segmentStart)
                        {
                            // ここまでのセクションを追加
                            sb.Append(lineText[segmentStart..index].ToHalf());
                            segmentStart = index;
                        }
                        parenCount++;
                        break;
                    case '」':
                        parenCount--;
                        if (parenCount is 0 && index > segmentStart)
                        {
                            // 「」の内側は半角化しない(カード名が含まれうるため)
                            sb.Append(lineText[segmentStart..index]);
                            segmentStart = index;
                        }
                        break;
                    case '<': // <br>の可能性がある
                        if (index + 3 < lineText.Length && lineText[index..(index + 4)].Equals("<br>", StringComparison.OrdinalIgnoreCase))
                        {
                            // ここまでのセクションを追加
                            sb.Append(lineText[segmentStart..index].ToHalf());
                            segmentStart = index + 4;
                            // "<br>"をスキップ
                            index = segmentStart;
                            // 改行する
                            sb.AppendLine();
                            isNewLine = true;
                        }
                        break;
                }
            }
            if (index > segmentStart)
            {
                // 残りのセクションを追加
                sb.Append(lineText[segmentStart..index].ToHalf());
            }
            return sb.ToString();
        }

        const string Class_Inside = "inside";
        const string Class_Time = "time";
        const string Class_PackName = "pack_name flex_1";
        const string Class_Number = "card_number";

        private static void UpdatePackList(Data.Card card, HtmlDocument? document, bool tcg)
        {
            if (document?.GetElementbyId(Id_UpdateList) is not { } node)
            {
                return;
            }
            var packs = card.PackInfo;
            foreach (var div in node.EnumerateNodes(Tags.Div, klass: Class_Inside))
            {
                DateTime date = default;
                string? name = null, number = null, pid = null;
                var dateDetected = TryGetString(div, Class_Time, out var span) && DateTime.TryParse(span, out date);
                if (TryGetString(div, Class_PackName, out span))
                {
                    name = new(span);
                }
                if (TryGetString(div, Class_Number, out span))
                {
                    number = new(span);
                }
                if (Url.TryGetPackId(div.InnerHtml, out span))
                {
                    pid = Data.CardPack.EnsureTcgSuffix(span, tcg);
                    if (!dateDetected && CardPack.TryGetFallbackDate(pid, out date))
                    {
                        dateDetected = true;
                    }
                }
                if (pid is not null && number is not null)
                {
                    packs.Add(new(pid, number, name ?? "", date));
                }
            }

            static bool TryGetString(HtmlNode node, string klass, out ReadOnlySpan<char> span)
            {
                if (node.SelectSingleNode(Tags.Div, klass: klass) is { } child)
                {
                    span = child.InnerText.AsSpan().Trim();
                    return true;
                }
                span = default;
                return false;
            }
        }
    }
}
