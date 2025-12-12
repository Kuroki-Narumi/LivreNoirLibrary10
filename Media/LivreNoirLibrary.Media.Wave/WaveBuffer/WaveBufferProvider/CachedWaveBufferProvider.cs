using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LivreNoirLibrary.Media.Wave
{
    public class CachedWaveBufferProvider : IWaveBufferProvider<string>
    {
        private readonly Dictionary<string, WaveBuffer> _buffers = [];

        public int OutputSampleRate { get; set => ChangeLayout(ref field, value); }
        public int OutputChannels { get; set => ChangeLayout(ref field, value); }

        public CachedWaveBufferProvider() { }

        private void ChangeLayout(ref int field, int value)
        {
            if (value != field)
            {
                if (value is not 0)
                {
                    Clear();
                }
                field = value;
            }
        }

        public void Clear()
        {
            foreach (var (_, buffer) in _buffers)
            {
                buffer.Dispose();
            }
            _buffers.Clear();
        }

        public bool Remove(string path) => _buffers.Remove(path);
        public int RemoveRange(IEnumerable<string> paths) => _buffers.RemoveRange(paths);
        public int RemoveRange(ReadOnlySpan<string> paths) => _buffers.RemoveRange(paths);

        public bool TryGetWaveBuffer(string path, [MaybeNullWhen(false)] out IWaveBuffer waveBuffer)
        {
            if (!_buffers.TryGetValue(path, out var data))
            {
                if (!File.Exists(path))
                {
                    ExConsole.Write($"file \"{path}\" is not found.");
                }
                else
                {
                    try
                    {
                        data = WaveBuffer.CreateUnsafe(OutputSampleRate, OutputChannels);
                        data.AutoDecode(path, false);
                        _buffers.Add(path, data);
                    }
                    catch (Exception ex)
                    {
                        ExConsole.Write($"failed to decode \"{path}\":");
                        ExConsole.Write(ex);
                        data = null;
                    }
                }
            }
            waveBuffer = data;
            return data is not null;
        }
    }
}
