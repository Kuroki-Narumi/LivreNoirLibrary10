using LivreNoirLibrary.Debug;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LivreNoirLibrary.Media.Wave
{
    public class WaveBufferProvider : IWaveBufferProvider<string>
    {
        private readonly WaveBuffer _buffer = new();

        public static WaveBufferProvider Default { get; } = new();

        public int OutputSampleRate { get; set; }
        public int OutputChannels { get; set; }

        private WaveBufferProvider() { }

        public void Clear()
        {
            _buffer.Clear();
        }

        public bool TryGetWaveBuffer(string path, [MaybeNullWhen(false)] out IWaveBuffer waveBuffer)
        {
            try
            {
                _buffer.SetLayout(OutputSampleRate, OutputChannels);
                _buffer.AutoDecode(path, false);
                waveBuffer = _buffer;
                return true;
            }
            catch (FileNotFoundException)
            {
                ExConsole.Write($"file \"{path}\" is not found.");
            }
            catch (Exception ex)
            {
                ExConsole.Write($"failed to decode \"{path}\":");
                ExConsole.Write(ex);
            }
            waveBuffer = null;
            return false;
        }
    }
}
