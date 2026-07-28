using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.ObjectModel
{
    public interface IId
    {
        int Id { get; }

        public static int GetId<T>(T obj) where T : IId => obj.Id;
    }
}
