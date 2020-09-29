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

namespace WPF_BaseStudy.BindingStudy
{
    /// <summary>
    /// BinDingStudy2.xaml 的交互逻辑
    /// </summary>
    public partial class BinDingStudy2 : UserControl
    {
        public BinDingStudy2()
        {
            InitializeComponent();

            ObjectDataProvider objpro = new ObjectDataProvider();
            objpro.ObjectInstance = new Caculate();
            objpro.MethodName = "Add";
            objpro.MethodParameters.Add("0");
            objpro.MethodParameters.Add("0");
            Binding bindingToArg1 = new Binding("MethodParameters[0]") { Source = objpro, BindsDirectlyToSource = true, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            Binding bindingToArg2 = new Binding("MethodParameters[1]") { Source = objpro, BindsDirectlyToSource = true, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            Binding bindToResult = new Binding(".") { Source = objpro };
            this.textBox1.SetBinding(TextBox.TextProperty, bindingToArg1);
            this.textBox2.SetBinding(TextBox.TextProperty, bindingToArg2);
            this.textBox3.SetBinding(TextBox.TextProperty, bindToResult);
        }
    }
    public class Caculate
    {
        public string Add(string arg1, string arg2)
        {
            double x = 0;
            double y = 0;
            double z = 0;
            if (double.TryParse(arg1, out x) && double.TryParse(arg2, out y))
            {
                z = x + y;
                return z.ToString();
            }
            return "Iput Error";
        }

        //其它方法省略
    }
}
