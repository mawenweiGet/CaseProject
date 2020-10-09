using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modbus.Device
{
	#region 自定义异常
	/// <summary>
	/// 通用HID设备异常
	/// </summary>
	public class HIDDeviceException : ApplicationException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="strMessage"></param>
		public HIDDeviceException(string strMessage) : base(strMessage) { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strMessage"></param>
		/// <returns></returns>
		public static HIDDeviceException GenerateWithWinError(string strMessage)
		{
			return new HIDDeviceException(string.Format("Msg:{0} WinEr:{1:X8}", strMessage, Marshal.GetLastWin32Error()));
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="strMessage"></param>
		/// <returns></returns>
		public static HIDDeviceException GenerateError(string strMessage)
		{
			return new HIDDeviceException(string.Format("Msg:{0}", strMessage));
		}
	}
	#endregion
	/// <summary>
	/// HID USB Device
	/// </summary>
	public class HIDDevice : Win32Usb, IDisposable
	{
		#region 事件句柄
		/// <summary>
		/// 当设备已发送新数据时调用事件处理程序
		/// </summary>
		public event DataRecievedEventHandler OnDataRecieved;

		/// <summary>
		/// 当设备获取新数据时调用事件处理程序
		/// </summary>
		public event DataSendEventHandler OnDataSend;

		/// <summary>
		/// 删除设备时调用的事件处理程序
		/// </summary>
		public event EventHandler OnDeviceRemoved;

		/// <summary>
		/// 当设备到达时调用事件处理程序
		/// </summary>
		public event EventHandler OnDeviceArrived;
		#endregion

		#region 私有变量
		/// <summary>用来读/写的文件流</summary>
		private FileStream m_oFile;
		/// <summary>输入报告的长度：由设备提供</summary>
		private int m_inputReportLength;
		/// <summary>输出报告的长度：由设备给</summary>
		private int m_outputReportLength;
		/// <summary>设备的句柄</summary>
		private IntPtr m_hHandle;

		/// <summary>
		/// 连接的设备信息变量
		/// </summary>
		private string m_manufacturerString;
		private string m_productString;
		private string m_serialNumberString;
		private string m_devicePath;
		private UInt16 m_version;
		private UInt16 m_vendorId;
		private UInt16 m_productId;
		private bool m_isConnected;

		//超时参数
		private int m_ReadTimeout;
		private int m_WriteTimeout;

		/// <summary>
		/// 读取模式（是否异步）
		/// </summary>
		private bool m_useAsyncReads;

		/// <summary>
		/// 设备已到达/已移除
		/// </summary>
		private readonly Guid m_deviceClass;
		private IntPtr m_usbEventHandle;
		private IntPtr m_formHandle;
		#endregion
		#region 初始化设备信息
		/// <summary>
		/// 
		/// </summary>
		public HIDDevice()
		{
			m_hHandle = IntPtr.Zero;
			m_isConnected = false;
			m_deviceClass = HIDGuid;
		}

		#endregion

		#region 资源释放处理
		/// <summary>
		/// 释放资源
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Dispose和Finale调用的处理程序
		/// </summary>
		/// <param name="bDisposing">是否为真</param>
		protected virtual void Dispose(bool bDisposing)
		{
			try
			{
				// 关闭托管资源
				if (bDisposing)
				{
					if (m_oFile != null)
					{
						m_oFile.Close();
						m_oFile = null;
					}
				}

				// 处理并最终确定，清除非托管资源
				if (m_hHandle != IntPtr.Zero)
				{
					CloseHandle(m_hHandle);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
		#endregion

		#region 已连接设备信息

		/// <summary>
		/// 标识制造商的名称
		/// </summary>
		public string ManufacturerString
		{
			get { return m_manufacturerString; }
		}

		/// <summary>
		/// 标识产品名称
		/// </summary>
		public string ProductString
		{
			get { return m_productString; }
		}

		/// <summary>
		/// 序列号
		/// </summary>
		public string SerialNumberString
		{
			get { return m_serialNumberString; }
		}

		/// <summary>
		/// 产品版本
		/// </summary>
		public UInt16 Version
		{
			get { return m_version; }
		}

		/// <summary>
		/// BCD格式的产品版本（字符串）
		/// </summary>
		public string VersionInBCD
		{
			get
			{
				if (((byte)(Version >> 24)) == 0)
				{
					return String.Format("{0}.{1}{2}", (byte)(m_version >> 16), (byte)(m_version >> 8), (byte)m_version);
				}

				return String.Format("{0}{1}.{2}{3}", (byte)(m_version >> 24), (byte)(m_version >> 16), (byte)(m_version >> 8), (byte)m_version);
			}
		}

		/// <summary>
		/// 完整设备文件路径
		/// </summary>
		public string DevicePath
		{
			get { return m_devicePath; }
		}

		/// <summary>
		/// Vendor ID
		/// </summary>
		public UInt16 VendorID
		{
			get { return m_vendorId; }
		}

		/// <summary>
		/// Product ID
		/// </summary>
		public UInt16 ProductID
		{
			get { return m_productId; }
		}

		/// <summary>
		/// 输入报告长度
		/// </summary>
		public short InReportLength
		{
			get { return (short)(m_inputReportLength > 0 ? m_inputReportLength - 1 : m_inputReportLength); }
		}

		/// <summary>
		/// 输出报告长度
		/// </summary>
		public short OutReportLength
		{
			get { return (short)(m_outputReportLength > 0 ? m_outputReportLength - 1 : m_outputReportLength); }
		}

		/// <summary>
		/// 链接状态
		/// </summary>
		public bool IsConnected
		{
			get { return m_isConnected; }
		}

		/// <summary>
		/// 读物USB数据响应超时时间
		/// </summary>
		public int ReadTimeout
		{
			get { return m_ReadTimeout; }
			set { m_ReadTimeout = value; }
		}
		/// <summary>
		/// 写入USB数据响应超时时间
		/// </summary>
		public int WriteTimeout
		{
			get { return m_WriteTimeout; }
			set { m_WriteTimeout = value; }
		}

		/// <summary>
		/// 是否异步读取（即：是否主动去获取数据，异步可通过）
		/// </summary>
		public bool IsAsyncRead
		{
			get { return m_useAsyncReads; }
		}

		#endregion

		#region 本地方法

		/// <summary>
		/// 链接设备
		/// </summary>
		public bool Connect(short vid, short pid, bool useAsyncRead)
		{
			var connectedDeviceList = GetInfoSets(vid, pid);

			if (connectedDeviceList.Count > 0)
			{
				return Connect(connectedDeviceList[0].DevicePath, useAsyncRead);
			}

			return false;
		}

		/// <summary>
		/// 链接设备,指定是否异步读取
		/// </summary>
		public bool Connect(string devicePath, bool useAsyncRead)
		{
			if (m_isConnected) return false;

			// 从设备路径创建文件
			if (useAsyncRead)//是否异步读取
			{
				m_hHandle = CreateFile(devicePath, GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
			}
			else
			{
				m_hHandle = CreateFile(devicePath, GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
			}
			if (m_hHandle != InvalidHandleValue || m_hHandle == null)   // 是否开放式...
			{
				IntPtr lpData;
				if (HidD_GetPreparsedData(m_hHandle, out lpData))   // 让windows将设备数据读入内部缓冲区
				{
					try
					{
						// 获取HID设备信息
						m_devicePath = devicePath;
						m_manufacturerString = GetManufacturerString(m_hHandle);
						m_productString = GetProductString(m_hHandle);
						m_serialNumberString = GetSerialNumber(m_hHandle);

						HIDD_ATTRIBUTES attributes;
						GetAttr(m_hHandle, out attributes);
						m_version = attributes.VersionNumber;
						m_vendorId = attributes.VendorID;
						m_productId = attributes.ProductID;

						HidP_Caps oCaps;
						HidP_GetCaps(lpData, out oCaps);    // 从内部缓冲区提取设备功能
						m_inputReportLength = oCaps.InputReportByteLength;  // 获取输入...
						m_outputReportLength = oCaps.OutputReportByteLength;    // ... 输出报告长度


						//m_oFile = new FileStream(m_hHandle, FileAccess.Read | FileAccess.Write, true, m_nInputReportLength, true);
						m_oFile = new FileStream(new SafeFileHandle(m_hHandle, false), FileAccess.Read | FileAccess.Write, m_inputReportLength, useAsyncRead);
						//m_oFile.ReadTimeout = m_ReadTimeout;
						//m_oFile.WriteTimeout = m_WriteTimeout;
					}
					catch (Exception)
					{
						throw HIDDeviceException.GenerateWithWinError("无法从hid获取详细数据");
					}
					finally
					{
						HidD_FreePreparsedData(ref lpData); // 在退出函数之前，必须释放GetPreparsedData中保留的内部缓冲区
					}
				}
				else    // GetPreparsedData 失败?返回异常
				{
					throw HIDDeviceException.GenerateWithWinError("获取准备数据失败");
				}
			}
			else    // 文件打开失败，返回异常
			{
				m_hHandle = IntPtr.Zero;
				throw HIDDeviceException.GenerateWithWinError("无法创建设备文件");
			}

			m_useAsyncReads = useAsyncRead;
			m_isConnected = true;

			if (m_useAsyncReads) BeginAsyncRead();  // 是否开启，异步读取

			return true;
		}

		/// <summary>
		/// 断开与设备的连接
		/// </summary>
		public void Disconnect()
		{
			if (!m_isConnected) return;

			m_isConnected = false;

			m_manufacturerString = String.Empty;
			m_productString = String.Empty;
			m_serialNumberString = String.Empty;
			m_devicePath = String.Empty;
			m_outputReportLength = 0;
			m_inputReportLength = 0;
			m_productId = 0;
			m_vendorId = 0;
			m_version = 0;

			if (m_oFile != null)
			{
				m_oFile.Close();
				m_oFile = null;
			}

			if (m_hHandle != IntPtr.Zero)
			{
				CloseHandle(m_hHandle);
			}
		}

		/// <summary>
		/// 删除设备时要执行的任何操作的虚拟处理程序。重写以使用
		/// </summary>
		protected virtual void HandleDataSend(HIDReport report)
		{
			OnDataSend?.Invoke(this, report);
		}

		/// <summary>
		/// 数据写入设备
		/// </summary>
		public bool Write(HIDReport report)
		{
			if (!m_isConnected)
			{
				return false;
			}

			if (report.Length > m_outputReportLength)
			{
				throw new Exception("Report len must not exceed " +
									m_outputReportLength.ToString(CultureInfo.InvariantCulture) +
									" bytes");
			}

			try
			{
				m_oFile.Write(report.ToArray(), 0, report.Length);
				HandleDataSend(report);
			}
			catch
			{
				return false; // The device was removed!
			}

			return true;
		}
		/// <summary>
		/// 写入数据
		/// </summary>
		/// <param name="array">数据字节</param>
		/// <param name="offset">开始位</param>
		/// <param name="count">结束位</param>
		/// <returns></returns>
		public bool Write(byte[] array, int offset, int count)
		{
			Byte[] res = new Byte[m_outputReportLength];
			if (!m_isConnected)
			{
				return false;
			}

			if (array.Length > m_outputReportLength)
			{
				throw new Exception("Report len must not exceed " +
									m_outputReportLength.ToString(CultureInfo.InvariantCulture) +
									" bytes");
			}
			else if (array.Length < m_outputReportLength)
			{
				res[0] = 0;
				for (int i = 0; i < m_outputReportLength; i++)
				{
					if (i < array.Length)
					{
						res[i + 1] = array[i];
					}
				}
			}
			else
			{
				res = array;
			}
			try
			{
				m_oFile.Write(res.ToArray(), offset, res.Length);
				HIDReport report = new HIDReport(0, array);
				HandleDataSend(report);
			}
			catch
			{
				return false; //设备已被删除！
			}

			return true;
		}
		/// <summary>
		/// 接收数据时要执行的任何操作的虚拟处理程序。重写以使用。
		/// </summary>
		/// <param name="report"></param>
		protected virtual void HandleDataReceived(HIDReport report)
		{
			//如果已分配，则激发事件处理程序
			OnDataRecieved?.Invoke(this, report);
		}

		/// <summary>
		/// 启动异步读取，在读取数据或设备时完成
		/// 已断开连接。使用回调。
		/// </summary>
		private void BeginAsyncRead()
		{
			var arrInputReport = new byte[m_inputReportLength];
			// 把我们用来接收东西的buff作为异步状态，这样当读取完成时我们就可以得到它了
			m_oFile.BeginRead(arrInputReport, 0, m_inputReportLength, new AsyncCallback(ReadCompleted), arrInputReport);
		}

		/// <summary>
		/// 回拨以上内容。请注意这一点，因为它将在异步读取的后台线程上调用
		/// </summary>
		/// <param name="iResult">异步结果参数</param>
		protected void ReadCompleted(IAsyncResult iResult)
		{
			if (!m_isConnected || (m_oFile == null)) return;

			// 检索读取缓冲区
			var arrBuff = (byte[])iResult.AsyncState;

			try
			{
				m_oFile.EndRead(iResult);   // call end read : 这将抛出在读取期间发生的任何异常

				try
				{
					HandleDataReceived(new HIDReport(arrBuff)); // 将新的输入报告传递给更高级别的处理程序
				}
				finally
				{
					BeginAsyncRead();   // 当所有这些都完成后，为下一个报告开始另一个阅读
				}
			}
			catch (IOException)  // 如果我们遇到IO异常，设备就会被移除
			{
				//手动设备移动 
				OnDeviceRemoved?.Invoke(this, new EventArgs());

				Dispose();

				// 设备已被删除！
			}
			catch (Exception)
			{
				// 设备已被删除！
			}
		}

		/// <summary>
		/// 此读取功能用于正常的同步读取
		/// </summary>
		/// <returns></returns>
		public HIDReport Read()
		{
			var readBuf = new byte[m_inputReportLength];
			if (m_useAsyncReads)
			{
				throw new Exception("在异步模式下操作时，无法执行同步读取");
			}
			//调用 readFile
			try
			{
				m_oFile.Read(readBuf, 0, readBuf.Length);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return new HIDReport(readBuf);
		}
		/// <summary>
		/// 同步读取指定段数据
		/// </summary>
		/// <param name="array"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public int Read(byte[] array, int offset, int count)
		{
			var readBuf = new byte[m_outputReportLength];
			return m_oFile.Read(readBuf, 0, readBuf.Length);
			//return m_oFile.Read(array, offset, count);
		}
		#endregion

		#region 静态法

		/// <summary>
		/// 返回已连接USB HID设备的列表信息
		/// </summary>
		public static List<HIDInfoSet> GetInfoSets()
		{
			try
			{
				return GetInfoSets(null, null);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// 返回已连接USB HID设备的列表信息
		/// </summary>
		public static List<HIDInfoSet> GetInfoSets(int VendorID)
		{
			try
			{
				return GetInfoSets(VendorID, null, null);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		///返回已连接USB HID设备的列表信息
		/// </summary>
		public static List<HIDInfoSet> GetInfoSets(int? VendorID, int? ProductID)
		{
			try
			{
				return GetInfoSets(VendorID, ProductID, null);
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// 返回已连接USB HID设备的列表信息
		/// </summary>
		public static List<HIDInfoSet> GetInfoSets(int? VendorID, int? ProductID, string SerialNumber)
		{
			string strSearch;


			if ((VendorID != null) && (ProductID != null))
			{
				strSearch = string.Format("vid_{0:x4}&pid_{1:x4}", VendorID, ProductID); // 首先，构建路径搜索字符串
			}
			else if (VendorID != null)
			{
				strSearch = string.Format("vid_{0:x4}&pid_", VendorID); // 首先，构建路径搜索字符串
			}
			else
			{
				strSearch = "vid_";
			}

			var connectedDeviceList = new List<HIDInfoSet>();
			var gHid = HIDGuid;     // 从Windows获取它用来表示HID-USB接口的GUID
			var hInfoSet = SetupDiGetClassDevs(ref gHid, null, IntPtr.Zero, DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);

			if (hInfoSet == InvalidHandleValue)
			{
				throw new Win32Exception();
			}

			try
			{
				var oInterface = new DeviceInterfaceData();
				oInterface.Size = Marshal.SizeOf(oInterface);
				int nIndex = 0;

				while (SetupDiEnumDeviceInterfaces(hInfoSet, 0, ref gHid, (uint)nIndex, ref oInterface))
				{
					var strDevicePath = GetDevicePath(hInfoSet, ref oInterface);

					// 做一个字符串搜索，如果我们找到了VID/PID字符串，那么我们就找到了我们的设备！
					if (strDevicePath.IndexOf(strSearch) >= 0)
					{
						var handle = Open(strDevicePath);
						if (handle != InvalidHandleValue)
						{
							IntPtr ptrToPreParsedData;
							if (HidD_GetPreparsedData(handle, out ptrToPreParsedData))
							{
								HIDD_ATTRIBUTES attributes;
								HidP_Caps caps;

								// 检查设备序列号（如果指定）
								if (((SerialNumber != null) && (GetSerialNumber(handle) == SerialNumber)) || (SerialNumber == null))
								{
									//获取HID设备信息
									var manufacturerString = GetManufacturerString(handle);
									var productString = GetProductString(handle);
									var serialNumberString = GetSerialNumber(handle);
									GetAttr(handle, out attributes);
									GetCaps(ptrToPreParsedData, out caps);

									// 将发现的HID设备添加到连接列表
									connectedDeviceList.Add(new HIDInfoSet(manufacturerString,
																		   productString,
																		   serialNumberString,
																		   strDevicePath,
																		   attributes.VendorID,
																		   attributes.ProductID,
																		   attributes.VersionNumber,
																		   caps.InputReportByteLength,
																		   caps.OutputReportByteLength));
								}

								if (handle != IntPtr.Zero)
								{
									CloseHandle(handle);
								}
							}
						}
					}
					nIndex++;
				}
			}
			catch (Exception ex)
			{
				throw HIDDeviceException.GenerateError(ex.ToString());				
			}
			finally
			{
				// 在开始之前，必须释放SetupDiGetClassDevs保留的InfoSet内存
				SetupDiDestroyDeviceInfoList(hInfoSet);
			}

			return connectedDeviceList;
		}

		/// <summary>
		/// 打开USB HID设备
		/// </summary>
		private static IntPtr Open(string devicePath)
		{
			return CreateFile(devicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, IntPtr.Zero);
		}

		/// <summary>
		/// 获取USB HID设备盖
		/// </summary>
		private static void GetCaps(IntPtr handle, out HidP_Caps caps)
		{
			if (HidP_GetCaps(handle, out caps) == 0)
			{
				throw new Win32Exception();
			}
		}

		/// <summary>
		/// 获取USB HID设备属性
		/// </summary>
		private static void GetAttr(IntPtr handle, out HIDD_ATTRIBUTES attributes)
		{
			attributes = new HIDD_ATTRIBUTES();
			attributes.Size = Marshal.SizeOf(attributes);
			if (HidD_GetAttributes(handle, ref attributes) == false)
			{
				throw new Win32Exception();
			}
		}

		/// <summary>
		/// 获取USB HID设备制造商字符串
		/// </summary>
		private static string GetManufacturerString(IntPtr handle)
		{
			var manufacturerString = new StringBuilder(128);
			if (HidD_GetManufacturerString(handle, manufacturerString, manufacturerString.Capacity) == false)
			{
				throw new Win32Exception();
			}
			return manufacturerString.ToString();
		}

		/// <summary>
		/// 获取USB HID设备产品字符串
		/// </summary>
		private static string GetProductString(IntPtr handle)
		{
			var productString = new StringBuilder(128);
			if (HidD_GetProductString(handle, productString, productString.Capacity) == false)
			{
				throw new Win32Exception();
			}
			return productString.ToString();
		}

		/// <summary>
		/// 获取USB HID设备序列号
		/// </summary>
		private static string GetSerialNumber(IntPtr handle)
		{
			var serialNumberString = new StringBuilder(127);
			if (HidD_GetSerialNumberString(handle, serialNumberString, serialNumberString.Capacity) == false)
			{
				//throw new Win32Exception();
				return "";
			}
			return serialNumberString.ToString();
		}

		/// <summary>
		/// 获取USB HID设备路径
		/// </summary>
		private static string GetDevicePath(IntPtr hInfoSet, ref DeviceInterfaceData oInterface)
		{
			uint nRequiredSize = 0;
			if (SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, IntPtr.Zero, 0, ref nRequiredSize, IntPtr.Zero) == false)
			{
				// TODO: Find a solution
				// Tip: http://stackoverflow.com/questions/1054748/setupdigetdeviceinterfacedetail-unexplainable-error
				//throw new HIDDeviceException();
			}
			var oDetail = new DeviceInterfaceDetailData();
			oDetail.Size = Marshal.SizeOf(typeof(IntPtr)) == 8 ? 8 : 5; // x86/x64 magic...
			if (SetupDiGetDeviceInterfaceDetail(hInfoSet, ref oInterface, ref oDetail, nRequiredSize, ref nRequiredSize, IntPtr.Zero) == false)
			{
				throw new Win32Exception();
			}
			return oDetail.DevicePath;
		}

		#endregion

		#region 公共方法
		/// <summary>
		/// 注册此应用程序，因此它将收到有关usb事件的通知。
		/// </summary>
		/// <param name="Handle">一个IntPtr，它是应用程序的句柄</param>
		/// <example> 此示例演示如何在窗体中实现此方法。
		/// <code>
		///protected override void OnHandleCreated(EventArgs e)
		///{
		///    base.OnHandleCreated(e);
		///    usb.RegisterHandle(Handle);
		///}
		///</code>
		///</example>
		public void RegisterHandle(IntPtr Handle)
		{
			m_usbEventHandle = RegisterForUsbEvents(Handle, m_deviceClass);
			m_formHandle = Handle;
		}

		/// <summary>
		/// 取消注册此应用程序，因此不会收到有关usb事件的通知
		/// </summary>
		/// <returns>如果注销成功，则返回</returns>
		public bool UnregisterHandle()
		{
			if (m_formHandle != null)
			{
				return UnregisterForUsbEvents(m_formHandle);
			}

			return false;
		}
		/// <summary>
		/// 设备插拔变动信息
		/// </summary>
		/// <param name="m"></param>
		public void ParseMessages(ref int m)
		{
			if (m == WM_DEVICECHANGE)   // 收到设备移除信息
			{
				switch (m)
				{
					case WM_DEVICECHANGE:    // inserted
						OnDeviceArrived?.Invoke(this, new EventArgs());
						break;

					case DEVICE_REMOVECOMPLETE: // removed
						OnDeviceRemoved?.Invoke(this, new EventArgs());
						break;
				}
			}
		}
		#endregion
	}
	#region 辅助类
	/// <summary>
	/// 
	/// </summary>
	public class Win32Usb
	{
		#region 常量
		/// <summary>Windows message sent when a device is inserted or removed</summary>
		public const int WM_DEVICECHANGE = 0x0219;
		/// <summary>WParam for above : A device was inserted</summary>
		public const int DEVICE_ARRIVAL = 0x8000;
		/// <summary>WParam for above : A device was removed</summary>
		public const int DEVICE_REMOVECOMPLETE = 0x8004;
		/// <summary>Used in SetupDiClassDevs to get devices present in the system</summary>
		protected const int DIGCF_PRESENT = 0x02;
		/// <summary>Used in SetupDiClassDevs to get device interface details</summary>
		protected const int DIGCF_DEVICEINTERFACE = 0x10;
		/// <summary>Used when registering for device insert/remove messages : specifies the type of device</summary>
		protected const int DEVTYP_DEVICEINTERFACE = 0x05;
		/// <summary>Used when registering for device insert/remove messages : we're giving the API call a window handle</summary>
		protected const int DEVICE_NOTIFY_WINDOW_HANDLE = 0;
		/// <summary>Purges Win32 transmit buffer by aborting the current transmission.</summary>
		protected const uint PURGE_TXABORT = 0x01;
		/// <summary>Purges Win32 receive buffer by aborting the current receive.</summary>
		protected const uint PURGE_RXABORT = 0x02;
		/// <summary>Purges Win32 transmit buffer by clearing it.</summary>
		protected const uint PURGE_TXCLEAR = 0x04;
		/// <summary>Purges Win32 receive buffer by clearing it.</summary>
		protected const uint PURGE_RXCLEAR = 0x08;
		/// <summary>CreateFile : Open file for read</summary>
		protected const uint GENERIC_READ = 0x80000000;
		/// <summary>CreateFile : Open file for write</summary>
		protected const uint GENERIC_WRITE = 0x40000000;
		/// <summary>CreateFile : file share for write</summary>
		protected const uint FILE_SHARE_WRITE = 0x2;
		/// <summary>CreateFile : file share for read</summary>
		protected const uint FILE_SHARE_READ = 0x1;
		/// <summary>CreateFile : Open handle for overlapped operations</summary>
		protected const uint FILE_FLAG_OVERLAPPED = 0x40000000;
		/// <summary>CreateFile : Resource to be "created" must exist</summary>
		protected const uint OPEN_EXISTING = 3;
		/// <summary>CreateFile : Resource will be "created" or existing will be used</summary>
		protected const uint OPEN_ALWAYS = 4;
		/// <summary>ReadFile/WriteFile : Overlapped operation is incomplete.</summary>
		protected const uint ERROR_IO_PENDING = 997;
		/// <summary>Infinite timeout</summary>
		protected const uint INFINITE = 0xFFFFFFFF;
		/// <summary>Simple representation of a null handle : a closed stream will get this handle. Note it is public for comparison by higher level classes.</summary>
		public static IntPtr NullHandle = IntPtr.Zero;
		/// <summary>Simple representation of the handle returned when CreateFile fails.</summary>
		protected static IntPtr InvalidHandleValue = new IntPtr(-1);
		#endregion

		#region hid.dll
		/// <summary>
		/// 
		/// </summary>
		public const Int16 HidP_Input = 0;
		/// <summary>
		/// /
		/// </summary>
		public const Int16 HidP_Output = 1;
		/// <summary>
		/// 
		/// </summary>
		public const Int16 HidP_Feature = 2;

		/// <summary>
		/// 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]	
		protected struct HIDD_ATTRIBUTES
		{
			/// <summary>
			/// 
			/// </summary>
			public Int32 Size;
			/// <summary>
			/// 
			/// </summary>
			public UInt16 VendorID;
			/// <summary>
			/// 
			/// </summary>
			public UInt16 ProductID;
			/// <summary>
			/// 
			/// </summary>
			public UInt16 VersionNumber;
		}

		/// <summary>
		/// Provides the capabilities of a HID device
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		protected struct HidP_Caps
		{
			/// <summary>
			/// 
			/// </summary>
			public short Usage;
			/// <summary>
			/// 
			/// </summary>
			public short UsagePage;
			/// <summary>
			/// 
			/// </summary>
			public short InputReportByteLength;
			/// <summary>
			/// 
			/// </summary>
			public short OutputReportByteLength;
			/// <summary>
			/// 
			/// </summary>
			public short FeatureReportByteLength;
			/// <summary>
			/// 
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
			public short[] Reserved;
			/// <summary>
			/// 
			/// </summary>
			public short NumberLinkCollectionNodes;
			/// <summary>
			/// 
			/// </summary>
			public short NumberInputButtonCaps;
			/// <summary>
			/// 
			/// </summary>
			public short NumberInputValueCaps;
			/// <summary>
			/// 
			/// </summary>
			public short NumberInputDataIndices;
			/// <summary>
			/// 
			/// </summary>
			public short NumberOutputButtonCaps;
			/// <summary>
			/// 
			/// </summary>
			public short NumberOutputValueCaps;
			/// <summary>
			/// 
			/// </summary>
			public short NumberOutputDataIndices;
			/// <summary>
			/// 
			/// </summary>
			public short NumberFeatureButtonCaps;
			/// <summary>
			/// 
			/// </summary>
			public short NumberFeatureValueCaps;
			/// <summary>
			/// 
			/// </summary>
			public short NumberFeatureDataIndices;
		}

		//  If IsRange is false, UsageMin is the Usage and UsageMax is unused.
		//  If IsStringRange is false, StringMin is the String index and StringMax is unused.
		//  If IsDesignatorRange is false, DesignatorMin is the designator index and DesignatorMax is unused.
		/// <summary>
		/// 
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		protected struct HidP_Value_Caps
		{
			/// <summary>
			/// 
			/// </summary>
			public Int16 UsagePage;
			/// <summary>
			/// 
			/// </summary>
			public Byte ReportID;
			/// <summary>
			/// 
			/// </summary>
			public Int32 IsAlias;
			/// <summary>
			/// /
			/// </summary>
			public Int16 BitField;
			/// <summary>
			/// 
			/// </summary>
			public Int16 LinkCollection;
			/// <summary>
			/// 
			/// </summary>
			public Int16 LinkUsage;
			/// <summary>
			/// 
			/// </summary>
			public Int16 LinkUsagePage;
			/// <summary>
			/// 
			/// </summary>
			public Int32 IsRange;
			/// <summary>
			/// 
			/// </summary>
			public Int32 IsStringRange;
			/// <summary>
			/// 
			/// 
			/// </summary>
			public Int32 IsDesignatorRange;
			/// <summary>
			/// 
			/// </summary>
			public Int32 IsAbsolute;
			/// <summary>
			/// 
			/// </summary>
			public Int32 HasNull;
			/// <summary>
			/// 
			/// </summary>
			public Byte Reserved;
			/// <summary>
			/// 
			/// </summary>
			public Int16 BitSize;
			/// <summary>
			/// 
			/// </summary>
			public Int16 ReportCount;
			/// <summary>
			/// 
			/// </summary>
			public Int16 Reserved2;
			/// <summary>
			/// 
			/// </summary>
			public Int16 Reserved3;
			/// <summary>
			/// 
			/// </summary>
			public Int16 Reserved4;
			/// <summary>
			/// 
			/// </summary>
			public Int16 Reserved5;
			/// <summary>
			/// 
			/// </summary>
			public Int16 Reserved6;
			/// <summary>
			/// 
			/// </summary>
			public Int32 LogicalMin;
			/// <summary>
			/// 
			/// </summary>
			public Int32 LogicalMax;
			/// <summary>
			/// 
			/// </summary>
			public Int32 PhysicalMin;
			/// <summary>
			/// 
			/// </summary>
			public Int32 PhysicalMax;
			/// <summary>
			/// 
			/// </summary>
			public Int16 UsageMin;
			/// <summary>
			/// 
			/// </summary>
			public Int16 UsageMax;
			/// <summary>
			/// 
			/// </summary>
			public Int16 StringMin;
			/// <summary>
			/// 
			/// </summary>
			public Int16 StringMax;
			/// <summary>
			/// 
			/// </summary>
			public Int16 DesignatorMin;
			/// <summary>
			/// 
			/// </summary>
			public Int16 DesignatorMax;
			/// <summary>
			/// 
			/// </summary>
			public Int16 DataIndexMin;
			/// <summary>
			/// 
			/// </summary>
			public Int16 DataIndexMax;
		}

		/// <summary>
		/// Gets the GUID that Windows uses to represent HID class devices
		/// </summary>
		/// <param name="gHid">An out parameter to take the Guid</param>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern void HidD_GetHidGuid(out Guid gHid);

		/// <summary>
		/// Flush Queue
		/// </summary>
		/// <param name="hFile">Device file handle</param>
		/// <returns></returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern bool HidD_FlushQueue(IntPtr hFile);

		/// <summary>
		/// Gets details from an open device. Reserves a block of memory which must be freed.
		/// </summary>
		/// <param name="hFile">Device file handle</param>
		/// <param name="lpData">Reference to the preparsed data block</param>
		/// <returns></returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern bool HidD_GetPreparsedData(IntPtr hFile, out IntPtr lpData);

		/// <summary>
		/// Frees the memory block reserved above.
		/// </summary>
		/// <param name="pData">Reference to preparsed data returned in call to GetPreparsedData</param>
		/// <returns></returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern bool HidD_FreePreparsedData(ref IntPtr pData);

		/// <summary>
		/// Gets a device's capabilities from the preparsed data.
		/// </summary>
		/// <param name="lpData">Preparsed data reference</param>
		/// <param name="oCaps">HidCaps structure to receive the capabilities</param>
		/// <returns>True if successful</returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern int HidP_GetCaps(IntPtr lpData, out HidP_Caps oCaps);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ReportType"></param>
		/// <param name="ValueCaps"></param>
		/// <param name="ValueCapsLength"></param>
		/// <param name="PreparsedData"></param>
		/// <returns></returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern int HidP_GetValueCaps(Int16 ReportType, ref Byte ValueCaps, ref Int16 ValueCapsLength, IntPtr PreparsedData);


		// http://msdn.microsoft.com/en-us/library/windows/hardware/ff538959%28v=vs.85%29.aspx
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="buffer"></param>
		/// <param name="bufferLength"></param>
		/// <returns></returns>
		[DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected static extern bool HidD_GetManufacturerString(IntPtr hFile, StringBuilder buffer, Int32 bufferLength);

		// http://msdn.microsoft.com/en-us/library/windows/hardware/ff539681%28v=vs.85%29.aspx
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="buffer"></param>
		/// <param name="bufferLength"></param>
		/// <returns></returns>
		[DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected static extern bool HidD_GetProductString(IntPtr hFile, StringBuilder buffer, Int32 bufferLength);

		// http://msdn.microsoft.com/en-us/library/windows/hardware/ff539683%28v=vs.85%29.aspx
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="buffer"></param>
		/// <param name="bufferLength"></param>
		/// <returns></returns>
		[DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected static extern bool HidD_GetSerialNumberString(IntPtr hFile, StringBuilder buffer, Int32 bufferLength);

		// http://msdn.microsoft.com/en-us/library/windows/hardware/ff538900%28v=vs.85%29.aspx
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="Attributes"></param>
		/// <returns></returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern bool HidD_GetAttributes(IntPtr hFile, ref HIDD_ATTRIBUTES Attributes);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="lpReportBuffer"></param>
		/// <param name="ReportBufferLength"></param>
		/// <returns></returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern bool HidD_GetFeature(IntPtr hFile, ref Byte lpReportBuffer, Int32 ReportBufferLength);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="lpReportBuffer"></param>
		/// <param name="ReportBufferLength"></param>
		/// <returns></returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern bool HidD_GetInputReport(IntPtr hFile, ref Byte lpReportBuffer, Int32 ReportBufferLength);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="NumberBuffers"></param>
		/// <returns></returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern bool HidD_GetNumInputBuffers(IntPtr hFile, ref Int32 NumberBuffers);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="lpReportBuffer"></param>
		/// <param name="ReportBufferLength"></param>
		/// <returns></returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern bool HidD_SetFeature(IntPtr hFile, ref Byte lpReportBuffer, Int32 ReportBufferLength);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="NumberBuffers"></param>
		/// <returns></returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern bool HidD_SetNumInputBuffers(IntPtr hFile, Int32 NumberBuffers);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hFile"></param>
		/// <param name="lpReportBuffer"></param>
		/// <param name="ReportBufferLength"></param>
		/// <returns></returns>
		[DllImport("hid.dll", SetLastError = true)]
		protected static extern bool HidD_SetOutputReport(IntPtr hFile, ref Byte lpReportBuffer, Int32 ReportBufferLength);
		#endregion

		#region setupapi.dll

		/// <summary>
		/// 提供有关单个USB设备的详细信息
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		protected struct DeviceInterfaceData
		{
			/// <summary>
			/// 
			/// </summary>
			public int Size;
			/// <summary>
			/// 
			/// </summary>
			public Guid InterfaceClassGuid;
			/// <summary>
			/// 
			/// </summary>
			public int Flags;
			/// <summary>
			/// 
			/// </summary>
			public int Reserved;
		}

		/// <summary>
		/// 访问设备的路径
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct DeviceInterfaceDetailData
		{
			/// <summary>
			/// 
			/// </summary>
			public int Size;
			/// <summary>
			/// 
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string DevicePath;
		}

		/// <summary>
		/// 在Windows中分配包含设备详细信息的InfoSet内存块。
		/// </summary>
		/// <param name="gClass">Class guid (e.g. HID guid)</param>
		/// <param name="strEnumerator">Not used</param>
		/// <param name="hParent">Not used</param>
		/// <param name="nFlags">Type of device details required (DIGCF_ constants)</param>
		/// <returns>A reference to the InfoSet</returns>
		[DllImport("setupapi.dll", SetLastError = true)]
		protected static extern IntPtr SetupDiGetClassDevs(ref Guid gClass, [MarshalAs(UnmanagedType.LPStr)] string strEnumerator, IntPtr hParent, uint nFlags);

		/// <summary>
		/// 释放上面调用中分配的InfoSet。
		/// </summary>
		/// <param name="lpInfoSet">Reference to InfoSet</param>
		/// <returns>true if successful</returns>
		[DllImport("setupapi.dll", SetLastError = true)]
		protected static extern int SetupDiDestroyDeviceInfoList(IntPtr lpInfoSet);

		/// <summary>
		/// 从信息集中获取设备的DeviceInterfaceData。
		/// </summary>
		/// <param name="lpDeviceInfoSet">InfoSet to access</param>
		/// <param name="nDeviceInfoData">Not used</param>
		/// <param name="gClass">Device class guid</param>
		/// <param name="nIndex">Index into InfoSet for device</param>
		/// <param name="oInterfaceData">DeviceInterfaceData to fill with data</param>
		/// <returns>True if successful, false if not (e.g. when index is passed end of InfoSet)</returns>
		[DllImport("setupapi.dll", SetLastError = true)]
		protected static extern bool SetupDiEnumDeviceInterfaces(IntPtr lpDeviceInfoSet, uint nDeviceInfoData, ref Guid gClass, uint nIndex, ref DeviceInterfaceData oInterfaceData);

		/// <summary>
		/// SetupDiGetDeviceInterfaceDetail
		/// Gets the interface detail from a DeviceInterfaceData. This is pretty much the device path.
		/// You call this twice, once to get the size of the struct you need to send (nDeviceInterfaceDetailDataSize=0)
		/// and once again when you've allocated the required space.
		/// </summary>
		/// <param name="lpDeviceInfoSet">InfoSet to access</param>
		/// <param name="oInterfaceData">DeviceInterfaceData to use</param>
		/// <param name="lpDeviceInterfaceDetailData">DeviceInterfaceDetailData to fill with data</param>
		/// <param name="nDeviceInterfaceDetailDataSize">The size of the above</param>
		/// <param name="nRequiredSize">The required size of the above when above is set as zero</param>
		/// <param name="lpDeviceInfoData">Not used</param>
		/// <returns></returns>
		[DllImport("setupapi.dll", SetLastError = true)]
		protected static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr lpDeviceInfoSet, ref DeviceInterfaceData oInterfaceData, IntPtr lpDeviceInterfaceDetailData, uint nDeviceInterfaceDetailDataSize, ref uint nRequiredSize, IntPtr lpDeviceInfoData);

		/// <summary>
		/// SetupDiGetDeviceInterfaceDetail
		/// Gets the interface detail from a DeviceInterfaceData. This is pretty much the device path.
		/// You call this twice, once to get the size of the struct you need to send (nDeviceInterfaceDetailDataSize=0)
		/// and once again when you've allocated the required space.
		/// </summary>
		/// <param name="lpDeviceInfoSet">InfoSet to access</param>
		/// <param name="oInterfaceData">DeviceInterfaceData to use</param>
		/// <param name="oDetailData">DeviceInterfaceDetailData to fill with data</param>
		/// <param name="nDeviceInterfaceDetailDataSize">The size of the above</param>
		/// <param name="nRequiredSize">The required size of the above when above is set as zero</param>
		/// <param name="lpDeviceInfoData">Not used</param>
		/// <returns></returns>
		[DllImport("setupapi.dll", SetLastError = true)]
		protected static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr lpDeviceInfoSet, ref DeviceInterfaceData oInterfaceData, ref DeviceInterfaceDetailData oDetailData, uint nDeviceInterfaceDetailDataSize, ref uint nRequiredSize, IntPtr lpDeviceInfoData);
		#endregion

		#region kernel32.dll
		/// <summary>
		/// An overlapped structure used for overlapped IO operations. The structure is
		/// only used by the OS to keep state on pending operations. You don't need to fill anything in if you
		/// unless you want a Windows event to fire when the operation is complete.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		protected struct Overlapped
		{
			/// <summary>
			/// 
			/// </summary>
			public uint Internal;
			/// <summary>
			/// 
			/// </summary>
			public uint InternalHigh;
			/// <summary>
			/// 
			/// </summary>
			public uint Offset;
			/// <summary>
			/// 
			/// </summary>
			public uint OffsetHigh;
			/// <summary>
			/// 
			/// </summary>
			public IntPtr Event;
		}

		/// <summary>
		/// Creates/opens a file, serial port, USB device... etc
		/// </summary>
		/// <param name="strName">Path to object to open</param>
		/// <param name="nAccess">Access mode. e.g. Read, write</param>
		/// <param name="nShareMode">Sharing mode</param>
		/// <param name="lpSecurity">Security details (can be null)</param>
		/// <param name="nCreationFlags">Specifies if the file is created or opened</param>
		/// <param name="nAttributes">Any extra attributes? e.g. open overlapped</param>
		/// <param name="lpTemplate">Not used</param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		protected static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPStr)] string strName, uint nAccess, uint nShareMode, IntPtr lpSecurity, uint nCreationFlags, uint nAttributes, IntPtr lpTemplate);

		/// <summary>
		/// Closes a window handle. File handles, event handles, mutex handles... etc
		/// </summary>
		/// <param name="hFile">Handle to close</param>
		/// <returns>True if successful.</returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		protected static extern int CloseHandle(IntPtr hFile);
		#endregion

		#region user32.dll
		/// <summary>
		/// Used when registering a window to receive messages about devices added or removed from the system.
		/// </summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
		public class DeviceBroadcastInterface
		{
			/// <summary>
			/// 
			/// </summary>
			public int Size;
			/// <summary>
			/// 
			/// </summary>
			public int DeviceType;
			/// <summary>
			/// 
			/// </summary>
			public int Reserved;
			/// <summary>
			/// 
			/// </summary>
			public Guid ClassGuid;
			/// <summary>
			/// 
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string Name;
		}

		/// <summary>
		/// Registers a window for device insert/remove messages
		/// </summary>
		/// <param name="hwnd">Handle to the window that will receive the messages</param>
		/// <param name="oInterface">DeviceBroadcastInterrface structure</param>
		/// <param name="nFlags">set to DEVICE_NOTIFY_WINDOW_HANDLE</param>
		/// <returns>A handle used when unregistering</returns>
		[DllImport("user32.dll", SetLastError = true)]
		protected static extern IntPtr RegisterDeviceNotification(IntPtr hwnd, DeviceBroadcastInterface oInterface, uint nFlags);

		/// <summary>
		/// Unregister from above.
		/// </summary>
		/// <param name="hHandle">Handle returned in call to RegisterDeviceNotification</param>
		/// <returns>True if success</returns>
		[DllImport("user32.dll", SetLastError = true)]
		protected static extern bool UnregisterDeviceNotification(IntPtr hHandle);
		#endregion

		#region Public methods
		/// <summary>
		/// Registers a window to receive windows messages when a device is inserted/removed. Need to call this
		/// from a form when its handle has been created, not in the form constructor. Use form's OnHandleCreated override.
		/// </summary>
		/// <param name="hWnd">Handle to window that will receive messages</param>
		/// <param name="gClass">Class of devices to get messages for</param>
		/// <returns>A handle used when unregistering</returns>
		public static IntPtr RegisterForUsbEvents(IntPtr hWnd, Guid gClass)
		{
			var oInterfaceIn = new DeviceBroadcastInterface();
			oInterfaceIn.Size = Marshal.SizeOf(oInterfaceIn);
			oInterfaceIn.ClassGuid = gClass;
			oInterfaceIn.DeviceType = DEVTYP_DEVICEINTERFACE;
			oInterfaceIn.Reserved = 0;
			return RegisterDeviceNotification(hWnd, oInterfaceIn, DEVICE_NOTIFY_WINDOW_HANDLE);
		}

		/// <summary>
		/// Unregisters notifications. Can be used in form dispose
		/// </summary>
		/// <param name="hHandle">Handle returned from RegisterForUSBEvents</param>
		/// <returns>True if successful</returns>
		public static bool UnregisterForUsbEvents(IntPtr hHandle)
		{
			return UnregisterDeviceNotification(hHandle);
		}

		/// <summary>
		/// Helper to get the HID guid.
		/// </summary>
		public static Guid HIDGuid
		{
			get
			{
				Guid gHid;
				HidD_GetHidGuid(out gHid);
				return gHid;
				//return new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); //gHid;
			}
		}
		#endregion
	}
	/// <summary>
	/// 此类包含已连接usb设备的信息
	/// </summary>
	public class HIDInfoSet
	{
		/// <summary>
		/// 标识制造商的名称
		/// </summary>
		public string ManufacturerString { get; private set; }

		/// <summary>
		/// 标识产品名称
		/// </summary>
		public string ProductString { get; private set; }

		/// <summary>
		/// 序列号
		/// </summary>
		public string SerialNumberString { get; private set; }

		/// <summary>
		/// 产品版本
		/// </summary>
		public UInt16 Version { get; private set; }

		/// <summary>
		/// 完整设备文件路径
		/// </summary>
		public string DevicePath { get; private set; }

		/// <summary>
		/// 供应商ID
		/// </summary>
		public UInt16 VendorID { get; private set; }

		/// <summary>
		/// 产品ID
		/// </summary>
		public UInt16 ProductID { get; private set; }

		/// <summary>
		/// 报告中字节长度
		/// </summary>
		public short InBytesLength { get; private set; }

		/// <summary>
		/// 输出报告字节长度
		/// </summary>
		public short OutBytesLength { get; private set; }

		/// <summary>
		/// ctor
		/// </summary>
		internal HIDInfoSet(string manufacturerString,
							string productString,
							string serialNumberString,
							string devicePath,
							UInt16 vid,
							UInt16 pid,
							UInt16 version,
							short inBytesLength,
							short outBytesLength)
		{
			ManufacturerString = manufacturerString;
			ProductString = productString;
			SerialNumberString = serialNumberString;
			Version = version;
			DevicePath = devicePath;
			VendorID = vid;
			ProductID = pid;
			InBytesLength = inBytesLength;
			OutBytesLength = outBytesLength;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string VersionInBCD()
		{
			if (((byte)(Version >> 24)) == 0)
			{
				return String.Format("{0}.{1}{2}", (byte)(Version >> 16), (byte)(Version >> 8), (byte)Version);
			}

			return String.Format("{0}{1}.{2}{3}", (byte)(Version >> 24), (byte)(Version >> 16), (byte)(Version >> 8), (byte)Version);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string GetInfo()
		{
			return String.Format("[{0:X4}/{1:X4}] {2} {3}", VendorID, ProductID, ManufacturerString, ProductString);
		}
	}
	/// <summary>
	/// 
	/// </summary>
	public class HIDReport : EventArgs
	{
		/// <summary>
		/// 
		/// </summary>
		public readonly byte ID;
		/// <summary>
		/// 
		/// </summary>
		public readonly byte[] Data;

		/// <summary>
		/// HIDReport Constructor
		/// </summary>
		public HIDReport(byte id, byte[] data)
		{
			ID = id;
			Data = data;
		}

		/// <summary>
		/// HIDReport Constructor
		/// </summary>
		public HIDReport(byte[] rawData)
		{
			ID = rawData[0];
			Data = new byte[rawData.Length - 1];
			Array.Copy(rawData, 1, Data, 0, Data.Length);
		}

		/// <summary>
		/// 
		/// </summary>
		public int Length
		{
			get { return Data.Length + 1; }
		}

		/// <summary>
		/// Report as Array
		/// </summary>
		public byte[] ToArray()
		{

			byte[] buffer = null;

			if (Data.Length > 0)
			{
				buffer = new byte[Data.Length + 1];
				buffer[0] = ID;
				Array.Copy(Data, 0, buffer, 1, Data.Length);
			}

			return buffer;
		}
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	public delegate void DataRecievedEventHandler(object sender, HIDReport args);
	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	public delegate void DataSendEventHandler(object sender, HIDReport args);
	#endregion
}

