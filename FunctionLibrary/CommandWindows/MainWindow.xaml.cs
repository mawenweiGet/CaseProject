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

namespace FunctionLibrary.CommandWindows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : UserControl
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MyCommandWindow.ReadCommandtEvent += new OnMessageArrivalHandler(consoleTextBox1_ReadCommandtEvent);
            this.MyCommandWindow.Clear();
        }
        private void consoleTextBox1_ReadCommandtEvent(object sender, ConsoleEditEventArgs e)
        {
            if (e.Message.ToLower() == "cls")
            {
                this.MyCommandWindow.Clear();
            }
            else if (!string.IsNullOrEmpty(e.Message))
            {
                this.MyCommandWindow.Write(e.Message);
            }
            else
            {
                this.MyCommandWindow.Write("错误的输入");
            }
        }
    }
}
