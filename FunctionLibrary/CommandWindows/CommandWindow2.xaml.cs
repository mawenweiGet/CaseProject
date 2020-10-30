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
using System.Windows.Shapes;

namespace FunctionLibrary.CommandWindows
{
    /// <summary>
    /// CommandWindow2.xaml 的交互逻辑
    /// </summary>
    public partial class CommandWindow2 : Window
    {
        string CommandStr = "";//获取的命令
        string DefaultCommand = "CMDNormal";//默认命令，推出使用
        int ItemIndex = 0;//命令下标索引
        bool FirstTime = true;
        List<string> commandList = new List<string>();
        MB_UsbHid_Operate m_usbHid = new MB_UsbHid_Operate();
        byte[] readDate = null;
        public CommandWindow2()
        {
            InitializeComponent();
            m_usbHid.hidDevice.OnDataRecieved += HidDevice_OnDataRecieved;
            this.KeyUp += new KeyEventHandler(ConsoleTextBox_KeyDown);
        }

        private void HidDevice_OnDataRecieved(object sender, HIDReport args)
        {
            readDate = args.Data;
            bool headLineFeed = false;
            int startIndex = 0;
            bool endLineFeed = false;
            string textAscii = string.Empty;//用来存储转换过后的ASCII码
            //换行符特殊处理（开头结果可能存在换行符）
            if (readDate[0] == 13 && readDate[1] == 10)
            {
                headLineFeed = true;
                startIndex = 1;
            }
            for (int i = 0; i < readDate.Length; i++)
            {
                if (headLineFeed)
                {
                    i = startIndex + 1;
                }
                textAscii += readDate[i].ToString("X");
                if (i + 2 <= readDate.Length - 1)
                {
                    if (readDate[i + 1] == 13 && readDate[i + 2] == 10)
                    {
                        endLineFeed = true;
                        break;
                    }
                }
                startIndex++;
            }
            string message = ASCII2Str(textAscii);
            if (headLineFeed)
            {
                message = "\n" + message;
            }
            if (endLineFeed)
            {
                message += "\r\n";
            }
            ReturnMessData(message);
        }
        public static string ASCII2Str(string textAscii)
        {
            try
            {
                int k = 0;//字节移动偏移量

                byte[] buffer = new byte[textAscii.Length / 2];//存储变量的字节

                for (int i = 0; i < textAscii.Length / 2; i++)
                {
                    //每两位合并成为一个字节
                    buffer[i] = byte.Parse(textAscii.Substring(k, 2), System.Globalization.NumberStyles.HexNumber);
                    k = k + 2;
                }
                //将字节转化成汉字
                return Encoding.Default.GetString(buffer);
            }
            catch (Exception ex)
            {
                return ("ASCII转含中文字符串异常" + ex.Message);
            }
        }
        #region 属性与事件
        /// <summary>
        /// 当前可以输入命令的行号
        /// </summary>
        public int InputLineNum { get; private set; }
        #endregion
        private void ConsoleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            GetLineNum();

            switch (e.Key)
            {
                case Key.Enter:
                    FirstTime = true;
                    CommandStr = lines[InputLineNum - 1];
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
            this.CommandGiveBack.Dispatcher.Invoke(new Action(delegate {
                this.CommandGiveBack.Select(this.CommandGiveBack.Text.Length, 0);
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
            if (FirstTime)
            {
                msg = "\r" + msg;
            }
            if (!string.IsNullOrEmpty(msg))
            {
                this.CommandGiveBack.Dispatcher.Invoke(new Action(delegate {
                    CommandGiveBack.Text += (msg);
                }));
            }
            else
            {
                this.CommandGiveBack.Dispatcher.Invoke(new Action(delegate {
                    this.CommandGiveBack.Text += (msg);
                }));
            }
            AddNewLine();
            FirstTime = false;
        }
        /// <summary>
        /// 增加命令前置
        /// </summary>
        internal void AddNewLine()
        {
            //命令为空
            CommandStr = string.Empty;
            this.CommandGiveBack.Dispatcher.Invoke(new Action(delegate {
                InputLineNum = this.CommandGiveBack.Text.Length - 1;
                this.CommandGiveBack.CaretIndex = InputLineNum;
                this.Focus();
            }));
        }
        //清除记录
        public void Clear()
        {
            this.MyCommand.Text = "";
        }
        //输出数据触发器
        private void ReadCommadeText()
        {
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
                    Clear();
                }
                else
                {
                    if (string.IsNullOrEmpty(CommandStr))
                    {
                        return;
                    }
                    if (m_usbHid.CommandCheckInitPort())
                    {
                        byte[] res = System.Text.ASCIIEncoding.Default.GetBytes(CommandStr);
                        m_usbHid.hidDevice.Write(res, 0, res.Length);
                    }
                    else
                    {
                        commandHandle = "设备连接有误！";
                        ReturnMessData(commandHandle);
                    }
                }
            }
        }
        public void ReturnMessData(string commandHandle)
        {
            Write(commandHandle);
            this.CommandGiveBack.Dispatcher.Invoke(new Action(delegate {
                InputLineNum = this.CommandGiveBack.Text.Length - 1;
                this.Focus();
            }));
            this.CommandGiveBack.Select(InputLineNum, 0);
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
            if (offset == tmpStr.Length)
            {
                this.MyCommand.Text = tmpStr + "\r\n" + GetItemStr(isUp);
            }
            else
            {
                tmpStr = tmpStr.Remove(offset);
                //this.MyCommand.Text = tmpStr + GetItemStr(isUp);
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
        private void Window_Closed(object sender, EventArgs e)
        {
            m_usbHid.hidDevice.OnDataRecieved -= HidDevice_OnDataRecieved;
            byte[] res = System.Text.ASCIIEncoding.Default.GetBytes(DefaultCommand);
            m_usbHid.hidDevice.Write(res, 0, res.Length);
        }

        private void CommandGiveBack_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.CommandGiveBack.Dispatcher.Invoke(new Action(delegate {
                CommandGiveBack.ScrollToEnd();
            }));
        }
    }
}
