using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace LocalTool
{
    public static class FileTool
    {
        /// <summary>
        /// 文件流操作类
        /// </summary>
        private static Stream stream;

        /// <summary>
        /// 记录程序错误日志（此方法不允许出错）
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="error">Exception错误</param>
        /// <returns></returns>
        public static void WriteErrorLog(Exception e)
        {
            StreamWriter file = null;
            try
            {
                //追加方式写入
                file = new StreamWriter(Directory.GetCurrentDirectory() + "\\log.txt", true);
                file.Write("\r\n时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    + "\r\n计算机：" + System.Net.Dns.GetHostName()
                    + "\r\n异常信息：" + e.Message +
                    "\r\n方法调用（底层->上层）：\r\n" + e.StackTrace + "\r\n");
                file.Close();
                file.Dispose();
            }
            catch (Exception)
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
        }
        #region 文件读取
        /// <summary>
        /// 读文件，生成string
        /// </summary>
        /// <param name="filepath">文件路径(默认当前程序路径下)</param>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int ReadFile(string filepath, ref string str)
        {
            string linestr = "";
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    while ((linestr = sr.ReadLine()) != null)
                    {
                        str += linestr;
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }
            return 1;
        }
        #endregion

        #region 文件写入
        /// <summary>
        /// 写文件，写入string
        /// </summary>
        /// <param name="filepath">文件路径(默认当前程序路径下)</param>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int WriteFile(string filepath, string str)
        {
            StreamWriter file = null;
            try
            {
                file = new StreamWriter(filepath, false);
                file.Write(str);
                file.Close();
                file.Dispose();
            }
            catch (Exception)
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
                return -1;
            }
            return 1;
        }
        /// <summary>写文件，写入byte数组</summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="bytes">数据字节</param>
        /// <param name="size">缓冲大小</param>
        /// <returns></returns>
        public static int WriteFile(string filepath, byte[] bytes, int size)
        {
            try
            {
                stream = new FileStream(filepath, FileMode.Create);
                stream.Write(bytes, 0, size);
            }
            catch (Exception)
            {
                if (stream != null)
                {
                    stream.Close();
                }
                return -1;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return 1;
        }
        #endregion

        #region 获取dll下的xml
        /// <summary>
        /// 获取dll下xml
        /// </summary>
        /// <param name="dllName">名称.dll</param>
        /// <param name="Namespace">命名空间.文件.xml</param>
        /// <returns></returns>
        public static Stream GetXmlobj(string dllName,string Namespace)
        {
            System.Reflection.Assembly dll = System.Reflection.Assembly.LoadFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\"+ dllName);
             return dll.GetManifestResourceStream(Namespace);
        }
        #endregion
    }
}
