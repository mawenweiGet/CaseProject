using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modbus.HelpClass
{
    public class MB_UsbHid_Operate : Modbus_UsbHid_Driver
    {
        ~MB_UsbHid_Operate()
        {
            if (hidDevice != null && hidDevice.IsConnected == true)
            {
                hidDevice.Disconnect();
            }
        }
        public IModbusSerialMaster master = null;
        public HIDDevice hidDevice = new HIDDevice();
        private void BeginConnect(string strDeviceDescription)
        {
            hidDevice.Connect(strDeviceDescription, true);
        }

        private const byte slaveID = 1;
        /// <summary>
        /// 建立TCP链接，使用前需给定通讯参数ModbusTcpData
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            try
            {
                BeginConnect(m_strDeviceSelected);

                var usbDeviceName = m_listDeviceDescription.Where
                    (x => x.DevicePath == m_strDeviceSelected).ToList();

                /* mww 2020.8.18 修改链接超时判断处理 */
                SetConnectParameter();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 通讯连接参数调整
        /// </summary>
        public void SetConnectParameter()
        {
            master = ModbusSerialMaster.CreateUsb(hidDevice);
            master.Transport.ReadTimeout = Convert.ToInt32(ReadTimeout);
            master.Transport.Retries = Convert.ToInt32(Retries);
            master.Transport.WaitToRetryMilliseconds = Convert.ToInt32(WaitToRetryMS);
            master.Transport.WriteTimeout = Convert.ToInt32(WriteTimeout);
        }

        public void RefreshAllDevice()
        {
            try
            {
                List<HIDInfoSet> m_connList = new List<HIDInfoSet>();
                m_connList = HIDDevice.GetInfoSets();

                this.m_listDeviceDescription.Clear();

                foreach (var item in m_connList)
                {
                    if (item.ProductString.ToUpper().Contains("CHENZHU"))
                    {
                        m_listDeviceDescription.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取USB设备报错:" + ex.Message);
            }
        }
        public bool CommandCheckInitPort()
        {
            if ((m_strDeviceSelected == "")
                   || (m_strDeviceSelected == null))
            {
                RefreshAllDevice();
                if (m_listDeviceDescription.Count > 0)
                {
                    m_strDeviceSelected = m_listDeviceDescription[0].DevicePath;
                }
            }
            if (!IsConnectOnline)//未打开
            {
                if (!Init())//尝试打开，失败就提示
                {
                    hidDevice.Dispose();
                    throw new Exception("errCOMNoConnect");
                }
            }
            return true;
        }
        private bool CheckInitPort()
        {
            if ((m_strDeviceSelected == "")
                || (m_strDeviceSelected == null))
            {
                throw new Exception("errCOMNoSelected");
            }
            if (!IsConnectOnline)//未打开
            {
                if (!Init())//尝试打开，失败就提示
                {
                    hidDevice.Dispose();
                    throw new Exception("errCOMNoConnect");
                }
            }
            return true;
        }


        public short[] ReadHoldingRegisters(ushort startAddress, ushort numberOfPoints)
        {
            Thread.Sleep(delayms);
            short[] data = new short[numberOfPoints];
            try
            {
                CheckInitPort();
                //Global.MBMutex.WaitOne();
                data = master.ReadHoldingRegisters(slaveID, startAddress, numberOfPoints);//可能抛出异常
                //Global.MBMutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                //Global.MBMutex.ReleaseMutex();
                throw;
            }
            return data;
        }

        const int delayms = 0;
        public short[] ReadInputRegisters(ushort startAddress, ushort numberOfPoints)
        {
            Thread.Sleep(delayms);
            short[] data = new short[numberOfPoints];
            try
            {
                CheckInitPort();
                //Global.MBMutex.WaitOne();
                data = master.ReadInputRegisters(slaveID, startAddress, numberOfPoints);
                //Global.MBMutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                //Global.MBMutex.ReleaseMutex();
            }
            return data;
        }

        public void WriteMultipleRegisters(ushort startAddress, short[] data)
        {
            Thread.Sleep(delayms);
            try
            {
                CheckInitPort();
                //Global.MBMutex.WaitOne();
                master.WriteMultipleRegisters(slaveID, startAddress, data);
                //Global.MBMutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                //Global.MBMutex.ReleaseMutex();
            }
        }

        public void WriteSingleRegister(ushort registerAddress, short value)
        {
            Thread.Sleep(delayms);
            try
            {
                CheckInitPort();
                //Global.MBMutex.WaitOne();
                master.WriteSingleRegister(slaveID, registerAddress, value);
                //Global.MBMutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                //Global.MBMutex.ReleaseMutex();
            }
        }

        ///<summary>写双寄存器Float数据</summary>
        public void WriteFloatDoubleRegister(ushort registerAddress, string valuestr)
        {
            CheckInitPort();
            byte[] bytes = BitConverter.GetBytes(Convert.ToSingle(valuestr));
            //后面是高字节 abcd  高低字节互换
            short datal = (short)(bytes[1] * 256 + bytes[0]);
            short datah = (short)(bytes[3] * 256 + bytes[2]);
            master.WriteSingleRegister(slaveID, registerAddress, datal);
            master.WriteSingleRegister(slaveID, ++registerAddress, datah);
        }
        ///<summary>读双寄存器Float数据</summary>
        public float ReadFloatDoubleRegister(ushort registerAddress)
        {
            CheckInitPort();
            short[] regs = master.ReadHoldingRegisters(slaveID, registerAddress, 2);

            byte[] lb = BitConverter.GetBytes(regs[0]);
            lb.Reverse();//高低字节互换
            byte[] hb = BitConverter.GetBytes(regs[1]);
            hb.Reverse();//高低字节互换
            byte[] data = new byte[4];
            lb.CopyTo(data, 0);
            hb.CopyTo(data, 2);
            return BitConverter.ToSingle(data, 0);
        }

        ///<summary>连接是否在线，开关按钮用。设置为0时会自动断开连接</summary>
        public bool IsConnectOnline
        {
            get
            {
                bool bvalue = false;
                if (hidDevice != null)
                {
                    bvalue = hidDevice.IsConnected;
                }
                return bvalue;
            }
            set
            {
                if (!value
                    && hidDevice != null
                    && hidDevice.IsConnected == true)
                {
                    hidDevice.Disconnect();
                }
            }
        }
        ///<summary>通讯连接连接状态，开关按钮用</summary>
        public string ConnectStateStr
        {
            get
            {
                string str = "离线";
                if (IsConnectOnline)
                {
                    str = "在线";
                }
                else
                {
                    str = "离线";
                }
                return str;
            }
        }


    }

    ///<summary>MobusTCP通讯参数</summary>
    public class Modbus_UsbHid_Driver : BaseModel
    {
        private string intervalTime = "50";
        ///<summary>通讯类完成一次通讯的间隔</summary>
        public string IntervalTime { get => intervalTime; set { intervalTime = value; OnPropertyChanged(); } }

        private string writeTimeout = "5000";
        ///<summary>写超时时间</summary>
        public string WriteTimeout { get => writeTimeout; set { writeTimeout = value; OnPropertyChanged(); } }

        ///<summary>读超时时间</summary>
        private string readTimeout = "5000";
        public string ReadTimeout { get => readTimeout; set { readTimeout = value; OnPropertyChanged(); } }

        private string retries = "3";
        ///<summary>重试次数</summary>
        public string Retries { get => retries; set { retries = value; OnPropertyChanged(); } }

        private string waitToRetryMS = "1000";
        ///<summary>等待重试间隔</summary>
        public string WaitToRetryMS { get => waitToRetryMS; set { waitToRetryMS = value; OnPropertyChanged(); } }



        public ObservableCollection<HIDInfoSet> m_listDeviceDescription = new ObservableCollection<HIDInfoSet>();
        private string _strDeviceSelected;
        public string m_strDeviceSelected
        {
            get
            {
                return _strDeviceSelected;
            }
            set
            {
                _strDeviceSelected = value;
                OnPropertyChanged();
            }
        }

    }
}
