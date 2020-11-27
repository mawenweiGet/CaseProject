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

namespace TemplateLibrary.BaseControlPromotionTemplate.DataGridControl
{
    /// <summary>
    /// TableControl3.xaml 的交互逻辑
    /// </summary>
    public partial class TableControl3 : UserControl
    {
        public TableControl3()
        {
            InitializeComponent();
            this.dataGrid.ItemsSource = new List<Score>
            { new Score { Name = "小红", Chinese = 90, Math = 80 },
              new Score { Name = "小明", Chinese = 60, Math = 90 },
              new Score { Name = "小李", Chinese = 95, Math = 58 },
              new Score { Name = "小雷", Chinese = 50, Math = 80 }
            };
        }
    }
    public class Score
    {
        public string Name { get; set; }
        public int Chinese { get; set; }
        public int Math { get; set; }
    }
    public class BlackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.Parse(value.ToString(), NumberStyles.Any) < 60)
            {

                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
