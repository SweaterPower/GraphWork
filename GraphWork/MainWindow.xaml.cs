using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace GraphWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CBFirst.ItemsSource = vertexes;
            CBSecond.ItemsSource = vertexes;
            AddVertex(this, new RoutedEventArgs());
            AddVertex(this, new RoutedEventArgs());
            CreateEdge(1, 2);
            //CreateEdge(1, 2);
            //CreateEdge(1, 2);
            //CreateEdge(1, 2);
            //CreateEdge(1, 2);
            UpdateSeparation();
        }

        private void AddVertex(object sender, RoutedEventArgs e)
        {
            Vertex v = new Vertex();
            v.PropertyChanged += Changed;
            mainCanvas.Children.Add(v);
            UpdateIndexes();
        }

        private void AddEdge(object sender, RoutedEventArgs e)
        {
            CreateEdge((int)CBFirst.SelectedItem, (int)CBSecond.SelectedItem);
            UpdateSeparation();
        }

        ObservableCollection<int> vertexes = new ObservableCollection<int>();
        int start = 0;

        void UpdateSeparation()
        {
            List<UIElement> lines = new List<UIElement>();
            List<UIElement> points = new List<UIElement>();
            foreach (var child in mainCanvas.Children)
            {
                var tmp = child as EdgeContainer;
                if (tmp == null)
                {
                    points.Add((UIElement)child);
                }
                else
                {
                    lines.Add((UIElement)child);
                }
            }
            mainCanvas.Children.Clear();
            foreach (var line in lines)
            {
                mainCanvas.Children.Add(line);
            }
            start = lines.Count;
            foreach (var point in points)
            {
                mainCanvas.Children.Add(point);
            }
            UpdateIndexes();
        }

        void UpdateIndexes()
        {
            vertexes.Clear();
            int index = 1;
            for (int i = start; i < mainCanvas.Children.Count; i++)
            {
                vertexes.Add(index);
                index++;
            }
        }

        //private void LUL(object sender, RoutedEventArgs e)
        //{
        //    CombinedGeometry g = new CombinedGeometry(GeometryCombineMode.Intersect, ((Edge)mainCanvas.Children[0]).RenderedGeometry, ((Vertex)mainCanvas.Children[1]).RenderGeom);
        //}

        void CreateVertex(int key, int value)
        {

        }

        void CreateVertex(int value)
        {

        }

        void CreateEdge(int from, int to)
        {
            from += start - 1;
            to += start - 1;
            EdgeContainer l = new EdgeContainer((Vertex)mainCanvas.Children[from], (Vertex)mainCanvas.Children[to]);
            mainCanvas.Children.Add(l);

            /*
            CenterConverter cc1 = new CenterConverter();
            MultiBinding b1 = new MultiBinding();
            b1.Converter = cc1;
            b1.Bindings.Add(new Binding() { Path = new PropertyPath("X"), Source = mainCanvas.Children[from] });
            b1.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualWidthProperty.Name), Source = mainCanvas.Children[from] });

            CenterConverter cc2 = new CenterConverter();
            MultiBinding b2 = new MultiBinding();
            b2.Converter = cc2;
            b2.Bindings.Add(new Binding() { Path = new PropertyPath("Y"), Source = mainCanvas.Children[from] });
            b2.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualHeightProperty.Name), Source = mainCanvas.Children[from] });

            CenterConverter cc3 = new CenterConverter();
            MultiBinding b3 = new MultiBinding();
            b3.Converter = cc3;
            b3.Bindings.Add(new Binding() { Path = new PropertyPath("X"), Source = mainCanvas.Children[to] });
            b3.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualHeightProperty.Name), Source = mainCanvas.Children[to] });

            CenterConverter cc4 = new CenterConverter();
            MultiBinding b4 = new MultiBinding();
            b4.Converter = cc4;
            b4.Bindings.Add(new Binding() { Path = new PropertyPath("Y"), Source = mainCanvas.Children[to] });
            b4.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualHeightProperty.Name), Source = mainCanvas.Children[to] });

            CenterConverter ccg = new CenterConverter();
            MultiBinding bg = new MultiBinding();
            bg.Converter = ccg;
            bg.Bindings.Add(new Binding() { Path = new PropertyPath(Canvas.MinWidthProperty), Source = mainCanvas });
            bg.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualHeightProperty.Name), Source = mainCanvas.Children[to] });

            mainCanvas.Children.Add(l);
            l.SetBinding(EdgeShape.X1Property, b1);
            l.SetBinding(EdgeShape.Y1Property, b2);
            l.SetBinding(EdgeShape.X2Property, b3);
            l.SetBinding(EdgeShape.Y2Property, b4);
            l.SetBinding(EdgeShape.GapProperty, bg);
            l.Stroke = Brushes.DarkOrange;
            l.HasArrow = true;
            l.StrokeThickness = 3;
            l.HeadWidth = 15;
            l.HeadHeight = 6;
            l.Curveture = linenum * 100;
            linenum *= -1;
            if (linenum <= 0)
                linenum--;*/
        }
        //int linenum = 0;
        public void Changed(object sender, PropertyChangedEventArgs e)
        {
            Vertex source = sender as Vertex;
            if (source != null)
                switch (e.PropertyName)
                {
                    case "X": Canvas.SetLeft(source, source.X); break;
                    case "Y": Canvas.SetTop(source, source.Y); break;
                }
        }
    }
}
