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
    /// Логика взаимодействия для EdgeInfo.xaml
    /// </summary>
    public partial class EdgeInfo : UserControl, INotifyPropertyChanged
    {
        public EdgeInfo()
        {
            InitializeComponent();
            OnXChanged += UpdateX;
            OnYChanged += UpdateY;
        }

        string text = "";
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public double tbX
        {
            get { return (double)GetValue(eiXProperty); }
            set { SetValue(eiXProperty, value); }
        }

        public double tbY
        {
            get { return (double)GetValue(eiYProperty); }
            set { SetValue(eiYProperty, value); }
        }

        public static readonly DependencyProperty eiXProperty;
        public static readonly DependencyProperty eiYProperty;

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("tbX"));
            return true;
        }

        private bool UpdateY(object value)
        {
            //Canvas.SetTop(this, (double)value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("tbY"));
            return true;
        }

        static EdgeInfo()
        {
            eiXProperty = DependencyProperty.Register("tbX", typeof(double), typeof(EdgeInfo), new PropertyMetadata(10.0, new PropertyChangedCallback(XChanged)));
            eiYProperty = DependencyProperty.Register("tbY", typeof(double), typeof(EdgeInfo), new PropertyMetadata(10.0, new PropertyChangedCallback(YChanged)));
        }
    }
}
