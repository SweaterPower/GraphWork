using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditGraph
{
    public class GraphWirth
    {
        public VertexWirth root;

        public VertexWirth Find(int key)
        {
            var vertex = root;
            while (vertex != null)
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
        int GetKey() => ++key;

        public bool AddEdge(int from, int to, int weight, bool direct)
        {
            if (direct)
                return AddDirectEdge(from, to, weight);
            else
                return AddUndirectEdge(from, to, weight);
        }

        public bool AddDirectEdge(int from, int to, int weight)
        {
            var f = Find(from);
            if (f == null) return false;

            var t = Find(to);
            if (t == null) return false;

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
            if (t.Trail == null)
                t.Trail = new EdgeWirth(f, weight, false);
            else
                t.Trail.GetEnd().Next = new EdgeWirth(f, weight, false);
            return true;
        }

        public bool AddVertex(int val, object data = null)
        {
            if (root != null)
                root.GetEnd().Next = new VertexWirth(GetKey(), val, data);
            else
                root = new VertexWirth(GetKey(), val, data);
            return true;
        }

        public bool AddVertex(int key, int val, object data = null)
        {
            var f = Find(key);
            if (f != null) return false;
            RenewKey(key);
            if (root != null)
                root.GetEnd().Next = new VertexWirth(key, val, data);
            else
                root = new VertexWirth(key, val, data);
            return true;
        }

        public bool DeleteDirectEdge(int from, int to)
        {
            var f = Find(from);
            if (f == null) return false;

            var t = Find(to);
            if (t == null) return false;

            return DeleteDirectEdge(f, t);
        }

        private bool DeleteDirectEdge(VertexWirth f, VertexWirth t)
        {
            EdgeWirth previous = null;
            var edge = f.Trail;
            while (edge != null)
            {
                if (edge.Id.Key == t.Key)
                {
                    if (!edge.Direct) return false;
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

            return DeleteUndirectEdge(f, t) && DeleteUndirectEdge(t, f);
        }

        private bool DeleteUndirectEdge(VertexWirth f, VertexWirth t)
        {
            EdgeWirth previous = null;
            var edge = f.Trail;
            while (edge != null)
            {
                if (edge.Id.Key == t.Key)
                {
                    if (edge.Direct) return false;
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
                    break;
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
                    if (edge.Direct) return false;
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

        public bool DeleteEdge(int from, int to)
        {
            var f = Find(from);
            if (f == null) return false;

            var t = Find(to);
            if (t == null) return false;

            var edge = f.Trail;
            while (edge != null)
            {
                if (edge.Id.Key == t.Key)
                    break;
                edge = edge.Next;
            }

            if (edge.Direct)
                return DeleteDirectEdge(f, t);
            else
                return DeleteUndirectEdge(f, t) && DeleteUndirectEdge(t, f);
        }

        public bool DeleteVertex(int key)
        {
            if (root == null) return false;

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
                                    DeleteDirectEdge(r.Key, f.Key);
                                }
                                else
                                {
                                    DeleteUndirectEdge(r.Key, f.Key);
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

        private void DFS(VertexWirth vertex, ref List<int> visited)
        {
            visited.Add(vertex.Key);
            var trail = vertex.Trail;
            while (trail != null)
            {
                if (!visited.Contains(trail.Id.Key))
                    DFS(trail.Id, ref visited);
                trail = trail.Next;
            }
        }

        public void DFS(int vertexID, ref List<int> visited)
        {
            var vertex = Find(vertexID);
            if (vertex != null)
                DFS(vertex, ref visited);
        }

        public int[,] Floid()
        {
            Dictionary<int, int> ids = new Dictionary<int, int>();
            var vertex = root;
            int index = 0;
            while (vertex != null)
            {
                ids.Add(vertex.Key, index);
                index++;
                vertex = vertex.Next;
            }
            int[,] a = new int[ids.Count, ids.Count];
            for (int i = 0; i < ids.Count; i++)
                for (int j = 0; j < ids.Count; j++)
                    a[i, j] = 99999;
            vertex = root;
            while (vertex != null)
            {
                var t = vertex.Trail;
                while (t != null)
                {
                    a[ids[vertex.Key], ids[t.Id.Key]] = t.Weight;
                    t = t.Next;
                }
                vertex = vertex.Next;
            }
            for (int i = 0; i < ids.Count; i++)
                for (int u = 0; u < ids.Count; u++)
                    for (int v = 0; v < ids.Count; v++)
                        a[u, v] = Math.Min(a[u, v], a[u, i] + a[i, v]);
            return a;
        }

        public List<int> PathsBetween(int from, int to)
        {
            List<int> ret = new List<int>();

            var f = Find(from);
            if (f == null) return ret;

            var t = Find(to);
            if (t == null) return ret;

            List<int> ids = new List<int>();
            var v = root;
            while (v != null)
            {
                ids.Add(v.Key);
                v = v.Next;
            }
            from = ids.IndexOf(from);
            to = ids.IndexOf(to);

            var M = this.ToAdjacencyMatrix2();
            int kMAX = 0;
            for (int i = 0; i < M.GetLength(0); i++)
                for (int j = 0; j < M.GetLength(1); j++)
                    if (M[i, j] > 0)
                    {
                        //M[i, j] = 1;
                        kMAX++;
                    }
            int[,] a = new int[M.GetLength(0), M.GetLength(1)];
            int[,] b = new int[M.GetLength(0), M.GetLength(1)];
            bool[] visited = new bool[M.GetLength(0)];
            Queue<int> Q = new Queue<int>();
            for (int i = 0; i < M.GetLength(0); i++)
                visited[i] = false;
            //DFS(M, from, ref visited, ref a);

            Q.Enqueue(from);
            visited[from] = true;

            while (Q.Count != 0)
            {
                int top = Q.Dequeue();
                if (top != to)
                    for (int i = 0; i < M.GetLength(0); ++i)
                    {
                        if (M[top, i] != 0 && (!visited[i]))
                        {
                            Q.Enqueue(i);
                            if (i != to)
                                visited[i] = true;
                            a[top, i] = 1;
                        }
                    }
            }

            Array.Copy(a, b, M.Length);
            ret.Add(b[from, to]);
            for (int i = 1; i <= kMAX; i++)
            {
                b = MultiplicationMatrixD(b, a);
                ret.Add(b[from, to]);
                //ret.Add(countPaths(M, M.GetLength(0), i));
            }

            return ret;
        }

        public int ShortestPathBetween(int from, int to)
        {
            var f = Find(from);
            if (f == null) return -1;

            var t = Find(to);
            if (t == null) return -1;

            List<int> ids = new List<int>();
            var v = root;
            while (v != null)
            {
                ids.Add(v.Key);
                v = v.Next;
            }
            from = ids.IndexOf(from);
            to = ids.IndexOf(to);

            return Floid()[from, to];
        }

        public int ProcMaxFlow(int s, int t, ref List<List<int>> chains, ref List<Tuple<int, int>> minCut)
        {
            var S = Find(s);
            if (S == null) return -1;

            var T = Find(t);
            if (T == null) return -1;

            List<int> ids = new List<int>();
            var v = root;
            while (v != null)
            {
                ids.Add(v.Key);
                v = v.Next;
            }
            s = ids.IndexOf(s);
            t = ids.IndexOf(t);

            var capacity = ToWeightMatrix();
            int flow = fordFulkerson(capacity, s, t, capacity.GetLength(0), ref chains, ref minCut);
            List<int> mins = new List<int>();
            capacity = ToWeightMatrix();
            foreach (var chain in chains.Skip(1))
            {
                int min = int.MaxValue;
                int last = chain[0];
                int ind = 0;
                for (int i = 1; i < chain.Count; i++)
                {
                    int tmp = capacity[last, chain[i]];
                    if (tmp < min)
                    {
                        min = tmp;
                        ind = i - 1;
                    }
                    last = chain[i];
                }
                if (minCut.FirstOrDefault((c) => { return c.Item1 == chain[ind + 1]; }) == null)
                {
                    mins.Add(min);
                    minCut.Add(new Tuple<int, int>(chain[ind], chain[ind + 1]));
                }
            }
            minCut.Add(new Tuple<int, int>(mins.Sum(), 0));
            return flow;
        }

        bool bfs(int[,] graph, int s, int t, ref int[] parent, int V) // поиск в ширину
        {
            bool[] visited = new bool[V];

            Queue<int> q = new Queue<int>();
            q.Enqueue(s);
            visited[s] = true;
            parent[s] = -1;

            while (q.Count != 0)
            {
                int u = q.Dequeue();

                for (int v = 0; v < V; v++)
                {
                    if (visited[v] == false && graph[u, v] > 0)
                    {
                        q.Enqueue(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }

            return (visited[t] == true);
        }


        int fordFulkerson(int[,] graph, int s, int t, int V, ref List<List<int>> chains, ref List<Tuple<int, int>> minCut) // алгоритм нахождения максимального потока
        {
            int u, v;
            int[] parent = new int[V];
            int max_flow = 0;
            List<int> chain2 = new List<int>();
            //List<int> mins = new List<int>();
            while (bfs(graph, s, t, ref parent, V))
            {
                List<int> chain = new List<int>();
                chain.Add(t);
                int path_flow = int.MaxValue;
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    path_flow = Math.Min(path_flow, graph[u, v]);
                    chain.Add(u);
                }

                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    graph[u, v] -= path_flow;
                }
                chain2.Add(path_flow);
                chain.Reverse();
                chains.Add(chain);
                max_flow += path_flow;

                //int min = int.MaxValue;
                //int last = chain[0];
                //int ind = 0;
                //for (int i = 1; i < chain.Count; i++)
                //{
                //    int tmp = graph[last, chain[i]];
                //    if (tmp > 0 && tmp < min)
                //    {
                //        min = tmp;
                //        ind = i-1;
                //    }
                //    last = chain[i];
                //}
                //mins.Add(min);
                //minCut.Add(new Tuple<int, int>(chain[ind], chain[ind + 1]));
            }
            chains.Insert(0, chain2);
            //minCut.Add(new Tuple<int, int>(mins.Sum(), 0));
            return max_flow;
        }

        //void DFS(int[,] a, int i, ref bool[] visited, ref int[,] b)
        //{
        //    int j;
        //    visited[i] = true;
        //    for (j = 0; j < a.GetLength(0); j++)
        //        if (!visited[j] && a[i, j] == 1)
        //        {
        //            b[i, j] = 1;
        //            DFS(a, j, ref visited, ref b);
        //        }
        //}

        //int countPaths(int[,] a, int n, int l)
        //{
        //    int paths = 0;
        //    int[,] b = new int[n, n];
        //    Array.Copy(a, b, a.Length);
        //    for (int i = 0; i < l - 2; i++)
        //        b = MultiplicationMatrixD(b, a);

        //    var c = MultiplicationMatrixD(b, a);
        //    int[,] x = new int[n, n];
        //    for (int i = 0; i < n; i++)
        //        for (int j = 0; j < n; j++)
        //            x[i, j] = c[i, j] - b[i, j];

        //    for (int i = 0; i < n; i++)
        //        for (int j = i + 1; j < n; j++)
        //            if (x[i, j] == 1)
        //                paths += 1;

        //    return paths;
        //}

        public int[,] ToWeightMatrix()
        {
            List<int> ids = new List<int>();
            var v = root;
            while (v != null)
            {
                ids.Add(v.Key);
                v = v.Next;
            }
            int[,] a = new int[ids.Count, ids.Count];
            v = root;
            while (v != null)
            {
                var t = v.Trail;
                while (t != null)
                {
                    a[ids.IndexOf(v.Key), ids.IndexOf(t.Id.Key)] = t.Weight;
                    t = t.Next;
                }
                v = v.Next;
            }
            return a;
        }

        public Tuple<int, int>[] ToAdjacencyList()
        {
            throw new NotImplementedException();
        }

        public int[,] ToAdjacencyMatrix()
        {
            List<int> ids = new List<int>();
            var v = root;
            while (v != null)
            {
                ids.Add(v.Key);
                v = v.Next;
            }
            int[,] a = new int[ids.Count, ids.Count];
            List<int> undirectEdgesF = new List<int>();
            List<int> undirectEdgesT = new List<int>();
            v = root;
            while (v != null)
            {
                var t = v.Trail;
                while (t != null)
                {
                    if (!t.Direct)
                        if (undirectEdgesF.Contains(t.Id.Key))
                            if (undirectEdgesT.Contains(v.Key))
                            {
                                t = t.Next;
                                continue;
                            }
                    if (!t.Direct)
                    {
                        undirectEdgesF.Add(v.Key);
                        undirectEdgesT.Add(t.Id.Key);
                    }
                    a[ids.IndexOf(v.Key), ids.IndexOf(t.Id.Key)]++;
                    t = t.Next;
                }
                v = v.Next;
            }
            return a;
        }

        public int[,] ToAdjacencyMatrix2()
        {
            List<int> ids = new List<int>();
            var v = root;
            while (v != null)
            {
                ids.Add(v.Key);
                v = v.Next;
            }
            int[,] a = new int[ids.Count, ids.Count];
            v = root;
            while (v != null)
            {
                var t = v.Trail;
                while (t != null)
                {
                    a[ids.IndexOf(v.Key), ids.IndexOf(t.Id.Key)]++;
                    t = t.Next;
                }
                v = v.Next;
            }
            return a;
        }

        public static int[,] MultiplicationMatrixD(int[,] a, int[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) { throw new Exception("Матрицы нельзя перемножить"); }

            int ma = a.GetLength(0);
            int mb = b.GetLength(0);
            int nb = b.GetLength(1);

            int[,] r = new int[ma, nb];

            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < nb; j++)
                {
                    r[i, j] = 0;
                    for (int k = 0; k < mb; k++)
                    {
                        r[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return r;
        }

        public void OutputMatrix(int[,] a)
        {
            Console.WriteLine("----------------------");
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write("{0} ", a[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("----------------------");
        }

        public int[,] ToAttainabilityMatrix()
        {
            var adj = this.ToAdjacencyMatrix();

            var adj2 = MultiplicationMatrixD(adj, adj);
            var adj3 = MultiplicationMatrixD(adj2, adj);
            var adj4 = MultiplicationMatrixD(adj3, adj);

            //OutputMatrix(adj);
            //OutputMatrix(adj2);
            //OutputMatrix(adj3);
            //OutputMatrix(adj4);

            for (int i = 0; i < adj.GetLength(0); i++)
            {
                for (int j = 0; j < adj.GetLength(1); j++)
                {
                    adj[i, j] += adj2[i, j] + adj3[i, j] + adj4[i, j];
                }
            }

            for (int i = 0; i < adj.GetLength(0); i++)
            {
                for (int j = 0; j < adj.GetLength(1); j++)
                {
                    adj[i, j] = adj[i, j] == 0 ? 0 : 1;
                }
            }

            return adj;
        }

        public int[,] ToIncidenceMatrix()
        {
            List<int> ids = new List<int>();
            int countV = 0;
            int countE = 0;
            var v = root;
            while (v != null)
            {
                countV++;
                ids.Add(v.Key);
                var t = v.Trail;
                while (t != null)
                {
                    countE++;
                    t = t.Next;
                }
                v = v.Next;
            }

            int[,] a = new int[countV, countE];
            int i = -1;
            int j = -1;
            v = root;
            while (v != null)
            {
                i++;
                var t = v.Trail;
                while (t != null)
                {
                    j++;
                    if (t.Direct)
                    {
                        a[i, j] = -1;
                        a[ids.IndexOf(t.Id.Key), j] = 1;
                    }
                    else
                    {
                        a[i, j] = 1;
                        a[ids.IndexOf(t.Id.Key), j] = 1;
                    }
                    t = t.Next;
                }
                v = v.Next;
            }
            return a;
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
