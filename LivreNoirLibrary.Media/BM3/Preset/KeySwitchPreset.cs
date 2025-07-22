using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.BM3
{
    public class KeySwitchPresetCollection : ObservableList<KeySwitchPreset>;

    public partial class KeySwitchPreset : ObservableObjectBase, INamedObject
    {
        [ObservableProperty]
        private string _name = "";
        [JsonConverter(typeof(Text.Base64JsonConverter))]
        [ObservableProperty]
        private byte[] _bytes = new byte[128];

        string? INamedObject.Name => _name;

        public unsafe void CopyFrom(KeySwitchOption[] from)
        {
            fixed (KeySwitchOption* fromPtr = from)
            fixed (byte* toPtr = _bytes)
            {
                SimdOperations.CopyFrom(toPtr, (byte*)fromPtr, 128);
            }
        }

        public unsafe void CopyTo(KeySwitchOption[] to)
        {
            fixed (byte* fromPtr = _bytes)
            fixed (KeySwitchOption* toPtr = to)
            {
                SimdOperations.CopyFrom((byte*)toPtr, fromPtr, 128);
            }
        }
    }
}
