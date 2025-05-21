using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Media.Wave.Chunks;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IWaveMetaData
    {
        FormatChunk Format { get; }
        List<RiffChunk> Chunks { get; }
    }

    public static class IWaveMetaDataExtentions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetChunk(this IWaveMetaData meta, string chid, [MaybeNullWhen(false)] out RiffChunk chunk) => TryGetChunk(meta, chunk => chunk.Chid == chid, out chunk);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetChunk(this IWaveMetaData meta, Predicate<RiffChunk> predicate, [MaybeNullWhen(false)] out RiffChunk chunk)
        {
            chunk = meta.Chunks.Find(predicate);
            return chunk is not null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetChunk<T>(this IWaveMetaData meta, [MaybeNullWhen(false)] out T chunk)
            where T : RiffChunk
        {
            chunk = meta.Chunks.Find(c => c is T) as T;
            return chunk is not null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetChunk<T>(this IWaveMetaData meta, string chid, [MaybeNullWhen(false)] out T chunk) where T : RiffChunk => TryGetChunk(meta, c => c.Chid == chid, out chunk);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetChunk<T>(this IWaveMetaData meta, Predicate<T> predicate, [MaybeNullWhen(false)] out T chunk)
            where T : RiffChunk
        {
            chunk = meta.Chunks.Find(c => c is T ct && predicate(ct)) as T;
            return chunk is not null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<RiffChunk> GetAllChunks(this IWaveMetaData meta, string chid) => meta.Chunks.FindAll(c => c.Chid == chid);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<RiffChunk> GetAllChunks(this IWaveMetaData meta, Predicate<RiffChunk> predicate) => meta.Chunks.FindAll(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> GetAllChunks<T>(this IWaveMetaData meta, string? chid = null) where T : RiffChunk => GetAllChunks<T>(meta, string.IsNullOrEmpty(chid) ? c => true : c => c.Chid == chid);

        public static List<T> GetAllChunks<T>(this IWaveMetaData meta, Predicate<T> predicate)
            where T : RiffChunk
        {
            List<T> result = [];
            foreach (var chunk in CollectionsMarshal.AsSpan(meta.Chunks))
            {
                if (chunk is T c && predicate(c))
                {
                    result.Add(c);
                }
            }
            return result;
        }

        public static T GetOrCreateChunk<T>(this IWaveMetaData meta, Func<T> ifnone)
            where T : RiffChunk
        {
            if (!TryGetChunk<T>(meta, out var result))
            {
                result = ifnone();
                meta.Chunks.Add(result);
            }
            return result;
        }

        public static T GetOrCreateChunk<T>(this IWaveMetaData meta, string chid, Func<T> ifnone)
            where T : RiffChunk
        {
            if (!TryGetChunk<T>(meta, chid, out var result))
            {
                result = ifnone();
                meta.Chunks.Add(result);
            }
            return result;
        }

        public static T GetOrCreateChunk<T>(this IWaveMetaData meta, Predicate<T> predicate, Func<T> ifnone)
            where T : RiffChunk
        {
            if (!TryGetChunk(meta, predicate, out var result))
            {
                result = ifnone();
                meta.Chunks.Add(result);
            }
            return result;
        }

        public static void ClearChunks(this IWaveMetaData meta) => meta.Chunks.Clear();

        public static int RemoveChunk(this IWaveMetaData meta, Predicate<RiffChunk> predicate) => meta.Chunks.RemoveAll(predicate);
        public static int RemoveChunk(this IWaveMetaData meta, string chid) => RemoveChunk(meta, c => c.Chid == chid);
        public static int RemoveChunk<T>(this IWaveMetaData meta) => meta.Chunks.RemoveAll(c => c is T);
        public static int RemoveChunk<T>(this IWaveMetaData meta, Predicate<T> predicate) => meta.Chunks.RemoveAll(c => c is T ct && predicate(ct));

        public static bool IsTempoSet(this IWaveMetaData meta) => TryGetChunk<Acid>(meta, out _);
        public static double GetTempo(this IWaveMetaData meta) => TryGetChunk<Acid>(meta, out var chunk) ? chunk.Tempo : IAudioMetaData.DefaultTempo;
        public static void SetTempo(this IWaveMetaData meta, double tempo)
        {
            var chunk = GetOrCreateChunk(meta, Acid.Create);
            chunk.SetTempo(tempo, 1);
        }

        public static TimeSignature GetTimeSignature(this IWaveMetaData meta) => TryGetChunk<Acid>(meta, out var chunk) ? new(chunk.SignatureNum, chunk.SignatureDen) : TimeSignature.Default;
        public static void SetTimeSignature(this IWaveMetaData meta, TimeSignature sign)
        {
            var chunk = GetOrCreateChunk(meta, Acid.Create);
            chunk.SetSignature(sign.Numerator, sign.Denominator);
        }

        public static bool TryGetList(this IWaveMetaData meta, string type, [MaybeNullWhen(false)] out LIST list) => TryGetChunk(meta, l => l.Type == type, out list);
        public static LIST GetOrCreateList(this IWaveMetaData meta, string type) => GetOrCreateChunk<LIST>(meta, l => l.Type == type, () => new(type));
        public static int RemoveList(this IWaveMetaData meta, string type) => RemoveChunk<LIST>(meta, l => l.Type == type);

        public static List<LIST> GetLists(this IWaveMetaData meta) => GetAllChunks<LIST>(meta);
        public static List<LIST> GetLists(this IWaveMetaData meta, string type, bool create = false)
        {
            var list = GetAllChunks<LIST>(meta, l => l.Type == type);
            if (create && list.Count is 0)
            {
                LIST chunk = new(type);
                meta.Chunks.Add(chunk);
                list.Add(chunk);
            }
            return list;
        }

        public static LIST GetOrCreateAdtlList(this IWaveMetaData meta) => GetOrCreateList(meta, ChunkIds.Associated);
        public static int RemoveAdtlList(this IWaveMetaData meta) => RemoveList(meta, ChunkIds.Associated);
        public static LIST GetOrCreateInfoList(this IWaveMetaData meta) => GetOrCreateList(meta, ChunkIds.Info);
        public static int RemoveInfoList(this IWaveMetaData meta) => RemoveList(meta, ChunkIds.Info);

        public static bool TryGetInfoText(this IWaveMetaData meta, string tag, [MaybeNullWhen(false)] out string text)
        {
            if (TryGetList(meta, ChunkIds.Info, out var list) && list.TryGetText(tag, out text))
            {
                return true;
            }
            text = null;
            return false;
        }

        public static string? GetInfoText(this IWaveMetaData meta, string tag) => TryGetList(meta, ChunkIds.Info, out var list) ? list.GetText(tag) : null;

        public static void SetInfoText(this IWaveMetaData meta, string tag, string? content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                GetOrCreateInfoList(meta).SetText(tag, content);
            }
            else if (TryGetList(meta, ChunkIds.Info, out var list))
            {
                list.RemoveAll(tag);
                if (list.Count is 0)
                {
                    meta.Chunks.Remove(list);
                }
            }
        }
    }
}
