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

namespace TestProject
{
    /// <summary>
    /// MyLabel.xaml 的交互逻辑
    /// </summary>
    public partial class MyLabel : UserControl
    {
        public static readonly DependencyProperty CaptionProperty =
    DependencyProperty.Register("Caption", typeof(string), typeof(MyLabel));
        public MyLabel()
        {
            InitializeComponent();
        }
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }



    }
}
