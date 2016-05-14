using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    [Serializable]
    class Interval
    {
        public Point[] Ranges { get; set; }
        public double[] Сoefficients { get; set; }

        public Interval()
        {
            Ranges = new Point[0];
            Сoefficients = new double[0];
        }

        public bool IsPointInInterval(Point p)
        {
            return (p.X >= Ranges[0].X && p.X <= Ranges[Ranges.Length - 1].X);
        }
    }
}
