using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Ogg
{
    public class InputStreamState(int serialNumber = -1) : StreamState(serialNumber)
    {
        private OggPage? _current_page;
        private int _expected_page_number;
        private int _page_segment_index;
        private int _page_body_index;

        private readonly List<byte[]> _body_buffers = [];
        private int _body_buffer_length;

        public void Clear()
        {
            _current_page = null;
            _body_buffers.Clear();
            _body_buffer_length = 0;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            Clear();
        }

        public override void PageIn(in OggPage page)
        {
            var pno = page.PageNumber;
            OggException.Verify_PageNumber(pno, _expected_page_number);
            base.PageIn(page);
            _expected_page_number = pno + 1;
        }

        public bool PacketOut(out PacketInfo packet)
        {
            var pages = _pages;
            var segIndex = _page_segment_index;
            var bodyIndex = _page_body_index;
            var bodyLength = 0;
            List<byte[]> buffers = _body_buffers;
            var bufferLength = _body_buffer_length;
            var completed = false;
            var page = _current_page;

            while (page is not null || pages.TryDequeue(out page))
            {
                var table = page.SegmentsTable;
                var max = page.SegmentsCount;
                var granulePosition = page.GranulePosition;
                while (segIndex < max)
                {
                    var seg = table[segIndex];
                    bodyLength += seg;
                    segIndex++;
                    if (seg is < 255)
                    {
                        completed = true;
                        break;
                    }
                }
                if (bodyLength is > 0)
                {
                    var b = page.Body.Slice(bodyIndex, bodyLength).ToArray();
                    buffers.Add(b);
                    bufferLength += bodyLength;
                }
                if (completed)
                {
                    var data = new byte[bufferLength];
                    var index = 0;
                    foreach (var buffer in CollectionsMarshal.AsSpan(buffers))
                    {
                        var bLen = buffer.Length;
                        Array.Copy(buffer, 0, data, index, bLen);
                        index += bLen;
                    }
                    bufferLength = 0;
                    buffers.Clear();
                    var eof = false;
                    if (segIndex >= max)
                    {
                        eof = page.HeaderType.IsEndOfStream();
                        Dequeue();
                    }
                    ReturnProcess();
                    packet = new(data, granulePosition, eof);
                    return true;
                }
                Dequeue();
            }
            ReturnProcess();
            packet = default;
            return false;

            void Dequeue()
            {
                segIndex = 0;
                bodyIndex = 0;
                bodyLength = 0;
                page = null;
            }

            void ReturnProcess()
            {
                _current_page = page;
                _page_segment_index = segIndex;
                _page_body_index = bodyIndex + bodyLength;
                _body_buffer_length = bufferLength;
            }
        }
    }
}
