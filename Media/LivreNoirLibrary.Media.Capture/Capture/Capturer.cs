using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Win32Api;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Direct3D;
using Windows.Win32.Graphics.Direct3D11;
using Windows.Win32.Graphics.Dxgi;
using Windows.Win32.System.Com;
using WinRT;

namespace LivreNoirLibrary.Media.Capture
{
    /// <summary>
    /// Windows Graphics Capture を使用したウィンドウキャプチャクラス。<br/>
    /// reference: https://github.com/radian-jp/WindowCaptureDemo/blob/main/WindowCapturer.cs
    /// </summary>
    /// <typeparam name="T">生成される画像オブジェクトの型。</typeparam>
    /// <param name="adapter">画像データの変換に使用するアダプター。</param>
    public class Capturer<T> : DisposableBase
    {
        private readonly Lock _lock = new();
        private readonly IImageAdapter<T> _adapter;

        // Direct3D11のオブジェクト
        private unsafe ID3D11Device* _d3dDevice;
        private unsafe ID3D11DeviceContext* _d3dContext;
        private unsafe ID3D11Texture2D* _textureDst;

        // WinRT(Windows.Graphics.Capture)のオブジェクト
        private IDirect3DDevice? _winrtD3DDevice;
        private GraphicsCaptureItem? _item;
        private Direct3D11CaptureFramePool? _framePool;
        private GraphicsCaptureSession? _session;
        private Direct3D11CaptureFrame? _latestFrame;

        public bool IsActive => _session is not null;
        public bool IsCursorCaptureEnabled
        {
            get;
            set
            {
                field = value;
                _session?.IsCursorCaptureEnabled = value;
            }
        }

        public Capturer(IImageAdapter<T> adapter)
        {
            _adapter = adapter;
            CreateD3DDevice();
        }

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();
            lock (_lock)
            {
                _item = null;
                DisposeHelper.NullDispose(ref _winrtD3DDevice);
                DisposeSession();
                unsafe
                {
                    DisposeHelper.NullRelease(ref _textureDst);
                    DisposeHelper.NullRelease(ref _d3dContext);
                    DisposeHelper.NullRelease(ref _d3dDevice);
                }
            }
        }

        public void StartWindowCapture(nint handle)
        {
            Stop();
            if (NativeMethods.IsWindow(handle))
            {
                _item = GraphicsCaptureItem.TryCreateFromWindowId(new Windows.UI.WindowId((ulong)handle));
                StartImpl(_item);
            }
        }

        public void StartMonitorCapture(nint handle)
        {
            Stop();
            try
            {
                _item = GraphicsCaptureItem.TryCreateFromDisplayId(new Windows.Graphics.DisplayId((ulong)handle));
                StartImpl(_item);
            }
            catch
            {
                Stop();
            }
        }

        public void Stop()
        {
            DisposeSession();
        }

        private void StartImpl(GraphicsCaptureItem item)
        {
            _framePool = Direct3D11CaptureFramePool.CreateFreeThreaded(
                _winrtD3DDevice,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                2,
                item.Size);
            _framePool.FrameArrived += OnFrameArrived;

            _session = _framePool.CreateCaptureSession(_item);
            _session.IsBorderRequired = false;
            _session.IsCursorCaptureEnabled = IsCursorCaptureEnabled;
            _session.StartCapture();
        }

        private void DisposeSession()
        {
            DisposeHelper.NullDispose(ref _latestFrame);
            DisposeHelper.NullDispose(ref _session);
            DisposeHelper.NullDispose(ref _framePool);
        }

        /// <inheritdoc cref="GetFrame(T?, out int, out int)"/>
        public T? GetFrame(T? image) => GetFrame(image, out _, out _);

        /// <summary>
        /// 最新のキャプチャフレームを取得し、指定された画像オブジェクトにコピーします。
        /// </summary>
        /// <param name="image">再利用する画像オブジェクト。null の場合は新しく作成されます。</param>
        /// <param name="width">キャプチャされた画像の幅。</param>
        /// <param name="height">キャプチャされた画像の高さ。</param>
        /// <returns>画像データが書き込まれた画像オブジェクト。※フレームがまだ取得できていない場合、書き込まれない事もあります。</returns>
        public T? GetFrame(T? image, out int width, out int height)
        {
            lock (_lock)
            {
                if (IsDisposed || _latestFrame is null)
                {
                    width = height = 0;
                    return image;
                }

                unsafe
                {
                    // WinRTのIDirect3DSurfaceから
                    // IDirect3DDxgiInterfaceAccess経由で
                    // ID3D11Texture2Dを取り出す
                    var surfacePtr = MarshalInterface<IDirect3DSurface>.FromManaged(_latestFrame.Surface);
                    ((IUnknown*)surfacePtr)->QueryInterface<Windows.Win32.System.WinRT.Direct3D11.IDirect3DDxgiInterfaceAccess>(out var access);
                    var iid = ID3D11Texture2D.IID_Guid;
                    void* pTextureSrc;
                    access->GetInterface(&iid, &pTextureSrc);
                    access->Release();
                    var textureSrc = (ID3D11Texture2D*)pTextureSrc;
                    textureSrc->GetDesc(out var desc);

                    // 転送先画像の準備
                    image = _adapter.Prepare(image, (int)desc.Width, (int)desc.Height);

                    // 同サイズのコピー先テクスチャが既にある場合は再利用される
                    CreateBufferTexture(desc);

                    _d3dContext->CopyResource(
                        (ID3D11Resource*)_textureDst,
                        (ID3D11Resource*)textureSrc);

                    textureSrc->Release();

                    _d3dContext->Map(
                        (ID3D11Resource*)_textureDst,
                        0,
                        D3D11_MAP.D3D11_MAP_READ,
                        0,
                        out var mapped);

                    width = (int)desc.Width;
                    height = (int)desc.Height;
                    // 画像データ転送
                    _adapter.Copy(image, (byte*)mapped.pData, width, height, (int)mapped.RowPitch);

                    _d3dContext->Unmap((ID3D11Resource*)_textureDst, 0);
                    return image;
                }
            }
        }

        /// <summary>
        /// Direct3Dデバイスを作成し、WinRT 互換のデバイスにラップします。
        /// </summary>
        private unsafe void CreateD3DDevice()
        {
            D3D_FEATURE_LEVEL level;
            ID3D11Device* device;
            ID3D11DeviceContext* context;

            // ネイティブDirect3D11デバイス作成
            HRESULT hr = PInvoke.D3D11CreateDevice(
                null,
                D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE,
                HMODULE.Null,
                D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT,
                null,
                0,
                PInvoke.D3D11_SDK_VERSION,
                &device,
                &level,
                &context);

            if (hr.Failed)
                Marshal.ThrowExceptionForHR(hr);

            _d3dDevice = device;
            _d3dContext = context;

            try
            {
                // WinRT用D3Dデバイスを取得（ID3D11Deviceと互換性が無いので別々に保持する）
                hr = _d3dDevice->QueryInterface<IDXGIDevice>(out var dxgiDevice);
                hr.ThrowOnFailure();
                Windows.Win32.System.WinRT.IInspectable* pWinRTD3DDevice;
                hr = PInvoke.CreateDirect3D11DeviceFromDXGIDevice(
                    dxgiDevice,
                    &pWinRTD3DDevice);

                dxgiDevice->Release();
                hr.ThrowOnFailure();

                _winrtD3DDevice =
                    MarshalInterface<IDirect3DDevice>.FromAbi(
                        (IntPtr)pWinRTD3DDevice);

                pWinRTD3DDevice->Release();
            }
            catch
            {
                DisposeHelper.NullRelease(ref _d3dDevice);
                DisposeHelper.NullRelease(ref _d3dContext);
                throw;
            }
        }

        /// <summary>
        /// 新しいフレームが到着したときに呼び出されます。
        /// </summary>
        private void OnFrameArrived(Direct3D11CaptureFramePool sender, object args)
        {
            lock (_lock)
            {
                if (IsDisposed || _framePool is null)
                    return;

                var frame = sender.TryGetNextFrame();
                if (frame is null)
                    return;

                // サイズ変更があった場合、
                // フレームプールを新しいサイズで再作成
                if (_latestFrame is not null && frame.ContentSize != _latestFrame.ContentSize)
                {
                    var size = frame.ContentSize;

                    frame.Dispose();
                    _latestFrame?.Dispose();
                    _latestFrame = null;

                    _framePool.Recreate(
                        _winrtD3DDevice,
                        DirectXPixelFormat.B8G8R8A8UIntNormalized,
                        2,
                        size);

                    return;
                }

                // 新しいフレームと入れ替え
                _latestFrame?.Dispose();
                _latestFrame = frame;
            }
        }

        /// <summary>
        /// CPUから読み取り可能なコピー先グテクスチャを作成または再作成します。
        /// </summary>
        /// <param name="src">コピー元テクスチャから取得したD3D11_TEXTURE2D_DESC構造体</param>
        private unsafe void CreateBufferTexture(D3D11_TEXTURE2D_DESC src)
        {
            if (_textureDst is not null)
            {
                // テクスチャのサイズが変わってないならそのまま使う
                _textureDst->GetDesc(out var current);
                if (current.Width == src.Width && current.Height == src.Height)
                    return;

                // サイズが変わっている場合、以前のテクスチャは解放
                _textureDst->Release();
                _textureDst = null;
            }

            // 新しいサイズでテクスチャを作成
            D3D11_TEXTURE2D_DESC desc = src;
            desc.BindFlags = 0;
            desc.CPUAccessFlags =
                D3D11_CPU_ACCESS_FLAG.D3D11_CPU_ACCESS_READ;
            desc.Usage = D3D11_USAGE.D3D11_USAGE_STAGING;
            desc.MiscFlags = 0;

            ID3D11Texture2D* texture;
            _d3dDevice->CreateTexture2D(
                &desc,
                null,
                &texture);

            _textureDst = texture;
        }
    }
}
