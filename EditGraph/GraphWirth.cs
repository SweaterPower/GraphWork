using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditGraph
{
    class GraphWirth : IGraph
    {
        public VertexWirth root;

        VertexWirth Find(int key)
        {
            var vertex = root;
            while (vertex.Next != null)
            {
                if (vertex.Key == key)
                    return vertex;
                vertex = vertex.Next;
            }
            return null;
        }

        int key = 0;

        public GraphWirth()
        {
            root = new VertexWirth(GetKey(), 0);
        }

        public GraphWirth(int value)
        {
            root = new VertexWirth(GetKey(), value);
        }

        public GraphWirth(int key, int value)
        {
            root = new VertexWirth(key, value);
            RenewKey(key);
        }

        void RenewKey(int k)
        {
            if (k > key)
            {
                key = k;
            }
        }
        int GetKey()
        {
            key++;
            return key;
        }

        public bool AddDirectEdge(int from, int to, int weight)
        {
            var f = Find(from);
            if (f == null) return false;

            var t = Find(to);
            if (f == null) return false;

            if (f.Trail == null)
                f.Trail = new EdgeWirth(t, weight, true);
            else
                f.Trail.GetEnd().Next = new EdgeWirth(t, weight, true);
            return true;
        }

        public bool AddUndirectEdge(int from, int to, int weight)
        {
            var f = Find(from);
            if (f == null) return false;

            var t = Find(to);
            if (f == null) return false;

            if (f.Trail == null)
                f.Trail = new EdgeWirth(t, weight, false);
            else
                f.Trail.GetEnd().Next = new EdgeWirth(t, weight, false);
            return true;
        }

        public bool AddVertex(int val)
        {
            root.GetEnd().Next = new VertexWirth(GetKey(), val);
            return true;
        }

        public bool AddVertex(int key, int val)
        {
            var f = Find(key);
            if (f != null) return false;
            RenewKey(key);
            root.GetEnd().Next = new VertexWirth(key, val);
            return true;
        }

        public bool DeleteDirectEdge(int from, int to)
        {
            var f = Find(from);
            if (f == null) return false;

            var t = Find(to);
            if (t == null) return false;

            EdgeWirth previous = null;
            var edge = f.Trail;
            while (edge != null)
            {
                if (edge.Id.Key == t.Key)
                {
                    var next = edge.Next;
                    if (previous == null)
                    {
                        if (next == null)
                        {
                            f.Trail = null;
                        }
                        else
                        {
                            f.Trail = next;
                        }
                    }
                    else
                    {
                        if (next == null)
                        {
                            previous.Next = null;
                        }
                        else
                        {
                            previous.Next = next;
                        }
                    }
                    return true;
                }
                previous = edge;
                edge = edge.Next;
            }
            return false;
        }

        public bool DeleteUndirectEdge(int from, int to)
        {
            var f = Find(from);
            if (f == null) return false;

            var t = Find(to);
            if (f == null) return false;

            EdgeWirth previous = null;
            var edge = f.Trail;
            while (edge != null)
            {
                if (edge.Id.Key == t.Key)
                {
                    var next = edge.Next;
                    if (previous == null)
                    {
                        if (next == null)
                        {
                            f.Trail = null;
                        }
                        else
                        {
                            f.Trail = next;
                        }
                    }
                    else
                    {
                        if (next == null)
                        {
                            previous.Next = null;
                        }
                        else
                        {
                            previous.Next = next;
                        }
                    }
                    return true;
                }
                previous = edge;
                edge = edge.Next;
            }
            previous = null;
            edge = t.Trail;
            while (edge != null)
            {
                if (edge.Id.Key == f.Key)
                {
                    var next = edge.Next;
                    if (previous == null)
                    {
                        if (next == null)
                        {
                            t.Trail = null;
                        }
                        else
                        {
                            t.Trail = next;
                        }
                    }
                    else
                    {
                        if (next == null)
                        {
                            previous.Next = null;
                        }
                        else
                        {
                            previous.Next = next;
                        }
                    }
                    return true;
                }
                previous = edge;
                edge = edge.Next;
            }
            return false;
        }

        public bool DeleteVertex(int key)
        {
            var f = Find(key);//находим вершину, которую собираемся удалить
            if (f == null) return false;

            VertexWirth previous = null;//предыдущая вершина
            var edge = root;//переменная для перебора вершин
            while (edge != null)//ищем вершину в списке вершин
            {
                if (edge.Key == f.Key)//нашли
                {
                    var r = root;
                    while (r.Next != null)//теперь убираем все ребра, которые ведут в эту вершину
                    {
                            var tmp = r.Trail;//идем по следу
                            while (tmp != null)
                            {
                                if (tmp.Id.Key == f.Key)//нашли ребро в удаляемую вершину
                                {
                                    if (tmp.Direct)//го его удалим
                                    {
                                        DeleteDirectEdge(tmp.Id.Key, f.Key);
                                    }
                                    else
                                    {
                                        DeleteUndirectEdge(tmp.Id.Key, f.Key);
                                    }
                                }
                                tmp = tmp.Next;
                            }
                        r = r.Next;
                    }
                    var trail = f.Trail;
                    while (trail != null)//убираем ребра, ведущие из этой вершины (на всякий)
                    {
                        trail.Id = null;
                        var tmp = trail.Next;
                        trail.Next = null;
                        trail = tmp;
                    }
                    var next = edge.Next;//ну и наконец удаляем ссылку на вершину из списка вершин
                    if (previous == null)//образовавшуюся дырочку (если есть) заштопаем
                    {
                        root = root.Next;
                    }
                    else
                    {
                        if (next != null)
                        {
                            previous.Next = next;
                        }
                    }
                    return true;//вы восхитительны
                }
                previous = edge;
                edge = edge.Next;
            }
            return false;//как
        }

        public Tuple<int, int>[] ToAdjacencyList()
        {
            throw new NotImplementedException();
        }

        public int[][] ToAdjacencyMatrix()
        {
            throw new NotImplementedException();
        }

        public int[][] ToAttainabilityMatrix()
        {
            throw new NotImplementedException();
        }

        public int[][] ToIncidenceMatrix()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            string fin = "GraphWirth:\n";
            var v = root;
            while (v != null)
            {
                fin += "Vertex:   " + v.Key.ToString() + " | " + v.Value.ToString() + "\nTrail:\n\t";
                var t = v.Trail;
                while (t != null)
                {
                    fin += t.Id.Key.ToString() + " | " + (t.Direct ? "direct" : "undirect") + " | " + t.Weight.ToString() + "\n\t";
                    t = t.Next;
                }
                fin += "\n------------------------\n";
                v = v.Next;
            }
            return fin;
        }
    }
}
