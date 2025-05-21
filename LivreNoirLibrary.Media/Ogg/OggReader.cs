using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.Ogg
{
    public class OggReader(Stream stream, bool leaveOpen = false) : BinaryReader(stream, Encoding.UTF8, leaveOpen)
    {
        private int _current_stream_number = -1;
        private readonly Dictionary<int, InputStreamState> _pages = [];

        public int CurrentStreamNumber => _current_stream_number;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                foreach (var (_, ss) in _pages)
                {
                    ss.Dispose();
                }
                _pages.Clear();
            }
        }

        public bool TryReadNextPage([MaybeNullWhen(false)] out OggPage page)
        {
            if (OggPage.TryRead(this, out page))
            {
                var pages = _pages;
                var sno = page.StreamNumber;
                _current_stream_number = sno;
                if (!pages.TryGetValue(sno, out var os))
                {
                    os = new(sno);
                    pages.Add(sno, os);
                }
                os.PageIn(page);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PacketOut(out PacketInfo packet, int streamNumber = -1)
        {
            if (streamNumber is <= 0)
            {
                streamNumber = _current_stream_number;
            }
            var pages = _pages;
            while (true)
            {
                if (pages.TryGetValue(streamNumber, out var os) && os.PacketOut(out packet))
                {
                    return true;
                }
                if (TryReadNextPage(out var page))
                {
                    if (streamNumber is <= 0)
                    {
                        streamNumber = page.StreamNumber;
                    }
                }
                else
                {
                    break;
                }
            }
            packet = default;
            return false;
        }
    }
}
