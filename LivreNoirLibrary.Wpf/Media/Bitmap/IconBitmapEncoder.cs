using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media
{
    public class IconBitmapEncoder
    {
        private readonly List<IconData> _data = [];

        public IconBitmapEncoder() { }
        public IconBitmapEncoder(BitmapSource bitmap) => Add(bitmap);
        public IconBitmapEncoder(params ReadOnlySpan<BitmapSource> bitmaps) => Add(bitmaps);
        public IconBitmapEncoder(BitmapDecoder decoder) => Add(decoder);

        public void Clear()
        {
            foreach (var data in _data)
            {
                data.Dispose();
            }
            _data.Clear();
        }

        public void Add(BitmapSource bitmap) => _data.Add(new(bitmap));

        public void Add(params ReadOnlySpan<BitmapSource> bitmaps)
        {
            foreach (var bitmap in bitmaps)
            {
                Add(bitmap);
            }
        }

        public void Add(BitmapDecoder decoder)
        {
            foreach (var bitmap in decoder.Frames)
            {
                Add(bitmap);
            }
        }

        public void Save(Stream stream)
        {
            using BinaryWriter writer = new(stream, Encoding.UTF8, true);
            Save(writer);
        }

        public void Save(BinaryWriter writer)
        {
            var stream = writer.BaseStream;
            var data = _data;
            var count = data.Count;
            // ICO header
            writer.Write((ushort)0); // reserved
            writer.Write((ushort)1); // 1 => icon, 2 => cursor
            writer.Write((ushort)count); // number of images
            // write directories
            var byteOffset = 6L /* header size */ + count * 16L /* directory size */;
            for (var i = 0; i < count; i++)
            {
                data[i].WriteDirectory(writer);
                writer.Write((uint)byteOffset);
                byteOffset += data[i]._byteSize;
            }
            // write data
            for (var i = 0; i < count; i++)
            {
                data[i].WriteContent(stream);
            }
        }

        private class IconData : DisposableBase
        {
            public readonly byte _width;
            public readonly byte _height;
            public readonly ushort _bitsPerPixel;
            public readonly uint _byteSize;
            private byte[]? _buffer;

            public IconData(BitmapSource source)
            {
                _width = (byte)source.PixelWidth;
                _height = (byte)source.PixelHeight;
                _bitsPerPixel = (ushort)source.Format.BitsPerPixel;
                MemoryStream ms = new();
                PngBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(ms);
                _buffer = ms.ToArray();
                _byteSize = (uint)_buffer.Length;
            }

            public void WriteDirectory(BinaryWriter writer)
            {
                writer.Write(_width);
                writer.Write(_height);
                writer.Write((byte)0);  // number of color pallettes
                writer.Write((byte)0);  // reserved
                writer.Write((ushort)1);  // number of color planes
                writer.Write(_bitsPerPixel);
                writer.Write(_byteSize);
            }

            public void WriteContent(Stream target) => target.Write(_buffer!, 0, _buffer!.Length);

            protected override void DisposeUnmanaged()
            {
                base.DisposeUnmanaged();
                _buffer = null;
            }
        }
    }
}
