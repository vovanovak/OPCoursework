using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    [Serializable]
    class InterpolationSquareMethod: InterpolationMethod
    {
        public override void Count(Interval interval, Point point)
        {
            InitializeCoefficients(interval);

            point.Y = interval.Сoefficients[0] + interval.Сoefficients[1] * point.X + interval.Сoefficients[2] * Math.Pow(point.X, 2);
        }

        public override void InitializeCoefficients(Interval interval)
        {
            if (interval.Сoefficients.Length <= 2)
            {
                interval.Сoefficients = new double[3];
                interval.Сoefficients[2] = ((interval.Ranges[2].Y - interval.Ranges[0].Y) /
                    ((interval.Ranges[2].X - interval.Ranges[0].X) *
                    (interval.Ranges[2].X - interval.Ranges[1].X))) -
                    ((interval.Ranges[1].Y - interval.Ranges[0].Y) /
                    ((interval.Ranges[1].X - interval.Ranges[0].X) *
                    (interval.Ranges[2].X - interval.Ranges[1].X)));

                interval.Сoefficients[1] = ((interval.Ranges[1].Y - interval.Ranges[0].Y) / (interval.Ranges[1].X - interval.Ranges[0].X)) -
                    interval.Сoefficients[2] * (interval.Ranges[1].X + interval.Ranges[0].X);

                interval.Сoefficients[0] = interval.Ranges[0].Y - interval.Сoefficients[1] * interval.Ranges[0].X - interval.Сoefficients[2] * Math.Pow(interval.Ranges[0].X, 2);
            }
        }

        public override List<Interval> BuildIntervals(ObservableCollection<Point> points)
        {
            List<Interval> intervals = new List<Interval>();

            points = new ObservableCollection<Point>(points.OrderBy(p => p.X));
            
            int i = 0;
            for (; i < points.Count - 2; i += 2)
            {
                intervals.Add(new Interval() { Ranges = new Point[] { points[i], points[i + 1], points[i + 2] }});
            }

            //if (hasTemporaryPoint)
            //    intervals.Last().HasTemporaryPoint = true;

            return intervals;
        }
    }
}
