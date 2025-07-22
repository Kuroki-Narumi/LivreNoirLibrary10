using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class Track
    {
        public bool BulkEdit(BulkEditOptions options, Selection? selection, out Selection newSelection)
        {
            var left = (double)options.RangeLeft;
            var right = (double)options.RangeRight;
            var posP = right > left;
            var pos_ex = !options.RangeExclusive;

            var meta = options.Target_Meta;
            var sysex = options.Target_SysEx;
            var cc = options.Target_CC;
            var note = options.Target_Note;
            if (!(meta || sysex || cc || note))
            {
                meta = sysex = cc = note = true;
            }
            var numbers = options.Numbers;
            var numberP = numbers.Count is > 0;

            var q_pos = options.PositionQuantize;
            var op_pos = ValueOperation.TryGetOperator(options.PositionOperationMode, options.PositionOperationValue, out var func_pos);
            var q_len = options.LengthQuantize_Auto ? q_pos : options.LengthQuantize;
            var op_len = ValueOperation.TryGetOperator(options.LengthOperationMode, options.LengthOperationValue, out var func_len);
            var q_vel = options.VelQuantize;
            var op_vel = ValueOperation.TryGetOperator(options.VelOperationMode, (double)options.VelOperationValue, out var func_vel);
            var op_nn = ValueOperation.TryGetOperator(options.NumberOperationMode, (double)options.NumberOperationValue, out var func_nn);

            Selection newList = [];

            bool Process(Rational pos, IObject obj)
            {
                if (posP)
                {
                    var dPos = (double)pos;
                    if (pos_ex ^ (dPos >= left && dPos < right))
                    {
                        return false;
                    }
                }
                if (obj is INote n)
                {
                    if (!note || (numberP && !n.MatchesNumber(numbers)))
                    {
                        return false;
                    }
                    obj = n.GetEdited(q_len, func_len, q_vel, func_vel, func_nn);
                }
                else if (obj is ControlChange)
                {
                    if (!cc)
                    {
                        return false;
                    }
                }
                else if (obj is IMetaObject)
                {
                    if (!meta)
                    {
                        return false;
                    }
                }
                else if (obj is SysEx)
                {
                    if (!sysex)
                    {
                        return false;
                    }
                }
                newList.Add(INote.GetEdit(pos, q_pos, func_pos), obj);
                return true;
            }

            var timeline = Timeline;
            if (options.Selection && selection is not null && selection.Count is > 0)
            {
                foreach (var (pos, obj) in selection.EachItem())
                {
                    if (Process(pos, obj))
                    {
                        timeline.Remove(pos, obj);
                    }
                }
            }
            else
            {
                timeline.RemoveIf(Process);
            }

            if (newList.Count is 0)
            {
                newSelection = selection ?? [];
                return false;
            }
            foreach (var (pos, obj) in newList.EachItem())
            {
                timeline.Add(pos, obj);
            }
            if (options.RemoveDuplicates)
            {
                timeline.RemoveDuplicated(newList);
            }
            newSelection = newList;
            return true;
        }
    }
}
