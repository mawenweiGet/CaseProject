using LiveCharts;
using LiveCharts.Configurations;
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

namespace LiveCharLibrary
{
    /// <summary>
    /// LiveChar3.xaml 的交互逻辑
    /// </summary>
    public partial class LiveChar3 : UserControl
    {
        public LiveChar3()
        {
            InitializeComponent();
            Customers = new ChartValues<CustomerVm>()
            {
                 new CustomerVm()
                 {
                   X=1,
                   Y=2
                 },
                 new CustomerVm()
                 {
                   X=2,
                   Y=3
                 },
                 new CustomerVm()
                 {
                   X=5,
                   Y=4
                 },
                 new CustomerVm()
                 {
                   X=10,
                   Y=20
                 },
            };
            //重定义数据映射
            var customerVmMapper = Mappers.Xy<CustomerVm>()
                .X(value => value.X)
                .Y(value => value.Y);
            //保存加载数据映射
            Charting.For<CustomerVm>(customerVmMapper);
            DataContext = this;
        }
        public ChartValues<CustomerVm> Customers { get; set; }
    }
}
