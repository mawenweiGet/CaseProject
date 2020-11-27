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
    /// ImageProcessing1.xaml 的交互逻辑
    /// </summary>
    public partial class ImageProcessing1 : UserControl
    {
        public ImageProcessing1()
        {
            InitializeComponent();
        }
        private void A_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Delta > 0)
                {
                    ZoomIn(e.GetPosition(ImgBox));
                }
                else
                {
                    ZoomOut(e.GetPosition(ImgBox));
                }
            }
            else
            {
                Point SBoffsetA = a.TranslatePoint(new Point(0, 0), ImgBox);//secondbase的左上角点
                Point SBoffsetB = a.TranslatePoint(new Point(a.ActualWidth, a.ActualHeight), ImgBox);//secondbase的右下角点
                if (e.Delta < 0 && SBoffsetB.Y > ImgBox.ActualHeight * 0.5)
                {
                    double top = Canvas.GetTop(a);
                    Canvas.SetTop(a, top - 20);

                    View_originOffsetY -= 20;
                    lastCenterPoint.Y += 20;
                }
                else if (e.Delta > 0 && SBoffsetA.Y < ImgBox.ActualHeight * 0.5)
                {
                    double bottom = Canvas.GetTop(a);
                    Canvas.SetTop(a, bottom + 20);

                    View_originOffsetY += 20;
                    lastCenterPoint.Y -= 20;
                }
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
                Point untransformedNewCenter = a.RenderTransform.Inverse.Transform(pt);
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
                Point untransformedNewCenter = a.RenderTransform.Inverse.Transform(pt);
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

        private void A_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(ImgBox);
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                Point SBoffsetA = a.TranslatePoint(new Point(0, 0), ImgBox);//secondbase的左上角点
                Point SBoffsetB = a.TranslatePoint(new Point(a.ActualWidth, a.ActualHeight), ImgBox);//secondbase的右下角点
                if ((point.X > View_lastMousePt.X && SBoffsetA.X < ImgBox.ActualWidth * 0.5)
                    || (point.X < View_lastMousePt.X && SBoffsetB.X > ImgBox.ActualWidth * 0.5))
                {
                    double x = point.X - View_lastMousePt.X;
                    double secondLeft = Canvas.GetLeft(a);
                    Canvas.SetLeft(a, secondLeft + x);
                    View_originOffsetX += x;
                    lastCenterPoint.X -= x;
                }
                if ((point.Y > View_lastMousePt.Y && SBoffsetA.Y < ImgBox.ActualHeight * 0.5)
                    || (point.Y < View_lastMousePt.Y && SBoffsetB.Y > ImgBox.ActualHeight * 0.5))
                {
                    double y = point.Y - View_lastMousePt.Y;
                    double secondTop = Canvas.GetTop(a);
                    Canvas.SetTop(a, secondTop + y);
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
