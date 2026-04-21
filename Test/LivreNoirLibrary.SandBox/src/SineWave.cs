using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.SandBox
{
    public static class SineWave
    {
        public static int Channels { get; set; } = 2;
        public static int SampleRate { get; set; } = 48000;
        public static int SampleCount { get; set; } = 134217728 + 16; // 2^27

        public static void Generate(string outputPath, double startFrequency, double endFrequency)
        {
            var channels = Channels;
            var sampleRate = SampleRate;
            var sampleCount = SampleCount;
            var frequencyStep = (endFrequency - startFrequency) / sampleCount;

            // 1秒分のバッファを確保
            using var buffer = ArrayPool.Rent<float>(channels * sampleRate);
            var span = buffer.Span;
            // エンコーダの作成
            using var encoder = new WaveEncoder(outputPath, new(sampleRate, channels, SampleFormat.Int16));
            var sampleWritten = 0;
            while (sampleCount is > 0)
            {
                var sampleToWrite = Math.Min(sampleCount, span.Length / channels);
                for (var i = 0; i < sampleToWrite; i++, sampleWritten++)
                {
                    var frequency = startFrequency + frequencyStep * sampleWritten;
                    var value = (float)Math.Sin(2 * Math.PI * frequency * sampleWritten / sampleRate);
                    for (var c = 0; c < channels; c++)
                    {
                        span[i * channels + c] = value;
                    }
                }
                encoder.Write(span[..(sampleToWrite * channels)]);
                sampleCount -= sampleToWrite;
                Console.WriteLine($"Progress: {sampleWritten / sampleRate}/{SampleCount / sampleRate}");
            }
        }
    }
}
