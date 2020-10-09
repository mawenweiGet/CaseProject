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

namespace TestProject
{
    /// <summary>
    /// CommandWondows1.xaml 的交互逻辑
    /// </summary>
    public partial class CommandWondows1 : CommandWondowsBase
    {
        string CommandStr = "";//获取的命令
        /// <summary>
        /// 命令字符集合
        /// </summary>
        List<string> commandList = new List<string>();
        public CommandWondows1()
        {
            InitializeComponent();
        }
        #region 事件与属性
        internal string _headText = "";
        /// <summary>
        /// 命令行前缀文本
        /// </summary>
        public string HeadText { get { return _headText; } set { _headText = value; } }

        #endregion


        private void MyCommand_KeyDown(object sender, KeyEventArgs e)
        {
            TextRange textRange = new TextRange(MyCommand.Document.ContentStart, MyCommand.Document.ContentEnd);

            if (textRange.Text == "")
            {
                e.Handled = true;
                return;
            }
        }
    }
}
