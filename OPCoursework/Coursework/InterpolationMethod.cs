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
        public static int OperationCount { get; set; }

        public abstract void Count(Interval interval, Point point);
        public abstract void InitializeCoefficients(Interval interval);
        public abstract List<Interval> BuildIntervals(ObservableCollection<Point> points);
        public abstract InterpolationType GetInterpolationType();
    }
}
