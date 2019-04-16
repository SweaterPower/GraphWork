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
            CBFirst.ItemsSource = graphContainer.vertexes;
            CBFirst.DisplayMemberPath = "Item2.Info";
            CBSecond.ItemsSource = graphContainer.vertexes;
            CBSecond.DisplayMemberPath = "Item2.Info";
            deleteVertexCB.ItemsSource = graphContainer.vertexes;
            deleteVertexCB.DisplayMemberPath = "Item2.Info";
            deleteEdgeCBFirst.ItemsSource = graphContainer.vertexes;
            deleteEdgeCBFirst.DisplayMemberPath = "Item2.Info";
            deleteEdgeCBSecond.ItemsSource = graphContainer.vertexes;
            deleteEdgeCBSecond.DisplayMemberPath = "Item2.Info";
            //graphContainer.AddVertex(2);
            //graphContainer.AddEdge(1, 2, true, 1);
            //graphContainer.AddVertex(3);
            //graphContainer.AddEdge(2, 3, true, 2);
            //graphContainer.AddVertex(4);
            //graphContainer.AddEdge(3, 4, true, 3);
            //graphContainer.AddVertex(5);
            //graphContainer.AddEdge(4, 5, true, 2);
            //graphContainer.AddVertex(6);
            //graphContainer.AddEdge(5, 6, true, 1);
            //graphContainer.AddVertex(7);
            //graphContainer.AddEdge(3, 7, true, 1);
            //graphContainer.AddVertex(8);
            //graphContainer.AddEdge(3, 8, true, 2);
            //graphContainer.AddVertex(9);
            //graphContainer.AddEdge(1, 9, true, 3);

            //graphContainer.AddVertex(2);
            //graphContainer.AddEdge(1, 2, false, 2);
            //graphContainer.AddVertex(3);
            //graphContainer.AddEdge(1, 3, false, 2);
            //graphContainer.AddVertex(4);
            //graphContainer.AddEdge(1, 4, false, 2);
            //graphContainer.AddVertex(5);
            //graphContainer.AddEdge(2, 5, false, 2);
            //graphContainer.AddVertex(6);
            //graphContainer.AddEdge(5, 6, false, 2);
            //graphContainer.AddVertex(7);
            //graphContainer.AddEdge(3, 7, false, 2);
            //graphContainer.AddVertex(8);
            //graphContainer.AddEdge(3, 8, false, 2);
            //graphContainer.AddVertex(9);
            //graphContainer.AddEdge(2, 9, false, 2);

            graphContainer.AddVertex(2);
            graphContainer.AddVertex(3);
            graphContainer.AddVertex(4);
            graphContainer.AddVertex(5);
            graphContainer.AddVertex(6);
            graphContainer.AddEdge(1, 2, true, 10);
            graphContainer.AddEdge(1, 3, true, 8);
            graphContainer.AddEdge(2, 3, true, 14);
            graphContainer.AddEdge(2, 4, true, 5);
            graphContainer.AddEdge(3, 5, true, 10);
            graphContainer.AddEdge(4, 5, true, 3);
            graphContainer.AddEdge(5, 6, true, 12);
            graphContainer.AddEdge(4, 6, true, 15);
            graphContainer.AddEdge(5, 2, true, 2);

            CBFirst.SelectedIndex = 0;
            CBSecond.SelectedIndex = 1;
        }

        private void AddVertex(object sender, RoutedEventArgs e)
        {
            if (TBValue.Text == "")
                graphContainer.AddVertex();
            else
                graphContainer.AddVertex(TBValue.Text);
        }

        private void AddEdge(object sender, RoutedEventArgs e)
        {
            var from = CBFirst.SelectedItem as Tuple<int, Vertex>;
            var to = CBSecond.SelectedItem as Tuple<int, Vertex>;
            int weight;
            bool b = int.TryParse(weightTB.Text, out weight);
            if (from != null && to != null)
                graphContainer.AddEdge(from.Item1, to.Item1, hasArrow.IsChecked.Value, b ? weight : 0);
            CBFirst.SelectedIndex = 0;
            CBSecond.SelectedIndex = 1;
        }

        private void DeleteVertex(object sender, RoutedEventArgs e)
        {
            var vertex = CBFirst.SelectedItem as Tuple<int, Vertex>;
            if (vertex != null)
                graphContainer.DeleteVertex(vertex.Item1);
        }

        private void DeleteEdge(object sender, RoutedEventArgs e)
        {
            var from = CBFirst.SelectedItem as Tuple<int, Vertex>;
            var to = CBSecond.SelectedItem as Tuple<int, Vertex>;
            if (from != null && to != null)
                graphContainer.DeleteEdge(from.Item1, to.Item1);
        }

        //Лабораторная 2 задание 3 (часть 1)
        public void RootsOfSpannigTrees(object sender, RoutedEventArgs e)
        {
            var roots = graphContainer.GetRootsOfSpannigTrees();
            spanRootsLB.ItemsSource = roots;
        }

        //Лабораторная 2 задание 1 (часть 2)
        public void BranchesOfLenght(object sender, RoutedEventArgs e)
        {
            var paths = graphContainer.GetPathsOfLength(int.Parse(searchIDTB.Text), int.Parse(lengthTB.Text));
            pathsLB.ItemsSource = paths;
        }

        //Лабораторная 3 задание 8
        public void NPereferial(object sender, RoutedEventArgs e)
        {
            var nper = graphContainer.GetNPereferial(int.Parse(nperIDTB.Text), int.Parse(nperLengthTB.Text));
            nperLB.ItemsSource = nper;
        }

        public void RunFloid(object sender, RoutedEventArgs e)
        {
            List<List<string>> lines = new List<List<string>>();
            var a = graphContainer.GetFloidMatrix();
            for (int i = 0; i < a.GetLength(0); i++)
            {
                List<string> line = new List<string>();
                for (int j = 0; j < a.GetLength(1); j++)
                    line.Add(a[i, j].ToString());
                lines.Add(line);
            }
            TextOutputWindow tow = new TextOutputWindow("Флойд", lines);
            tow.Show();
        }

        public void RunFindPaths(object sender, RoutedEventArgs e)
        {
            var list = graphContainer.GetPaths(int.Parse(pathFromTB.Text), int.Parse(pathToTB.Text), int.Parse(pathLngTB.Text));
            TextOutputWindow tow = new TextOutputWindow("Пути", list);
            tow.Show();
        }

        public void RunFindFlow(object sender, RoutedEventArgs e)
        {
            var list = graphContainer.GetFlow(int.Parse(flowFromTB.Text), int.Parse(flowToTB.Text));
            TextOutputWindow tow = new TextOutputWindow("Поток", list);
            tow.Show();
        }
    }
}
