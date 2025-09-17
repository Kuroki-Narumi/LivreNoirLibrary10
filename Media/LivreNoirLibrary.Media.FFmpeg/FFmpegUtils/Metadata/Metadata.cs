using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public static unsafe partial class FFmpegUtils
    {
        public static void CopyMetaTags<Tin, Tout>(this Tin source, Tout target, AVDictMode mode = AVDictMode.None)
            where Tin : IMetaTag
            where Tout : IMetaTag
        {
            var src = source.GetDictPointer();
            var dst = target.GetDictPointer();
            if (dst is not null && src is not null && *src is not null)
            {
                ffmpeg.av_dict_copy(dst, *src, (int)mode);
            }
        }

        internal static MetaTag[] GetMetaTags(AVDictionary** ptr)
        {
            if (ptr is null || *ptr is null)
            {
                return [];
            }
            var count = ffmpeg.av_dict_count(*ptr);
            var i = 0;
            var result = new MetaTag[count];
            AVDictionaryEntry* tag = null;
            while ((tag = ffmpeg.av_dict_iterate(*ptr, tag)) is not null)
            {
                var key = GetString(tag->key) ?? "";
                var value = GetString(tag->value);
                result[i++] = new(key, value);
            }
            return result;
        }

        internal static int GetMetaTags(AVDictionary** ptr, List<MetaTag> list)
        {
            if (ptr is null || *ptr is null)
            {
                return 0;
            }
            var count = ffmpeg.av_dict_count(*ptr);
            var i = 0;
            AVDictionaryEntry* tag = null;
            while ((tag = ffmpeg.av_dict_iterate(*ptr, tag)) is not null)
            {
                var key = GetString(tag->key) ?? "";
                var value = GetString(tag->value);
                list.Add(new(key, value));
            }
            return i;
        }

        public static MetaTag[] GetMetaTags<T>(this T obj) where T : IMetaTag => GetMetaTags(obj.GetDictPointer());
        public static void GetMetaTags<T>(this T obj, List<MetaTag> list) where T : IMetaTag => GetMetaTags(obj.GetDictPointer(), list);
        public static MetaTagEnumerable<T> EnumMetaTags<T>(this T obj) where T : IMetaTag => new(obj);

        public static string? GetMetaTag<T>(this T obj, string key)
            where T : IMetaTag
        {
            var ptr = obj.GetDictPointer();
            if (ptr is not null)
            {
                var tag = ffmpeg.av_dict_get(*ptr, key, null, 0);
                if (tag is not null)
                {
                    return GetString(tag->value) ?? "";
                }
            }
            return null;
        }

        public static bool TryGetMetaTag<T>(this T obj, string key, [MaybeNullWhen(false)]out string value)
            where T : IMetaTag
        {
            var ptr = obj.GetDictPointer();
            if (ptr is not null)
            {
                var tag = ffmpeg.av_dict_get(*ptr, key, null, 0);
                if (tag is not null)
                {
                    value = GetString(tag->value) ?? "";
                    return true;
                }
            }
            value = null;
            return false;
        }

        public static void ClearMetaTags<T>(this T obj)
            where T : IMetaTag
        {
            var ptr = obj.GetDictPointer();
            if (ptr is not null)
            {
                ffmpeg.av_dict_free(ptr);
            }
        }

        public static void RemoveMetaTag<T>(this T obj, string key)
            where T : IMetaTag
        {
            var ptr = obj.GetDictPointer();
            if (ptr is not null)
            {
                ffmpeg.av_dict_set(ptr, key, null, 0).CheckError(ThrowInvalidOperationException);
            }
        }

        public static void SetMetaTag<T>(this T obj, MetaTag tag, AVDictMode mode = AVDictMode.None)
            where T : IMetaTag
        {
            var ptr = obj.GetDictPointer();
            if (ptr is not null)
            {
                ffmpeg.av_dict_set(ptr, tag.Key, tag.Value, (int)mode).CheckError(ThrowInvalidOperationException);
            }
        }

        public static void SetMetaTag<T>(this T obj, string key, string? value, AVDictMode mode = AVDictMode.None)
            where T : IMetaTag
        {
            var ptr = obj.GetDictPointer();
            if (ptr is not null)
            {
                ffmpeg.av_dict_set(ptr, key, value, (int)mode).CheckError(ThrowInvalidOperationException);
            }
        }

        private static readonly Lock _meta_cache_lock = new(); 
        private static readonly List<MetaTag> _meta_cache = [];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckSetMetaTagError()
        {
            if (_meta_cache.Count is > 0)
            {
                ThrowInvalidOperationException($"Failed to set tags:\n{string.Join('\n', _meta_cache)}");
            }
        }

        public static void SetMetaTags<T>(this T obj, ReadOnlySpan<MetaTag> tags, AVDictMode mode = AVDictMode.None)
            where T : IMetaTag
        {
            var flag = (int)mode;
            var ptr = obj.GetDictPointer();
            if (ptr is null)
            {
                return;
            }
            lock (_meta_cache_lock)
            {
                var list = _meta_cache;
                list.Clear();
                foreach (var (key, value) in tags)
                {
                    ffmpeg.av_dict_set(ptr, key, value, flag).CheckError(m => list.Add(new(key, value)));
                }
                CheckSetMetaTagError();
            }
        }

        public static void SetMetaTags<T, TEnum>(this T obj, TEnum tags, AVDictMode mode = AVDictMode.None)
            where T : IMetaTag
            where TEnum : IEnumerable<MetaTag>
        {
            var flag = (int)mode;
            var ptr = obj.GetDictPointer();
            if (ptr is null)
            {
                return;
            }
            lock (_meta_cache_lock)
            {
                var list = _meta_cache;
                list.Clear();
                foreach (var (key, value) in tags)
                {
                    ffmpeg.av_dict_set(ptr, key, value, flag).CheckError(m => list.Add(new(key, value)));
                }
                CheckSetMetaTagError();
            }
        }

        public static void SetMetaTags<T>(this T obj, IReadOnlyDictionary<string, string?> tags, AVDictMode mode = AVDictMode.None)
            where T : IMetaTag
        {
            var flag = (int)mode;
            var ptr = obj.GetDictPointer();
            if (ptr is null)
            {
                return;
            }
            lock (_meta_cache_lock)
            {
                var list = _meta_cache;
                list.Clear();
                foreach (var (key, value) in tags)
                {
                    ffmpeg.av_dict_set(ptr, key, value, flag).CheckError(m => list.Add(new(key, value)));
                }
                CheckSetMetaTagError();
            }
        }
    }
}
