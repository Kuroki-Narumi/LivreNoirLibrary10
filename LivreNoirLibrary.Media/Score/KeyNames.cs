using System;

namespace LivreNoirLibrary.Media.Score
{
    public static class KeyNames
    {
        public const string C = "C";
        public const string D = "D";
        public const string E = "E";
        public const string F = "F";
        public const string G = "G";
        public const string A = "A";
        public const string B = "B";
        public const string CSharp = "C#";
        public const string DSharp = "D#";
        public const string ESharp = "E#";
        public const string FSharp = "F#";
        public const string GSharp = "G#";
        public const string ASharp = "A#";
        public const string BSharp = "B#";
        public const string CFlat = "Cb";
        public const string DFlat = "Db";
        public const string EFlat = "Eb";
        public const string FFlat = "Fb";
        public const string GFlat = "Gb";
        public const string AFlat = "Ab";
        public const string BFlat = "Bb";

        private static readonly string[] _names = [C, CSharp, D, DSharp, E, F, FSharp, G, GSharp, A, ASharp, B];
        public static string Get(int number) => $"{_names[number % 12]}{number / 12 - 1}";
    }
}
