using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GraphWork
{
   /* class XConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double x1 = (double)values[0] + (double)values[2] / 2.0;
            double y1 = (double)values[1] + (double)values[3] / 2.0;
            double x2 = (double)values[4] + (double)values[6] / 2.0;
            double y2 = (double)values[5] + (double)values[7] / 2.0;

            y1 = (double)values[8] - y1;
            y2 = (double)values[8] - y2;

            double a = x2;
            double b = y2;
            double k = (y1 - y2) / (x2 - x1);
            double p = (x1 * y2 - x2 * y1) / (x2 - x1);
            double R = x2 - (double)values[4];

            //double X1 = ((2 * a - 2 * p * k - 2 * b * k) + Math.Sqrt((2 * p * k - 2 * a + 2 * b * k) * (2 * p * k - 2 * a + 2 * b * k)
            //    - 4 * (1 + k * k) * (a * a + p * p + 2 * b * p + b * b - R * R))) / (2 * (1 + k * k));
            //double X2 = ((2 * a - 2 * p * k - 2 * b * k) - Math.Sqrt((2 * p * k - 2 * a + 2 * b * k) * (2 * p * k - 2 * a + 2 * b * k)
            //    - 4 * (1 + k * k) * (a * a + p * p + 2 * b * p + b * b - R * R))) / (2 * (1 + k * k));

            double A = (1 + k * k);
            double B = (2 * p * k - 2 * a - 2 * b * k);
            double C = (a * a + p * p - 2 * b * p + b * b - R * R);

            double X1 = (-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A);
            double X2 = (-B - Math.Sqrt(B * B - 4 * A * C)) / (2 * A);

            if (Math.Abs(x1 - X1) <= Math.Abs(x1 - X2))
            {
                return X1;
            }
            else
            {
                return X2;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class YConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double x1 = (double)values[0] + (double)values[2] / 2.0;
            double y1 = (double)values[1] + (double)values[3] / 2.0;
            double x2 = (double)values[4] + (double)values[6] / 2.0;
            double y2 = (double)values[5] + (double)values[7] / 2.0;

            double a = x2;
            double b = y2;
            double k = (y1 - y2) / (x2 - x1);
            double p = (x1 * y2 - x2 * y1) / (x2 - x1);
            double R = x2 - (double)values[4];

            //double X1 = ((2 * a - 2 * p * k - 2 * b * k) + Math.Sqrt((2 * p * k - 2 * a + 2 * b * k) * (2 * p * k - 2 * a + 2 * b * k)
            //    - 4 * (1 + k * k) * (a * a + p * p + 2 * b * p + b * b - R * R))) / (2 * (1 + k * k));
            //double X2 = ((2 * a - 2 * p * k - 2 * b * k) - Math.Sqrt((2 * p * k - 2 * a + 2 * b * k) * (2 * p * k - 2 * a + 2 * b * k)
            //    - 4 * (1 + k * k) * (a * a + p * p + 2 * b * p + b * b - R * R))) / (2 * (1 + k * k));

            double A = (1 + k * k);
            double B = (2 * p * k - 2 * a - 2 * b * k);
            double C = (a * a + p * p - 2 * b * p + b * b - R * R);

            double X1 = (-B + Math.Sqrt(B * B - 4 * A * C)) / (2 * A);
            double X2 = (-B - Math.Sqrt(B * B - 4 * A * C)) / (2 * A);

            double Y1 = -1 * k * X1 - p;
            double Y2 = -1 * k * X2 - p;

            if (Math.Abs(y1 - Y1) <= Math.Abs(y1 - Y2))
            {
                return Y1;
            }
            else
            {
                return Y2;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    } */

    class CenterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)values[0] + ((double)values[1] / 2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class CenterConverter2 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)values[0] - ((double)values[1] / 2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class HalfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 2.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
