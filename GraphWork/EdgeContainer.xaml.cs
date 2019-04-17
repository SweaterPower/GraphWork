using System;
using System.Collections.Generic;
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
{ //RESET CURVETURE ON DELET
    public partial class EdgeContainer : UserControl
    {
        public Vertex from;
        public Vertex to;
        public bool direct;
        public int weight;
        Canvas parentCanvas;
        GraphContainer parent;
        int linenum = 0;
        bool inverted = false;
        bool minus = false;

        public EdgeContainer(Vertex f, Vertex t, Canvas pc, GraphContainer p, bool d, int w = 0)
        {
            InitializeComponent();
            parent = p;
            parentCanvas = pc;
            from = f;
            to = t;
            direct = d;
            weight = w;
            AddEdge(f, t, d, w);
        }

        void AddEdge(Vertex from, Vertex to, bool direct, int weight = 0)
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
            b3.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualWidthProperty.Name), Source = to });

            CenterConverter cc4 = new CenterConverter();
            MultiBinding b4 = new MultiBinding();
            b4.Converter = cc4;
            b4.Bindings.Add(new Binding() { Path = new PropertyPath("Y"), Source = to });
            b4.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualHeightProperty.Name), Source = to });

            HalfConverter ccg = new HalfConverter();
            Binding bg = new Binding();
            bg.Converter = ccg;
            bg.Path = new PropertyPath(Vertex.ActualWidthProperty);
            bg.Source = to;

            parentCanvas.Children.Add(l);
            l.SetBinding(EdgeShape.X1Property, b1);
            l.SetBinding(EdgeShape.Y1Property, b2);
            l.SetBinding(EdgeShape.X2Property, b3);
            l.SetBinding(EdgeShape.Y2Property, b4);
            l.SetBinding(EdgeShape.GapProperty, bg);
            l.HasArrow = true;
            l.StrokeThickness = 1.5;
            l.HeadWidth = 10;
            l.HeadHeight = 3;
            l.HasArrow = direct;
            l.MouseDown += this.MyMouseDown;

            //Binding wi = new Binding();
            //wi.Path = new PropertyPath(EdgeShape.ActualWidthProperty);
            //wi.Source = l;
            //parentCanvas.SetBinding(Canvas.WidthProperty, wi);
            //Binding he = new Binding();
            //he.Path = new PropertyPath(EdgeShape.ActualHeightProperty);
            //he.Source = l;
            //parentCanvas.SetBinding(Canvas.HeightProperty, he);

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

            EdgeInfo t = new EdgeInfo();
            t.MouseDown += this.MyMouseDown;

            CenterConverter2 cc5 = new CenterConverter2();
            MultiBinding b5 = new MultiBinding();
            b5.Converter = cc5;
            b5.Bindings.Add(new Binding() { Path = new PropertyPath(EdgeShape.TopXProperty.Name), Source = l });
            b5.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualWidthProperty.Name), Source = t });

            CenterConverter2 cc6 = new CenterConverter2();
            MultiBinding b6 = new MultiBinding();
            b6.Converter = cc6;
            b6.Bindings.Add(new Binding() { Path = new PropertyPath(EdgeShape.TopYProperty.Name), Source = l });
            b6.Bindings.Add(new Binding() { Path = new PropertyPath(Vertex.ActualHeightProperty.Name), Source = t });

            //Binding tbX = new Binding();
            //tbX.Source = l;
            //tbX.Path = new PropertyPath(EdgeShape.TopXProperty);
            //Binding tbY = new Binding();
            //tbY.Source = l;
            //tbY.Path = new PropertyPath(EdgeShape.TopYProperty);

            t.SetBinding(EdgeInfo.eiXProperty, b5);
            t.SetBinding(EdgeInfo.eiYProperty, b6);
            t.PropertyChanged += Changed;
            parentCanvas.Children.Add(t);
            t.Txt = weight.ToString();
        }

        public void AddFromTo(bool direct, int weight = 0)
        {
            inverted = false;
            AddEdge(from, to, direct, weight);
        }

        public void AddToFrom(bool direct, int weight = 0)
        {
            inverted = true;
            AddEdge(to, from, direct, weight);
        }

        public void Changed(object sender, PropertyChangedEventArgs e)
        {
            EdgeInfo source = sender as EdgeInfo;
            if (source != null)
                switch (e.PropertyName)
                {
                    case "tbX": Canvas.SetLeft(source, source.tbX); break;
                    case "tbY": Canvas.SetTop(source, source.tbY); break;
                }
        }

        private void MyMouseDown(object sender, MouseButtonEventArgs e)
        {
            parent.MyMouseDown(this, e.LeftButton);
        }

        //private void mainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        inverted = true;
        //        Add(to, from);
        //    }
        //    else
        //    {
        //        inverted = false;
        //        Add(from, to);
        //    }
        //}
    }
}
