using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IFlow : INoteObject
    {
        string BmsHeader { get; }
        string BmsFooter { get; }
    }
}
