using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static LivreNoirLibrary.Media.Ogg.OggPage;

namespace LivreNoirLibrary.Media.Ogg
{
    public class OutputStreamState(int serialNumber = -1) : StreamState(serialNumber)
    {
        public int FlushBufferLength { get; set; } = 4096;
        public int FlushPacketCount { get; set; } = 4;

        private readonly Queue<PacketInfo> _packet_buffer = [];
        private int _buffer_written;
        private int _packet_written;
        private int _page_number;
        private bool _continued;
        private long _last_pos = -1;

        public void Clear()
        {
            _packet_buffer.Clear();
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            Clear();
        }

        public void PacketIn(in PacketInfo info)
        {
            var (data, granulePosition, eof) = info;
            var len = data.Length;
            var q = len / SegmentUnit;
            var buffer = _packet_buffer;
            var index = 0;
            PacketInfo segment;
            for (; q is > 0; q--)
            {
                segment = new(data[index..(index + SegmentUnit)], granulePosition, false);
                buffer.Enqueue(segment);
                index += SegmentUnit;
            }
            segment = new(index >= len ? [] : data[index..], granulePosition, eof);
            buffer.Enqueue(segment);
            _buffer_written += len;
            _packet_written++;
            if (_buffer_written > FlushBufferLength && _packet_written >= FlushPacketCount)
            {
                Flush();
            }
        }

        public bool PageOut([MaybeNullWhen(false)]out OggPage page, bool force = false)
        {
            if (base.PageOut(out page))
            {
                return true;
            }
            else if (force)
            {
                Flush();
                return base.PageOut(out page);
            }
            page = null;
            return false;
        }

        public void Flush()
        {
            var packets = _packet_buffer;
            if (packets.Count is 0)
            {
                return;
            }
            var segCount = Math.Min(packets.Count, MaxSegmentsCount);
            var header = new byte[MinHeaderLength + segCount];
            header[Index_SegmentsCount] = (byte)segCount;
            var segTable = header.AsSpan(Index_SegmentsTable, segCount);
            var buffer = ArrayPool<byte>.Shared.Rent(MaxBodyLength);
            var granulePos = _last_pos;
            var bufferWritten = _buffer_written;
            var packetWritten = _packet_written;
            try
            {
                HeaderType type = 0;
                if (_page_number is 0)
                {
                    granulePos = 0;
                    type |= HeaderType.BeginningOfStream;
                }
                if (_continued)
                {
                    type |= HeaderType.Continued;
                }
                var index = 0;
                for (var i = 0; i < segCount; i++)
                {
                    var (data, pos, eof) = packets.Dequeue();
                    var len = data.Length;
                    segTable[i] = (byte)len;
                    Array.Copy(data, 0, buffer, index, len);
                    index += len;
                    if (len is < SegmentUnit)
                    {
                        packetWritten--;
                        if (pos is >= 0)
                        {
                            granulePos = pos;
                        }
                    }
                    if (eof)
                    {
                        type |= HeaderType.EndOfStream;
                    }
                }
                header[Index_HeaderType] = (byte)type;
                OggPage page = new(header, buffer[0..index])
                {
                    GranulePosition = granulePos,
                    StreamNumber = _serial_number,
                    PageNumber = _page_number,
                };
                page.Initialize();
                base.PageIn(page);
                bufferWritten -= index;
                _continued = packets.Count is > 0;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
                _buffer_written = bufferWritten;
                _packet_written = packetWritten;
                _last_pos = granulePos;
                _page_number++;
            }
        }
    }
}
