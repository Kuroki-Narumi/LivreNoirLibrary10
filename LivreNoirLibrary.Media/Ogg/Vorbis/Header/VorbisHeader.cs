using System;

namespace LivreNoirLibrary.Media.Ogg.Vorbis
{
    public static class VorbisHeader
    {
        public static void InitializeHeader(Span<byte> data, PacketType type)
        {
            data[0] = (byte)type;
            CapturePattern.CopyTo(data[1..]);
        }

        public static bool IsValidHeader(ReadOnlySpan<byte> data, PacketType type) => (byte)type == data[0] && CapturePattern.SequenceEqual(data.Slice(1, 6));
        public static bool IsValidHeader(PacketInfo packet, PacketType type) => IsValidHeader(packet.Data, type);

        private static readonly byte[] _cap = [(byte)'v', (byte)'o', (byte)'r', (byte)'b', (byte)'i', (byte)'s'];
        public static ReadOnlySpan<byte> CapturePattern => _cap;
        public const uint CapturePattern_LSB = 0x62726f76; // Raw bytes of "vorb" in the little endian
        public const ushort CapturePattern_MSB = 0x7369;   // Raw bytes of "is" in the little endian

        public const int Index_CapturePattern_LSB = sizeof(byte);
        public const int Index_CapturePattern_MSB = Index_CapturePattern_LSB + sizeof(uint);
        public const int CommonHeaderLength = Index_CapturePattern_MSB + sizeof(ushort);

        public const int Index_VorbisVersion = CommonHeaderLength;
        public const int Index_Channels = Index_VorbisVersion + sizeof(uint);
        public const int Index_SampleRate = Index_Channels + sizeof(byte);
        public const int Index_MaximumBitrate = Index_SampleRate + sizeof(uint);
        public const int Index_NominalBitrate = Index_MaximumBitrate + sizeof(int);
        public const int Index_MinimumBitrate = Index_NominalBitrate + sizeof(int);
        public const int Index_BlockSize = Index_MinimumBitrate + sizeof(int);
        public const int Index_Framing_Flag = Index_BlockSize + sizeof(byte);
        public const int IdHeaderLength = Index_Framing_Flag + sizeof(byte);

        public const int Index_VendorLength = CommonHeaderLength;
        public const int Index_Vendor = Index_VorbisVersion + sizeof(uint);
    }
}
