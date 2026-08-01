using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.MasterDuel
{
    public class DuelLogCollection : ObservableList<DuelLog>
    {
        public void RenameTag(string? oldName, string? newName)
        {
            foreach (var item in AsSpan())
            {
                item.RenameTag(oldName, newName);
            }
        }
    }
}
