using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Ogg.Vorbis
{
    public class VorbisCommentEditor : DisposableBase
    {
        private IdHeader _id_header;
        private readonly VorbisCommentList _comments = [];
        private PacketInfo _setup_header;

        private readonly Queue<PacketInfo> _packets = [];
        private readonly OutputStreamState _output = new();
        private bool _loaded;
        private bool _need_headerOut = true;

        public IdHeader IdHeader => _id_header;
        public int SampleRate => IdHeader.SampleRate;
        public int Channels => IdHeader.Channels;
        public VorbisCommentList Comments => _comments;

        private static void Verify(bool throwUnless) => OggException.ThrowInvalidDataIf(!throwUnless);

        public void Load(Stream source, bool leaveOpen)
        {
            Clear();
            using OggReader input = new(source, leaveOpen);
            var output = _output;
            var packets = _packets;
            // ヘッダの読み込み
            // IDヘッダ
            Verify(input.PacketOut(out var packet) && VorbisHeader.IsValidHeader(packet, PacketType.Identification));
            _id_header = IdHeader.Create(packet);
            var sno = output.SerialNumber = input.CurrentStreamNumber;
            // コメントヘッダ
            Verify(input.PacketOut(out packet, sno) && VorbisHeader.IsValidHeader(packet, PacketType.Comment));
            _comments.LoadPacket(packet);
            // セットアップヘッダ
            Verify(input.PacketOut(out packet, sno) && VorbisHeader.IsValidHeader(packet, PacketType.Setup));
            _setup_header = packet;
            // 残りのパケット(音声データ)を全て抽出
            while (input.PacketOut(out packet, sno))
            {
                packets.Enqueue(packet);
            }
            _loaded = true;
        }

        public void Clear()
        {
            _packets.Clear();
            _output.Clear();
            _id_header = default;
            _setup_header = default;
            _comments.Clear();
            _loaded = false;
            _need_headerOut = false;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            Clear();
        }

        public void AddComment(string key, string value)
        {
            Verify(_need_headerOut);
            Comments.Add(key, value);
        }

        public void AddComment<T>(T source)
            where T : IEnumerable<VorbisComment>
        {
            Verify(_need_headerOut);
            Comments.AddRange(source);
        }

        public void WriteHeader()
        {
            Verify(!_loaded);
            Verify(_need_headerOut);
            _need_headerOut = false;
            var output = _output;
            // id header
            output.PacketIn(IdHeader.ToPacket());
            output.Flush();
            // comment & setup header
            output.PacketIn(Comments.ToPacket());
            output.PacketIn(_setup_header);
            output.Flush();
        }

        public void Dump(Stream target)
        {
            if (_need_headerOut)
            {
                WriteHeader();
            }
            var input = _packets;
            var output = _output;
            var force = false;
            while (true)
            {
                if (output.PageOut(out var page, force))
                {
                    page.Dump(target);
                }
                else if (input.TryDequeue(out var packet))
                {
                    force |= packet.IsEOF;
                    output.PacketIn(packet);
                }
                else
                {
                    break;
                }
            }
        }

        public static bool IsSupported(string path) => IsSupported(File.OpenRead(path), false);
        public static bool IsSupported(Stream source, bool leaveOpen = true)
        {
            var pos = source.Position;
            try
            {
                using OggReader input = new(source, leaveOpen);
                // IDヘッダ
                if (input.PacketOut(out var packet) && VorbisHeader.IsValidHeader(packet, PacketType.Identification))
                {
                    var sno = input.CurrentStreamNumber;
                    if (input.PacketOut(out packet, sno) && VorbisHeader.IsValidHeader(packet, PacketType.Comment) &&
                        input.PacketOut(out packet, sno) && VorbisHeader.IsValidHeader(packet, PacketType.Setup))
                    {
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                if (leaveOpen)
                {
                    source.Position = pos;
                }
            }
        }
    }
}
