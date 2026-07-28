using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    partial class GraphView
    {
        public class Element : IClear
        {
            public string? Name { get; internal set; }
            public double X { get; internal set; }
            public double Y { get; internal set; }

            public bool Contains(double x, double y, double radiusSquared)
            {
                var dx = x - X;
                var dy = y - Y;
                return (dx * dx + dy * dy) <= radiusSquared;
            }

            public virtual void Clear()
            {
                Name = null;
                X = 0;
                Y = 0;
            }
        }

        public class Node : Element
        {
            public object Key { get; internal set; } = null!;

            public override void Clear()
            {
                base.Clear();
                Key = null!;
            }
        }

        public class Edge : Element
        {
            public object From { get; internal set; } = null!;
            public object To { get; internal set; } = null!;
            public double CenterX { get; internal set; } = double.NaN;
            public double CenterY { get; internal set; } = double.NaN;
            public bool IsSelected { get; internal set; }

            public double X1 { get; private set; }
            public double Y1 { get; private set; }
            public double X2 { get; private set; }
            public double Y2 { get; private set; }

            public override void Clear()
            {
                base.Clear();
                From = null!;
                To = null!;
                CenterX = double.NaN;
                CenterY = double.NaN;
                IsSelected = false;
            }

            public void EnsurePosition(Func<object, Node?> getFunc)
            {
                if (getFunc(From) is { } node1 && getFunc(To) is { } node2)
                {
                    X1 = node1.X;
                    Y1 = node1.Y;
                    X2 = node2.X;
                    Y2 = node2.Y;
                    X = CenterX.Validate((X1 + X2) / 2);
                    Y = CenterY.Validate((Y1 + Y2) / 2);
                }
                else
                {
                    X1 = Y1 = X = Y = X2 = Y2 = double.NaN;
                }
            }
        }
    }
}
