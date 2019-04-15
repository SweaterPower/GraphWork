using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace GraphWork
{
    /// <summary>
    /// Логика взаимодействия для TextOutputWindow.xaml
    /// </summary>
    public partial class TextOutputWindow : Window
    {
        public TextOutputWindow(string title, List<List<string>> lines)
        {
            InitializeComponent();
            Title = title;
            int spacing = 0;
            foreach (var line in lines)
                foreach (var val in line)
                {
                    int tmp = val.Length;
                    if (tmp > spacing)
                        spacing = tmp;
                }
            spacing += 5;
            string format = "{0,-" + spacing + "}";
            int rowIndex = 1;
            int colIndex = 1;
            colsTB.Text += Environment.NewLine;
            foreach (var val in lines[0])
            {
                colsTB.Text += String.Format(format, colIndex);
                colIndex++;
            }
            foreach (var line in lines)
            {
                rowsTB.Text += "     " + rowIndex.ToString() + Environment.NewLine;
                rowIndex++;
                foreach (var val in line)
                {
                    outTB.Text += String.Format(format, val);
                }
                outTB.Text += Environment.NewLine;
            }

        }

        public TextOutputWindow(string title, List<string> lines)
        {
            InitializeComponent();
            Title = title;
            foreach (var line in lines)
            {
                outTB.Text +=line;
                outTB.Text += Environment.NewLine;
            }
        }
    }
}