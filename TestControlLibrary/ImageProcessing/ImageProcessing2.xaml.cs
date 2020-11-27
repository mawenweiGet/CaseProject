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

namespace TestControlLibrary.ImageProcessing
{
    /// <summary>
    /// ImageProcessing2.xaml 的交互逻辑
    /// </summary>
    public partial class ImageProcessing2 : UserControl
    {
        public ImageProcessing2()
        {
            InitializeComponent();
        }
        private void GridMainChart_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Delta > 0)
                {
                    ZoomIn(e.GetPosition(canvasMainChart));
                }
                else
                {
                    ZoomOut(e.GetPosition(canvasMainChart));
                }
                e.Handled = true;
            }

        }

        double View_originOffsetX = 0;
        double View_originOffsetY = 0;
        Point View_lastMousePt = new Point();
        Point lastCenterPoint = new Point();
        private void ZoomIn(Point pt)
        {
            if (stf.ScaleX < 5)
            {
                pt.X -= View_originOffsetX;
                pt.Y -= View_originOffsetY;
                //原有中心点
                Point untransformedOldCenter = new Point(this.stf.CenterX, this.stf.CenterY);
                Point transformedOldCenter = this.stf.Transform(untransformedOldCenter);
                //新中心点
                Point untransformedNewCenter = gridMainChart.RenderTransform.Inverse.Transform(pt);
                Point transformedNewCenter = pt;
                //位置修正
                double adjustX = transformedNewCenter.X - transformedOldCenter.X -
                                untransformedNewCenter.X + untransformedOldCenter.X;
                double adjustY = transformedNewCenter.Y - transformedOldCenter.Y -
                                untransformedNewCenter.Y + untransformedOldCenter.Y;
                //更新
                this.ttf.X = adjustX;
                this.ttf.Y = adjustY;
                stf.CenterX = untransformedNewCenter.X;
                stf.CenterY = untransformedNewCenter.Y;

                stf.ScaleX = stf.ScaleX * 1.10;
                stf.ScaleY = stf.ScaleY * 1.10;
            }
        }

        private void ZoomOut(Point pt)
        {
            if (stf.ScaleX > 1)
            {
                pt.X -= View_originOffsetX;
                pt.Y -= View_originOffsetY;
                //原有中心点
                Point untransformedOldCenter = new Point(this.stf.CenterX, this.stf.CenterY);
                Point transformedOldCenter = this.stf.Transform(untransformedOldCenter);
                //新中心点
                Point untransformedNewCenter = gridMainChart.RenderTransform.Inverse.Transform(pt);
                Point transformedNewCenter = pt;
                //位置修正
                double adjustX = transformedNewCenter.X - transformedOldCenter.X -
                                untransformedNewCenter.X + untransformedOldCenter.X;
                double adjustY = transformedNewCenter.Y - transformedOldCenter.Y -
                                untransformedNewCenter.Y + untransformedOldCenter.Y;
                //更新
                this.ttf.X = adjustX;
                this.ttf.Y = adjustY;
                stf.CenterX = untransformedNewCenter.X;
                stf.CenterY = untransformedNewCenter.Y;

                stf.ScaleX = stf.ScaleX * 0.90;
                stf.ScaleY = stf.ScaleY * 0.90;
            }
        }
        private void GridMainChart_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(canvasMainChart);
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                Point SBoffsetA = gridMainChart.TranslatePoint(new Point(0, 0), canvasMainChart);//secondbase的左上角点
                Point SBoffsetB = gridMainChart.TranslatePoint(new Point(gridMainChart.ActualWidth, gridMainChart.ActualHeight), gridRangeChartView);//secondbase的右下角点
                if ((point.X > View_lastMousePt.X && SBoffsetA.X < canvasMainChart.ActualWidth * 0.5)
                    || (point.X < View_lastMousePt.X && SBoffsetB.X > canvasMainChart.ActualWidth * 0.5))
                {
                    double x = point.X - View_lastMousePt.X;
                    double secondLeft = Canvas.GetLeft(gridMainChart);
                    Canvas.SetLeft(gridMainChart, secondLeft + x);
                    View_originOffsetX += x;
                    lastCenterPoint.X -= x;
                }
                if ((point.Y > View_lastMousePt.Y && SBoffsetA.Y < canvasMainChart.ActualHeight * 0.5)
                    || (point.Y < View_lastMousePt.Y && SBoffsetB.Y > canvasMainChart.ActualHeight * 0.5))
                {
                    double y = point.Y - View_lastMousePt.Y;
                    double secondTop = Canvas.GetTop(gridMainChart);
                    Canvas.SetTop(gridMainChart, secondTop + y);
                    View_originOffsetY += y;
                    lastCenterPoint.Y -= y;
                }
                View_lastMousePt = point;
                e.Handled = true;
            }
            else
            {

                View_lastMousePt = point;
            }
        }
    }
}
