using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// CommandWondows2.xaml 的交互逻辑
    /// </summary>
    public partial class CommandWondows2 : UserControl
    {
        internal string _headText = "";
        string CommandStr = "";//获取的命令
        int ItemIndex = 0;//命令下标索引
        List<string> commandList = new List<string>();
        public CommandWondows2()
        {
            InitializeComponent();

            this.KeyUp += new KeyEventHandler(ConsoleTextBox_KeyDown);
            this.MyCommand.TextWrapping =  TextWrapping.Wrap;

        }
        #region 属性与事件
        /// <summary>
        /// 用于表示命令行前缀
        /// </summary>
        public string HeadText { get { return _headText; } set { _headText = value; } }
        /// <summary>
        /// 当前可以输入命令的行号
        /// </summary>
        public int InputLineNum { get; private set; }
        /// <summary>
        /// 发送数据的事件
        /// </summary>
        public event OnMessageArrivalHandler ReadCommandtEvent;
        //防闪操作
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;
        #endregion
        private void ConsoleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (string.IsNullOrEmpty(HeadText))
                        CommandStr = "test";
                    else
                        CommandStr = CommandStr.Replace(HeadText, "");
                    ReadCommadeText();//传输命令
                    e.Handled = true;
                    break;
                case Key.Up:
                    UpdateLog(true);
                    e.Handled = true;
                    break;
                case Key.Down:
                    UpdateLog(false);
                    e.Handled = true;
                    break;
            }
            this.MyCommand.Select(this.MyCommand.Text.Length, 0);
        }
        /// <summary>
        /// 在屏幕上显示字符串
        /// </summary>
        /// <param name="msg">字符串</param>
        public void Write(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                this.MyCommand.Text+=("\n" + msg);
            }
            else
            {
                this.MyCommand.Text += (msg);
            }

            AddNewLine();
        }
        /// <summary>
        /// 用于插入头部数据,配合HeadText使用
        /// </summary>
        private void InsertHeadText()
        {
            this.MyCommand.Text += (HeadText);
        }
        /// <summary>
        /// 增加命了前置
        /// </summary>
        internal void AddNewLine()
        {
            //命令为空
            CommandStr = string.Empty;
            //加入新的行
            this.MyCommand.Text += ("\n");
            InsertHeadText();
            InputLineNum = this.MyCommand.Text.Length - 1;
            this.Focus();

        }
        public void Clear()
        {
            this.MyCommand.Text = "";
        }
        //输出数据触发器
        private void ReadCommadeText()
        {
            if (ReadCommandtEvent != null)
            {
                if (!string.IsNullOrEmpty(CommandStr) && !commandList.Contains(CommandStr))
                {
                    commandList.Add(CommandStr);
                    ItemIndex++;
                }
                ReadCommandtEvent(this, ConsoleEditEventArgs.CreatEventArgs(CommandStr));
            }
        }
        private void UpdateLog(bool isUp)
        {
            if (commandList.Count == 0)
                return;
            string tmpStr = this.MyCommand.Text;
            int offset = tmpStr.LastIndexOf(HeadText);
            offset += HeadText.Length;
            SendMessage(new IntPtr(), WM_SETREDRAW, 0, IntPtr.Zero);
            if (offset == tmpStr.Length)
            {
                this.MyCommand.Text = tmpStr + GetItemStr(isUp);
            }
            else
            {
                tmpStr = tmpStr.Remove(offset);
                this.MyCommand.Text = tmpStr + GetItemStr(isUp);
            }
            SendMessage(new IntPtr(), WM_SETREDRAW, 0, IntPtr.Zero);
        }
        private string GetItemStr(bool isUp)
        {

            if (isUp)
            {
                ItemIndex--;
                if (ItemIndex <= 0)
                {
                    ItemIndex = 0;
                }
                return commandList[ItemIndex];
            }
            else
            {
                ItemIndex++;
                if (ItemIndex >= commandList.Count)
                {
                    ItemIndex = commandList.Count - 1;
                    return "";
                }
                return commandList[ItemIndex];
            }
        }
    }
}
