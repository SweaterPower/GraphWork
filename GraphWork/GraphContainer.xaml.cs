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

        void AddVertexVisual(int key, int value, object data = null)
        {
            Vertex v = new Vertex();
            v.PropertyChanged += Changed;
            mainCanvas.Children.Add(v);
            //UpdateIndexes();
            var s = data as string;
            if (s == null)
                v.Info = "Vertex " + (key).ToString();
            else
                v.Info = s + " (" + value + ")";
            v.Diameter = 60;
            v.Measure(mainCanvas.RenderSize);
            v.Arrange(new Rect(0, 0, mainCanvas.DesiredSize.Width, mainCanvas.DesiredSize.Height));
            vertexes.Add(new Tuple<int, Vertex>(key, v));
        }

        void AddEdgeVisual(int from, int to, bool direct, int weight = 0)
        {
            var e = (edges.Keys.FirstOrDefault((a) => { return a.Item1 == from && a.Item2 == to; }));
            if (e == null)
            {
                var fromVertex = vertexes.FirstOrDefault((v) => { return v.Item1 == from; });
                var toVertex = vertexes.FirstOrDefault((v) => { return v.Item1 == to; });
                if (fromVertex != null && toVertex != null)
                {
                    EdgeContainer l = new EdgeContainer(fromVertex.Item2, toVertex.Item2, mainCanvas, direct, weight);
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
                    k.Item1.AddFromTo(direct, weight);
                }
                else
                {
                    k.Item1.AddToFrom(direct, weight);
                }
            }
        }

        public void DeleteEdge(int from, int to)
        {
            graph.DeleteEdge(from, to);
            VisualiseGraph();
        }

        public void DeleteVertex(int id)
        {
            graph.DeleteVertex(id);
            VisualiseGraph();
        }

        GraphWirth graph;
        public ObservableCollection<Tuple<int, Vertex>> vertexes = new ObservableCollection<Tuple<int, Vertex>>();
        public Dictionary<Tuple<int, int>, Tuple<EdgeContainer, bool>> edges = new Dictionary<Tuple<int, int>, Tuple<EdgeContainer, bool>>();
        //List<int> BFSlist = new List<int>();
        List<List<int>> levels = new List<List<int>>();

        public void AddVertex(string info = "")
        {
            graph.AddVertex(vertexes.Count + 1, info);
            VisualiseGraph();
        }

        public void AddVertex(int value, string info = "")
        {
            graph.AddVertex(value, info);
            VisualiseGraph();
        }

        public void AddEdge(int from, int to, bool direct, int weight = 0)
        {
            graph.AddEdge(from, to, weight, direct);
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
            List<int> undirectEdgesF = new List<int>();
            List<int> undirectEdgesT = new List<int>();
            root = graph.root;
            while (root != null)
            {
                var trail = root.Trail;
                while (trail != null)
                {
                    if (trail.Direct)
                        AddEdgeVisual(root.Key, trail.Id.Key, trail.Direct, trail.Weight);
                    else
                    {
                        if (!undirectEdgesF.Contains(trail.Id.Key))
                            if (!undirectEdgesT.Contains(root.Key))
                            {
                                AddEdgeVisual(root.Key, trail.Id.Key, trail.Direct, trail.Weight);
                                undirectEdgesF.Add(root.Key);
                                undirectEdgesT.Add(trail.Id.Key);
                            }
                    }
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

        //void UpdateBFSlist()
        //{
        //    BFSlist.Clear();
        //    List<int> visited = new List<int>();
        //    Queue<VertexWirth> tovisit = new Queue<VertexWirth>();

        //    VertexWirth vertex = graph.root;
        //    tovisit.Enqueue(vertex);
        //    while (tovisit.Count != 0)
        //    {
        //        vertex = tovisit.Dequeue();
        //        BFSlist.Add(vertex.Key);
        //        visited.Add(vertex.Key);
        //        EdgeWirth trail = vertex.Trail;
        //        while (trail != null)
        //        {
        //            if (!visited.Contains(trail.Id.Key))
        //                tovisit.Enqueue(trail.Id);

        //            trail = trail.Next;
        //        }
        //    }
        //    vertex = graph.root;
        //    while (vertex != null)
        //    {
        //        if (!visited.Contains(vertex.Key))
        //            BFSlist.Add(vertex.Key);
        //        vertex = vertex.Next;
        //    }
        //}

        public List<string> GetRootsOfSpannigTrees()
        {
            List<string> ret = new List<string>();
            List<int> res = new List<int>();
            List<int> visited = new List<int>();

            var ids = vertexes.Select((a) => { return a.Item1; }).ToList();
            foreach (var id in ids)
            {
                graph.DFS(id, ref visited);
                if (visited.Count == ids.Count)
                    res.Add(id);
                visited.Clear();
            }
            foreach (var id in res)
            {
                var item = vertexes.FirstOrDefault((a) => { return a.Item1 == id; });
                if (item != null)
                    ret.Add(item.Item2.Info + " (" + item.Item1 + ")");
            }

            return ret;
        }

        public List<string> GetPathsOfLength(int root, int length)
        {
            List<string> ret = new List<string>();
            List<int> res = new List<int>();
            List<int> visited = new List<int>();
            Queue<VertexWirth> tovisit = new Queue<VertexWirth>();
            Queue<int> paths = new Queue<int>();

            VertexWirth vertex = graph.Find(root);
            if (vertex != null)
            {
                tovisit.Enqueue(vertex);
                paths.Enqueue(0);
                while (tovisit.Count != 0)
                {
                    vertex = tovisit.Dequeue();
                    visited.Add(vertex.Key);
                    int path = paths.Dequeue();
                    if (path == length)
                        res.Add(vertex.Key);
                    EdgeWirth trail = vertex.Trail;
                    while (trail != null)
                    {
                        if (!visited.Contains(trail.Id.Key))
                        {
                            tovisit.Enqueue(trail.Id);
                            paths.Enqueue(path + trail.Weight);
                        }

                        trail = trail.Next;
                    }
                }

                foreach (var id in res)
                {
                    var item = vertexes.FirstOrDefault((a) => { return a.Item1 == id; });
                    if (item != null)
                        ret.Add(item.Item2.Info + " (" + item.Item1 + ")");
                }
            }

            return ret;
        }

        public List<string> GetNPereferial(int root, int length)
        {
            List<string> ret = new List<string>();
            List<int> res = new List<int>();
            List<int> visited = new List<int>();
            Queue<VertexWirth> tovisit = new Queue<VertexWirth>();
            Queue<int> paths = new Queue<int>();

            VertexWirth vertex = graph.Find(root);
            if (vertex != null)
            {
                tovisit.Enqueue(vertex);
                paths.Enqueue(0);
                while (tovisit.Count != 0)
                {
                    vertex = tovisit.Dequeue();
                    visited.Add(vertex.Key);
                    int path = paths.Dequeue();
                    if (path > length)
                        res.Add(vertex.Key);
                    EdgeWirth trail = vertex.Trail;
                    while (trail != null)
                    {
                        if (!visited.Contains(trail.Id.Key))
                        {
                            tovisit.Enqueue(trail.Id);
                            paths.Enqueue(path + trail.Weight);
                        }

                        trail = trail.Next;
                    }
                }

                foreach (var id in res)
                {
                    var item = vertexes.FirstOrDefault((a) => { return a.Item1 == id; });
                    if (item != null)
                        ret.Add(item.Item2.Info + " (" + item.Item1 + ")");
                }
            }

            return ret;
        }

        public int[,] GetFloidMatrix()
        {
            return graph.Floid();
        }

        public List<string> GetPaths(int from, int to, int length)
        {
            List<string> ret = new List<string>();
            var path = graph.PathsBetween(from, to);
            ret.Add("Кол-во путей длины " + length + ": " + path[length - 1]);
            ret.Add("Общее кол-во путей: " + path.Sum());
            ret.Add("Длина кратчайшего пути: " + graph.ShortestPathBetween(from, to));
            return ret;
        }

        public List<string> GetFlow(int from, int to)
        {
            List<string> ret = new List<string>();
            List <List<int>> chains = new List<List<int>>();
            var flow = graph.ProcMaxFlow(from, to, ref chains);
            ret.Add(flow.ToString());
            foreach (var chain in chains)
            {
                string s = "";
                foreach (var val in chain)
                {
                    s += (val+1).ToString() + " ";
                }
                ret.Add(s);
            }
            return ret;
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
