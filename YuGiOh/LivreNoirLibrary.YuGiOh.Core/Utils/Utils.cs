using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Utils
    {
        public static string GetFullPath(string path) => System.IO.Path.Join(General.GetAssemblyDir(), path);

        public static DateTime DateStart => Periods[0];

        public static readonly DateTime[] Periods =
        [
            DateTime.Parse("1999-02-04"),
            DateTime.Parse("2000-04-01"),
            DateTime.Parse("2002-05-01"),
            DateTime.Parse("2004-05-01"),
            DateTime.Parse("2006-05-02"),
            DateTime.Parse("2008-03-15"),
            DateTime.Parse("2010-03-20"),
            DateTime.Parse("2012-03-17"),
            DateTime.Parse("2014-03-21"),
            DateTime.Parse("2017-03-25"),
            DateTime.Parse("2020-04-01"),
            DateTime.Parse("2023-04-01"),
            DateTime.Parse("2025-04-01"),
        ];

        public static DateTime Truncate(this DateTime dt) => new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
    }
}
