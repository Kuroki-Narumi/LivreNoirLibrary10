using System;

namespace LivreNoirLibrary.Media.Capture
{
    /// <summary>
    /// キャプチャしたフレームを特定の画像オブジェクトに変換・コピーするためのアダプター<br/>
    /// reference: https://github.com/radian-jp/WindowCaptureDemo/blob/main/IImageAdapter.cs
    /// </summary>
    public interface IImageAdapter
    {
        /// <summary>
        /// 画像データ転送
        /// </summary>
        /// <param name="srcPtr">転送元ポインタ</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="srcStride">転送元ストライド（画像の横1ラインあたりのバイト数）</param>
        unsafe void WritePixels(byte* srcPtr, int width, int height, int srcStride);
    }
}
