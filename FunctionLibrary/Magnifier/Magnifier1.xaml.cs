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

namespace FunctionLibrary.Magnifier
{
    /// <summary>
    /// Magnifier1.xaml 的交互逻辑
    /// </summary>
    public partial class Magnifier1 : UserControl
    {
        public Magnifier1()
        {
            InitializeComponent();
        }

        private void contextArea_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point rate = new Point(2, 2);
            Point pos = e.MouseDevice.GetPosition(rootLayout);  //相对于outsideGrid 获取鼠标的坐标  
            Rect viewBox = vb.Viewbox;    //这里的Viewbox和前台的一样   这里就是获取前台Viewbox的值
            double xoffset = 0;  //因为鼠标要让它在矩形(放大镜)的中间  那么我们就要让矩形的左上角重新移动位置  
            double yoffset = 0;

            xoffset = magnifierEllipse.ActualWidth / 2;
            yoffset = magnifierEllipse.ActualHeight / 2;

            viewBox.X = pos.X - xoffset + (magnifierEllipse.ActualWidth - magnifierEllipse.ActualWidth / rate.X) / 2;
            viewBox.Y = pos.Y - yoffset + (magnifierEllipse.ActualHeight - magnifierEllipse.ActualHeight / rate.Y) / 2;
            vb.Viewbox = viewBox;
            Canvas.SetLeft(magnifierCanvas, pos.X - xoffset);  //同理重新定位Canvas magnifierCanvas的坐标  
            Canvas.SetTop(magnifierCanvas, pos.Y - yoffset);
        }
    }
}
