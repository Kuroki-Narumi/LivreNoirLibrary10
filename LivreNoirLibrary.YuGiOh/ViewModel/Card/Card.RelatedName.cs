using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public partial class Card
    {
        [GeneratedRegex(@"(?<!(?:そ|ター|カード|相手)の効果は)「(?<name1>[^「」]*(?:「[^「」]+」[^「」]*)*)」(?!効果は|効果を持つ|として適用)|(?<!(?:resolve|effect) becomes? |effect, )""(?<name2>[^""]*)""")]
        private static partial Regex Regex_Name { get; }

        [GeneratedRegex(@"属性は(「[^」]+?」)+としても")]
        private static partial Regex Regex_Name_Attr { get; }

        public string[] RelatedList => _related_created ? _related : GetRelated();
        private string[] _related = [];
        private bool _related_created;

        private string[] GetRelated()
        {
            _related_created = true;
            HashSet<string> set = [_name];
            int index = 0;
            Match match;
            List<(int, int)> attr = [];
            for (; ; )
            {
                match = Regex_Name_Attr.Match(_text, index);
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
                match = Regex_Name.Match(_text, index);
                if (match.Success)
                {
                    var name = match.Groups["name1"].Success ? match.Groups["name1"].Value : match.Groups["name2"].Value;
                    index = match.Index;
                    if (name != _name && !attr.Any(v => index >= v.Item1 && index <= v.Item2))
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
