using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    [Serializable]
    class Interpolation
    {
        public ObservableCollection<Point> Points { get; set; }
        public List<Interval> Intervals { get; set; }
        public InterpolationMethod Method { get; set; }
        public bool HasTemporaryPoint { get; set; }

        public Interpolation()
        {
            Points = new ObservableCollection<Point>();
            Intervals = new List<Interval>();
            Method = new InterpolationLinearMethod();
        }

        public Interpolation(ObservableCollection<Point> points, InterpolationMethod method)
        {
            Points = points;
            Method = method;
            Intervals = Method.BuildIntervals(points);
        }

        public Interpolation(bool serializable)
        {
            if (serializable)
            {
                bool res = this.Deserialize();
                if (!res)
                {
                    Points = new ObservableCollection<Point>(new Point[] { new Point(3, 1), new Point(0, 4), new Point(1, 1), new Point(5, 6) });
                    Method = new InterpolationLinearMethod();
                }
            }
            else
            {
                Points = new ObservableCollection<Point>(new Point[] { new Point(3, 1), new Point(0, 4), new Point(1, 1), new Point(5, 6) });
                Method = new InterpolationLinearMethod();
            }
        }

        public void Count(Point point)
        {
            foreach (var i in Intervals)
            {
                if (i.IsPointInInterval(point))
                {
                    Method.Count(i, point);
                    break;
                }
            }
        }

        public void AddPoint(Point point)
        {
            if (Points.Any(p => p.X == point.X))
            {
                if (Points.Last().X == point.X)
                {
                    HasTemporaryPoint = false;
                    Points.Last().Y = point.Y;
                    Intervals = Method.BuildIntervals(Points);
                    Serialize();   
                }
                return;
            }

            if (HasTemporaryPoint)
            {
                Points.Remove(Points.Last());
                HasTemporaryPoint = false;
                Points.Add(point);
            }
            else
            {
                Points.Add(point);
                if (Points.Count > 1 && Method.GetInterpolationType() == InterpolationType.Square && (Points.Count - 3) % 2 != 0) 
                {
                    HasTemporaryPoint = true;
                    Point[] max = Points.OrderByDescending(p => p.X).ToArray();
                    Point temp = new Point(2 * max[0].X - max[1].X, max[1].Y);
                    Points.Add(temp);
                }
            }

            Intervals = Method.BuildIntervals(Points);
            Serialize();
        }

        public Interval GetInterval(Point p)
        {
            foreach (var i in Intervals)
            {
                if (i.IsPointInInterval(p))
                {
                    return i;
                }
            }
            return null;
        }

        public bool Serialize()
        {
            
            try
            {
                using (FileStream fs = File.Open("data.bin", FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, this);
                }

                return true;
            }
            catch (IOException e)
            {
                return false;
            }
        }
        public bool Deserialize()
        {
            try
            {
                using (FileStream fs = File.Open("data.bin", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Interpolation interpolation = (Interpolation)formatter.Deserialize(fs);

                    this.Method = interpolation.Method;
                    this.Points = new ObservableCollection<Point>(interpolation.Points.OrderBy(p => p.X));
                    this.Intervals = Method.BuildIntervals(Points);
                }

                return true;
            }
            catch (IOException e)
            {
                
                return false;
            }
        }
    }
}
