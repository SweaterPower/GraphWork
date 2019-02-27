﻿using System;
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
            CBSecond.ItemsSource = graphContainer.vertexes;
            graphContainer.AddVertex(2);
            graphContainer.AddEdge(1, 2, true);
            CBFirst.SelectedIndex = 0;
            CBSecond.SelectedIndex = 1;
        }

        private void AddVertex(object sender, RoutedEventArgs e)
        {
            graphContainer.AddVertex();
        }

        private void AddEdge(object sender, RoutedEventArgs e)
        {
            graphContainer.AddEdge((int)CBFirst.SelectedItem, (int)CBSecond.SelectedItem, hasArrow.IsChecked.Value);
            CBFirst.SelectedIndex = 0;
            CBSecond.SelectedIndex = 1;
        }
    }
}
