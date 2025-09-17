using System;
using System.Text.Json;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.UnsafeOperations;

namespace LivreNoirLibrary.Media.Ogg.Vorbis
{
    public struct IdHeader : IJsonWriter
    {
        private static readonly int[] _valid_block_sizes = [64, 128, 256, 512, 1024, 2048, 4096, 8192];
        private static readonly int _valid_block_count = _valid_block_sizes.Length;

        private byte _channels;
        private int _rate;
        private int _bitrate_max;
        private int _bitrate_nom;
        private int _bitrate_min;
        private ushort _blocksize_small;
        private ushort _blocksize_large;

        public int Channels { readonly get => _channels; set => _channels = (byte)Math.Clamp(value, 1, 255); }
        public int SampleRate { readonly get => _rate; set => _rate = Math.Max(1, value); }
        public int MaximumBitrate { readonly get => _bitrate_max; set => _bitrate_max = value; }
        public int NominalBitrate { readonly get => _bitrate_nom; set => _bitrate_nom = value; }
        public int MinimumBitrate { readonly get => _bitrate_min; set => _bitrate_min = value; }

        public int SmallBlockSize
        {
            readonly get => _blocksize_small;
            set
            {
                CheckBlockSizeRange(ref value);
                _blocksize_small = (ushort)value;
            }
        }

        public int LargeBlockSize
        {
            readonly get => _blocksize_large;
            set
            {
                CheckBlockSizeRange(ref value);
                _blocksize_large = (ushort)value;
            }
        }

        private static void CheckBlockSizeRange(ref int value)
        {
            var index = _valid_block_sizes.AsSpan().BinarySearch(value);
            if (index is < 0)
            {
                index = ~index;
                value = index >= _valid_block_count ? _valid_block_sizes[^1] : _valid_block_sizes[index];
            }
        }

        private readonly void Verify(int framingFlag)
        {
            OggException.ThrowInvalidDataIf(
                _rate is < 1 ||
                _channels is < 1 ||
                _blocksize_small < _valid_block_sizes[0] ||
                _blocksize_small > _blocksize_large ||
                _blocksize_large > _valid_block_sizes[^1] ||
                (framingFlag & 1) is not 1
                );
        }

        public static IdHeader Create(in PacketInfo packet)
        {
            var data = packet.Data;
            OggException.ThrowInvalidDataIf(
                !VorbisHeader.IsValidHeader(data, PacketType.Identification) ||
                data.Get<int>(VorbisHeader.Index_VorbisVersion) is not 0
                );
            IdHeader result = new()
            {
                _channels = data[VorbisHeader.Index_Channels],
                _rate = data.Get<int>(VorbisHeader.Index_SampleRate),
                _bitrate_max = data.Get<int>(VorbisHeader.Index_MaximumBitrate),
                _bitrate_nom = data.Get<int>(VorbisHeader.Index_NominalBitrate),
                _bitrate_min = data.Get<int>(VorbisHeader.Index_MinimumBitrate)
            };
            var b01 = data[VorbisHeader.Index_BlockSize];
            result._blocksize_small = (ushort)(1 << (b01 & 0xF));
            result._blocksize_large = (ushort)(1 << ((b01 >> 4) & 0xF));
            result.Verify(data[VorbisHeader.Index_Framing_Flag]);
            return result;
        }

        public readonly PacketInfo ToPacket()
        {
            var data = new byte[VorbisHeader.IdHeaderLength];
            VorbisHeader.InitializeHeader(data, PacketType.Identification);
            data[VorbisHeader.Index_Channels] = _channels;
            data.Set(VorbisHeader.Index_SampleRate, _rate);
            data.Set(VorbisHeader.Index_MaximumBitrate, _bitrate_max);
            data.Set(VorbisHeader.Index_NominalBitrate, _bitrate_nom);
            data.Set(VorbisHeader.Index_MinimumBitrate, _bitrate_min);
            var b0 = int.Log2(_blocksize_small);
            var b1 = int.Log2(_blocksize_large);
            data[VorbisHeader.Index_BlockSize] = (byte)((b0 & 0xF) | ((b1 & 0xF) << 4));
            data[VorbisHeader.Index_Framing_Flag] = 1;
            return new(data, 0, false);
        }

        public readonly void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("vorbis version", 0);
            writer.WriteNumber("channels", _channels);
            writer.WriteNumber("sample rate", _rate);
            writer.WriteNumber("maximum bitrate", _bitrate_max / 1000m);
            writer.WriteNumber("nominal bitrate", _bitrate_nom / 1000m);
            writer.WriteNumber("minimum bitrate", _bitrate_min / 1000m);
            writer.WriteNumber("small block size", _blocksize_small);
            writer.WriteNumber("large block size", _blocksize_large);
            writer.WriteEndObject();
        }
    }
}
