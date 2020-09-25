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
using TemplateLibrary.ComboBox;

namespace TestControlLibrary.DataAdjustment.RangeAdjustment
{
    /// <summary>
    /// RangeAdjustment1.xaml 的交互逻辑
    /// </summary>
    public partial class RangeAdjustment1 : UserControl
    {
        public RangeAdjustment1()
        {
            InitializeComponent();

            this.Explain_Label.Content = "输出设置";

            ComboBoxTemplate1 input = new ComboBoxTemplate1();

            this.Content_StackPane.Children.Add(input);



        }
    }
}
