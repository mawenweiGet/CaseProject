using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Events;
using LiveCharts.Wpf;
using LocalTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf_Arrows_Package;

namespace TestProject
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Line myLine = new Line() 
            //{
            //    X1 = 150,
            //    Y1 = 30,
            //    X2 = 250,
            //    Y2 = 30,
            //    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#898989")),
            //    StrokeThickness =3,
            //};
            //TextCanvas.Children.Add(myLine);
            //Arrow arrow = new Arrow();
            //arrow.StrokeThickness = 10;
            //arrow.X1 = 50;
            //arrow.Y1 = 50;
            //arrow.X2 = 100;
            //arrow.Y2 = 100;
            //arrow.HeadWidth = 20;
            //arrow.HeadHeight = 20;
            //arrow.Stroke = Brushes.Black;
            //TextCanvas.Children.Add(arrow);


            //ArrowLine al = new ArrowLine()
            //{
            //    ArrowEnds = ArrowEnds.Both,
            //    IsArrowClosed = true,
            //    IsConnecting = false,
            //    StrokeThickness = 5,
            //    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#898989")),
            //    StartPoint = new Point() { X = 150, Y = 30 },
            //    EndPoint = new Point() { X = 250, Y = 30 },
            //};
            //TextCanvas.Children.Add(al);

            //ArrowLine line = new ArrowLine()
            //{
            //    ArrowEnds = ArrowEnds.None,
            //    IsArrowClosed = true,
            //    IsConnecting = true,
            //    StrokeThickness = 2,
            //    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#898989")),
            //    StartPoint = new Point() { X = 150, Y = 40 },
            //    EndPoint = new Point() { X = 250, Y = 40 },
            //};
            //TextCanvas.Children.Add(line);

            ArrowLine lines = new ArrowLine()
            {
                ArrowEnds = ArrowEnds.Both,
                ArrowAngle=40,
                ArrowLength=10,
                IsArrowClosed = true,
                IsConnecting = true,
                StrokeThickness = 2,
                Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#898989")),
                StartPoint = new Point() { X = 150, Y = 100 },
                EndPoint = new Point() { X = 250, Y = 100 },
            };
            TextCanvas.Children.Add(lines);
        }

    }
}
