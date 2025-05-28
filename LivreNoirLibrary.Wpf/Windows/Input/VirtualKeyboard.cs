using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Input
{
    public delegate void VirtualKeyboardEventHandler(Key key, ModifierKeys modifier);

    public static unsafe partial class VirtualKeyboard
    {
        private static event VirtualKeyboardEventHandler? PressedInternal;
        private static event VirtualKeyboardEventHandler? ReleasedInternal;

        public static event VirtualKeyboardEventHandler? Pressed
        {
            add
            {
                PressedInternal += value;
                Start();
            }
            remove
            {
                PressedInternal -= value;
                StopIfEmpty();
            }
        }

        public static event VirtualKeyboardEventHandler? Released
        {
            add
            {
                ReleasedInternal += value;
                Start();
            }
            remove
            {
                ReleasedInternal -= value;
                StopIfEmpty();
            }
        }

        private static readonly sbyte[] _buffer = new sbyte[256];
        private static bool _active;

        public static void Start()
        {
            if (!_active)
            {
                _active = true;
                CompositionTarget.Rendering += OnUpdate;
            }
        }

        private static void StopIfEmpty()
        {
            if (PressedInternal is null && ReleasedInternal is null)
            {
                Stop();
            }
        }

        public static void Stop()
        {
            if (_active)
            {
                _active = false;
                CompositionTarget.Rendering -= OnUpdate;
            }
        }

        private static void OnUpdate(object? sender, EventArgs e)
        {
            fixed (sbyte* ptr = _buffer)
            fixed (KeyMap* keys = _available_keys)
            fixed (byte* states = _press_states)
            {
                var max = _available_keys.Length;
                if (NativeMethods.Win32Api.GetKeyboardState((nint)ptr) is not 0)
                {
                    var modifier = Keyboard.Modifiers;
                    for (var i = 0; i < max; i++)
                    {
                        var (vk, key) = _available_keys[i];
                        if (ptr[vk] is < 0)
                        {
                            if (states[i] is 0)
                            {
                                PressedInternal?.Invoke(key, modifier);
                            }
                            states[i] = 1;
                        }
                        else if (states[i] is not 0)
                        {
                            ReleasedInternal?.Invoke(key, modifier);
                            states[i] = 0;
                        }
                    }
                }
            }
        }

        private static readonly KeyMap[] _available_keys = CreateKeyMaps();
        private static readonly byte[] _press_states = new byte[_available_keys.Length];

        private static KeyMap[] CreateKeyMaps()
        {
            List<KeyMap> list = [];
            void AddRange(int start, int end)
            {
                for (var i = start; i <= end; i++)
                {
                    list.Add(i);
                }
            }
            list.AddRange(0x08, 0x09, 0x0C, 0x0D, 0x13, 0x1B);
            AddRange(0x20, 0x39);
            AddRange(0x41, 0x5A);
            list.Add(0x5D);
            AddRange(0x5F, 0x87);
            AddRange(0x90, 0x96);
            AddRange(0xA6, 0xB7);
            AddRange(0xBA, 0xC0);
            AddRange(0xDB, 0xDF);
            AddRange(0xE1, 0xE6);
            AddRange(0xE9, 0xFB);
            list.AddRange(0xFD, 0xFE);
            return [.. list];
        }

        private readonly struct KeyMap(byte vk, Key key)
        {
            public readonly byte VK = vk;
            public readonly Key Key = key;
            public static implicit operator KeyMap(int vk) => new((byte)vk, KeyInterop.KeyFromVirtualKey(vk));
            public void Deconstruct(out byte vk, out Key key)
            {
                vk = VK;
                key = Key;
            }
        }
    }
}
