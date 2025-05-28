using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using LivreNoirLibrary.Windows.NativeMethods;

namespace LivreNoirLibrary.Windows.Input
{
    public static partial class Hotkey
    {
        private static readonly Dictionary<int, KeyBindInfo> _key_binds = [];
        private static bool _stopped;

        public static void Start()
        {
            if (_stopped)
            {
                _stopped = false;
                foreach (var (_, info) in _key_binds)
                {
                    TryRegister(info.Id, info.Key, info.Modifier, info.NoRepeat, false);
                }
            }
        }

        public static void Stop()
        {
            if (!_stopped)
            {
                foreach (var (id, _) in _key_binds)
                {
                    _ = Win32Api.UnregisterHotKey(0, id);
                }
                _stopped = true;
            }
        }

        internal static void HandleMessage(nint wParam, ref bool handled)
        {
            if (_key_binds.TryGetValue(unchecked((int)wParam), out var info))
            {
                info.Handler();
                handled = true;
            }
        }

        public static bool Register(int id, Action action, Key key, ModifierKeys modifier = 0, bool noRepeat = false)
        {
            if (id is < 0 or >= 0xC000)
            {
                throw new ArgumentOutOfRangeException($"id must be >= 0 and < 0xC000 (given: {id:X4}");
            }
            InputManager.Initialize();
            if (IsFreeCore(key, modifier))
            {
                _key_binds[id] = new(id, key, modifier, noRepeat, action);
                return TryRegister(id, key, modifier, noRepeat, _stopped);
            }
            return false;
        }

        public static bool IsFree(Key key, ModifierKeys modifier = 0) => IsFreeCore(key, modifier) && TryRegister(0, key, modifier, false, true);

        private static bool IsFreeCore(Key key, ModifierKeys modifier)
        {
            foreach (var (_, info) in _key_binds)
            {
                if (info.Key == key && info.Modifier == modifier)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool TryRegister(int id, Key key, ModifierKeys modifier, bool noRepeat, bool unregister)
        {
            var m = (int)modifier;
            if (noRepeat)
            {
                m |= 0x4000;
            }
            var vk = KeyInterop.VirtualKeyFromKey(key);
            var result = Win32Api.RegisterHotKey(0, id, m, vk) != 0;
            if (unregister)
            {
                _ = Win32Api.UnregisterHotKey(0, id);
            }
            return result;
        }

        public static bool Remove(int id)
        {
            _key_binds.Remove(id);
            return Win32Api.UnregisterHotKey(0, id) is not 0;
        }

        private record KeyBindInfo(int Id, Key Key, ModifierKeys Modifier, bool NoRepeat, Action Handler);
    }
}
