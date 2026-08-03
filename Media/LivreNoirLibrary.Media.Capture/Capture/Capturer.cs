using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Win32Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class Capturer : DisposableBase, ICapturer
    {
        public event EventHandler? CaptureTargetClosed;

        private readonly Lock _lock = new();

        private SynchronizationContext? _syncContext;
        private readonly UnmanagedArray<byte> _pendingBuffer;
        private int _pendingWidth, _pendingHeight, _pendingStride;
        private bool _hasPendingFrame;

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

        public IImageAdapter Adapter 
        {
            get;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                lock (_lock)
                {
                    field = value;
                }
            }
        }

        public bool IsCursorCaptureEnabled
        {
            get;
            set
            {
                lock (_lock)
                {
                    field = value;
                    _session?.IsCursorCaptureEnabled = value;
                }
            }
        }

        public bool IsActive => _session is not null;

        public nint CapturingWindowHandle { get; private set; }
        public nint CapturingMonitorHandle { get; private set; }

        /// <summary>
        /// 指定された<see cref="IImageAdapter"/>を通じてキャプチャを行う<see cref="Capturer"/>インスタンスを作成します。
        /// </summary>
        /// <param name="adapter">画像データの変換に使用するアダプター。</param>
        public Capturer(IImageAdapter adapter)
        {
            ArgumentNullException.ThrowIfNull(adapter);
            Adapter = adapter;
            _pendingBuffer = new();
            CreateD3DDevice();
        }

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();
            lock (_lock)
            {
                DisposeSession();
                DisposeHelper.NullDispose(ref _winrtD3DDevice);
                unsafe
                {
                    DisposeHelper.NullRelease(ref _textureDst);
                    DisposeHelper.NullRelease(ref _d3dContext);
                    DisposeHelper.NullRelease(ref _d3dDevice);
                }
                _pendingBuffer.Free();
            }
        }

        /// <summary>
        /// 実行中のキャプチャセッションを終了します。キャプチャ中でない場合は何も起こりません。
        /// </summary>
        public void Stop()
        {
            lock (_lock)
            {
                DisposeSession();
            }
        }

        private void DisposeSession()
        {
            _framePool?.FrameArrived -= OnFrameArrived;
            _item?.Closed -= OnCaptureItemClosed;
            DisposeHelper.NullDispose(ref _latestFrame);
            DisposeHelper.NullDispose(ref _session);
            DisposeHelper.NullDispose(ref _framePool);
            _item = null;
            _hasPendingFrame = false;
            CapturingWindowHandle = 0;
            CapturingMonitorHandle = 0;
        }

        /// <summary>
        /// ウィンドウハンドルを指定してキャプチャセッションを開始します。
        /// </summary>
        /// <param name="handle">キャプチャ対象ウィンドウのハンドル。</param>
        public void StartWindowCapture(nint handle)
        {
            lock (_lock)
            {
                DisposeSession();
                if (NativeMethods.IsWindow(handle))
                {
                    var item = GraphicsCaptureItem.TryCreateFromWindowId(new Windows.UI.WindowId((ulong)handle));
                    StartImpl(item);
                    CapturingWindowHandle = handle;
                }
            }
        }

        /// <summary>
        /// モニターハンドルを指定してキャプチャセッションを開始します。
        /// </summary>
        /// <param name="handle">キャプチャ対象モニターのハンドル。</param>
        public void StartMonitorCapture(nint handle)
        {
            lock (_lock)
            {
                DisposeSession();
                try
                {
                    var item = GraphicsCaptureItem.TryCreateFromDisplayId(new Windows.Graphics.DisplayId((ulong)handle));
                    StartImpl(item);
                    CapturingMonitorHandle = handle;
                }
                catch
                {
                    DisposeSession();
                }
            }
        }

        private void StartImpl(GraphicsCaptureItem item)
        {
            _syncContext = SynchronizationContext.Current;
            item.Closed += OnCaptureItemClosed;
            _item = item;

            _framePool = Direct3D11CaptureFramePool.CreateFreeThreaded(
                _winrtD3DDevice,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                2,
                item.Size);
            _framePool.FrameArrived += OnFrameArrived;

            _session = _framePool.CreateCaptureSession(item);
            _session.IsBorderRequired = false;
            _session.IsCursorCaptureEnabled = IsCursorCaptureEnabled;
            _session.StartCapture();
        }

        private void OnCaptureItemClosed(GraphicsCaptureItem sender, object args)
        {
            Console.WriteLine("CaptureTargetClosed");
            lock (_lock)
            {
                if (!ReferenceEquals(sender, _item))
                {
                    return;
                }
                DisposeSession();
            }
            CaptureTargetClosed?.Invoke(this, EventArgs.Empty);
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

        /// <summary>
        /// 新しいフレームが到着したときに呼び出されます。
        /// </summary>
        private void OnFrameArrived(Direct3D11CaptureFramePool sender, object args)
        {
            var ctx = _syncContext;
            lock (_lock)
            {
                var t0 = Stopwatch.GetTimestamp();
                if (IsDisposed || _framePool is null)
                {
                    return;
                }
                var frame = sender.TryGetNextFrame();
                if (frame is null)
                {
                    return;
                }

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

                    // 同サイズのコピー先テクスチャが既にある場合は再利用される
                    CreateBufferTexture(desc);

                    _d3dContext->CopyResource((ID3D11Resource*)_textureDst, (ID3D11Resource*)textureSrc);

                    textureSrc->Release();

                    _d3dContext->Map(
                        (ID3D11Resource*)_textureDst,
                        0,
                        D3D11_MAP.D3D11_MAP_READ,
                        0,
                        out var mapped);

                    var srcPtr = (byte*)(mapped.pData);
                    var width = (int)desc.Width;
                    var height = (int)desc.Height;
                    var pitch = (int)mapped.RowPitch;

                    // 同期コンテキストが存在しない場合は直接IImageAdapter.WritePixelsを呼ぶ
                    if (ctx is null)
                    {
                        Adapter?.WritePixels(srcPtr, width, height, pitch);
                    }
                    // 同期コンテキストが存在する場合は一度内部バッファに書き込む
                    else
                    {
                        var buffer = _pendingBuffer;
                        var bufferSize = (nuint)(height * pitch);
                        buffer.EnsureSize(bufferSize, false);
                        SimdOperations.CopyFrom(buffer.Pointer, srcPtr, bufferSize);
                        _pendingWidth = width;
                        _pendingHeight = height;
                        _pendingStride = pitch;
                        _hasPendingFrame = true;
                    }

                    _d3dContext->Unmap((ID3D11Resource*)_textureDst, 0);
                }
                // Console.WriteLine($"Capturer: frame processed in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");
            }
            // 同期コンテキストでフレーム更新
            ctx?.Post(ProcessPendingFrame, null);
        }

        private void ProcessPendingFrame(object? state)
        {
            UnmanagedArray<byte> buffer;
            int width, height, stride;
            lock (_lock)
            {
                if (IsDisposed || !_hasPendingFrame)
                {
                    return;
                }
                buffer = _pendingBuffer;
                width = _pendingWidth;
                height = _pendingHeight;
                stride = _pendingStride;
            }
            if (width > 0 && height > 0)
            {
                unsafe
                {
                    Adapter?.WritePixels(buffer.Pointer, width, height, stride);
                }
            }
        }
    }
}
