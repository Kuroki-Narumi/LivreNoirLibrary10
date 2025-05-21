using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using LivreNoirLibrary.Windows.NativeMethods;

namespace LivreNoirLibrary.Windows.Input
{
    public static partial class Hotkey
    {
        private static nint _window_handle;
        private static readonly Dictionary<int, KeyBindInfo> _key_binds = [];
        private static bool _stopped;
        private static readonly object _message_receiver = new();

        public static void Init(Window window)
        {
            _window_handle = window.GetHandle();
            window.Closed += (s, e) => Dispose();
            ComponentDispatcher.ThreadPreprocessMessage += _message_receiver.ExecuteMessage;
        }

        public static void Start()
        {
            if (_stopped)
            {
                _stopped = false;
                foreach (var kv in _key_binds)
                {
                    Register(kv.Value);
                }
            }
        }

        public static void Stop()
        {
            if (!_stopped)
            {
                foreach (var kv in _key_binds)
                {
                    _ = Win32Api.UnregisterHotKey(_window_handle, kv.Key);
                }
                _stopped = true;
            }
        }

        private static void ExecuteMessage(this object? obj, ref MSG msg, ref bool handled)
        {
            if (msg.message != (int)WM.Hotkey) { return; }
            if (_key_binds.TryGetValue(msg.wParam.ToInt32(), out var info))
            {
                info.Action();
                handled = true;
            }
        }

        private static void Dispose()
        {
            ComponentDispatcher.ThreadPreprocessMessage -= _message_receiver.ExecuteMessage;
            Stop();
            _key_binds.Clear();
            _window_handle = default;
        }

        public static bool Register(object obj, KeyInput key, Action action)
        {
            KeyBindInfo info = new(obj, key, action);
            var id = info.Listener.GetHashCode();
            foreach (var kv in _key_binds)
            {
                if (kv.Key != id && kv.Value.KeyBind == key)
                {
                    return false;
                }
            }
            if (!_key_binds.TryAdd(id, info))
            {
                _key_binds[id] = info;
            }

            return Register(new(obj, key, action));
        }

        public static bool IsRegistrationValid(KeyInput key)
        {
            var id = Guid.NewGuid().GetHashCode();
            var m = (int)key.Modifier;
            var vk = KeyInterop.VirtualKeyFromKey(key.Key);
            var result = Win32Api.RegisterHotKey(_window_handle, id, m, vk) != 0;
            _ = Win32Api.UnregisterHotKey(_window_handle, id);
            return result;
        }

        private static bool Register(KeyBindInfo info)
        {
            var id = info.Listener.GetHashCode();
            _ = Win32Api.UnregisterHotKey(_window_handle, id);
            var m = (int)info.KeyBind.Modifier;
            var vk = KeyInterop.VirtualKeyFromKey(info.KeyBind.Key);
            var result = Win32Api.RegisterHotKey(_window_handle, id, m, vk) != 0;
            if (_stopped)
            {
                _ = Win32Api.UnregisterHotKey(_window_handle, id);
            }
            return result;
        }

        public static bool Remove(object obj)
        {
            return Remove(obj.GetHashCode());
        }

        private static bool Remove(int id)
        {
            _key_binds.Remove(id);
            return Win32Api.UnregisterHotKey(_window_handle, id) != 0;
        }

        private record KeyBindInfo(object Listener, KeyInput KeyBind, Action Action);
    }
}
