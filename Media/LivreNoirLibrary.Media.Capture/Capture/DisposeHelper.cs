using System;
using System.Collections.Generic;
using System.Text;
using Windows.Win32.System.Com;

namespace LivreNoirLibrary.Media.Capture
{
    public static class DisposeHelper
    {
        /// <summary>
        /// IDisposableオブジェクトを解放し、null を代入します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="o">解放するオブジェクト</param>
        public static void NullDispose<T>(ref T? o) where T : class, IDisposable
        {
            o?.Dispose();
            o = null;
        }

        /// <summary>
        /// 構造体ベースCOMオブジェクトを解放し、ポインタに null を代入します。
        /// </summary>
        /// <typeparam name="T">COMインターフェイスの型。</typeparam>
        /// <param name="pCom">解放するCOMインターフェースポインタ</param>
        public static unsafe void NullRelease<T>(ref T* pCom) 
            where T : unmanaged
        {
            if (pCom != null)
            {
                ((IUnknown*)pCom)->Release();
                pCom = null;
            }
        }
    }
}
