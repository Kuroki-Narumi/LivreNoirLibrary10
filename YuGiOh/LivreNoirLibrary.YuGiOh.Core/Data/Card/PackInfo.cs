using System;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class PackInfo(string pid, string number, string name, DateTime date) : IEquatable<PackInfo>, IComparable<PackInfo>
    {
        public string ProductId { get; } = pid;
        public string Number { get; } = number;
        public string Name { get; } = name;
        public DateTime Date { get; } = date;

        public bool IsTcg => CardPack.IsTcgPack(ProductId);
        public string DateText => CardPack.GetDateText(Date);

        public int CompareTo(PackInfo? other)
        {
            if (other is null)
            {
                return -1;
            }
            var c = other.Date.CompareTo(Date);
            if (c is not 0)
            {
                return c;
            }
            return ProductId.CompareTo(other.ProductId, StringComparison.Ordinal);
        }

        public bool Equals(PackInfo? other) => other is not null && ProductId == other.ProductId && Number == other.Number;
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is PackInfo other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(ProductId, Number);

        public static bool operator ==(PackInfo left, PackInfo right) => left.Equals(right);
        public static bool operator !=(PackInfo left, PackInfo right) => !left.Equals(right);
        public static bool operator <(PackInfo left, PackInfo right) => left.CompareTo(right) < 0;
        public static bool operator <=(PackInfo left, PackInfo right) => left.CompareTo(right) <= 0;
        public static bool operator >(PackInfo left, PackInfo right) => left.CompareTo(right) > 0;
        public static bool operator >=(PackInfo left, PackInfo right) => left.CompareTo(right) >= 0;
    }
}
