using System;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using LivreNoirLibrary.Windows.NativeMethods;

namespace LivreNoirLibrary.Windows.Input
{
    public static partial class InputManager
    {
        static InputManager()
        {
            InitializeMouseHorizontalWheel();
            ComponentDispatcher.ThreadPreprocessMessage += _message_receiver.ExecuteMessage;
        }

        private static readonly MessageReceiver _message_receiver = new();

        private class MessageReceiver()
        {
            public void ExecuteMessage(ref MSG msg, ref bool handled)
            {
                switch ((WM)msg.message)
                {
                    case WM.MouseHorizontalWheel:
                        HandleMouseHorizontalWheel(msg.wParam);
                        break;
                    case WM.Hotkey:
                        Hotkey.HandleMessage(msg.wParam, ref handled);
                        break;
                }
            }
        }

        // スタティックコンストラクタ呼び出し用のダミー
        public static void Initialize() { }
    }
}
