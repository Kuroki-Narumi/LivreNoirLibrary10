using System;
using System.Windows.Input;
using System.Windows.Interop;
using LivreNoirLibrary.Win32Api;

namespace LivreNoirLibrary.Windows.Input
{
    public static partial class InputManager
    {
        static InputManager()
        {
            ComponentDispatcher.ThreadPreprocessMessage += ExecuteMessage;
        }

        // スタティックコンストラクタ呼び出し用のダミー
        public static void Initialize() { }

        private static void ExecuteMessage(ref MSG msg, ref bool handled)
        {
            if (handled)
            {
                return;
            }
            switch ((WM)msg.message)
            {
                case WM.MouseHorizontalWheel:
                    HandleMouseHorizontalWheel(msg.wParam);
                    break;
                case WM.HotKey:
                    HandleHotKeyMessage(msg.hwnd, msg.wParam, ref handled);
                    break;
            }
        }
    }
}
