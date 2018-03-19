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
    /// Логика взаимодействия для GraphContainer.xaml
    /// </summary>
    public partial class GraphContainer : UserControl
    {
        /*TODO: (перенести функционал из кода главного окна)
         отображение графа
         выбор вершин и связей
         увеличение части изображения? (нинад)
         автоматическая расстановка вершин
         */
        public GraphContainer()
        {
            InitializeComponent();
        }

        public void AddVertex()
        {
            Vertex v = new Vertex();
            v.PropertyChanged += Changed;
            mainCanvas.Children.Add(v);
            UpdateIndexes();
        }

        public void AddEdge(int from, int to)
        {
            from += start - 1;
            to += start - 1;
            EdgeContainer l = new EdgeContainer((Vertex)mainCanvas.Children[from], (Vertex)mainCanvas.Children[to]);
            mainCanvas.Children.Add(l);
            UpdateSeparation();
        }

        public ObservableCollection<int> vertexes = new ObservableCollection<int>();
        public ObservableCollection<int> edges = new ObservableCollection<int>();
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
