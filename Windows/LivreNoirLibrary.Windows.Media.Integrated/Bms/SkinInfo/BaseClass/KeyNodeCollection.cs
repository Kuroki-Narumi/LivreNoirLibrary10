using System;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public class KeyNodeCollection<T>() : StringKeyCollection<T>
        where T : IKeyNode
    {
        protected override string GetKey(T item) => item.Key ?? "";
    }
}
