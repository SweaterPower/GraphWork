using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditGraph
{
    class Program
    {
        static void Test1()
        {
            GraphWirth g = new GraphWirth();
            Console.WriteLine(g.AddVertex(15));
            Console.WriteLine(g.AddVertex(20));
            Console.WriteLine(g.AddVertex(25));
            Console.WriteLine(g.AddVertex(30));
            Console.WriteLine(g.AddDirectEdge(1, 2, 21));
            Console.WriteLine(g.AddDirectEdge(1, 3, 22));
            Console.WriteLine(g.AddUndirectEdge(1, 4, 23));
            Console.WriteLine(g.AddDirectEdge(2, 3, 24));
            Console.WriteLine(g.AddUndirectEdge(3, 4, 25));
            Console.Write(g.ToString());
            Console.ReadKey();

            g.DeleteDirectEdge(2, 3);
            Console.Write(g.ToString());
            Console.ReadKey();

            g.DeleteUndirectEdge(3, 4);
            Console.Write(g.ToString());
            Console.ReadKey();

            g.DeleteUndirectEdge(4, 1);
            Console.Write(g.ToString());
            Console.ReadKey();

            g.DeleteVertex(1);
            Console.Write(g.ToString());
            Console.ReadKey();
        }

        static void Test2()
        {
            GraphWirth g = new GraphWirth();
            Console.WriteLine(g.AddVertex(15));
            Console.WriteLine(g.AddVertex(20));
            Console.WriteLine(g.AddVertex(25));
            Console.WriteLine(g.AddVertex(30));
            Console.WriteLine(g.AddDirectEdge(1, 2, 21));
            Console.WriteLine(g.AddDirectEdge(1, 3, 22));
            Console.WriteLine(g.AddUndirectEdge(1, 4, 23));
            Console.WriteLine(g.AddDirectEdge(2, 3, 24));
            Console.WriteLine(g.AddUndirectEdge(3, 4, 25));
            Console.WriteLine(g.AddDirectEdge(4, 2, 24));
            Console.WriteLine(g.AddDirectEdge(2, 5, 24));
            Console.ReadKey();
            Console.Write(g.ToString());
            Console.ReadKey();

            var a = g.ToAdjacencyMatrix();
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j< a.GetLength(1); j++)
                {
                    Console.Write("{0} ", a[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("<><><><><><><><><>");
            a = g.ToIncidenceMatrix();
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write("{0,2} ", a[i, j]);
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        static void Test3()
        {
            GraphWirth g = new GraphWirth();
            Console.WriteLine(g.AddVertex(15));
            Console.WriteLine(g.AddVertex(20));
            Console.WriteLine(g.AddVertex(25));
            Console.WriteLine(g.AddDirectEdge(1, 2, 21));
            Console.WriteLine(g.AddDirectEdge(2, 3, 22));
            Console.WriteLine(g.AddDirectEdge(3, 4, 23));
            Console.WriteLine(g.AddDirectEdge(4, 1, 23));
            Console.WriteLine(g.AddUndirectEdge(1, 3, 25));
            Console.WriteLine(g.AddUndirectEdge(2, 4, 24));
            Console.WriteLine(g.DeleteUndirectEdge(2, 4));
            Console.Write(g.ToString());

            Console.WriteLine("Incidence Matrix:");
            var a = g.ToIncidenceMatrix();
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write("{0,2} ", a[i, j]);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Adjacency Matrix:");
            a = g.ToAdjacencyMatrix();
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write("{0} ", a[i, j]);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Attainability Matrix:");
            a = g.ToAttainabilityMatrix();
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    Console.Write("{0} ", a[i, j]);
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Test3();
        }
    }
}
