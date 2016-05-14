using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        Interpolation interpolation;
        int step = 20;
        double k = 5;
        int maxCount = 9;
        
        public MainWindow()
        {
            InitializeComponent();

            interpolation = new Interpolation(true);

            if (interpolation.Method.GetType().Name == typeof(InterpolationLinearMethod).Name)
                comboBoxMethod.SelectedIndex = 0;
            else
                comboBoxMethod.SelectedIndex = 1;
            
            lstPoints.ItemsSource = interpolation.Points;
            lstPoints.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription
                ("X", System.ComponentModel.ListSortDirection.Ascending));
            
            draw();
        }

        private void draw()
        {
            canvas.Children.Clear();

            drawCoords();
            drawIntervals();
            drawPoints();
        }

        private void drawCoords()
        {
            Line myLine1 = new Line();

            myLine1.Stroke = System.Windows.Media.Brushes.Black;
            myLine1.X1 = 0;
            myLine1.X2 = canvas.Width;
            myLine1.Y1 = canvas.Height / 2;
            myLine1.Y2 = myLine1.Y1;

            myLine1.StrokeThickness = 1.5;
            canvas.Children.Add(myLine1);

            for (int i = 0; i < maxCount; i++)
            {
                Line lblValueLine = new Line();

                lblValueLine.Stroke = System.Windows.Media.Brushes.Black;
                lblValueLine.X1 = i * step + canvas.Width / 2;
                lblValueLine.X2 = lblValueLine.X1;
                lblValueLine.Y1 = canvas.Height / 2 - k;
                lblValueLine.Y2 = canvas.Height / 2 + k;
                lblValueLine.StrokeThickness = 1.5;
                canvas.Children.Add(lblValueLine);

                if (i != 0)
                {
                    Label lbl = new Label();
                    lbl.Content = i;
                    lbl.FontSize = 11;

                    lbl.Margin = new Thickness(i * step + canvas.Width / 2 - 7.5, canvas.Height / 2 + 1, 0, 0);

                    canvas.Children.Add(lbl);
                }
            }

            for (int i = 0; i < maxCount; i++)
            {
                Line lblValueLine = new Line();

                lblValueLine.Stroke = System.Windows.Media.Brushes.Black;
                lblValueLine.X1 = -i * step + canvas.Width / 2;
                lblValueLine.X2 = -i * step + canvas.Width / 2;
                lblValueLine.Y1 = canvas.Height / 2 - k;
                lblValueLine.Y2 = canvas.Height / 2 + k;

                lblValueLine.StrokeThickness = 1.5;
                canvas.Children.Add(lblValueLine);

                if (i != 0)
                {
                    Label lbl = new Label();
                    lbl.Content = -i;
                    lbl.FontSize = 11;
                    lbl.Margin = new Thickness(-i * step + canvas.Width / 2 - 10, canvas.Height / 2 + 1, 0, 0);

                    canvas.Children.Add(lbl);
                }
            }

            Line myLine2 = new Line();

            myLine2.Stroke = System.Windows.Media.Brushes.Black;
            myLine2.X1 = canvas.Width / 2;
            myLine2.X2 = canvas.Width / 2;
            myLine2.Y1 = 0;
            myLine2.Y2 = canvas.Height;
            myLine2.StrokeThickness = 1.5;
            canvas.Children.Add(myLine2);

            for (int i = 0; i < maxCount; i++)
            {
                Line lblValueLine = new Line();

                lblValueLine.Stroke = System.Windows.Media.Brushes.Black;
                lblValueLine.X1 = canvas.Width / 2 - k;
                lblValueLine.X2 = canvas.Width / 2 + k;
                lblValueLine.Y1 = i * step + canvas.Height / 2;
                lblValueLine.Y2 = lblValueLine.Y1;
                lblValueLine.StrokeThickness = 1.5;
                canvas.Children.Add(lblValueLine);

                if (i != 0)
                {
                    Label lbl = new Label();
                    lbl.Content = -i;
                    lbl.FontSize = 11;
                    lbl.HorizontalAlignment = HorizontalAlignment.Center;
                    lbl.VerticalAlignment = VerticalAlignment.Center;
                    lbl.Margin = new Thickness(canvas.Width / 2 - 25, i * step + canvas.Height / 2 - 12, 0, 0);

                    canvas.Children.Add(lbl);
                }
            }


            Label lbl0 = new Label();
            lbl0.Content = 0;
            lbl0.FontSize = 11;
            lbl0.Margin = new Thickness(canvas.Width / 2 - 14, canvas.Height / 2 + 1, 0, 0);

            canvas.Children.Add(lbl0);

            for (int i = 0; i < maxCount; i++)
            {
                Line lblValueLine = new Line();

                lblValueLine.Stroke = System.Windows.Media.Brushes.Black;
                lblValueLine.X1 = canvas.Width / 2 - k;
                lblValueLine.X2 = canvas.Width / 2 + k;
                lblValueLine.Y1 = -i * step + canvas.Height / 2;
                lblValueLine.Y2 = lblValueLine.Y1;
                lblValueLine.HorizontalAlignment = HorizontalAlignment.Center;
                lblValueLine.VerticalAlignment = VerticalAlignment.Center;
                lblValueLine.StrokeThickness = 1.5;
                canvas.Children.Add(lblValueLine);

                if (i != 0)
                {
                    Label lbl = new Label();
                    lbl.Content = i;
                    lbl.FontSize = 11;
                    lbl.Margin = new Thickness(canvas.Width / 2 - 23.5, -i * step + canvas.Height / 2 - 15, 0, 0);

                    canvas.Children.Add(lbl);
                }

            }


            Line lineArrowX1 = new Line();

            lineArrowX1.Stroke = System.Windows.Media.Brushes.Black;
            lineArrowX1.X1 = (canvas.Width - step / 1.75);
            lineArrowX1.X2 = canvas.Width;
            lineArrowX1.Y1 = canvas.Height / 2 - step / 3;
            lineArrowX1.Y2 = canvas.Height / 2;
            lineArrowX1.StrokeThickness = 1.5;
            canvas.Children.Add(lineArrowX1);

            Line lineArrowX2 = new Line();

            lineArrowX2.Stroke = System.Windows.Media.Brushes.Black;
            lineArrowX2.X1 = (canvas.Width - step / 1.75);
            lineArrowX2.X2 = canvas.Width;
            lineArrowX2.Y1 = canvas.Height / 2 + step / 3;
            lineArrowX2.Y2 = canvas.Height / 2;
            lineArrowX2.StrokeThickness = 1.5;
            canvas.Children.Add(lineArrowX2);

            Label lblX = new Label();
            lblX.Content = "x";
            lblX.FontSize = 12;
            lblX.Margin = new Thickness(canvas.Width - 10, canvas.Height / 2, 0, 0);
            canvas.Children.Add(lblX);

            Line lineArrowY1 = new Line();

            lineArrowY1.Stroke = System.Windows.Media.Brushes.Black;
            lineArrowY1.X1 = canvas.Width / 2 - step / 3;
            lineArrowY1.X2 = canvas.Width / 2;
            lineArrowY1.Y1 = step / 1.75;
            lineArrowY1.Y2 = 0;
            lineArrowY1.StrokeThickness = 1.5;
            canvas.Children.Add(lineArrowY1);

            Line lineArrowY2 = new Line();

            lineArrowY2.Stroke = System.Windows.Media.Brushes.Black;
            lineArrowY2.X1 = canvas.Width / 2 + step / 3;
            lineArrowY2.X2 = canvas.Width / 2;
            lineArrowY2.Y1 = step / 1.75;
            lineArrowY2.Y2 = 0;
            lineArrowY2.StrokeThickness = 1.5;
            canvas.Children.Add(lineArrowY2);

            Label lblY = new Label();
            lblY.Content = "y";
            lblY.FontSize = 12;
            lblY.Margin = new Thickness(canvas.Width / 2 - 23.5, -10, 0, 0);
            canvas.Children.Add(lblY);
        }

        private void drawIntervals()
        {
            if (drawPolynom.IsChecked == true)
            {
                foreach (Interval i in interpolation.Intervals)
                {
                    drawInterval(i);
                }
            }
        }

        private void drawInterval(Interval interval)
        {
            if (comboBoxMethod.SelectedIndex == 0)
            {
                Line line = new Line();

                line.Stroke = System.Windows.Media.Brushes.Black;
                line.X1 = canvas.Width / 2 + step * interval.Ranges[0].X;
                line.X2 = canvas.Width / 2 + step * interval.Ranges[1].X;
                line.Y1 = canvas.Height / 2 - step * interval.Ranges[0].Y;
                line.Y2 = canvas.Height / 2 - step * interval.Ranges[1].Y;
                line.StrokeThickness = 1.5;

                canvas.Children.Add(line);
            }
            else
            {
                double graphicStep = 0.0025;
                double iterationCount = (interval.Ranges[2].X - interval.Ranges[0].X) / graphicStep;
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
                    graphPoint.Margin = new Thickness(canvas.Width / 2 + graphX * step - 0.75, (canvas.Height / 2) - functionRes * step - 0.75, 0, 0);

                    canvas.Children.Add(graphPoint);
                }
            }


        }

        private void drawPoints()
        {
            for (int i = 0; i < interpolation.Points.Count; i++)
            {
                Ellipse point = new Ellipse();
                point.Width = 4;
                point.Height = 4;
                point.HorizontalAlignment = HorizontalAlignment.Center;
                point.VerticalAlignment = VerticalAlignment.Center;
                point.StrokeThickness = 1.5;
                point.Fill = new SolidColorBrush(Colors.Black);
                point.Margin = new Thickness(canvas.Width / 2 + step * interpolation.Points[i].X - 2,
                    (canvas.Height / 2) - step * interpolation.Points[i].Y - 2, 0, 0);
                canvas.Children.Add(point);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            double x;
            double.TryParse(txtAddX.Text, out x);

            double y;
            double.TryParse(txtAddY.Text, out y);


            Point p = new Point(x, y);
            
            interpolation.AddPoint(p);

            lstPoints.Items.Refresh();

            draw();
        }

        private void btnCount_Click(object sender, RoutedEventArgs e)
        {
            double x;
            double.TryParse(txtX.Text, out x);

            Point p = new Point(x, 0);

            interpolation.Count(p);

            

            draw();
        
            Interval interval = interpolation.GetInterval(p);

            if (interval != null)
            {
                Ellipse point = new Ellipse();
                point.Width = 4;
                point.Height = 4;
                point.StrokeThickness = 1.5;
                point.Fill = new SolidColorBrush(Colors.Black);
                point.Margin = new Thickness(canvas.Width / 2 + step * p.X - 2, (canvas.Height / 2) - step * p.Y - 2, 0, 0);
                canvas.Children.Add(point);

                Ellipse pointBorder = new Ellipse();
                pointBorder.Width = 9;
                pointBorder.Height = 9;
                pointBorder.StrokeThickness = 1.5;
                pointBorder.Stroke = new SolidColorBrush(Colors.Red);
                pointBorder.Margin = new Thickness(canvas.Width / 2 + step * p.X - 4.5, (canvas.Height / 2) - step * p.Y - 4.5, 0, 0);
                canvas.Children.Add(pointBorder);

                lblY.Content = p.Y;

                if (drawPolynom.IsChecked != true)
                    drawInterval(interval);
            }
            else
            {
                MessageBox.Show("Точка не належить ні одному з даних інтервалів!", "Інтерполяція", MessageBoxButton.OK, MessageBoxImage.Warning);
                lblY.Content = "Невизначеність";
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            deletePoint();
        }

        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            interpolation.HasTemporaryPoint = false;
            interpolation.Points.Clear();
            interpolation.Intervals.Clear();
            draw();
            lstPoints.Items.Refresh();
        }

        private void comboBoxMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (interpolation != null)
            {
                if (comboBoxMethod.SelectedIndex == 0)
                    interpolation = new Interpolation(interpolation.Points, new InterpolationLinearMethod());
                else
                    interpolation = new Interpolation(interpolation.Points, new InterpolationSquareMethod());

            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            interpolation.Serialize();
        }

        private void lstPoints_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                deletePoint();
            }
        }

        private void deletePoint()
        {
            if (lstPoints.SelectedIndex >= 0)
            {
                if (interpolation.HasTemporaryPoint)
                {
                    interpolation.Points.Remove(interpolation.Points.Last());
                    interpolation.HasTemporaryPoint = false;
                }
                if (lstPoints.SelectedIndex != -1)
                {
                    interpolation.Points.RemoveAt(lstPoints.SelectedIndex);
                }
                interpolation.Intervals = interpolation.Method.BuildIntervals(interpolation.Points);
                lstPoints.Items.Refresh();
                draw();
            }
        }

        private void drawPolynom_Checked(object sender, RoutedEventArgs e)
        {
            drawIntervals();
        }

        private void drawPolynom_Unchecked(object sender, RoutedEventArgs e)
        {
            draw();
        }
    }
}
