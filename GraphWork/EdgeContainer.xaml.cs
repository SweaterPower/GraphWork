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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphWork
{
    public partial class EdgeContainer : UserControl
    {
        Vertex from;
        Vertex to;
        int linenum = 0;
        bool inverted = false;
        bool minus = false;

        public EdgeContainer(Vertex f, Vertex t)
        {
            InitializeComponent();
            from = f;
            to = t;
            AddEdge(f, t);
        }

        void AddEdge(Vertex from, Vertex to)
        {
            EdgeShape l = new EdgeShape();

            CenterConverter cc1 = new CenterConverter();
            MultiBinding b1 = new MultiBinding();
            b1.Converter = cc1;
            b1.Bindings.Add(new Binding() { Path = new PropertyPath("X"), Source = from });
            b1.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualWidthProperty.Name), Source = from });

            CenterConverter cc2 = new CenterConverter();
            MultiBinding b2 = new MultiBinding();
            b2.Converter = cc2;
            b2.Bindings.Add(new Binding() { Path = new PropertyPath("Y"), Source = from });
            b2.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualHeightProperty.Name), Source = from });

            CenterConverter cc3 = new CenterConverter();
            MultiBinding b3 = new MultiBinding();
            b3.Converter = cc3;
            b3.Bindings.Add(new Binding() { Path = new PropertyPath("X"), Source = to });
            b3.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualHeightProperty.Name), Source = to });

            CenterConverter cc4 = new CenterConverter();
            MultiBinding b4 = new MultiBinding();
            b4.Converter = cc4;
            b4.Bindings.Add(new Binding() { Path = new PropertyPath("Y"), Source = to });
            b4.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualHeightProperty.Name), Source = to });

            CenterConverter ccg = new CenterConverter();
            MultiBinding bg = new MultiBinding();
            bg.Converter = ccg;
            bg.Bindings.Add(new Binding() { Path = new PropertyPath(Canvas.MinWidthProperty), Source = mainCanvas });
            bg.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualHeightProperty.Name), Source = to });

            mainCanvas.Children.Add(l);
            l.SetBinding(EdgeShape.X1Property, b1);
            l.SetBinding(EdgeShape.Y1Property, b2);
            l.SetBinding(EdgeShape.X2Property, b3);
            l.SetBinding(EdgeShape.Y2Property, b4);
            l.SetBinding(EdgeShape.GapProperty, bg);
            l.HasArrow = true;
            l.StrokeThickness = 1.5;
            l.HeadWidth = 10;
            l.HeadHeight = 3;

            if (inverted)
            {
                l.Stroke = Brushes.DarkRed;
                if (minus)
                {
                    minus = false;
                    l.Curveture = -1 * linenum * 100;
                }
                else
                {
                    minus = true;
                    l.Curveture = linenum * 100;
                    linenum++;
                }
            }
            else
            {
                l.Stroke = Brushes.DarkGreen;
                if (minus)
                {
                    minus = false;
                    l.Curveture = linenum * 100;
                }
                else
                {
                    minus = true;
                    l.Curveture = -1 * linenum * 100;
                    linenum++;
                }
            }
        }

        public void Add(Vertex first, Vertex second)
        {
            AddEdge(first, second);
        }

        private void mainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                inverted = true;
                Add(to, from);
            }
            else
            {
                inverted = false;
                Add(from, to);
            }
        }
    }
}
