using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalTool
{
    public static class FilePath_Tool
    {
        /// <summary>
        /// 当前程序运行文件路径
        /// </summary>
        /// <returns></returns>
        public static string Current_Run_Path()
        {
           return AppDomain.CurrentDomain.BaseDirectory;
        }

    }
}
