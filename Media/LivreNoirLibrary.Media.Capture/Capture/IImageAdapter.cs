using System;

namespace LivreNoirLibrary.Media.Capture
{
    /// <summary>
    /// キャプチャしたフレームを特定の画像オブジェクトに変換・コピーするためのアダプター<br/>
    /// reference: https://github.com/radian-jp/WindowCaptureDemo/blob/main/IImageAdapter.cs
    /// </summary>
    /// <typeparam name="T">画像オブジェクトの型</typeparam>
    public interface IImageAdapter<T>
    {
        /// <summary>
        /// 転送先画像オブジェクトの準備
        /// </summary>
        /// <param name="previousDstImage">前回転送先に使用した画像オブジェクト(nullの場合は新規作成される)</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns>転送先画像オブジェクト</returns>
        T Prepare(T? previousDstImage, int width, int height);

        /// <summary>
        /// 画像データ転送
        /// </summary>
        /// <param name="dstImage">転送先画像オブジェクト</param>
        /// <param name="srcPtr">転送元ポインタ</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="srcStride">転送元ストライド（画像の横1ラインあたりのバイト数）</param>
        unsafe void Copy(T dstImage, byte* srcPtr, int width, int height, int srcStride);
    }
}
