using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EditGraph;

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
            graph = new GraphWirth(1);
        }

        void AddVertexVisual(int value)
        {
            Vertex v = new Vertex();
            v.PropertyChanged += Changed;
            mainCanvas.Children.Add(v);
            UpdateIndexes();
            v.Info = "Vertex:" + Environment.NewLine + (vertexes.Count).ToString();
            v.Diameter = 60;
        }

        void AddEdgeVisual(int from, int to, bool direct)
        {
            var e = (edges.Keys.FirstOrDefault((a) => { return a.Item1 == from && a.Item2 == to; }));
            if (e == null)
            {
                EdgeContainer l = new EdgeContainer((Vertex)mainCanvas.Children[from + start - 1], (Vertex)mainCanvas.Children[to + start - 1], mainCanvas, direct);
                mainCanvas.Children.Add(l);
                edges.Add(new Tuple<int, int>(from, to), new Tuple<EdgeContainer, bool>(l, true));
                edges.Add(new Tuple<int, int>(to, from), new Tuple<EdgeContainer, bool>(l, false));
                UpdateSeparation();
            }
            else
            {
                var k = edges[e];
                if (k.Item2)
                {
                    k.Item1.AddFromTo(direct);
                }
                else
                {
                    k.Item1.AddToFrom(direct);
                }
            }
        }

        void DeleteEdge(int from, int to)
        {
            var e = (edges.Keys.FirstOrDefault((a) => { return a.Item1 == from && a.Item2 == to; }));
            if (e == null)
            {
                MessageBox.Show("No such edge...");
            }
            else
            {
                edges.Remove(e);
            }
        }

        void DeleteVertex(int id)
        {
            vertexes.Remove(id);
        }

        GraphWirth graph;
        public ObservableCollection<int> vertexes = new ObservableCollection<int>();
        public SortedDictionary<Tuple<int, int>, Tuple<EdgeContainer, bool>> edges = new SortedDictionary<Tuple<int, int>, Tuple<EdgeContainer, bool>>();
        int start = 0;

        public void AddVertex()
        {
            graph.AddVertex(vertexes.Count + 1);
            VisualiseGraph();
        }

        public void AddVertex(int value)
        {
            graph.AddVertex(value);
            VisualiseGraph();
        }

        public void AddEdge(int from, int to, bool direct)
        {
            if (direct)
                graph.AddDirectEdge(from, to, 0);
            else
                graph.AddUndirectEdge(from, to, 0);
            VisualiseGraph();
        }

        void VisualiseGraph()
        {
            mainCanvas.Children.Clear();
            vertexes.Clear();
            edges.Clear();
            var root = graph.root;
            while (root != null)
            {
                AddVertexVisual(root.Value);
                root = root.Next;
            }
            root = graph.root;
            while (root != null)
            {
                var trail = root.Trail;
                while (trail != null)
                {
                    AddEdgeVisual(root.Key, trail.Id.Key, trail.Direct);
                    trail = trail.Next;
                }
                root = root.Next;
            }
            UpdateSeparation();
        }

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
            Random rnd = new Random();
            mainCanvas.Children.Clear();
            foreach (var line in lines)
            {
                mainCanvas.Children.Add(line);
            }
            start = lines.Count;
            foreach (var point in points)
            {
                mainCanvas.Children.Add(point);
                var vertex = point as Vertex;
                if (vertex != null)
                {
                    vertex.X = rnd.NextDouble() * (mainCanvas.ActualWidth - 50);
                    vertex.Y = rnd.NextDouble() * (mainCanvas.ActualHeight - 50);
                }
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
                    case "X":
                        if (source.X > 0)
                        {
                            Canvas.SetLeft(source, source.X);
                        }
                        else
                        {
                            source.X = 0;
                        }
                        if (source.X + source.ActualWidth < mainCanvas.ActualWidth)
                        {
                            Canvas.SetLeft(source, source.X);
                        }
                        else
                        {
                            source.X = mainCanvas.ActualWidth - source.ActualWidth;
                        }
                        break;
                    case "Y":
                        if (source.Y > 0)
                        {
                            Canvas.SetTop(source, source.Y);
                        }
                        else
                        {
                            source.Y = 0;
                        }
                        if (source.Y + source.ActualHeight < mainCanvas.ActualHeight)
                        {
                            Canvas.SetTop(source, source.Y);
                        }
                        else
                        {
                            source.Y = mainCanvas.ActualHeight - source.ActualHeight;
                        }
                        break;
                }
        }
    }
}
