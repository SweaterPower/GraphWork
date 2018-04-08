using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditGraph
{
    interface IGraph
    {
        bool AddVertex(int val); // key - автоинкремент
        bool AddVertex(int key, int val);
        bool DeleteVertex(int key);
        bool AddDirectEdge(int from, int to, int weight);
        bool AddUndirectEdge(int from, int to, int weight);
        bool DeleteDirectEdge(int from, int to);
        bool DeleteUndirectEdge(int from, int to);
        int[,] ToAdjacencyMatrix(); // строки/столбцы соответствуют вершинам      отсортированным по key
                                     // отсутствие ребра можно обозначать нулем
        int[,] ToIncidenceMatrix();
        // int[][] ToAccessibilityMatrix(); Accessibility не о том оказалась
        int[,] ToAttainabilityMatrix();
        Tuple<int, int>[] ToAdjacencyList();

    }
}
