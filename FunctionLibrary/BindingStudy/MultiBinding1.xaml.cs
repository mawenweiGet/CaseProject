using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace FunctionLibrary.BindingStudy
{
    /// <summary>
    /// MultiBinding1.xaml 的交互逻辑
    /// </summary>
    public partial class MultiBinding1 : UserControl
    {
        public MultiBinding1()
        {
            InitializeComponent();

            MultiBinding mb = new MultiBinding() { Mode = BindingMode.OneWay };

            Binding b1 = new Binding("IsChecked") { Source = cond1 };
            Binding b2 = new Binding("IsChecked") { Source = cond2 };
            Binding b3 = new Binding("(Validation.HasError)") { Source = m_txtBoxContent };

            mb.Bindings.Add(b1);
            mb.Bindings.Add(b2);
            mb.Bindings.Add(b3);

            mb.Converter = new ConverterClass();
            SetBinding(TextBox.TextProperty, mb);
        }

        private void onClicked(object sender, RoutedEventArgs e)
        {

        }

        private void m_txtBoxContent_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
    /// <summary>
    /// 数据转换
    /// </summary>
    public class ConverterClass : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = true;
            foreach (var item in values)
            {
                if ((bool)item == false)
                {
                    result = false;
                }
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
