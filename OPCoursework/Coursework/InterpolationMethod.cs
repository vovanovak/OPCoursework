using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    
    [Serializable]
    abstract class InterpolationMethod
    {
        public static int OperationCount { get; set; } // Кількість операцій

        public abstract void Count(Interval interval, Point point); // Обрахування значення Y
        public abstract void InitializeCoefficients(Interval interval); // Ініціалізація коефіціентів
        public abstract List<Interval> BuildIntervals(ObservableCollection<Point> points); // Побудова інтервалів
        public abstract InterpolationType GetInterpolationType(); // Тип інтерполяції
    }
}
