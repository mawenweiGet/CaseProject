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

namespace FunctionLibrary.ModbusCommStudy
{
    /// <summary>
    /// ToolBar_Project.xaml 的交互逻辑
    /// </summary>
    public partial class ToolBar_Project : UserControl
    {
        public MB_UsbHid_Operate m_MbOperate = new MB_UsbHid_Operate();
        public ToolBar_Project()
        {
            InitializeComponent();

            m_MbOperate.RefreshAllDevice();

            portName.ItemsSource = m_MbOperate.m_listDeviceDescription;
            portName.DisplayMemberPath = "ProductString";
            portName.SelectedValuePath = "DevicePath";
            portName.SetBinding(ComboBox.SelectedValueProperty,
                    new Binding("m_strDeviceSelected")
                    { Source = m_MbOperate });

           
        }

        private void BtnSetCommParameter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Upload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Download_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
