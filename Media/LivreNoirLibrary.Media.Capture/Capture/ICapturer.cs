using System;

namespace LivreNoirLibrary.Media.Capture
{
    public interface ICapturer
    {
        /// <summary>
        /// キャプチャ対象が存在しなくなったことによりキャプチャが終了した場合に発火するイベント。
        /// </summary>
        public event EventHandler? CaptureTargetClosed;

        /// <summary>
        /// 画像データの変換に使用するアダプター。
        /// </summary>
        public IImageAdapter Adapter { get; set; }

        /// <summary>
        /// マウスカーソルをキャプチャに含めるかどうかを示すフラグ。
        /// </summary>
        public bool IsCursorCaptureEnabled { get; set; }

        /// <summary>
        /// キャプチャが実行中であるかどうかを示すフラグ。
        /// </summary>
        public bool IsActive { get; }

        /// <summary>
        /// キャプチャ中のウィンドウのハンドル。
        /// </summary>
        public nint CapturingWindowHandle { get; }

        /// <summary>
        /// キャプチャ中のモニターのハンドル。
        /// </summary>
        public nint CapturingMonitorHandle { get; }
    }
}
