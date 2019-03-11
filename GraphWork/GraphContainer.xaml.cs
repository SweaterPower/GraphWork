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
            mainCanvas.SizeChanged += mainCanvas_SizeChanged;
        }

        void AddVertexVisual(int key, int value)
        {
            Vertex v = new Vertex();
            v.PropertyChanged += Changed;
            mainCanvas.Children.Add(v);
            //UpdateIndexes();
            v.Info = "Vertex " + (key).ToString();
            v.Diameter = 60;
            v.Measure(mainCanvas.RenderSize);
            v.Arrange(new Rect(0, 0, mainCanvas.DesiredSize.Width, mainCanvas.DesiredSize.Height));
            vertexes.Add(new Tuple<int, Vertex>(key, v));
        }

        void AddEdgeVisual(int from, int to, bool direct)
        {
            var e = (edges.Keys.FirstOrDefault((a) => { return a.Item1 == from && a.Item2 == to; }));
            if (e == null)
            {
                var fromVertex = vertexes.FirstOrDefault((v) => { return v.Item1 == from; });
                var toVertex = vertexes.FirstOrDefault((v) => { return v.Item1 == to; });
                if (fromVertex != null && toVertex != null)
                {
                    EdgeContainer l = new EdgeContainer(fromVertex.Item2, toVertex.Item2, mainCanvas, direct);
                    mainCanvas.Children.Add(l);
                    edges.Add(new Tuple<int, int>(from, to), new Tuple<EdgeContainer, bool>(l, true));
                    edges.Add(new Tuple<int, int>(to, from), new Tuple<EdgeContainer, bool>(l, false));
                    UpdateSeparation();
                }
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
            var toremove = vertexes.FirstOrDefault((v) => { return v.Item1 == id; });
            if (toremove != null)
                vertexes.Remove(toremove);
        }

        GraphWirth graph;
        public ObservableCollection<Tuple<int, Vertex>> vertexes = new ObservableCollection<Tuple<int, Vertex>>();
        public Dictionary<Tuple<int, int>, Tuple<EdgeContainer, bool>> edges = new Dictionary<Tuple<int, int>, Tuple<EdgeContainer, bool>>();
        List<int> BFSlist = new List<int>();
        List<List<int>> levels = new List<List<int>>();

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
                AddVertexVisual(root.Key, root.Value);
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
            //UpdateBFSlist();
            UpdateVertexesLayout();
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
            mainCanvas.Children.Clear();
            foreach (var line in lines)
            {
                mainCanvas.Children.Add(line);
            }
            foreach (var point in points)
            {
                mainCanvas.Children.Add(point);
            }
        }

        List<int> LevelUp(ref Queue<VertexWirth> vertexes, ref List<int> visited)
        {
            List<int> ret = new List<int>();
            Queue<VertexWirth> newLevel = new Queue<VertexWirth>();

            while (vertexes.Count != 0)
            {
                var trail = vertexes.Dequeue().Trail;
                while (trail != null)
                {
                    if (!visited.Contains(trail.Id.Key))
                    {
                        newLevel.Enqueue(trail.Id);
                        ret.Add(trail.Id.Key);
                        visited.Add(trail.Id.Key);
                    }
                    trail = trail.Next;
                }
            }
            while (newLevel.Count != 0)
            {
                vertexes.Enqueue(newLevel.Dequeue());
            }
            return ret;
        }

        void UpdateVertexesLayout()
        {
            /*int count = vertexes.Count;
            int power = 1;
            int himan = 2;
            int grayskull = 2;
            while (himan < count)
            {
                himan *= grayskull;
                power++;
            }
            int rowCount = count / power;
            int colCount = power + count % power;//rowCount + count % power;
            double stepX = mainCanvas.ActualWidth / colCount;
            double stepY = mainCanvas.ActualHeight / rowCount;
            int rowIndex = 0;
            int colIndex = 0;
            foreach (var key in BFSlist)
            {
                var vertex = vertexes.FirstOrDefault((v) => { return v.Item1 == key; }).Item2;
                if (vertex != null)
                {
                    //vertex.X = rnd.NextDouble() * (mainCanvas.ActualWidth - 50);
                    //vertex.Y = rnd.NextDouble() * (mainCanvas.ActualHeight - 50);
                    vertex.X = (stepX * colIndex) + stepX / 4;
                    vertex.Y = (stepY * rowIndex) + stepY / 4;
                    colIndex++;
                    if (colIndex >= colCount)
                    {
                        colIndex = 0;
                        rowIndex++;
                    }
                }
            }*/
            foreach (var level in levels)
                level.Clear();
            levels.Clear();
            var root = graph.root;
            List<int> visited = new List<int>();
            Queue<VertexWirth> vertexes = new Queue<VertexWirth>();

            List<int> rootLevel = new List<int>();
            rootLevel.Add(root.Key);
            levels.Add(rootLevel);
            vertexes.Enqueue(root);
            visited.Add(root.Key);

            while (vertexes.Count != 0)
            {
                var level = LevelUp(ref vertexes, ref visited);
                levels.Add(level);
            }
            root = graph.root;
            List<int> lastLevel = new List<int>();
            while (root != null)
            {
                if (!visited.Contains(root.Key))
                {
                    var trail = root.Trail;
                    List<int> dom = null;
                    int batya = -1;
                    while (trail != null)
                    {
                        dom = levels.FirstOrDefault((l) => { return l.Contains(trail.Id.Key); });
                        if (dom != null)
                        {
                            batya = trail.Id.Key;
                            break;
                        }
                        trail = trail.Next;
                    }
                    if (dom != null)
                    {
                        dom.Insert(dom.IndexOf(batya), root.Key);
                    }
                    else
                    {
                        lastLevel.Add(root.Key);
                    }
                }
                root = root.Next;
            }
            if (lastLevel.Count != 0)
                levels.Add(lastLevel);

            bool even = true;
            double width = mainCanvas.ActualWidth;
            double stepY = mainCanvas.ActualHeight / levels.Count;
            double Y = stepY / 4;
            foreach (var level in levels)
            {
                double stepX = width / level.Count;
                double X = stepX / 3 * (even ? 2 : 1);
                foreach (var vertexKey in level)
                {
                    var vertexTuple = this.vertexes.FirstOrDefault((v) => { return v.Item1 == vertexKey; });
                    if (vertexTuple != null)
                    {
                        var vertex = vertexTuple.Item2;
                        vertex.X = X;
                        vertex.Y = Y;
                    }
                    X += stepX;
                }
                Y += stepY + stepY / 4 * (even ? 2 : 1);
                even = !even;
            }
        }

        //void UpdateIndexes()
        //{
        //    vertexes.Clear();
        //    int index = 1;
        //    for (int i = start; i < mainCanvas.Children.Count; i++)
        //    {
        //        vertexes.Add(index);
        //        index++;
        //    }
        //}

        void UpdateBFSlist()
        {
            BFSlist.Clear();
            List<int> visited = new List<int>();
            Queue<VertexWirth> tovisit = new Queue<VertexWirth>();

            VertexWirth vertex = graph.root;
            tovisit.Enqueue(vertex);
            while (tovisit.Count != 0)
            {
                vertex = tovisit.Dequeue();
                BFSlist.Add(vertex.Key);
                visited.Add(vertex.Key);
                EdgeWirth trail = vertex.Trail;
                while (trail != null)
                {
                    if (!visited.Contains(trail.Id.Key))
                        tovisit.Enqueue(trail.Id);

                    trail = trail.Next;
                }
            }
            vertex = graph.root;
            while (vertex != null)
            {
                if (!visited.Contains(vertex.Key))
                    BFSlist.Add(vertex.Key);
                vertex = vertex.Next;
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

        private void mainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.IsEmpty || e.NewSize == new Size())
            {
                e.Handled = true;
                return;
            }
            UpdateVertexesLayout();
        }
    }
}
