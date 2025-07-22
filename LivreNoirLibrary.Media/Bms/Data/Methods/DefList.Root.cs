using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BmsData
    {
        public void RemoveDefWithBasename(DefType type, string basename)
        {
            foreach (var data in EachData())
            {
                if (data != this)
                {
                    data.Inherit(this);
                }
                RemoveDefWithBasenameInternal(type, basename);
            }
        }

        public DefIndexCollection GetUsedDefList()
        {
            var lnEnd = LnObj is 0;
            DefIndexCollection used = [];
            foreach (var data in EachData())
            {
                foreach (var (_, note) in data.Timeline)
                {
                    if (note.IsIndex(lnEnd) && note.IsNonZero())
                    {
                        used.Add(BmsUtils.GetDefType(note.Lane), note.Id);
                    }
                }
            }
            return used;
        }
    }
}
