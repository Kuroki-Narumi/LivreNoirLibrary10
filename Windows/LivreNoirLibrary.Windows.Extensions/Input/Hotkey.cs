using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using LivreNoirLibrary.Win32Api;

namespace LivreNoirLibrary.Windows.Input
{
    public static partial class InputManager
    {
        private static readonly Dictionary<nint, HotKeyHandler> _hotKeyInfos = [];
        private static nint _hotKeyHwnd;
        private static bool _stopped;

        /// <summary>
        /// Attempts to register a hotkey for the specified window using the Win32Api and returns the hotkey ID.
        /// </summary>
        /// <remarks>
        /// Only the main window can register hotkeys. If the specified window is not the main window, an <see cref="ArgumentException"/> will be thrown.
        /// </remarks>
        /// <param name="window">The window for which to register the hotkey.</param>
        /// <param name="key">The key to register.</param>
        /// <param name="modifier">The modifier keys.</param>
        /// <param name="action">The action to execute when the hotkey is pressed.</param>
        /// <param name="setHandled">Indicates whether the hotkey message should be marked as handled.</param>
        /// <param name="noRepeat">Indicates whether the hotkey should not repeat when held down.</param>
        /// <returns>The hotkey ID if successful, otherwise -1.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static int RegisterHotKey(this Window window, Key key, ModifierKeys modifier, Action action, bool setHandled = true, bool noRepeat = false)
        {
            if (Application.Current.MainWindow != window)
            {
                throw new ArgumentException("Only the main window can register hotkeys.");
            }
            if (_hotKeyHwnd == 0)
            {
                _hotKeyHwnd = window.GetHandle();
                window.Closed += OnMainWindowClosed;
            }
            var id = GetHotKeyIdFromKey(key, modifier, out var mod, out var vk);
            if (noRepeat)
            {
                mod |= 0x4000;
            }
            if (NativeMethods.RegisterHotKey(_hotKeyHwnd, id, mod, vk))
            {
                HotKeyHandler info = new(action, id, mod, vk, setHandled);
                _hotKeyInfos[id] = info;
                return id;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Unregisters a hotkey with the specified key and modifier for the main window.
        /// </summary>
        /// <param name="key">The key to unregister.</param>
        /// <param name="modifier">The modifier keys.</param>
        /// <returns><see langword="true"/> if the hotkey was successfully unregistered; otherwise, <see langword="false"/>.</returns>
        public static bool UnregisterHotKey(Key key, ModifierKeys modifier)
        {
            var id = GetHotKeyIdFromKey(key, modifier, out _, out _);
            return UnregisterHotKey(id);
        }

        /// <summary>
        /// Unregisters a hotkey with the specified ID for the main window.
        /// </summary>
        /// <param name="id">The hotkey ID to unregister.</param>
        /// <returns><see langword="true"/> if the hotkey was successfully unregistered; otherwise, <see langword="false"/>.</returns>
        public static bool UnregisterHotKey(int id)
        {
            if (_hotKeyInfos.Remove(id) && _hotKeyHwnd is not 0)
            {
                return NativeMethods.UnregisterHotKey(_hotKeyHwnd, id);
            }
            return false;
        }

        /// <summary>
        /// Unregisters all hotkeys for the main window.
        /// </summary>
        public static void UnregisterAllHotKeys()
        {
            UnregisterAllHotKeyImpl();
            _hotKeyInfos.Clear();
        }

        public static int GetHotKeyIdFromKey(Key key, ModifierKeys modifier, out int mod, out int vk)
        {
            mod = (int)modifier;
            vk = KeyInterop.VirtualKeyFromKey(key);
            return (mod << 8) | vk;
        }

        public static void StopHotKey()
        {
            if (_hotKeyHwnd is not 0 && !_stopped)
            {
                _stopped = true;
                UnregisterAllHotKeyImpl();
            }
        }

        public static void RestartHotKey()
        {
            var hwnd = _hotKeyHwnd;
            if (hwnd is not 0 && _stopped)
            {
                _stopped = false;
                foreach (var (_, info) in _hotKeyInfos)
                {
                    _ = NativeMethods.RegisterHotKey(hwnd, info.Id, info.ModKey, info.Key);
                }
            }
        }

        private static void UnregisterAllHotKeyImpl()
        {
            var hwnd = _hotKeyHwnd;
            foreach (var (_, info) in _hotKeyInfos)
            {
                _ = NativeMethods.UnregisterHotKey(hwnd, info.Id);
            }
        }

        private static void HandleHotKeyMessage(nint hwnd, nint wParam, ref bool handled)
        {
            if (hwnd == _hotKeyHwnd && _hotKeyInfos.TryGetValue(wParam, out var info))
            {
                info.Handler();
                if (info.SetHandled)
                {
                    handled = true;
                }
            }
        }

        private static void OnMainWindowClosed(object? sender, EventArgs e)
        {
            UnregisterAllHotKeys();
            if (sender is Window window)
            {
                window.Closed -= OnMainWindowClosed;
            }
        }

        private record struct HotKeyHandler(Action Handler, int Id, int ModKey, int Key, bool SetHandled);
    }
}
