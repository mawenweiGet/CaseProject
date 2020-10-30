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

namespace WPF_BaseStudy.Delegate
{
    /// <summary>
    /// MainWondow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWondow : UserControl
    {
        delegate void MydelNoparameter();
        delegate void Mydel(ref int x);//传参委托
        delegate int MydelReturnValue();//有返回值的委托
        delegate int MydelParameterReturn(int X);//带参数有返回值的委托
        MyClass myclass = new MyClass();
        public MainWondow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt.Tag.ToString() == "1")
            {
                MydelNoparameter MydelNoparameter = myclass.Add1;
                this.ReturnValue.Text = "无参无返回值的委托";
            }
            if (bt.Tag.ToString() == "2")
            {
                Mydel mydel = myclass.Add2;
                mydel += myclass.Add3;
                mydel += myclass.Add2;

                int x = Convert.ToInt32(this.Parameters.Text.ToString());
                mydel(ref x);
                this.ReturnValue.Text = x.ToString();
            }
            if (bt.Tag.ToString() == "3")
            {
                MydelReturnValue mydelReturnValue = myclass.Add4;
                this.ReturnValue.Text = mydelReturnValue().ToString();
            }
            if (bt.Tag.ToString() == "4")
            {
                MydelParameterReturn mydelParameterReturn = myclass.Add5;
                this.ReturnValue.Text = mydelParameterReturn(Convert.ToInt32(this.Parameters.Text.ToString())).ToString();
            }
        }
        public class MyClass
        {
            public void Add1()
            {
                Console.WriteLine("无参无返回值的委托");
            }
            public void Add2(ref int x)        //这里它会改变参数x的值
            {
                x += 2;
            }
            public void Add3(ref int x)        //这里也会改变参数x的值
            {
                x += 3;
            }
            public int Add4()
            {
                return 100;
            }
            public int Add5(int x)
            {
                return x*x;
            }
        }
    }
}
