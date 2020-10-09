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

namespace TestProject
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.MyCommandWindow.HeadText = "[Test]";
            this.MyCommandWindow.ReadCommandtEvent += new OnMessageArrivalHandler(consoleTextBox1_ReadCommandtEvent);

        }

        private void consoleTextBox1_ReadCommandtEvent(object sender, ConsoleEditEventArgs e)
        {
            if (e.Message.ToLower() == "test")
            {
                this.MyCommandWindow.HeadText = "[TestMode]";
                this.MyCommandWindow.Write("欢迎进入测试模式");
            }
            else if (e.Message == "cls")
            {
                this.MyCommandWindow.Clear();
                this.MyCommandWindow.Write("");
            }
            else
            {
                this.MyCommandWindow.Write("错误的输入");
            }
        }
    }
}
