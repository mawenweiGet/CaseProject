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
        //����Event��ӦUSB���ݷ���֪ͨ�¼�       
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
            //�յ�USB�������ݽ����֪ͨEvent�¼���Ӧ
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
            //��ʼ��Event����ʼ�ź�״̬Ϊfalse
            event_RWCtrl = new ManualResetEventSlim(false);
        }

        public int Read(byte[] buffer, int offset, int size)
        {
            if (bFlag==true)
            {
                //����Event�¼���Ӧ�̣߳��ж�ָ��ʱ�����Ƿ��л�ȡ��Ӧ�¼�����ȡʧ�ܲ��ڵȴ��Ժ��ȡ
                bool bFlagTimeResult = event_RWCtrl.WaitHandle.WaitOne(m_usb.ReadTimeout, false);
                if (bFlagTimeResult == false)
                {
                    //�׳��쳣��Ϣ���ϲ㴦��
                    throw (new Exception("USB ���ؽ����Ӧ��ʱ"));
                }
                //�ر�֪ͨ��Ӧ�¼�
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
