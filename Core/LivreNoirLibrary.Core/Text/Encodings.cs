using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Text
{
    public static class Encodings
    {
        public static List<Encoding> GetAllEncodings()
        {
            List<Encoding> list = [];
            foreach (var e in CodePagesEncodingProvider.Instance.GetEncodings())
            {
                list.Add(e.GetEncoding());
            }
            return list;
        }

        public static Encoding Get(int codepage) => CodePagesEncodingProvider.Instance.GetEncoding(codepage) ?? Encoding.UTF8;
        public static Encoding Get(string name) => CodePagesEncodingProvider.Instance.GetEncoding(name) ?? Encoding.UTF8;
        public static Encoding Get(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback) => 
            CodePagesEncodingProvider.Instance.GetEncoding(name, encoderFallback, decoderFallback) ?? Encoding.UTF8;

        public static Encoding Shift_JIS { get; } = Get("shift-jis");
    }
}
