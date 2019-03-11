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
            graphContainer.AddVertex(2);
            graphContainer.AddEdge(1, 2, true);
            graphContainer.AddVertex(3);
            graphContainer.AddEdge(2, 3, true);
            graphContainer.AddVertex(4);
            graphContainer.AddEdge(3, 4, true);
            graphContainer.AddVertex(5);
            graphContainer.AddEdge(4, 5, true);
            graphContainer.AddVertex(6);
            graphContainer.AddEdge(5, 6, true);
            graphContainer.AddVertex(7);
            graphContainer.AddEdge(3, 7, true);
            graphContainer.AddVertex(8);
            graphContainer.AddEdge(3, 8, true);
            graphContainer.AddVertex(9);
            graphContainer.AddEdge(1, 9, true);
            CBFirst.SelectedIndex = 0;
            CBSecond.SelectedIndex = 1;
        }

        private void AddVertex(object sender, RoutedEventArgs e)
        {
            graphContainer.AddVertex();
        }

        private void AddEdge(object sender, RoutedEventArgs e)
        {
            var from = CBFirst.SelectedItem as Tuple<int, Vertex>;
            var to = CBSecond.SelectedItem as Tuple<int, Vertex>;
            //graphContainer.AddEdge((int)CBFirst.SelectedItem, (int)CBSecond.SelectedItem, hasArrow.IsChecked.Value);
            if (from != null && to != null)
                graphContainer.AddEdge(from.Item1, to.Item1, hasArrow.IsChecked.Value);
            CBFirst.SelectedIndex = 0;
            CBSecond.SelectedIndex = 1;
        }
    }
}
