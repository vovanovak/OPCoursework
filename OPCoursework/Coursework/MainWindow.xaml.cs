using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coursework
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Interpolation interpolation; // Інтерполяція
        string filename = "results.txt"; // Файл результатів
        int step = 20; // Крок, за який відрисовується 1 поділка
        double k = 5; // 0.5 довжини поділки на осі координат
        int maxCount = 9; // Максимальна кількість поділок
        int lineCoef = 25; // Коефіціент відрисування стрілки
        NumberFormatInfo info;
        string separator; // Роздільник в дроб. числі
        double stepIteration = 0; // Крок за 1 ітерацію

        public MainWindow()
        {
            InitializeComponent();

            info = CultureInfo.CurrentCulture.NumberFormat; // Визначення роздільника(".", ",") в дробовому числі
            separator = info.CurrencyDecimalSeparator;

            interpolation = new Interpolation(true);

            if (interpolation.Method.GetType().Name == typeof(InterpolationLinearMethod).Name)
                comboBoxMethod.SelectedIndex = 0;
            else
                comboBoxMethod.SelectedIndex = 1;
            
            lstPoints.ItemsSource = interpolation.Points;
            lstPoints.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription
                ("X", System.ComponentModel.ListSortDirection.Ascending)); // Сортування списку точок за координатою Х
            
            draw();
        }

        private void draw()
        {
            canvas.Children.Clear();

            if (interpolation.Points.Count > 0)
            {
                drawCoords();
                drawIntervals();
                drawPoints();
            }
        }

        private void drawCoords() // Відрисування осі координат
        {
            
            double countX = interpolation.Points.Max(p => Math.Abs(p.X));
            double countY = interpolation.Points.Max(p => Math.Abs(p.Y));
            maxCount = (int)(Math.Floor(countX > countY ? countX : countY));

            Size desiredSize = new Size(double.PositiveInfinity, double.PositiveInfinity);

            Label test = new Label();
            test.Content = Convert.ToString(maxCount) + ".0";
            test.Measure(desiredSize);
           
            step = (int)test.DesiredSize.Width; // Визначення кроку

            if (step < 25)
                step = 25;

            stepIteration = ((maxCount) / ((double)(canvas.Width / 2 - step / 2) / step));
            //Визначення кроку ітерації


            //Відрисування осі Х
            Line myLine1 = new Line();

            myLine1.Stroke = System.Windows.Media.Brushes.Black;
            myLine1.X1 = 0;
            myLine1.X2 = canvas.Width;
            myLine1.Y1 = canvas.Height / 2;
            myLine1.Y2 = myLine1.Y1;
       
            myLine1.StrokeThickness = 1.5;
            canvas.Children.Add(myLine1);

            double value = 0;
            
            for (double i = 0; i <= maxCount; i += stepIteration)
            {
                Line lblValueLine = new Line();

                lblValueLine.Stroke = System.Windows.Media.Brushes.Black;
                lblValueLine.X1 = (i / stepIteration) * step + canvas.Width / 2;
                lblValueLine.X2 = lblValueLine.X1;
                lblValueLine.Y1 = canvas.Height / 2 - k;
                lblValueLine.Y2 = canvas.Height / 2 + k;
                lblValueLine.StrokeThickness = 1.5;
                canvas.Children.Add(lblValueLine);

                if (i != 0)
                {
                    value = Math.Round(i, 1);
                    Label lbl = new Label();
                    lbl.Content = value;
                    lbl.FontSize = 11;
                    lbl.Measure(desiredSize);

                    lbl.Margin = new Thickness((i / stepIteration) * step + canvas.Width / 2 - lbl.DesiredSize.Width / 2, canvas.Height / 2 + 1, 0, 0);

                    canvas.Children.Add(lbl);
                }
            }

            

            for (double i = 0; i <= maxCount; i+=stepIteration)
            {
                Line lblValueLine = new Line();

                lblValueLine.Stroke = System.Windows.Media.Brushes.Black;
                lblValueLine.X1 = (-i / stepIteration) * step + canvas.Width / 2;
                lblValueLine.X2 = (-i / stepIteration) * step + canvas.Width / 2;
                lblValueLine.Y1 = canvas.Height / 2 - k;
                lblValueLine.Y2 = canvas.Height / 2 + k;

                lblValueLine.StrokeThickness = 1.5;
                canvas.Children.Add(lblValueLine);

                if (i != 0)
                {
                    value = Math.Round(i, 1);
                    Label lbl = new Label();
                    lbl.Content = -value;
                    lbl.FontSize = 11;

                    lbl.Measure(desiredSize);
                    
                    lbl.Margin = new Thickness((-i / stepIteration) * step + canvas.Width / 2 - lbl.DesiredSize.Width / 2, canvas.Height / 2 + 1, 0, 0);
                    
                    canvas.Children.Add(lbl);
                }
            }
            //----------------------------------

            //Відрисування осі Y
            Line myLine2 = new Line();

            myLine2.Stroke = System.Windows.Media.Brushes.Black;
            myLine2.X1 = canvas.Width / 2;
            myLine2.X2 = canvas.Width / 2;
            myLine2.Y1 = 0;
            myLine2.Y2 = canvas.Height;
            myLine2.StrokeThickness = 1.5;
            canvas.Children.Add(myLine2);

            for (double i = 0; i <= maxCount; i += stepIteration)
            {
                Line lblValueLine = new Line();

                lblValueLine.Stroke = System.Windows.Media.Brushes.Black;
                lblValueLine.X1 = canvas.Width / 2 - k;
                lblValueLine.X2 = canvas.Width / 2 + k;
                lblValueLine.Y1 = (i / stepIteration) * step + canvas.Height / 2;
                lblValueLine.Y2 = lblValueLine.Y1;
                lblValueLine.StrokeThickness = 1.5;
                canvas.Children.Add(lblValueLine);

                if (i != 0)
                {
                    value = Math.Round(i, 1);

                    Label lbl = new Label();
                    lbl.Content = -value;
                    lbl.FontSize = 11;

                    lbl.Measure(desiredSize);

                    lbl.Margin = new Thickness(canvas.Width / 2 - lbl.DesiredSize.Width, (i / stepIteration) * step + canvas.Height / 2 - 12, 0, 0);

                    canvas.Children.Add(lbl);
                }
            }

           
            Label lbl0 = new Label();
            lbl0.Content = 0;
            lbl0.FontSize = 11;
            lbl0.Margin = new Thickness(canvas.Width / 2 - 14, canvas.Height / 2 + 1, 0, 0);

            canvas.Children.Add(lbl0);

            for (double i = 0; i <= maxCount; i += stepIteration)
            {
                Line lblValueLine = new Line();

                lblValueLine.Stroke = System.Windows.Media.Brushes.Black;
                lblValueLine.X1 = canvas.Width / 2 - k;
                lblValueLine.X2 = canvas.Width / 2 + k;
                lblValueLine.Y1 = (-i / stepIteration) * step + canvas.Height / 2;
                lblValueLine.Y2 = lblValueLine.Y1;
               
                lblValueLine.StrokeThickness = 1.5;
                canvas.Children.Add(lblValueLine);

                if (i != 0)
                {
                    value = Math.Round(i, 1);
                    Label lbl = new Label();
                    lbl.Content = value;
                    lbl.FontSize = 11;
                    lbl.Measure(desiredSize);
                    lbl.Margin = new Thickness(canvas.Width / 2 - lbl.DesiredSize.Width, (-i / stepIteration) * step + canvas.Height / 2 - 15, 0, 0);

                    canvas.Children.Add(lbl);
                }

            }
            //----------------------------------


            // Відрисування стрілок на коорд. осях і назв осей
            Line lineArrowX1 = new Line();

            lineArrowX1.Stroke = System.Windows.Media.Brushes.Black;
            lineArrowX1.X1 = (canvas.Width - lineCoef / 1.75);
            lineArrowX1.X2 = canvas.Width;
            lineArrowX1.Y1 = canvas.Height / 2 - lineCoef / 3;
            lineArrowX1.Y2 = canvas.Height / 2;
            lineArrowX1.StrokeThickness = 1.5;
            canvas.Children.Add(lineArrowX1);

            Line lineArrowX2 = new Line();

            lineArrowX2.Stroke = System.Windows.Media.Brushes.Black;
            lineArrowX2.X1 = (canvas.Width - lineCoef / 1.75);
            lineArrowX2.X2 = canvas.Width;
            lineArrowX2.Y1 = canvas.Height / 2 + lineCoef / 3;
            lineArrowX2.Y2 = canvas.Height / 2;
            lineArrowX2.StrokeThickness = 1.5;
            canvas.Children.Add(lineArrowX2);

            Label lblX = new Label();
            lblX.Content = "x";
            lblX.FontSize = 12;
            lblX.Measure(desiredSize);
            lblX.Margin = new Thickness(canvas.Width - lblX.DesiredSize.Width, canvas.Height / 2, 0, 0);
            canvas.Children.Add(lblX);

            Line lineArrowY1 = new Line();

            lineArrowY1.Stroke = System.Windows.Media.Brushes.Black;
            lineArrowY1.X1 = canvas.Width / 2 - lineCoef / 3;
            lineArrowY1.X2 = canvas.Width / 2;
            lineArrowY1.Y1 = 25 / 1.75;
            lineArrowY1.Y2 = 0;
            lineArrowY1.StrokeThickness = 1.5;
            canvas.Children.Add(lineArrowY1);

            Line lineArrowY2 = new Line();

            lineArrowY2.Stroke = System.Windows.Media.Brushes.Black;
            lineArrowY2.X1 = canvas.Width / 2 + lineCoef / 3;
            lineArrowY2.X2 = canvas.Width / 2;
            lineArrowY2.Y1 = 25 / 1.75;
            lineArrowY2.Y2 = 0;
            lineArrowY2.StrokeThickness = 1.5;
            canvas.Children.Add(lineArrowY2);

            Label lblY = new Label();
            lblY.Content = "y";
            lblY.FontSize = 12;
            lblY.Margin = new Thickness(canvas.Width / 2 - 23.5, -10, 0, 0);
            canvas.Children.Add(lblY);

            //----------------------------------
        }

        private void drawIntervals() // Відрисовка інтервалів
        {
            if (drawPolynom.IsChecked == true)
            {
                foreach (Interval i in interpolation.Intervals)
                {
                    drawInterval(i);
                }
            }
        }

        private void drawInterval(Interval interval) // Відрисовка інтервалу
        {
            if (comboBoxMethod.SelectedIndex == 0) // Якщо метод лінійний - рисуємо лінію
            {
                Line line = new Line();

                line.Stroke = System.Windows.Media.Brushes.Black;
                line.X1 = canvas.Width / 2 + step / stepIteration * interval.Ranges[0].X;
                line.X2 = canvas.Width / 2 + step / stepIteration * interval.Ranges[1].X;
                line.Y1 = canvas.Height / 2 - step / stepIteration * interval.Ranges[0].Y;
                line.Y2 = canvas.Height / 2 - step / stepIteration * interval.Ranges[1].Y;
                line.StrokeThickness = 1.5;

                canvas.Children.Add(line);
            }
            else // Якщо метод квадратичний - рисуємо параболу
            {
                double diff = (interval.Ranges[2].X - interval.Ranges[0].X);
                double graphicStep = 0.0025 * Math.Pow(10, Convert.ToString((int)Math.Round(diff, 0)).Length - 1);
                double iterationCount = diff / graphicStep;
                double graphX = interval.Ranges[0].X;

                for (double i = 0; i < iterationCount; i++, graphX += graphicStep)
                {
                    if (interval.Сoefficients.Length == 0)
                        interpolation.Method.InitializeCoefficients(interval);

                    double functionRes = interval.Сoefficients[0] + interval.Сoefficients[1] * graphX
                        + interval.Сoefficients[2] * Math.Pow(graphX, 2);

                    Ellipse graphPoint = new Ellipse();
                    graphPoint.Width = 1.5;
                    graphPoint.Height = 1.5;
                    graphPoint.StrokeThickness = 1.5;
                    graphPoint.Fill = new SolidColorBrush(Colors.Black);
                    graphPoint.Margin = new Thickness(canvas.Width / 2 + graphX * step / stepIteration - 0.75, (canvas.Height / 2) - functionRes * step / stepIteration - 0.75, 0, 0);

                    canvas.Children.Add(graphPoint);
                }
            }


        }

        private void drawPoints() // Відрисовуємо дані точки
        {
            for (int i = 0; i < interpolation.Points.Count; i++)
            {
                Ellipse point = new Ellipse();
                point.Width = 4;
                point.Height = 4;
                point.StrokeThickness = 1.5;
                point.Fill = new SolidColorBrush(Colors.Black);
                point.Margin = new Thickness(canvas.Width / 2 + (step / stepIteration) * interpolation.Points[i].X - 2,
                    (canvas.Height / 2) - (step / stepIteration) * interpolation.Points[i].Y - 2, 0, 0);
                canvas.Children.Add(point);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e) // Додавання точки
        {
            double x;
            ValidationResult valid = isNumberStringValid(txtAddX.Text); // Валідація
            if (valid != ValidationResult.Proper)
            {
                outputErrorMessage(valid, "X");
            }
            else
            {
                double.TryParse(txtAddX.Text, out x);
                valid = isNumberStringValid(txtAddY.Text);
                double y;
                if (valid == ValidationResult.Proper)
                {
                    double.TryParse(txtAddY.Text, out y);

                    Point p = new Point(x, y);

                    bool res = interpolation.AddPoint(p);

                    if (!res)
                    {

                        MessageBox.Show("Точка з даною координатою Х вже існує!", "Інтерполяція",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    lstPoints.Items.Refresh();

                    draw();
                }
                else
                {
                    outputErrorMessage(valid, "Y");
                }
            }
        }

        private void outputErrorMessage(ValidationResult res, string fieldName) // Виведення повідомлення про помилку
        {
            switch (res)
            {
                case ValidationResult.Empty:
                    MessageBox.Show("Поле " + fieldName + " пусте!", "Інтерполяція",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
                case ValidationResult.NotValidEnter:
                    MessageBox.Show("Неправильно введене число " + fieldName , "Інтерполяція",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }

        private void btnCount_Click(object sender, RoutedEventArgs e) // Обрахування значення Y
        {
            ValidationResult valid = isNumberStringValid(txtX.Text); // Валідація

            if (valid != ValidationResult.Proper)
            {
                outputErrorMessage(valid, "X");
            }
            else
            {
                double x;
                double.TryParse(txtX.Text, out x);

                Point p = new Point(x, 0);


                var watch = System.Diagnostics.Stopwatch.StartNew();
                interpolation.Count(p, filename);
                watch.Stop();
                var elapsedMs = (double)watch.ElapsedTicks / TimeSpan.TicksPerMillisecond;

                draw();

                Interval interval = interpolation.GetInterval(p);

                if (interval != null)
                {
                    //Відрисування інтервалу і виведення точки

                    lblInterval.Content = "x є [" + interval.Ranges[0].X + "; " + interval.Ranges.Last().X + "]";

                    lblPolynom.Content = interval.GetPolynom();
                    lblRunTime.Content = elapsedMs + "мс. ";
                    lblOperationsCount.Content = InterpolationMethod.OperationCount;
                    lblIterationsCount.Content = Interpolation.IterationCount;

                    Ellipse point = new Ellipse();
                    point.Width = 4;
                    point.Height = 4;
                    point.StrokeThickness = 1.5;
                    point.Fill = new SolidColorBrush(Colors.Black);
                    point.Margin = new Thickness(canvas.Width / 2 + step / stepIteration * p.X - 2, (canvas.Height / 2) - step / stepIteration * p.Y - 2, 0, 0);
                    canvas.Children.Add(point);

                    Ellipse pointBorder = new Ellipse();
                    pointBorder.Width = 9;
                    pointBorder.Height = 9;
                    pointBorder.StrokeThickness = 1.5;
                    pointBorder.Stroke = new SolidColorBrush(Colors.Red);
                    pointBorder.Margin = new Thickness(canvas.Width / 2 + step / stepIteration * p.X - 4.5, (canvas.Height / 2) - step / stepIteration * p.Y - 4.5, 0, 0);
                    canvas.Children.Add(pointBorder);

                    lblY.Content = Math.Round(p.Y, 6);

                    if (drawPolynom.IsChecked != true)
                        drawInterval(interval);
                }
                else
                {
                    MessageBox.Show("Точка не належить ні одному з даних інтервалів!", "Інтерполяція",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    lblY.Content = "Невизначеність";
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) // Видалення точки
        {
            deletePoint();
        }

        private void btnDeleteAll_Click(object sender, RoutedEventArgs e) //Видалення всіх точок
        {
            interpolation.HasTemporaryPoint = false;
            interpolation.Points.Clear();
            interpolation.Intervals.Clear();
            draw();
            lstPoints.Items.Refresh();
        }

        private void comboBoxMethod_SelectionChanged(object sender, SelectionChangedEventArgs e) // Зміна вибору методу
        {
            if (interpolation != null)
            {
                if (comboBoxMethod.SelectedIndex == 0)
                {
                    interpolation.RemoveTemporaryPoint();
                    interpolation = new Interpolation(interpolation.Points, new InterpolationLinearMethod());
                }
                else
                {
                    interpolation = new Interpolation(interpolation.Points, new InterpolationSquareMethod());
                    interpolation.AddTemporaryPoint();
                    interpolation.Intervals = interpolation.Method.BuildIntervals(interpolation.Points);
                }

                lstPoints.Items.Refresh();
                draw();
            }
        }

        private void Window_Closed(object sender, EventArgs e) // Збереження даних при закриванні форми
        {
            interpolation.Serialize();
        }

        private void lstPoints_KeyDown(object sender, KeyEventArgs e) // Видалення при натисненні клавіші Del
        {
            if (e.Key == Key.Delete)
            {
                deletePoint();
            }
        }

        private void deletePoint() // Видалення точки
        {
            if (lstPoints.SelectedIndex >= 0)
            {
                interpolation.RemovePoint(lstPoints.SelectedIndex);
                interpolation.Intervals = interpolation.Method.BuildIntervals(interpolation.Points);
                lstPoints.Items.Refresh();
                draw();
            }
        }

        private void drawPolynom_Checked(object sender, RoutedEventArgs e) //Відрисування інтервалів
        {
            drawIntervals();
        }

        private void drawPolynom_Unchecked(object sender, RoutedEventArgs e) // Очищення від інтервалів
        {
            draw();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) // Зміна розміру вікна
        {
            canvas.Width = e.NewSize.Height / 2;
            canvas.Height = e.NewSize.Height / 2;
            draw();
        }

        private ValidationResult isNumberStringValid(string str) // Валідація рядка
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                
                return ValidationResult.Empty;
            }
           
            for (int i = 0; i < str.Length; i++)
            {
                if (!(char.IsDigit(str.ElementAt(i)) || separator.Contains(str[i]) || str[i] == '-'))
                {
                   
                    return ValidationResult.NotValidEnter;
                }
            }

            return ValidationResult.Proper;
        }

        private void btnResultsOpen_Click(object sender, RoutedEventArgs e) // Відкриття файлу результатів
        {
            if (File.Exists(filename)){
                Process.Start("notepad.exe", filename);
            } else {
                 MessageBox.Show("Файл " + filename + " не існує!", "Інтерполяція",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

