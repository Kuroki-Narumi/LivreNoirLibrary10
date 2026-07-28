using LivreNoirLibrary.IO;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public static partial class MonsterLike
    {
        const string StatusExpression = @"\([^)]*?(?:攻|ATK)[^)]+\)";

        [GeneratedRegex(@$"(?:(?:効果)?モンスター(?:カード)?(?:扱い)?|(?:Effect\s?)?Monsters?)\s*{StatusExpression}")]
        private static partial Regex Regex_TrapMonster { get; }

        [GeneratedRegex(@"(?<=\()[^)]+(?=\))")]
        private static partial Regex Regex_Status { get; }

        private const string NamedToken1 = "「[^」]+トークン」";
        private const string NamedToken2 = "\"[^\"]+Tokens?\"";

        [GeneratedRegex($"{NamedToken1}|{NamedToken2}")]
        private static partial Regex Regex_NamedToken { get; }

        [GeneratedRegex(@$"{NamedToken1}\s*({StatusExpression}|[^。]+特殊召喚)|Special Summon\s*(?:\d+\s*)?{NamedToken2}\s*{StatusExpression}")]
        private static partial Regex Regex_NamedTokenGenerator { get; }

        [GeneratedRegex(@$"{NamedToken1}(?:は|が|に|の|または|を(?!(?:\d+体)?特殊))")]
        private static partial Regex Regex_NamedTokenReferer { get; }

        [GeneratedRegex(@"(?<!こ|そ)(?:に|の)トークン(?!名|以外|を除く|」)")]
        private static partial Regex Regex_TokenReferer { get; }

        [GeneratedRegex(@"(?<!相手は)トークン(?<ex1>以外|を除く)|(?<ex2>except (?:a )?)Token")]
        private static partial Regex Regex_TokenNegativeReferer { get; }

        public static void ParseMonsterLikeCards(ICardEnumerable source, ICollection<Card> trapMonsters, TokenCollection tokens)
        {
            tokens.Clear();
            trapMonsters.Clear();
            foreach (var card in source.CardEnumerable)
            {
                ParseMonsterLikeImpl(card, card.Text, trapMonsters, tokens);
                ParseMonsterLikeImpl(card, card.PendulumText, trapMonsters, tokens);
            }
        }

        private static void ParseMonsterLikeImpl(Card card, ReadOnlySpan<char> text, ICollection<Card> trapMonsters, TokenCollection tokens)
        {
            foreach (var range in Regex_TrapMonster.EnumerateMatches(text))
            {
                var span = text.Slice(range.Index, range.Length);
                var newCard = card.Clone();
                newCard.Id = card.Id;
                newCard.CardType = card.CardType | CardType.MonsterLike;
                newCard.HasEffect = span.StartsWith("効果") || span.StartsWith("Effect", StringComparison.OrdinalIgnoreCase);
                foreach (var statusRange in Regex_Status.EnumerateMatches(span))
                {
                    ParseStatus(newCard, span.Slice(statusRange.Index, statusRange.Length));
                    break;
                }
                trapMonsters.Add(newCard);
                break;
            }
            foreach (var range in Regex_NamedTokenGenerator.EnumerateMatches(text))
            {
                var span = text.Slice(range.Index, range.Length);
                Token token = null!;
                foreach (var range2 in Regex_NamedToken.EnumerateMatches(span))
                {
                    token = tokens.GetOrAdd(span.Slice(range2.Index + 1, range2.Length - 2));
                    break;
                }
                foreach (var statusRange in Regex_Status.EnumerateMatches(span))
                {
                    ParseStatus(token, span.Slice(statusRange.Index, statusRange.Length));
                    break;
                }
                token.AddGenerator(card);
            }
            foreach (var range in Regex_NamedToken.EnumerateMatches(text))
            {
                // 「」を除いた部分が名前
                var token = tokens.GetOrAdd(text.Slice(range.Index + 1, range.Length - 2));
                if (token.Generators.Contains(card))
                {
                    if (Regex_NamedTokenReferer.IsMatch(text))
                    {
                        token.AddReferer(card);
                    }
                }
                else
                {
                    token.AddReferer(card);
                }
            }
            if (Regex_TokenReferer.IsMatch(text))
            {
                tokens.Referers.Add(card);
            }
            if (Regex_TokenNegativeReferer.IsMatch(text))
            {
                tokens.NegativeReferers.Add(card);
            }
        }

        [GeneratedRegex("[・･/]")]
        private static partial Regex Regex_Separator { get; }

        public static void ParseStatus(Card target, ReadOnlySpan<char> input)
        {
            var autoAtk = false;
            foreach (var range in Regex_Separator.EnumerateSplits(input))
            {
                var segment = input[range].Trim();
                if(segment.Length is 0)
                {
                    continue;
                }
                else if (TryGetNumber(["星", "Level"], segment, out var value))
                {
                    target.Level = value;
                }
                else if (TryGetNumber(["攻", "ATK"], segment, out value))
                {
                    if (value >= -1)
                    {
                        target.Atk = value;
                    }
                    else
                    {
                        autoAtk = true;
                    }
                }
                else if (TryGetNumber(["守", "DEF"], segment, out value))
                {
                    target.Def = value;
                }
                else if (Vocab.TryGetAttribute(segment, out var attr))
                {
                    target.Attribute = attr;
                }
                else if (Vocab.TryGetMonsterType(segment, out var mType))
                {
                    target.MonsterType = mType;
                }
                else if (Vocab.TryGetSingleAbility(segment, out var abi))
                {
                    target.Ability |= abi;
                }
            }
            if (autoAtk)
            {
                target.Atk = target.Def;
            }

            static bool TryGetNumber(ReadOnlySpan<string> prefixes, ReadOnlySpan<char> segment, out int value)
            {
                foreach (var prefix in prefixes)
                {
                    if (segment.StartsWith(prefix))
                    {
                        var subSpan = segment[prefix.Length..].Trim();
                        if (int.TryParse(subSpan, out value))
                        {
                            return true;
                        }
                        value = segment[^1] switch
                        {
                            '?' or '？' => -1,
                            _ => -2,
                        };
                        return true;
                    }
                }
                value = default;
                return false;
            }
        }
    }
}
