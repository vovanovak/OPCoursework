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

            InitializeCoefficients(interval);

            point.Y = interval.Сoefficients[0] + interval.Сoefficients[1] * point.X; 
             // Обрахування значення Y, за коефіціентами проміжку

            OperationCount += 2;
        }
        

        public override void InitializeCoefficients(Interval interval) // Ініціалізуємо коефіціенти
        {
        
            interval.Сoefficients = new double[2];
            interval.Сoefficients[1] = (interval.Ranges[1].Y - interval.Ranges[0].Y)
                / (interval.Ranges[1].X - interval.Ranges[0].X);

            OperationCount += 3;

            interval.Сoefficients[0] = interval.Ranges[0].Y - interval.Сoefficients[1] * interval.Ranges[0].X;

            OperationCount += 2;
            
        }

        public override List<Interval> BuildIntervals(ObservableCollection<Point> points) // Побудова інтервалів
        {
            List<Interval> intervals = new List<Interval>();

            points = new ObservableCollection<Point>(points.OrderBy(p => p.X));

            for (int i = 0; i < points.Count - 1; i++)
            {
                intervals.Add(new Interval() { Ranges = new Point[] { points[i], points[i + 1] }});                
            }

            return intervals;
        }

        public override InterpolationType GetInterpolationType() // Повернення типу інтерполяції
        {
            return InterpolationType.Linear;
        }
    }
}
