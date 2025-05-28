using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public partial class PackInfo() : ObservableObjectBase
    {
        [ObservableProperty]
        internal string _productId = "";
        [ObservableProperty]
        internal string _number = "";

        public string Name => CardPool.Instance.GetPack(_productId).Name;
        public DateTime Date => CardPool.Instance.GetPack(_productId).Date;
        public string DateString => CardPool.Instance.GetPack(_productId).DateString;

        public bool IsTcg() => CardPack.IsTcgPack(_productId);

        public PackInfo(Serializable.PackInfo info) : this()
        {
            _productId = info.ProductId ?? "";
            _number = info.Number ?? "";
        }
    }
}
