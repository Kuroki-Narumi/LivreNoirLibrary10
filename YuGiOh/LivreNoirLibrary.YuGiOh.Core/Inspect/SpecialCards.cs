using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Inspect
{
    public static class SpecialCards
    {
        public const int Goyoku = 4844; // 強欲な壺(2枚ドロー)
        public const int GoKen = 8993; // 強欲で謙虚な壺(3枚めくって1枚取る、特殊召喚不可)
        public const int GoDon = 12465; // 強欲で貪欲な壺(2枚ドロー、デッキ10枚消費)
        public const int GoKin = 14144; // 強欲で金満な壺(2枚ドロー、EX6枚消費)
        public const int KinKen = 15756; // 金満で謙虚な壺(6枚めくって1枚取る、EX6枚消費)

        public const int NariGobu = 4895; // 成金ゴブリン(1枚ドロー、相手のLP1000回復)
        public const int MunoRengoku = 8824; // 無の煉獄(1枚ドロー、エンドフェイズに全て捨てる)
        public const int ChickenRace = 11851; // チキンレース(1枚ドロー、自分のLP1000消費)

        // 指定ドローソース
        public static readonly HashSet<int> NamedDrawSource = [GoKen, GoDon, GoKin, KinKen];

        // 汎用ドローソース
        public static readonly HashSet<int> DrawSource = [Goyoku, NariGobu, MunoRengoku, ChickenRace];

        // メイン1の開始時にしか発動できないカード
        public static readonly HashSet<int> Main1 =
        [
            5127,  // 大寒波
            8214,  // カーム・マジック
            9139,  // 大熱波
            9171,  // インヴェルズの斥候
            9531,  // 完全防音壁
            10794, // 鏡鳴する武神
            10796, // 貪欲で無欲な壺
            GoKin,
            16184, // あまびえさん
            16417, // ピリ・レイスの地図
            17727, // 墓守の刻印
            18208, // 魔界劇団のゲネプロ
            21343, // フィッシュアンドビッズ
        ];
    }
}
