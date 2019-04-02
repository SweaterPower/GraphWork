using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphWork
{
    class EdgeShape : Shape
    {
        /*TODO:
         цвет линии
         толщина линии
         наличие стрелки [сделано]
         отображение свойств
         толщина границы (увеличивать при выделении)
         выделение
             */

        #region Dependency Properties

        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(EdgeShape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(EdgeShape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(EdgeShape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(EdgeShape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HeadWidthProperty = DependencyProperty.Register("HeadWidth", typeof(double), typeof(EdgeShape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HeadHeightProperty = DependencyProperty.Register("HeadHeight", typeof(double), typeof(EdgeShape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty HasArrowProperty = DependencyProperty.Register("HasArrow", typeof(bool), typeof(EdgeShape), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public static readonly DependencyProperty GapProperty = DependencyProperty.Register("Gap", typeof(double), typeof(EdgeShape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty CurvetureProperty = DependencyProperty.Register("Curveture", typeof(double), typeof(EdgeShape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty TopXProperty = DependencyProperty.Register("TopX", typeof(double), typeof(EdgeShape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty TopYProperty = DependencyProperty.Register("TopY", typeof(double), typeof(EdgeShape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

        #region CLR Properties

        [TypeConverter(typeof(LengthConverter))]
        public double X1
        {
            get { return (double)base.GetValue(X1Property); }
            set { base.SetValue(X1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y1
        {
            get { return (double)base.GetValue(Y1Property); }
            set { base.SetValue(Y1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double X2
        {
            get { return (double)base.GetValue(X2Property); }
            set { base.SetValue(X2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y2
        {
            get { return (double)base.GetValue(Y2Property); }
            set { base.SetValue(Y2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double HeadWidth
        {
            get { return (double)base.GetValue(HeadWidthProperty); }
            set { base.SetValue(HeadWidthProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double HeadHeight
        {
            get { return (double)base.GetValue(HeadHeightProperty); }
            set { base.SetValue(HeadHeightProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Gap
        {
            get { return (double)base.GetValue(GapProperty); }
            set { base.SetValue(GapProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Curveture
        {
            get { return (double)base.GetValue(CurvetureProperty); }
            set { base.SetValue(CurvetureProperty, value); }
        }

        public bool HasArrow
        {
            get { return (bool)base.GetValue(HasArrowProperty); }
            set { base.SetValue(HasArrowProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double TopX
        {
            get { return (double)base.GetValue(TopXProperty); }
            set { base.SetValue(TopXProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double TopY
        {
            get { return (double)base.GetValue(TopYProperty); }
            set { base.SetValue(TopYProperty, value); }
        }
        #endregion

        #region Overrides

        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    InternalDrawArrowGeometry(context);
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();

                return geometry;
            }
        }

        #endregion

        #region Privates

        private void InternalDrawArrowGeometry(StreamGeometryContext context)
        {
            //double k = (-1 * (Y1 - Y2)) / (X2 - X1);
            //double a = Math.Atan(k);
            //if (X1 > X2)
            //{
            //    a = Math.PI - a;
            //    a *= -1;
            //}

            //Point pt1 = new Point(X1, this.Y1);
            //Point pt2 = new Point(X2, this.Y2);

            //double height = Math.Sqrt((pt2.X - pt1.X) * (pt2.X - pt1.X) + (pt2.Y - pt1.Y) * (pt2.Y - pt1.Y));
            //double width = Math.Abs(Curveture) * 2.5 + height / 10;

            //double Ax = (X1 + X2) / 2;
            //double Ay = (Y1 + Y2) / 2;
            //double b = Math.Atan(-1 / (-1 * (Y1 - Y2) / (X2 - X1)));
            //if (Y1 >= Ay)
            //{
            //    b = Math.PI - b;
            //    b *= -1;
            //}
            //double modifier = Curveture * 0.2;
            //double Bx = X1 + modifier * Math.Cos(b);
            //double By = Y1 + modifier * Math.Sin(b);
            //TopX = Ax + modifier * Math.Cos(b);
            //TopY = Ay + modifier * Math.Sin(b);

            ////double length = Math.Sqrt((Bx - X2) * (Bx - X2) + (By - Y2) * (By - Y2));
            //Point ptGap = new Point(X2 - (Gap + 3) * Math.Cos(a), Y2 - (Gap + 3) * Math.Sin(a));
            //ptGap = new Point(ptGap.X + modifier * Math.Cos(b), ptGap.Y + modifier * Math.Sin(b));

            //Point ptEnd = new Point(X2 - (Gap + 23) * Math.Cos(a), Y2 - (Gap + 23) * Math.Sin(a));
            //ptEnd = new Point(ptGap.X + modifier * Math.Cos(b), ptGap.Y + modifier * Math.Sin(b));

            //double theta = Math.Atan2(By - ptGap.Y, Bx - ptGap.X);
            //double sint = Math.Sin(theta);
            //double cost = Math.Cos(theta);

            //Point pt3 = new Point(
            //    ptGap.X + (HeadWidth * cost - HeadHeight * sint),
            //    ptGap.Y + (HeadWidth * sint + HeadHeight * cost));

            //Point pt4 = new Point(
            //    ptGap.X + (HeadWidth * cost + HeadHeight * sint),
            //    ptGap.Y - (HeadHeight * cost - HeadWidth * sint));

            //context.BeginFigure(pt1, true, false);
            //context.LineTo(new Point(Bx, By), true, true);
            //////context.LineTo(ptGap, true, true);
            ////context.LineTo(new Point(Bx, By), true, true);
            ////context.LineTo(new Point(Ax, Ay), true, true);
            ////context.LineTo(pt1, true, true);
            ////context.ArcTo(ptEnd, new Size(height, width), a / Math.PI * 180, false, Curveture > 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true, true);
            //context.LineTo(ptGap, true, true);
            //if (HasArrow)
            //{
            //    context.LineTo(pt3, true, true);
            //    context.LineTo(ptGap, true, true);
            //    context.LineTo(pt4, true, true);
            //    context.LineTo(ptGap, true, true);
            //}
            //context.LineTo(pt2, true, true);
            /*Point pt1 = new Point(X1, this.Y1);
            Point pt2 = new Point(X2, this.Y2);

            double k = (-1 * (Y1 - Y2)) / (X2 - X1);
            double a = Math.Atan(k);
            if (X1 > X2)
            {
                a = Math.PI - a;
                a *= -1;
            }

            double Ax = (X1 + X2) / 2;
            double Ay = (Y1 + Y2) / 2;
            double b = Math.Atan(-1 / (-1 * (Y1 - Y2) / (X2 - X1)));
            if (Y1 >= Ay)
            {
                b = Math.PI - b;
                b *= -1;
            }

            double modifier = Curveture * 0.2;
            double Bx = X1 + modifier * Math.Cos(b);
            double By = Y1 + modifier * Math.Sin(b);

            double gapx = X2 - (Gap + 3) * Math.Cos(a);
            double gapy = Y2 - (Gap + 3) * Math.Sin(a);
            Point ptGap = new Point(gapx + modifier * Math.Cos(b), gapy + modifier * Math.Sin(b));

            k = (-1 * (Y2 - ptGap.Y)) / (X2 - ptGap.X);
            double theta = Math.Atan(k);
            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);

            double height = Math.Sqrt((pt2.X - pt1.X) * (pt2.X - pt1.X) + (pt2.Y - pt1.Y) * (pt2.Y - pt1.Y));
            double width = Math.Abs(Curveture) * 2.5 + height / 10;

            Point pt3 = new Point(
                ptGap.X + (HeadWidth * cost - HeadHeight * sint),
                ptGap.Y + (HeadWidth * sint + HeadHeight * cost));

            Point pt4 = new Point(
                ptGap.X + (HeadWidth * cost + HeadHeight * sint),
                ptGap.Y - (HeadHeight * cost - HeadWidth * sint));

            TopX = Ax + modifier * Math.Cos(b);
            TopY = Ay + modifier * Math.Sin(b);
            context.BeginFigure(pt1, true, false);
            context.ArcTo(ptGap, new Size(height, width), a / Math.PI * 180, false, Curveture > 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true, true);
            //context.LineTo(ptGap, true, true);
            if (HasArrow)
            {
                context.LineTo(pt3, true, true);
                context.LineTo(ptGap, true, true);
                context.LineTo(pt4, true, true);
                context.LineTo(ptGap, true, true);
            }
            context.LineTo(pt2, true, true);*/
            double k = (Y2 - Y1) / (X2 - X1);
            double theta = Math.Atan(k);
            if (X1 > X2)
            {
                theta = Math.PI - theta;
                theta *= -1;
            }
            double b = Math.Atan(-1 / k);
            //if (Y1 >= Y2)
            //{
            //    b = Math.PI - b;
            //    b *= -1;
            //}

            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);
            double sina = Math.Sin(b);
            double cosa = Math.Cos(b);

            //Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1)) / 2;
            double modifier = Curveture * -0.2;
            double Ax = X1 + modifier * cosa;
            double Ay = Y1 + modifier * sina;
            double Bx = X2 + modifier * cosa;
            double By = Y2 + modifier * sina;

            TopX = (Ax + Bx) / 2;
            TopY = (Ay + By) / 2;

            Point pt0 = new Point(X1, Y1);
            Point pt5 = new Point(X2, Y2);
            Point pt1 = new Point(Ax, Ay);
            Point pt2 = new Point(Bx, By);

            double gapx = Bx - (Gap + 3) * cost;
            double gapy = By - (Gap + 3) * sint;
            Point ptGap = new Point(gapx, gapy);

            Point pt3 = new Point(
                ptGap.X - (HeadWidth * cost - HeadHeight * sint),
                ptGap.Y - (HeadWidth * sint + HeadHeight * cost));

            Point pt4 = new Point(
                ptGap.X - (HeadWidth * cost + HeadHeight * sint),
                ptGap.Y + (HeadHeight * cost - HeadWidth * sint));
            context.BeginFigure(pt0, true, false);
            context.LineTo(pt1, true, true);
            context.LineTo(ptGap, true, true);
            if (HasArrow)
            {
                context.LineTo(pt3, true, true);
                context.LineTo(ptGap, true, true);
                context.LineTo(pt4, true, true);
                context.LineTo(ptGap, true, true);
            }
            context.LineTo(pt2, true, true);
            context.LineTo(pt5, true, true);
        }

        #endregion

    }
}
