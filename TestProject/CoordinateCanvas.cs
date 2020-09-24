using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestProject
{
    /// <summary>
    /// 自定义坐标系Canvas
    /// </summary>
    public class CoordinateCanvas : Canvas
    {

        /// <summary>
        /// 原点X坐标
        /// </summary>
        public double OriginX
        {
            get { return (double)GetValue(OriginXProperty); }
            set { SetValue(OriginXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OriginX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OriginXProperty =
            DependencyProperty.Register("OriginX", typeof(double), typeof(CoordinateCanvas), new PropertyMetadata(0.0, OnOriginPropertyChanged));

        private static void OnOriginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as UIElement;
            if (uiElement != null)
            {
                uiElement.InvalidateArrange();
            }
        }

        /// <summary>
        /// 原点Y坐标
        /// </summary>
        public double OriginY
        {
            get { return (double)GetValue(OriginYProperty); }
            set { SetValue(OriginYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OriginY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OriginYProperty =
            DependencyProperty.Register("OriginY", typeof(double), typeof(CoordinateCanvas), new PropertyMetadata(0.0, OnOriginPropertyChanged));

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Point originalPoint = new Point();
            if (OriginX > 0 && OriginX <= arrangeSize.Width)
                originalPoint.X = OriginX;
            if (OriginY > 0 && OriginY <= arrangeSize.Height)
                originalPoint.Y = OriginY;
            foreach (UIElement element in InternalChildren)
            {
                if (element == null) continue;
                double x = 0.0;
                double y = 0.0;
                double left = GetLeft(element);
                if (!double.IsNaN(left))
                {
                    x = left;
                }
                double top = GetTop(element);
                if (!double.IsNaN(top))
                {
                    y = top;
                }
                element.Arrange(new Rect(new Point(originalPoint.X + x, originalPoint.Y + y), element.DesiredSize));
            }
            return arrangeSize;
        }
    }
}
