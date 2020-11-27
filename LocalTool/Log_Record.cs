using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTool
{
    public class Log_Record
    {
        /// <summary>
        /// 记录程序错误日志（此方法不允许出错）
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="error">Exception错误</param>
        /// <returns></returns>
        public void WriteErrorLog(Exception e)
        {
            StreamWriter file = null;
            try
            {
                //追加方式写入
                file = new StreamWriter(Directory.GetCurrentDirectory() + "\\Log\\log.txt", true);
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
    }
}
