using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using LivreNoirLibrary.UnsafeOperations;

namespace LivreNoirLibrary.Media.Ogg
{
    public class OggPage
    {
        public const int SegmentUnit = 255;
        public const int MaxSegmentsCount = 255;
        public const int MinHeaderLength = Index_SegmentsTable;
        public const int MaxHeaderLength = MinHeaderLength + MaxSegmentsCount;
        public const int MaxBodyLength = SegmentUnit * MaxSegmentsCount;

        public const byte CapturePattern_O = (byte)'O';
        private static readonly byte[] _ggS = [(byte)'g', (byte)'g', (byte)'S'];
        public const uint CapturePatternNumber = 0x5367674f; // Raw bytes of "OggS" in the little endian
        public const int Index_StreamStructureVersion = sizeof(uint);
        public const int Index_HeaderType = Index_StreamStructureVersion + sizeof(byte);
        public const int Index_GranulePosition = Index_HeaderType + sizeof(byte);
        public const int Index_StreamSerialNumber = Index_GranulePosition + sizeof(long);
        public const int Index_PageSequenceNumber = Index_StreamSerialNumber + sizeof(int);
        public const int Index_Checksum = Index_PageSequenceNumber + sizeof(int);
        public const int Index_SegmentsCount = Index_Checksum + sizeof(uint);
        public const int Index_SegmentsTable = Index_SegmentsCount + sizeof(byte);

        private readonly byte[] _header;
        private readonly byte[] _body;

        public int HeaderLength => _header.Length;
        public int BodyLength => _body.Length;

        public HeaderType HeaderType
        {
            get => (HeaderType)_header[Index_HeaderType];
            set => _header[Index_HeaderType] = (byte)value;
        }

        public long GranulePosition
        {
            get => _header.Get<long>(Index_GranulePosition);
            set => _header.Set(Index_GranulePosition, value);
        }

        public int StreamNumber
        {
            get => _header.Get<int>(Index_StreamSerialNumber);
            set => _header.Set(Index_StreamSerialNumber, value);
        }

        public int PageNumber
        {
            get => _header.Get<int>(Index_PageSequenceNumber);
            set => _header.Set(Index_PageSequenceNumber, value);
        }

        public uint Checksum
        {
            get => _header.Get<uint>(Index_Checksum);
            internal set => _header.Set(Index_Checksum, value);
        }

        public int SegmentsCount
        {
            get => _header[Index_SegmentsCount];
            set => _header[Index_SegmentsCount] = (byte)value;
        }

        public Span<byte> SegmentsTable => _header.AsSpan(Index_SegmentsTable, SegmentsCount);
        public Span<byte> Body => _body;

        internal OggPage(byte[] header, byte[] body)
        {
            _header = header;
            _body = body;
        }

        private OggPage() : this(new byte[MinHeaderLength], [])
        {
            Initialize();
        }

        public void Initialize()
        {
            _header.Set(0, CapturePatternNumber);
            _header[Index_StreamStructureVersion] = 0;
        }

        private string GetSegmentsTableText() => GetSegmentsTableText(SegmentsTable);

        public static string GetSegmentsTableText(ReadOnlySpan<byte> segmentsTable)
        {
            StringBuilder sb = new();
            sb.Append('[');
            var flag = false;
            foreach (var b in segmentsTable)
            {
                if (flag)
                {
                    sb.Append('-');
                }
                else
                {
                    flag = true;
                }
                sb.Append(b);
            }
            sb.Append(']');
            return sb.ToString();
        }

        public override string ToString()
            => $"OggPage{{Type={HeaderType}, GranulePosition={GranulePosition}, SerialNo.={StreamNumber}, PageNo.={PageNumber}, Segments({SegmentsCount})={GetSegmentsTableText()}}}";

        public string ToString(long lastPosition)
        {
            if (lastPosition is 0)
            {
                return ToString();
            }
            var pos = GranulePosition;
            var len = pos - lastPosition;
            return $"OggPage{{Type={HeaderType}, Pos={pos}(+{len}), SNo.={StreamNumber}, PNo.={PageNumber}, Segments({SegmentsCount})={GetSegmentsTableText()}}}";
        }

        public static bool TryRead(BinaryReader reader, [MaybeNullWhen(false)]out OggPage page)
        {
            var ggS = _ggS.AsSpan();
            var buffer = ArrayPool<byte>.Shared.Rent(255);
            try
            {
                while (true)
                {
                    try
                    {
                        // capture pattern "OggS"
                        if (reader.ReadByte() is CapturePattern_O)
                        {
                            buffer[0] = CapturePattern_O;
                            var read = reader.Read(buffer, 1, 3);
                            if (read is not 3 || !ggS.SequenceEqual(buffer.AsSpan(1, 3)))
                            {
                                continue;
                            }
                            // stream structure version
                            OggException.Verify_StructureVersion(reader.ReadByte());
                            // header contents
                            read = reader.Read(buffer, Index_HeaderType, MinHeaderLength - Index_HeaderType);
                            OggException.ThrowIfEndOfStream(read, MinHeaderLength - Index_HeaderType);
                            int segCount = buffer[Index_SegmentsCount];
                            var header = new byte[MinHeaderLength + segCount];
                            Array.Copy(buffer, header, MinHeaderLength);
                            // segments
                            read = reader.Read(buffer, 0, segCount);
                            OggException.ThrowIfEndOfStream(read, segCount);
                            Array.Copy(buffer, 0, header, Index_SegmentsTable, segCount);
                            // body
                            var bodyLen = 0;
                            foreach (var segment in buffer.AsSpan(0, segCount))
                            {
                                bodyLen += segment;
                            }
                            var body = new byte[bodyLen];
                            read = reader.Read(body, 0, bodyLen);
                            OggException.ThrowIfEndOfStream(read, bodyLen);
                            page = new(header, body);
                            return true;
                        }
                    }
                    catch (EndOfStreamException)
                    {
                        page = null;
                        return false;
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static uint GetChecksum(byte[] header, byte[] body)
        {
            uint c = 0;
            c = CRC.Update(c, header);
            c = CRC.Update(c, body);
            return c;
        }

        public void SetChecksum()
        {
            Checksum = 0;
            Checksum = GetChecksum(_header, _body);
        }

        public void Dump(Stream stream)
        {
            SetChecksum();
            stream.Write(_header, 0, _header.Length);
            stream.Write(_body, 0, _body.Length);
        }
    }
}
