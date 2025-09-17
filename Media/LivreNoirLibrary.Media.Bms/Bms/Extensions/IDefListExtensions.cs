using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class IDefListExtensions
    {
        extension(IDefListCollection obj)
        {
            public int MaxIndex
            {
                get
                {
                    var c = 0;
                    if (obj is DefListCollection d)
                    {
                        foreach (var (_, list) in d)
                        {
                            c = Math.Max(c, list.MaxIndex);
                        }
                    }
                    else
                    {
                        foreach (var (_, list) in obj.EnumerateList())
                        {
                            c = Math.Max(c, list.MaxIndex);
                        }
                    }
                    return c;
                }
            }

            public bool HasValue
            {
                get
                {

                    if (obj is DefListCollection d)
                    {
                        foreach (var (_, list) in d)
                        {
                            if (list.Count is > 0)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var (_, list) in obj.EnumerateList())
                        {
                            if (list.Count is > 0)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
            }

            public bool ContainsKey(DefType type, int key) => obj.TryGetList(type, out var list) && list.ContainsKey((short)key);

            public bool TryGetValue(DefType type, int key, [MaybeNullWhen(false)] out string value)
            {
                if (obj.TryGetList(type, out var list) && list.TryGetValue((short)key, out value))
                {
                    return true;
                }
                value = null;
                return false;
            }

            public bool TryGetKey(DefType type, string value, out int key)
            {
                if (obj.TryGetList(type, out var list) && list.TryGetKey(value, out var k))
                {
                    key = k;
                    return true;
                }
                key = -1;
                return false;
            }

            public bool Set(DefType type, int key, string? value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return obj.Remove(type, key);
                }
                // compatible
                switch (type)
                {
                    case DefType.ExWav:
                        value = $"{Tags.ExWav} {value}";
                        type = DefType.Wav;
                        break;
                    case DefType.Bga:
                        value = $"{Tags.Bga} {value}";
                        type = DefType.Bmp;
                        break;
                    case DefType.ExBmp:
                        value = $"{Tags.ExBmp} {value}";
                        type = DefType.Bmp;
                        break;
                    case DefType.AtBga:
                        value = $"{Tags.AtBga} {value}";
                        type = DefType.Bmp;
                        break;
                }
                var list = obj.GetOrAddList(type);
                var i = (short)key;
                if (!list.TryGetValue(i, out var current) || current != value)
                {
                    list.Set(i, value);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool Remove(DefType type, int key)
            {
                // compatible
                switch (type)
                {
                    case DefType.ExWav:
                        type = DefType.Wav;
                        break;
                    case DefType.Bga:
                    case DefType.ExBmp:
                    case DefType.AtBga:
                        type = DefType.Bmp;
                        break;
                }
                if (obj.TryGetList(type, out var list))
                {
                    if (list.Remove((short)key))
                    {
                        if (list.Count is 0)
                        {
                            obj.RemoveList(type);
                        }
                        return true;
                    }
                }
                return false;
            }

            public IEnumerable<(int Key, string Value)> EnumerateKeyValue(DefType type, int maxIndex = Constants.DefMax_Default)
            {
                if (obj.TryGetList(type, out var list))
                {
                    for (short i = 1; i < maxIndex; i++)
                    {
                        if (list.TryGetValue(i, out var value))
                        {
                            yield return (i, value);
                        }
                    }
                }
            }
        }
    }
}