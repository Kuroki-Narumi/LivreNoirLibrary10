using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Numerics
{
    partial struct Rational
    {
        public const ushort HalfDenominatorLimit = 1_000; // 10^3
        public const int FloatDenominatorLimit = 10_000_000; // 10^7
        public const long DoubleDenominatorLimit = 1_000_000_000_000_000; // 10^15

        /// <summary>
        /// Attempts to approximate the specified double-precision floating-point value as a rational number with a
        /// denominator not exceeding the given limit.
        /// </summary>
        /// <remarks>If the value is zero, the result is 0/1. If the value is an integer, the denominator
        /// is 1. The method returns false if the value is out of the representable range for rationalization.</remarks>
        /// <param name="value">The double-precision floating-point value to approximate as a rational number.</param>
        /// <param name="denominatorLimit">The maximum allowed value for the denominator of the resulting rational approximation. Must be positive.</param>
        /// <param name="numerator">When this method returns, contains the numerator of the rational approximation if the conversion succeeds;
        /// otherwise, zero.</param>
        /// <param name="denominator">When this method returns, contains the denominator of the rational approximation if the conversion succeeds;
        /// otherwise, zero.</param>
        /// <returns>true if the value was successfully approximated as a rational number within the specified denominator limit;
        /// otherwise, false.</returns>
        public static bool TryRationalize(double value, long denominatorLimit, out long numerator, out long denominator)
        {
            numerator = denominator = default;
            if (IsOutOfRange(value))
            {
                return false;
            }
            if (value == 0)
            {
                numerator = 0;
                denominator = 1;
                return true;
            }
            var (sign, absValue) = value is < 0 ? (-1, -value) : (1, value);
            var intPart = (long)Math.Truncate(absValue);
            if (intPart == absValue)
            {
                numerator = sign * intPart;
                denominator = 1;
                return true;
            }
            (var num, denominator) = RationalizeUnsafe(absValue - intPart, denominatorLimit);
            numerator = sign * (intPart * denominator + num);
            return true;
        }

        /// <inheritdoc cref="TryRationalize(double, long, out long, out long)" />
        public static bool TryRationalize(float value, long denominatorLimit, out long numerator, out long denominator)
        {
            numerator = denominator = default;
            if (IsOutOfRange(value))
            {
                return false;
            }
            if (value == 0)
            {
                numerator = 0;
                denominator = 1;
                return true;
            }
            var (sign, absValue) = value is < 0 ? (-1, -value) : (1, value);
            var intPart = (long)Math.Truncate(absValue);
            if (intPart == absValue)
            {
                numerator = sign * intPart;
                denominator = 1;
                return true;
            }
            (var num, denominator) = RationalizeUnsafe(absValue - intPart, denominatorLimit);
            numerator = sign * (intPart * denominator + num);
            return true;
        }

        /// <inheritdoc cref="TryRationalize(double, long, out long, out long)" />
        public static bool TryRationalize(decimal value, long denominatorLimit, out long numerator, out long denominator)
        {
            numerator = denominator = default;
            if (IsOutOfRange(value))
            {
                return false;
            }
            if (value == 0)
            {
                numerator = 0;
                denominator = 1;
                return true;
            }
            var (sign, absValue) = value is < 0 ? (-1, -value) : (1, value);
            var intPart = (long)Math.Truncate(absValue);
            if (intPart == absValue)
            {
                numerator = sign * intPart;
                denominator = 1;
                return true;
            }
            (var num, denominator) = RationalizeUnsafe(absValue - intPart, denominatorLimit);
            numerator = sign * (intPart * denominator + num);
            return true;
        }

        /// <summary>
        /// Converts the specified value to a fractional representation with a denominator not exceeding the specified limit.
        /// </summary>
        /// <param name="value">
        /// The value to convert to a fraction.
        /// </param>
        /// <param name="denominatorLimit">
        /// The maximum allowed denominator of the resulting fraction. 
        /// Must be between 1 and <see cref="DoubleDenominatorLimit"/>.
        /// </param>
        /// <returns>
        /// A tuple containing the numerator and denominator of the fraction that best approximates the input value,
        /// subject to the denominator limit.
        /// </returns>
        /// <exception cref="OverflowException"></exception>
        public static (long Numerator, long Denominator) Rationalize(double value, long denominatorLimit = DoubleDenominatorLimit)
        {
            if (!TryRationalize(value, denominatorLimit, out var numerator, out var denominator))
            {
                ThrowOverflowException();
            }
            return (numerator, denominator);
        }

        /// <inheritdoc cref="Rationalize(double, long)"/>
        public static (long Numerator, long Denominator) Rationalize(float value, long denominatorLimit = FloatDenominatorLimit)
        {
            if (!TryRationalize(value, denominatorLimit, out var numerator, out var denominator))
            {
                ThrowOverflowException();
            }
            return (numerator, denominator);
        }

        /// <inheritdoc cref="Rationalize(double, long)"/>
        public static (long Numerator, long Denominator) Rationalize(decimal value, long denominatorLimit = FloatDenominatorLimit)
        {
            if (!TryRationalize(value, denominatorLimit, out var numerator, out var denominator))
            {
                ThrowOverflowException();
            }
            return (numerator, denominator);
        }

        /// <summary>
        /// Converts the specified value to a fractional representation with a denominator not exceeding the specified limit.
        /// </summary>
        /// <remarks>
        /// This method does not perform input validation, so passing in inappropriate values can lead to serious problems, 
        /// including infinite loops.
        /// </remarks>
        /// <param name="value">
        /// The value to convert to a fraction. Must be greater than 0 and less than 1.
        /// </param>
        /// <param name="denominatorLimit">
        /// The maximum allowed denominator of the resulting fraction. Must be between 1 and <see cref="DoubleDenominatorLimit"/>.
        /// </param>
        /// <returns>
        /// A tuple containing the numerator and denominator of the fraction that best approximates the input value,
        /// subject to the denominator limit.
        /// </returns>
        /// <exception cref="OverflowException"></exception>
        public static (long Numerator, long Denominator) RationalizeUnsafe(double value, long denominatorLimit = DoubleDenominatorLimit)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(denominatorLimit, DoubleDenominatorLimit, nameof(denominatorLimit));
            ArgumentOutOfRangeException.ThrowIfLessThan(denominatorLimit, 1, nameof(denominatorLimit));

            var p = 0L;
            var q = 1L;
            var r = 1L;
            var s = 1L;
            while (true)
            {
                checked
                {
                    var pr = p + r;
                    var qs = q + s;
                    if (qs > denominatorLimit)
                    {
                        /*
                         * diff1 = value - p / q;
                         * diff2 = r / s - value;
                         * 
                         * diff1 <= diff2
                         *   -> value - p / q <= r / s - value
                         *   -> value * 2 <= p / q + r / s
                         *   -> value * 2qs <= ps + rq
                         */
                        return value * 2 * q * s <= (double)p * s + (double)r * q ? (p, q) : (r, s);
                    }
                    /*
                     * current = pr / qs;
                     * diff = value - current;
                     * 
                     * diff < 0
                     *   -> value - current < 0 
                     *   -> value - pr / qs < 0
                     *   -> value * qs - pr < 0
                     */
                    var diff = value * qs - pr;
                    if (diff is 0)
                    {
                        return (pr, qs);
                    }
                    /*
                     * Reference: https://qiita.com/okaponta_/items/36d485004d04b37519a3
                     */
                    var rs = r - value * s;
                    var qp = value * q - p;
                    if (diff is < 0)
                    {
                        var x = Math.Max(1, (long)Math.Min((rs / qp) - 1, (denominatorLimit - s) / q));
                        r += p * x;
                        s += q * x;
                    }
                    else
                    {
                        var x = Math.Max(1, (long)Math.Min((qp / rs) - 1, (denominatorLimit - q) / s));
                        p += r * x;
                        q += s * x;
                    }
                }
            }
        }

        /// <inheritdoc cref="RationalizeUnsafe(double, long)"/>
        public static (long Numerator, long Denominator) RationalizeUnsafe(float value, long denominatorLimit = FloatDenominatorLimit)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(denominatorLimit, FloatDenominatorLimit, nameof(denominatorLimit));
            ArgumentOutOfRangeException.ThrowIfLessThan(denominatorLimit, 1, nameof(denominatorLimit));

            var p = 0L;
            var q = 1L;
            var r = 1L;
            var s = 1L;
            while (true)
            {
                checked
                {
                    var pr = p + r;
                    var qs = q + s;
                    if (qs > denominatorLimit)
                    {
                        return value * 2 * q * s <= (float)p * s + (float)r * q ? (p, q) : (r, s);
                    }
                    var diff = value * qs - pr;
                    if (diff is 0)
                    {
                        return (pr, qs);
                    }
                    var rs = r - value * s;
                    var qp = value * q - p;
                    if (diff is < 0)
                    {
                        var x = Math.Max(1, (long)Math.Min((rs / qp) - 1, (denominatorLimit - s) / q));
                        r += p * x;
                        s += q * x;
                    }
                    else
                    {
                        var x = Math.Max(1, (long)Math.Min((qp / rs) - 1, (denominatorLimit - q) / s));
                        p += r * x;
                        q += s * x;
                    }
                }
            }
        }

        /// <inheritdoc cref="RationalizeUnsafe(double, long)"/>
        public static (long Numerator, long Denominator) RationalizeUnsafe(decimal value, long denominatorLimit = DoubleDenominatorLimit)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(denominatorLimit, DoubleDenominatorLimit, nameof(denominatorLimit));
            ArgumentOutOfRangeException.ThrowIfLessThan(denominatorLimit, 1, nameof(denominatorLimit));

            var p = 0L;
            var q = 1L;
            var r = 1L;
            var s = 1L;
            while (true)
            {
                checked
                {
                    var pr = p + r;
                    var qs = q + s;
                    if (qs > denominatorLimit)
                    {
                        return value * 2 * q * s <= (decimal)p * s + (decimal)r * q ? (p, q) : (r, s);
                    }
                    var diff = value * qs - pr;
                    if (diff is 0)
                    {
                        return (pr, qs);
                    }
                    var rs = r - value * s;
                    var qp = value * q - p;
                    if (diff is < 0)
                    {
                        var x = Math.Max(1, Math.Min((long)(rs / qp) - 1, (denominatorLimit - s) / q));
                        r += p * x;
                        s += q * x;
                    }
                    else
                    {
                        var x = Math.Max(1, Math.Min((long)(qp / rs) - 1, (denominatorLimit - q) / s));
                        p += r * x;
                        q += s * x;
                    }
                }
            }
        }
    }
}
