using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    [Serializable]
    class InterpolationLinearMethod: InterpolationMethod
    {
        public override void Count(Interval interval, Point point)
        {
            OperationCount = 0;
            if (interval.IsPointInInterval(point))
            {
                InitializeCoefficients(interval);

                point.Y = interval.Сoefficients[0] + interval.Сoefficients[1] * point.X;

                OperationCount += 2;
            }
        }

        public override void InitializeCoefficients(Interval interval)
        {
            if (interval.Сoefficients.Length < 1 || interval.Сoefficients.Length > 2)
            {
                interval.Сoefficients = new double[2];
                interval.Сoefficients[1] = (interval.Ranges[1].Y - interval.Ranges[0].Y)
                    / (interval.Ranges[1].X - interval.Ranges[0].X);

                OperationCount += 3;

                interval.Сoefficients[0] = interval.Ranges[0].Y - interval.Сoefficients[1] * interval.Ranges[0].X;

                OperationCount += 2;
            }
        }

        public override List<Interval> BuildIntervals(ObservableCollection<Point> points)
        {
            List<Interval> intervals = new List<Interval>();

            points = new ObservableCollection<Point>(points.OrderBy(p => p.X));

            for (int i = 0; i < points.Count - 1; i++)
            {
                intervals.Add(new Interval() { Ranges = new Point[] { points[i], points[i + 1] }});                
            }

            return intervals;
        }

        public override InterpolationType GetInterpolationType()
        {
            return InterpolationType.Linear;
        }
    }
}
