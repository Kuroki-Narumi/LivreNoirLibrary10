using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Text
{
    public static partial class StringExtensions
    {
        public static string AutoFormat(this TimeSpan time) => time.Ticks switch
        {
            >= TimeSpan.TicksPerDay => time.ToString(@"d\d\ h\:mm\:ss"),
            >= TimeSpan.TicksPerHour => time.ToString(@"h\:mm\:ss\.f"),
            >= TimeSpan.TicksPerMinute => time.ToString(@"m\:ss\.ff"),
            _ => time.ToString(@"s\.ffff"),
        };
    }
}
