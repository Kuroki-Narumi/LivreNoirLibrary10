using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class Card
    {
        [GeneratedRegex(@"(?<!(?:そ|ター|カード|相手)の効果は)「(?<name1>[^「」]*(?:「[^「」]+」[^「」]*)*)」(?!効果は|効果を持つ|として適用)|(?<!(?:resolve|effect) becomes? |effect, )""(?<name2>[^""]*)""")]
        private static partial Regex Regex_Name { get; }

        [GeneratedRegex(@"属性(?:は|を)(「[^」]」)+としても")]
        private static partial Regex Regex_Name_Attr { get; }

        private string[]? _related = [];
        public IEnumerable<string> RelatedList => _related ?? CreateRelatedList();

        private string[] CreateRelatedList()
        {
            using var o1 = ObjectPool.Rent<HashSet<string>>(out var set);
            using var o2 = ObjectPool.Rent<List<(int, int)>>(out var attr);
            set.Add(Name);
            int index = 0;
            Match match;
            var text = Text;
            for (; ; )
            {
                match = Regex_Name_Attr.Match(text, index);
                if (match.Success)
                {
                    index = match.Index + match.Length;
                    attr.Add((match.Index, index));
                }
                else
                {
                    break;
                }
            }
            index = 0;
            for (; ; )
            {
                match = Regex_Name.Match(text, index);
                if (match.Success)
                {
                    var name = match.Groups["name1"].Success ? match.Groups["name1"].Value : match.Groups["name2"].Value;
                    index = match.Index;
                    if (name != Name && !attr.Any(v => index >= v.Item1 && index <= v.Item2))
                    {
                        set.Add(name);
                    }
                    index += match.Length;
                }
                else
                {
                    break;
                }
            }
            _related = [.. set];
            return _related;
        }
    }
}
