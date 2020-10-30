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

namespace TemplateLibrary.BaseControlTemplate.Button
{
    /// <summary>
    /// ButonTemplate2.xaml 的交互逻辑
    /// </summary>
    public partial class ButonTemplate2 : UserControl
    {
        public ButonTemplate2()
        {
            InitializeComponent();
        }

        private void Btn_StartMonitor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.btn_StartMonitor.Tag.ToString()
                        == "开始监控")
                {
                    this.btn_StartMonitor.Content = "\uedd6";
                    this.btn_StartMonitor.Tag = "停止监控";
                }
                else
                {
                    this.btn_StartMonitor.Content = "\uedde";
                    this.btn_StartMonitor.Tag = "开始监控";
                }
            }
            catch (Exception)
            {
                this.btn_StartMonitor.Content = "\uedde";
                this.btn_StartMonitor.Tag = "开始监控";
            }
        }
    }
}
