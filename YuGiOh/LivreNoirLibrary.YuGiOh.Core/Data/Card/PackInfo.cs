using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class PackInfo() : ObservableObjectBase
    {
        public string ProductId { get; set => SetValue(ref field, value); } = "";
        public string Number { get; set => SetValue(ref field, value); } = "";

        public string Name => CardPool.Instance.GetPack(ProductId).Name;
        public DateTime Date => CardPool.Instance.GetPack(ProductId).Date;
        public string DateString => CardPool.Instance.GetPack(ProductId).DateText;

        public bool IsTcg() => CardPack.IsTcgPack(ProductId);

        public PackInfo(Serializable.PackInfo info) : this()
        {
            ProductId = info.ProductId ?? "";
            Number = info.Number ?? "";
        }
    }
}
