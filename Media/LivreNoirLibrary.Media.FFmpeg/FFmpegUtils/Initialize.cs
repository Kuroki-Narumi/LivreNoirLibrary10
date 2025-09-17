using System;
using System.IO;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public static partial class FFmpegUtils
    {
        static FFmpegUtils()
        {
            Initialize();
        }

        private static bool _initialized;

        public static void Initialize()
        {
            if (!_initialized)
            {
                var rootPath = Path.Combine(IO.General.GetAssemblyDir(), "DLL");
                ffmpeg.RootPath = rootPath;
                _initialized = true;
            }
        }

        public static void WarmupCodec()
        {
            Task.Run(() =>
            {
                Console.WriteLine("Warmup FFmpeg");
                var t0 = System.Diagnostics.Stopwatch.GetTimestamp();
                var buffer = (stackalloc float[64]);
                using MemoryStream ms = new();
                using (AudioEncoder enc = new(ms, OutputFormat.ByFilename("hoge.ogg"), new(44100, 2, 240000)))
                {
                    enc.Write(buffer);
                }
                ms.Position = 0;
                using (AudioDecoder dec = new(ms, true))
                {
                    dec.Read(buffer);
                }
                Console.WriteLine($"Warmup FFmpeg finished in {System.Diagnostics.Stopwatch.GetElapsedTime(t0).TotalMilliseconds:F3}ms");
            });
        }
    }
}
