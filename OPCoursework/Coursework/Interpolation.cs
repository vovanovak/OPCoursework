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
        public static int IterationCount { get; set; } // Кількість ітерацій
        public ObservableCollection<Point> Points { get; set; } // Колекція точок
        public List<Interval> Intervals { get; set; } // Список інтервалів
        public InterpolationMethod Method { get; set; } // Метод інтерполяції
        public bool HasTemporaryPoint { get; set; } // Логічне поле, яке описує чи має колекція тимчасову точку

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
                    Points = new ObservableCollection<Point>();
                    Intervals = new List<Interval>();
                    Method = new InterpolationLinearMethod();
                }
            }
            else
            {
                Points = new ObservableCollection<Point>(new Point[] { new Point(3, 1), new Point(0, 4), new Point(1, 1), new Point(5, 6) });
                Method = new InterpolationLinearMethod();
            }
        }

        public void Count(Point point, string filename) // Обрахування значення Y
        {
            IterationCount = 0;
            foreach (var i in Intervals) // Знаходимо необхідний інтервал
            {
                IterationCount++;
                if (i.IsPointInInterval(point)) 
                {
                    Method.Count(i, point); // Обраховуємо значення Y
                    WriteResultToFile(filename, point, i); // Записуємо в файл результатів
                    break;
                }
               
            }
        }

        public bool AddPoint(Point point) // Додавання точки
        {
            if (Points.Count > 0)
            {
                if (Points.Last().X == point.X && HasTemporaryPoint)
                {
                    HasTemporaryPoint = false; // Якщо остання точка є тимчасовою
                    Points.Last().Y = point.Y; // і Х тимчасової = Х точки, яку ми додаємо
                    //Присвоюємо ординаті останньої точки значення Y 
                }
                else
                {
                    if (Points.Any(p => p.X == point.X)) // Якщо містить точку, яка не є тимчасовою
                        return false; // Повертаємо хибу, бо точки з однаковими абсцисами не можна додавати

                    if (HasTemporaryPoint) 
                    {
                        Points.Remove(Points.Last()); // Якщо має тимчасову точку, видаляємо її
                        HasTemporaryPoint = false;
                        Points.Add(point); //Додаємо нову точку
                    }
                    else
                    {
                        Points.Add(point); //Інакше додаємо точку
                        AddTemporaryPoint();
                    }
                }
            }
            else
            {
                Points.Add(point);
            }
            Intervals = Method.BuildIntervals(Points); // Ініціалізуємо список інтервалів
            Serialize(); // Зберігаємо зміни
            return true;
        }

        public Interval GetInterval(Point p)
        {
            foreach (var i in Intervals)
            {
                if (i.IsPointInInterval(p)) // Знаходимо інтервал, в якому міститься точка p
                {
                    return i;
                }
            }
            return null;
        }

        public void RemovePoint(int selectedIndex) //Видалення точки за вказаним індексом
        {
            if (HasTemporaryPoint)
            {
                Points.Remove(Points.Last());
                HasTemporaryPoint = false;
            }
            if (selectedIndex >= 0 && selectedIndex < Points.Count)
            {
                Points.RemoveAt(selectedIndex);
            }
            Intervals = Method.BuildIntervals(Points);
            Serialize();
        }

        public void RemoveTemporaryPoint() // Видалення тимчасової точки
        {
            if (HasTemporaryPoint)
            {
                HasTemporaryPoint = false;
                Points.Remove(Points.Last());
                Intervals = Method.BuildIntervals(Points);
            }
        }

        public void AddTemporaryPoint()
        {
            if (Points.Count > 1 && Method.GetInterpolationType() == 
                InterpolationType.Square && (Points.Count - 3) % 2 != 0) // Умова додавання тимчасової точки
            {
                HasTemporaryPoint = true;
                Point[] max = Points.OrderByDescending(p => p.X).ToArray();
                Point temp = new Point(2 * max[0].X - max[1].X, max[1].Y);
                Points.Add(temp);
            }
        }

        public bool Serialize() // Збереження змін в файл
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

        public bool Deserialize() // Зчитування даних з файлу
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

        public void WriteResultToFile(string filename, Point point, Interval interval) // Записування результатів в файл
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Method: {0}", (Method.GetInterpolationType() == InterpolationType.Linear) ? "Linear" : "Square"));
            sb.Append("Intervals: ");
            for (int i = 0; i < Intervals.Count - 1; i++)
            {
                sb.Append(Intervals[i].ToString() + ", ");
            }
            sb.AppendLine(Intervals.Last().ToString() + ";");
            sb.AppendLine("Current interval: " + interval.ToString() + ";");
            sb.AppendLine(string.Format("Point X: {0}; Y: {1};", point.X, point.Y));
            sb.AppendLine();

            File.AppendAllText(filename, sb.ToString());
        }
    }
}
