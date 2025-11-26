using LivreNoirLibrary.Debug;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LivreNoirLibrary.Media.Wave
{
    public class WaveBufferProvider : IWaveBufferProvider
    {
        private readonly WaveBuffer _buffer = new();

        public static WaveBufferProvider Default { get; } = new();

        private WaveBufferProvider() { }

        public bool TryGetWaveBuffer(string path, [MaybeNullWhen(false)] out IWaveBuffer waveBuffer)
        {
            try
            {
                _buffer.AutoDecode(path);
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
