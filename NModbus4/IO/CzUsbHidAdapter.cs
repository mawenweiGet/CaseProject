namespace Modbus.IO
{
    using global::Modbus.Device;
    using System;
    using System.Diagnostics;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Unme.Common;

    /// <summary>
    ///     Concrete Implementor - http://en.wikipedia.org/wiki/Bridge_Pattern
    /// </summary>
    internal class CzUsbHidAdapter : IStreamResource
    {
        private HIDDevice m_usb;
        byte[] readDate =null;
        int offsetAll = 0;
        bool bFlag = false;
        //定义Event响应USB数据返回通知事件       
        ManualResetEventSlim event_RWCtrl; 
        public CzUsbHidAdapter(HIDDevice usbClient)
        {
            Debug.Assert(usbClient != null, "Argument usbClient cannot be null.");
            m_usb = usbClient;
            m_usb.OnDataRecieved += M_usb_OnDataRecieved;
        }
        private void M_usb_OnDataRecieved(object sender, HIDReport args)
        {
           readDate = args.Data;
            //收到USB返回数据结果，通知Event事件响应
            if (event_RWCtrl!=null)
            {
                event_RWCtrl.Set();
            }
        }
        public int InfiniteTimeout
        {
            get { return Timeout.Infinite; }
        }

        public int ReadTimeout
        {
            get { return m_usb.ReadTimeout; }
            set { m_usb.ReadTimeout = value; }
        }

        public int WriteTimeout
        {
            get { return m_usb.WriteTimeout; }
            set { m_usb.WriteTimeout = value; }
        }

        public void Write(byte[] buffer, int offset, int size)
        {
            m_usb.Write(buffer, offset, size);
            bFlag = true;  
            offsetAll = 0;
            //初始化Event，初始信号状态为false
            event_RWCtrl = new ManualResetEventSlim(false);
        }

        public int Read(byte[] buffer, int offset, int size)
        {
            if (bFlag==true)
            {
                //阻塞Event事件响应线程，判断指定时间内是否有获取响应事件，获取失败不在等待稍后获取
                bool bFlagTimeResult = event_RWCtrl.WaitHandle.WaitOne(m_usb.ReadTimeout, false);
                if (bFlagTimeResult == false)
                {
                    //抛出异常信息由上层处理。
                    throw (new Exception("USB 返回结果响应超时"));
                }
                //关闭通知响应事件
                event_RWCtrl.Reset();
                bFlag = false;
            }
            
            for (int i = 0; i < size; i++)
            {
                buffer[i] = readDate[offsetAll + i + offset];
            }
            offsetAll = offsetAll + offset + size;
            return size;
        }

        public void DiscardInBuffer()
        {
            //_tcpClient.GetStream().Flush();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                m_usb.Dispose();
        }
    }
}
