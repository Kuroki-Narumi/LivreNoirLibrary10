using LivreNoirLibrary.Debug;
using System;
using System.Collections.Generic;
using System.Text;

namespace LNImageEditor
{
    internal class Gomi
    {
        private void Calc9D6(int target)
        {
            var span = (stackalloc int[6]);
            span = [1, 2, 3, 4, 5, 6];
            foreach (var a in span)
            {
                var a_total = a * a; // ^2
                a_total *= a_total; // ^4
                a_total *= a_total; // ^8
                a_total *= a; // ^9
                if (a_total > target)
                {
                    break;
                }
                if (a_total + 2015538 < target)
                {
                    continue;
                }
                foreach (var b in span)
                {
                    var b_total = b * b; // ^2
                    b_total *= b_total; // ^4
                    b_total *= b_total; // ^8
                    b_total += a_total;
                    if (b_total > target)
                    {
                        break;
                    }
                    if (b_total + 335922 < target)
                    {
                        continue;
                    }
                    foreach (var c in span)
                    {
                        var c_total = c * c; // ^2
                        c_total *= c_total * c_total * c; // ^7
                        c_total += b_total;
                        if (c_total > target)
                        {
                            break;
                        }
                        if (c_total + 55986 < target)
                        {
                            continue;
                        }
                        foreach (var d in span)
                        {
                            var d_total = d * d; // ^2
                            d_total *= d_total * d_total; // ^6
                            d_total += c_total;
                            if (d_total > target)
                            {
                                break;
                            }
                            if (d_total + 9330 < target)
                            {
                                continue;
                            }
                            foreach (var e in span)
                            {
                                var e_total = e * e; // ^2
                                e_total *= e_total; // ^4
                                e_total *= e; // ^5
                                e_total += d_total;
                                if (e_total > target)
                                {
                                    break;
                                }
                                if (e_total + 1554 < target)
                                {
                                    continue;
                                }
                                foreach (var f in span)
                                {
                                    var f_total = f * f; // ^2
                                    f_total *= f_total; // ^4
                                    f_total += e_total;
                                    if (f_total > target)
                                    {
                                        break;
                                    }
                                    if (f_total + 258 < target)
                                    {
                                        continue;
                                    }
                                    foreach (var g in span)
                                    {
                                        var g_total = g * g * g + f_total;
                                        if (g_total > target)
                                        {
                                            break;
                                        }
                                        if (g_total + 42 < target)
                                        {
                                            continue;
                                        }
                                        foreach (var h in span)
                                        {
                                            var h_total = h * h + g_total;
                                            if (h_total > target)
                                            {
                                                break;
                                            }
                                            if (h_total + 6 < target)
                                            {
                                                continue;
                                            }
                                            var i = target - h_total;
                                            ExConsole.Write($"target={target}, resol=[{i}, {h}, {g}, {f}, {e}, {d}, {c}, {b}, {a}]");
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ExConsole.Write($"target={target}, not resolved");
        }
    }
}
