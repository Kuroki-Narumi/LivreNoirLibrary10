using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Media.Bmson
{
    public static class Constants
    {
        public static readonly Version DefaultVersion = new(1,0,0);
        public static KeyType DefaultModeHint => KeyType.Beat_7;
        public const double DefaultJudgeRank = 100;
        public const double DefaultTotal = 100;
        public const long DefaultResolution = 240;

        public const decimal DefaultScrollRate = 1;
    }
}
