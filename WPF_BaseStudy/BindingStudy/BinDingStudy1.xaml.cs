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

namespace WPF_BaseStudy.BindingStudy
{
    /// <summary>
    /// BinDingStudy1.xaml 的交互逻辑
    /// </summary>
    public partial class BinDingStudy1 : UserControl
    {
        public BinDingStudy1()
        {
            InitializeComponent();
        }
    }
    /// <summary>
    /// 自定义事件转换
    /// </summary>
    public class TimeConver : IValueConverter
    {
        //当值从绑定源传播给绑定目标时，调用方法Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            DateTime date = (DateTime)value;
            return date.ToString("yyyy-MM-dd");
        }

        //当值从绑定目标传播给绑定源时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as string;
            DateTime txtDate;
            if (DateTime.TryParse(str, out txtDate))
            {
                return txtDate;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
