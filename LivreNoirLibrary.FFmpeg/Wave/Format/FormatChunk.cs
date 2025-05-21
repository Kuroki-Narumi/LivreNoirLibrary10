using System;
using System.Collections.Generic;
using System.IO;
using System.Buffers;
using System.Text.Json;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Wave
{
    public readonly struct FormatChunk(FormatType tag, ushort channels, uint sampleRate, uint bytesPerSecond, ushort blockAlign, ushort bits) : 
        IRiffChunk<FormatChunk>, IDumpable<FormatChunk>, IJsonWriter
    {
        public string Chid => ChunkIds.Format;
        public uint ByteSize => HasExtension ? 40u : Tag is FormatType.PCM ? 16u : 18u;

        /// <summary>
        /// format type
        /// </summary>
        public readonly FormatType Tag = tag;
        /// <summary>
        /// number of channels
        /// </summary>
        public readonly ushort Channels = channels;
        /// <summary>
        /// sample rate
        /// </summary>
        public readonly uint SampleRate = sampleRate;
        /// <summary>
        /// for buffer estimation
        /// </summary>
        public readonly uint BytesPerSecond = bytesPerSecond;
        /// <summary>
        /// bytes per (sample * channels)
        /// </summary>
        public readonly ushort BlockAlign = blockAlign;
        /// <summary>
        /// number of bits per sample of mono data
        /// </summary>
        public readonly ushort Bits = bits;
        public readonly bool HasExtension;
        public readonly ushort ValidBits;
        public readonly uint ChannelMask;
        public readonly Guid SubFormat;

        public FormatChunk(FormatType tag, ushort channels, uint sampleRate, uint bytesPerSecond, ushort blockAlign, ushort bits, ushort validBits, uint channelMask, Guid subFormat) : 
            this(tag, channels, sampleRate, bytesPerSecond, blockAlign, bits)
        {
            HasExtension = true;
            ValidBits = validBits;
            ChannelMask = channelMask;
            SubFormat = subFormat;
        }

        public static FormatChunk Create(int sampleRate, int channels, SampleFormat format)
        {
            var tag = format is SampleFormat.Float32 ? FormatType.Float : FormatType.PCM;
            var bits = format is SampleFormat.Float32 ? 32 : (int)format;
            var blockAlign = bits * channels / 8;
            var bytesPerSecond = blockAlign * sampleRate;
            return new(tag, (ushort)channels, (uint)sampleRate, (uint)bytesPerSecond, (ushort)blockAlign, (ushort)bits);
        }

        public static int CalculateBlockAlign(int channels, SampleFormat format)
        {
            return (format is SampleFormat.Float32 ? 32 : (int)format) * channels / 8;
        }

        public static FormatChunk Load(BinaryReader reader) => reader.ReadRiffChunk<FormatChunk>(ChunkIds.Format);
        public static FormatChunk LoadWithoudChid(BinaryReader reader) => reader.ReadRiffChunk<FormatChunk>();
        public static FormatChunk LoadContents(BinaryReader reader, uint length)
        {
            var tag = (FormatType)reader.ReadUInt16();
            var channels = reader.ReadUInt16();
            var sampleRate = reader.ReadUInt32();
            var bytesPerSecond = reader.ReadUInt32();
            var blockAlign = reader.ReadUInt16();
            var bits = reader.ReadUInt16();
            if (length is >= 18)
            {
                var extSize = reader.ReadUInt16();
                var buffer = ArrayPool<byte>.Shared.Rent(extSize);
                try
                {
                    if (extSize is 22)
                    {
                        var validBits = reader.ReadUInt16();
                        var channelMask = reader.ReadUInt32();
                        reader.Read(buffer, 0, 16);
                        var subFormat = new Guid(buffer.AsSpan(0, 16));
                        return new(tag, channels, sampleRate, bytesPerSecond, blockAlign, bits, validBits, channelMask, subFormat);
                    }
                    reader.Read(buffer, 0, extSize);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
            return new(tag, channels, sampleRate, bytesPerSecond, blockAlign, bits);
        }

        public void Dump(BinaryWriter writer) => writer.WriteRiffChunk(this);
        public void DumpContents(BinaryWriter writer)
        {
            writer.Write((ushort)Tag);
            writer.Write(Channels);
            writer.Write(SampleRate);
            writer.Write(BytesPerSecond);
            writer.Write(BlockAlign);
            writer.Write(Bits);
            if (HasExtension)
            {
                writer.Write((uint)22);
                writer.Write(ValidBits);
                writer.Write(ChannelMask);
                var buffer = ArrayPool<byte>.Shared.Rent(16);
                SubFormat.TryWriteBytes(buffer);
                writer.Write(buffer, 0, 16);
                ArrayPool<byte>.Shared.Return(buffer);
            }
            if (Tag is not FormatType.PCM)
            {
                writer.Write((ushort)0);
            }
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options) => this.WriteJsonBasic(writer, options);
        public void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            if (Enum.IsDefined(Tag))
            {
                writer.WriteString("tag", Tag.ToString());
            }
            else
            {
                writer.WriteNumber("tag", (int)Tag);
            }
            writer.WriteNumber("channels", Channels);
            writer.WriteNumber("sample rate", SampleRate);
            writer.WriteNumber("bytes per second", BytesPerSecond);
            writer.WriteNumber("block align", BlockAlign);
            writer.WriteNumber("bits", Bits);
            if (HasExtension)
            {
                writer.WriteNumber("valid bits", ValidBits);
                writer.WriteNumber("channel mask", ChannelMask);
                writer.WriteString("sub format", SubFormat.ToString());
            }
        }

        public bool TryGetSampleFormat(out SampleFormat format)
        {
            if (Tag is FormatType.Float)
            {
                format = SampleFormat.Float32;
                return Bits is 32;
            }
            format = (SampleFormat)Bits;
            return Tag is FormatType.PCM && format.IsValid();
        }
    }
}
