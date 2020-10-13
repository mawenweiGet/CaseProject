using Modbus.Device;
using Modbus.HelpClass;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// CommandWondows1.xaml 的交互逻辑
    /// </summary>
    public partial class CommandWondows1 : UserControl
    {
        internal string _headText = "";
        string CommandStr = "";//获取的命令
        int ItemIndex = 0;//命令下标索引
        List<string> commandList = new List<string>();
        MB_UsbHid_Operate usbHid = new MB_UsbHid_Operate();
        byte[] readDate = null;
        public CommandWondows1()
        {
            InitializeComponent();
            usbHid.RefreshAllDevice();
            usbHid.hidDevice.OnDataRecieved += HidDevice_OnDataRecieved;
            this.KeyUp += new KeyEventHandler(ConsoleTextBox_KeyDown);
            this.MyCommand.TextWrapping = TextWrapping.Wrap;
        }

        private void HidDevice_OnDataRecieved(object sender, HIDReport args)
        {
            readDate = args.Data;
            ReturnMessData(System.Text.Encoding.ASCII.GetString(readDate));
            //ReturnMessData(Encoding.UTF8.GetString(readDate));
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
        #endregion
        private void ConsoleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            GetLineNum();

            switch (e.Key)
            {
                case Key.Enter:

                    if (string.IsNullOrEmpty(HeadText))
                        CommandStr = lines[InputLineNum - 1];
                    else
                        CommandStr = lines[InputLineNum - 1].Replace(HeadText, "");
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
            Thread.Sleep(5);
            this.MyCommand.Dispatcher.Invoke(new Action(delegate {
                this.MyCommand.Select(this.MyCommand.Text.Length, 0);
            }));
        }
        StringCollection lines = new StringCollection();
        public StringCollection GetLineNum()
        {
            lines.Clear();

            InputLineNum = this.MyCommand.LineCount;

            for (int line = 0; line < InputLineNum; line++)
                lines.Add(this.MyCommand.GetLineText(line));
            return lines;
        }
        /// <summary>
        /// 在屏幕上显示字符串
        /// </summary>
        /// <param name="msg">字符串</param>
        public void Write(string msg)
        {
            msg = msg.Replace("\0", "");
            if (!string.IsNullOrEmpty(msg))
            {
                this.MyCommand.Dispatcher.Invoke(new Action(delegate {
                    MyCommand.Text += ("\n" + msg);
                }));
            }
            else
            {
                this.MyCommand.Dispatcher.Invoke(new Action(delegate {
                    this.MyCommand.Text += (msg);
                }));
            }

            AddNewLine();
        }
        /// <summary>
        /// 用于插入头部数据,配合HeadText使用
        /// </summary>
        private void InsertHeadText()
        {
            this.MyCommand.Dispatcher.Invoke(new Action(delegate {
                this.MyCommand.Text += (HeadText);
            }));
        }
        /// <summary>
        /// 增加命令前置
        /// </summary>
        internal void AddNewLine()
        {
            //命令为空
            CommandStr = string.Empty;
            //加入新的行
            this.MyCommand.Dispatcher.Invoke(new Action(delegate {
                this.MyCommand.Text += ("\r\n");
            }));
            InsertHeadText();
            this.MyCommand.Dispatcher.Invoke(new Action(delegate {
                InputLineNum = this.MyCommand.Text.Length - 1;
                this.MyCommand.CaretIndex = InputLineNum;
                this.Focus();
            }));

        }
        //清除记录
        public void Clear()
        {
            this.MyCommand.Text = HeadText;
        }
        //输出数据触发器
        private void ReadCommadeText()
        {
            if (ReadCommandtEvent != null)
            {
                //记录指令
                if (!string.IsNullOrEmpty(CommandStr) && !commandList.Contains(CommandStr))
                {
                    commandList.Add(CommandStr);
                    ItemIndex++;
                }
                //执行指令，反馈执行结果内容
                string commandHandle = "命令" + CommandStr + "处理结果！";
                if (CommandStr.ToLower().Equals("cls"))
                {
                    ReadCommandtEvent(this, ConsoleEditEventArgs.CreatEventArgs(CommandStr));
                }
                else
                {
                    if (string.IsNullOrEmpty(CommandStr))
                    {
                        return;
                    }
                    if (usbHid.CommandCheckInitPort())
                    {
                        byte[] res = System.Text.ASCIIEncoding.Default.GetBytes(CommandStr);
                        usbHid.hidDevice.Write(res, 0, res.Length);
                    }
                    else
                    {
                        commandHandle = "设备连接有误！";
                        ReadCommandtEvent(this, ConsoleEditEventArgs.CreatEventArgs(commandHandle));
                    }
                }
            }
        }
        public void ReturnMessData(string commandHandle)
        {
            ReadCommandtEvent(this, ConsoleEditEventArgs.CreatEventArgs(commandHandle));
            this.MyCommand.Dispatcher.Invoke(new Action(delegate {
                InputLineNum = this.MyCommand.Text.Length - 1;
                this.Focus();
            }));
        }
        //翻查指令记录
        private void UpdateLog(bool isUp)
        {
            if (commandList.Count == 0)
                return;
            string tmpStr = this.MyCommand.Text;
            if (string.IsNullOrEmpty(tmpStr))
            {
                this.MyCommand.Text = GetItemStr(isUp);
                return;
            }
            else
            {
                GetLineNum();
                if (lines.Count == 1)
                {
                    this.MyCommand.Text = GetItemStr(isUp);
                    return;
                }
            }
            int offset = tmpStr.LastIndexOf("\r\n");
            offset += HeadText.Length;
            if (offset == tmpStr.Length)
            {
                this.MyCommand.Text = tmpStr + "\r\n" + GetItemStr(isUp);
            }
            else
            {
                tmpStr = tmpStr.Remove(offset);
                this.MyCommand.Text = tmpStr + "\r\n" + GetItemStr(isUp);
            }
        }
        /// <summary>
        /// 获取前后一次指令记录结果
        /// </summary>
        /// <param name="isUp">前或后一次</param>
        /// <returns></returns>
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
