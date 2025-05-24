using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Wave;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BmsData
    {
        public bool ApplyMultiDef(MultiDefOptions options, string directory) => ApplyMultiDef(options, directory, null, CancellationToken.None);
        public bool ApplyMultiDef(MultiDefOptions options, string directory, ProgressReporter? reporter, CancellationToken c)
        {
            var modified = false;
            reporter?.Report("Checking Timeline ...", 0, 100);
            var indexes = options.Indexes;
            Predicate<Note> selector = indexes.Count is 0 ? n => n.IsPlayableSound() && n.Id is not 0 : n => n.IsPlayableSound() && indexes.Contains(n.Id);

            // path 1: IDごとにtick位置のリストを作成
            SortedDictionary<int, List<(long, Note)>> idList = [];
            TimeCounter counter = new(this);
            foreach (var (_, beat, note) in EachNote())
            {
                if (selector(note))
                {
                    idList.GetOrAdd(note.Id).Add((counter.Beat2Ticks(beat), note));
                }
            }

            c.ThrowIfCancellationRequested();
            // path 2: 置換先のIDリストを作成
            reporter?.Report("Processing ...", 1, 100);
            var current = 0;
            var max = 98.0 / idList.Count;
            WaveBuffer buffer = new();
            var minInterval = (options.MinimumInterval * TimeSpan.TicksPerSecond).RoundToLong();
            var threshold = WaveBuffer.Level2Value(options.Threshold);
            var maxCount = options.MaxCount;
            var requiredTicks = (stackalloc long[maxCount]);
            List<(int, int, string, List<(Note, int)>)> replace = [];
            foreach (var (index, list) in idList)
            {
                c.ThrowIfCancellationRequested();
                if (TryGetWavePath(index, directory, out var name, out var path))
                {
                    try
                    {
                        buffer.AutoDecode(path);
                    }
                    catch
                    {
                        ExConsole.Write($"Failed to open {path}");
                        continue;
                    }
                }
                else
                {
                    continue;
                }
                var interval = Math.Max(buffer.FindLastSound(threshold) * TimeSpan.TicksPerSecond / buffer.SampleRate, minInterval);
                requiredTicks.Clear();
                List<(Note, int)> replaceList = [];
                var maxIndex = 0;
                foreach (var (tick, note) in CollectionsMarshal.AsSpan(list))
                {
                    var min = long.MaxValue;
                    var minIndex = 0;
                    for (var i = 0; i < maxCount; i++)
                    {
                        var requiredTick = requiredTicks[i];
                        if (requiredTick <= tick)
                        {
                            minIndex = i;
                            break;
                        }
                        if (requiredTick < min)
                        {
                            min = requiredTick;
                            minIndex = i;
                        }
                    }
                    if (minIndex is not 0)
                    {
                        replaceList.Add((note, minIndex - 1));
                    }
                    requiredTicks[minIndex] = tick + interval;
                    if (minIndex > maxIndex)
                    {
                        maxIndex = minIndex;
                    }
                }
                reporter?.Report($"Processing ({current} of {max})", 1.0 + current * max, 100);
                if (maxIndex is not 0)
                {
                    modified = true;
                    replace.Add((index, maxIndex, name, replaceList));
                }
            }
            c.ThrowIfCancellationRequested();
            // path 3: ID置換
            if (modified)
            {
                reporter?.Report("Replacing ...", 99, 100);
                var defIndex = DefLists.FindFreeIndex(DefType.Wav);
                Dictionary<int, List<int>> insert = [];
                foreach (var (originalIndex, duplicateCount, defValue, replaceList) in CollectionsMarshal.AsSpan(replace))
                {
                    c.ThrowIfCancellationRequested();
                    List<int> insertedIndex = [];
                    foreach (var (note, replaceIndex) in CollectionsMarshal.AsSpan(replaceList))
                    {
                        if (replaceIndex >= insertedIndex.Count)
                        {
                            insertedIndex.Add(defIndex);
                            defIndex = DefLists.FindFreeIndex(DefType.Wav, defIndex);
                        }
                        note.Id = insertedIndex[replaceIndex];
                    }
                    insert.Add(originalIndex, insertedIndex);
                }

                // path 4: ソート
                if (options.InsertDefIndex)
                {
                    reporter?.Report("Sorting ...", 99.9, 100);
                    DefIndexMap map = new();
                    var maxDefIndex = MaxDefIndex;
                    var offset = 0;
                    for (var i = 0; i < MaxDefIndex; i++)
                    {
                        map.Set(i, i + offset);
                        if (insert.TryGetValue(i, out var inserted))
                        {
                            foreach (var newIndex in CollectionsMarshal.AsSpan(inserted))
                            {
                                offset++;
                                map.Set(newIndex, i + offset);
                            }
                        }
                    }
                    DefMap(DefType.Wav, map);
                }
            }
            return modified;
        }
    }
}
