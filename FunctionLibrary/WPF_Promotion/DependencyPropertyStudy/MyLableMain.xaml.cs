using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace FunctionLibrary.WPF_Promotion.DependencyPropertyStudy
{
    /// <summary>
    /// MyLableMain.xaml 的交互逻辑
    /// </summary>
    public partial class MyLableMain : UserControl
    {
        TestData td = new TestData();
        public MyLableMain()
        {
            InitializeComponent();

            //MyLabel ml = new MyLabel()
            //{
            //    Width = double.NaN,
            //    Height = double.NaN,
            //    //Caption = 212121,
            //    //MyShow=false
            //};
            //this.Textbox1.SetBinding(TextBox.TextProperty,
            //    new Binding("data")
            //    { 
            //    Source=this.td,
            //    });

            //ml.SetBinding(MyLabel.MyPropertyProperty,
            //    new Binding("Caption")
            //    {
            //        Source = this.td
            //    });
            //ml.SetBinding(MyLabel.MyShowProperty,
            //    new Binding("show")
            //    {
            //        Source = this.td,
            //    });
            //(ml as MyLabel).Visibility = Visibility.Hidden;

            //this.T_StackPanel.Children.Add(ml);
        }
        public class TestData
        {
            public string data = "784532654321";
            public bool show = false;
        }
    }
}
