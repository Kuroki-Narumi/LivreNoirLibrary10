using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LivreNoirLibrary.Media.Wave
{
    public class CachedWaveBufferProvider : IWaveBufferProvider<string>, IClear
    {
        private readonly Dictionary<string, WaveBuffer?> _buffers = [];

        public int SampleRate { get; set => ChangeLayout(ref field, value); } = WaveBuffer.DefaultSampleRate;
        public int Channels { get; set => ChangeLayout(ref field, value); } = WaveBuffer.DefaultChannels;

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
                buffer?.Dispose();
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
                if (File.Exists(path))
                {
                    try
                    {
                        data = WaveBuffer.CreateUnsafe(SampleRate, Channels);
                        data.AutoDecode(path, false);
                    }
                    catch (Exception ex)
                    {
                        data = null;
                        ExConsole.Write($"failed to decode \"{path}\":");
                        ExConsole.Write(ex);
                    }
                }
                else
                {
                    //ExConsole.Write($"file \"{path}\" is not found.");
                }
                _buffers.Add(path, data);
            }
            waveBuffer = data;
            return data is not null;
        }
    }
}
