using System;
using System.Collections.Generic;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface IIdEnumerable
    {
        public IEnumerable<int> IdEnumerable { get; }
    }
}
