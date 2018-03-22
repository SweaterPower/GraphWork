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
{
    /// <summary>
    /// Логика взаимодействия для Vertex.xaml
    /// </summary>
    public partial class Vertex : UserControl, INotifyPropertyChanged
    {
        /*TODO:
         перетаскивание вершины [сделано]
         отображение свойств
         цвет заливки
         толщина границы (делать границу толще при выделении)
         собственно выделение (знаю одну фишку изи делается без форычей и фомичей)
             */

        public Vertex()
        {
            InitializeComponent();
            X = 10;
            Y = 10;
            OnXChanged += UpdateX;
            OnYChanged += UpdateY;
        }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty XProperty;
        public static readonly DependencyProperty YProperty;

        private static event ValidateValueCallback OnXChanged;
        private static event ValidateValueCallback OnYChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private static void XChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnXChanged?.Invoke(e.NewValue);
        }

        private static void YChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnYChanged?.Invoke(e.NewValue);
        }

        private bool UpdateX(object value)
        {
            //Canvas.SetLeft(this, (double)value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
            return true;
        }

        private bool UpdateY(object value)
        {
            //Canvas.SetTop(this, (double)value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
            return true;
        }

        static Vertex()
        {
            XProperty = DependencyProperty.Register("X", typeof(double), typeof(Vertex), new PropertyMetadata(10.0, new PropertyChangedCallback(XChanged)));
            YProperty = DependencyProperty.Register("Y", typeof(double), typeof(Vertex), new PropertyMetadata(10.0, new PropertyChangedCallback(YChanged)));
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            X += e.HorizontalChange;
            Y += e.VerticalChange;
        }
    }
}
