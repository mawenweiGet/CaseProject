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
    /// BinDingStudy3.xaml 的交互逻辑
    /// </summary>
    public partial class BinDingStudy3 : UserControl
    {
        public BinDingStudy3()
        {
            InitializeComponent();
            SetBinding();
        }
        /// <summary>
        /// 第一，二个TextBox用于输入用户名，要求数据必须一致
        /// 第三，四个TextBox用于显示输入的邮箱，要求数据必须一致。
        /// 当TextBox的内容全部符合要求的时候，Button可用。
        /// </summary>
        private void SetBinding()
        {
            //准备基础Binding
            Binding bind1 = new Binding("Text") { Source = textBox1 };
            Binding bind2 = new Binding("Text") { Source = textBox2 };
            Binding bind3 = new Binding("Text") { Source = textBox3 };
            Binding bind4 = new Binding("Text") { Source = textBox4 };

            //准备MultiBinding
            MultiBinding mb = new MultiBinding() { Mode = BindingMode.OneWay };
            mb.Bindings.Add(bind1);//注意，MultiBinding对子元素的顺序是很敏感的。
            mb.Bindings.Add(bind2);
            mb.Bindings.Add(bind3);
            mb.Bindings.Add(bind4);
            mb.Converter = new MultiBindingConverter();
            ///将Binding和MultyBinding关联
            this.btnSubmit.SetBinding(Button.IsEnabledProperty, mb);
        }
    }
    public class MultiBindingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!values.Cast<string>().Any(text => string.IsNullOrEmpty(text)) && values[0].ToString() == values[1].ToString() && values[2].ToString() == values[3].ToString())
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 该方法不会被调用
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
