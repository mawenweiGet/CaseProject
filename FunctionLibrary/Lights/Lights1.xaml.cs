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

namespace FunctionLibrary.Lights
{
    /// <summary>
    /// Lights1.xaml 的交互逻辑
    /// </summary>
    public partial class Lights1 : UserControl
    {
        public Lights1()
        {
            InitializeComponent();
        }
        private void OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            // 在此处添加事件处理程序实现。
            Application.Current.Shutdown();
        }
    }
}
