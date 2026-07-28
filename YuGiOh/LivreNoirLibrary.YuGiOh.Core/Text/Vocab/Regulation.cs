
namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public const string Unusable = "使用不可";
        public const string Forbidden = "禁止";
        public const string Limit1 = "制限";
        public const string Limit2 = "準制限";
        public const string Unlimited = "無制限";
        public const string Specified = "特別指定";

        public static string GetLimitText(int limit, bool unlimited = false)
        {
            return limit switch
            {
                LimitCount.Unusable => Unusable,
                LimitCount.Forbidden => Forbidden,
                LimitCount.Limit1 => Limit1,
                LimitCount.Limit2 => Limit2,
                LimitCount.Specified => Specified,
                _ => unlimited ? Unlimited : "",
            };
        }
    }
}
