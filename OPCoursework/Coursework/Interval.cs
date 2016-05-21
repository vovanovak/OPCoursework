using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    [Serializable]
    class Interval // Інтервал
    {
        public Point[] Ranges { get; set; } // Точки, що входять в інтервал
        public double[] Сoefficients { get; set; } // Коефіціенти

        public Interval()
        {
            Ranges = new Point[0];
            Сoefficients = new double[0];
        }

        public bool IsPointInInterval(Point p) // Чи міститься точка в інтервалі
        {
            return (p.X >= Ranges[0].X && p.X <= Ranges[Ranges.Length - 1].X);
        }

        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < Ranges.Length - 1; i++)
            {
                sb.Append(string.Format("({0};{1}), ", Ranges[i].X, Ranges[i].Y));
            }

            sb.Append(string.Format("({0};{1})", Ranges.Last().X, Ranges.Last().Y));
            sb.Append("]");
            return sb.ToString();
        }
    }
}
