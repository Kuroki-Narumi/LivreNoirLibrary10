using System;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Midi
{
    public class KeySwitchPresetCollection : ObservableList<KeySwitchPreset>;

    public partial class KeySwitchPreset : ObservableObjectBase, INamedObject
    {
        public string Name { get; set => SetValue(ref field, value); } = "";
        [JsonConverter(typeof(Text.Base64JsonConverter))]
        public byte[] Bytes { get; set => SetValue(ref field, value); } = new byte[128];

        string? INamedObject.Name => Name;

        public unsafe void CopyFrom(KeySwitchOption[] from)
        {
            fixed (KeySwitchOption* fromPtr = from)
            fixed (byte* toPtr = Bytes)
            {
                SimdOperations.CopyFrom(toPtr, (byte*)fromPtr, 128);
            }
        }

        public unsafe void CopyTo(KeySwitchOption[] to)
        {
            fixed (byte* fromPtr = Bytes)
            fixed (KeySwitchOption* toPtr = to)
            {
                SimdOperations.CopyFrom((byte*)toPtr, fromPtr, 128);
            }
        }
    }
}
