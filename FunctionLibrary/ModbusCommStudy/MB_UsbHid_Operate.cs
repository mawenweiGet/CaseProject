using BaseClassLibrary;
using LocalTool;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FunctionLibrary.ModbusCommStudy
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
        public List<HIDInfoSet> m_connList = new List<HIDInfoSet>();
        /// <summary>
        /// 设备标识（多设设备连接区分使用）
        /// </summary>
        private const byte slaveID = 1;
        private void BeginConnect(string strDeviceDescription)
        {
            hidDevice.Connect(strDeviceDescription, true);
        }
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

                //USB设备连接字符串保存到setting中
                //Properties.Settings.Default.Default_UsbConnectionObj
                //    = usbDeviceName[0].ProductString;
                //Properties.Settings.Default.Save();

                SetConnectParameter();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #region 通讯属性参数相关
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
        ///<summary>连接是否在线，开关按钮用。设置为false时会自动断开连接</summary>
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
                OnPropertyChanged("");
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
        /// <summary>
        /// USB产品过滤大写字符（如ZHENZHU）
        /// </summary>
        public string ProductStr= "CHENZHU";
        /// <summary>
        /// 数据读写延时ms单位
        /// </summary>
        public const int Delay_ms = 0;
        #endregion
        #region 获取设备连接以及检查连接状态
        public void RefreshAllDevice()
        {
            try
            {
                m_connList = HIDDevice.GetInfoSets();

                this.m_listDeviceDescription.Clear();

                foreach (var item in m_connList)
                {
                    if (item.ProductString.ToUpper().Contains(ProductStr))
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
        /// <summary>
        /// 检查设备连接情况
        /// </summary>
        /// <returns></returns>
        private bool CheckInitPort()
        {
            if ((m_strDeviceSelected == "")
                || (m_strDeviceSelected == null))
            {
                throw new Exception("未设置设备连接字符串");
            }
            if (!IsConnectOnline)//未打开
            {
                if (!Init())//尝试打开，失败就提示
                {
                    hidDevice.Dispose();
                    throw new Exception("尝试设备连接失败");
                }
            }
            return true;
        }
        #endregion
        #region 读写数据信息
        /// <summary>
        /// 读取保持寄存器的连续块
        /// </summary>
        /// <param name="startAddress">开始位置</param>
        /// <param name="numberOfPoints">长度</param>
        /// <returns></returns>
        public short[] ReadHoldingRegisters(ushort startAddress, ushort numberOfPoints)
        {
            Thread.Sleep(Delay_ms);
            short[] data = new short[numberOfPoints];
            try
            {
                CheckInitPort();
                data = master.ReadHoldingRegisters(slaveID, startAddress, numberOfPoints);//可能抛出异常
            }
            catch (Exception e)
            {
                throw;
            }
            return data;
        }
        /// <summary>
        /// 读取输入寄存器的连续块
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="numberOfPoints"></param>
        /// <returns></returns>
        public short[] ReadInputRegisters(ushort startAddress, ushort numberOfPoints)
        {
            Thread.Sleep(Delay_ms);
            short[] data = new short[numberOfPoints];
            try
            {
                CheckInitPort();
                data = master.ReadInputRegisters(slaveID, startAddress, numberOfPoints);
            }
            catch (Exception e)
            {
                throw;
            }
            return data;
        }
        /// <summary>
        /// 写入由1到123个连续寄存器组成的块。
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="data"></param>
        public void WriteMultipleRegisters(ushort startAddress, short[] data)
        {
            Thread.Sleep(Delay_ms);
            try
            {
                CheckInitPort();
                master.WriteMultipleRegisters(slaveID, startAddress, data);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// 写入一个保持寄存器
        /// </summary>
        /// <param name="registerAddress"></param>
        /// <param name="value"></param>
        public void WriteSingleRegister(ushort registerAddress, short value)
        {
            Thread.Sleep(Delay_ms);
            try
            {
                CheckInitPort();
                master.WriteSingleRegister(slaveID, registerAddress, value);
            }
            catch (Exception e)
            {
                throw;
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
        #endregion
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
        /// <summary>
        /// 所有可连接设备信息
        /// </summary>
        public ObservableCollection<HIDInfoSet> m_listDeviceDescription = new ObservableCollection<HIDInfoSet>();
        
        private string _strDeviceSelected;
        /// <summary>
        /// 设备连接符
        /// </summary>
        public string m_strDeviceSelected{get{return _strDeviceSelected;}set{_strDeviceSelected = value;OnPropertyChanged();}}

    }
}
