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
using Wpf_Arrows_Package;

namespace FunctionLibrary.ArrowLine
{
    /// <summary>
    /// ArrowLine1.xaml 的交互逻辑
    /// </summary>
    public partial class ArrowLine1 : UserControl
    {
        public ArrowLine1()
        {

            InitializeComponent();

            Wpf_Arrows_Package.ArrowLine al = new Wpf_Arrows_Package.ArrowLine()
            {
                ArrowEnds = ArrowEnds.Both,
                ArrowAngle = 40,
                ArrowLength = 10,
                IsArrowClosed = true,
                IsConnecting = true,
                StrokeThickness = 1,
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#898989")),
                StartPoint = new Point() { X = 180, Y = 100 },
                EndPoint = new Point() { X = 280, Y = 100 },
            };
            TextCanvas.Children.Add(al);

            Wpf_Arrows_Package.ArrowLine als = new Wpf_Arrows_Package.ArrowLine()
            {
                ArrowEnds = ArrowEnds.Both,
                ArrowAngle = 40,
                ArrowLength = 10,
                IsArrowClosed = true,
                IsConnecting = true,
                StrokeThickness = 5,
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#898989")),
                StartPoint = new Point() { X = 180, Y = 120 },
                EndPoint = new Point() { X = 280, Y = 120 },
            };
            TextCanvas.Children.Add(als);

        }
    }
}
