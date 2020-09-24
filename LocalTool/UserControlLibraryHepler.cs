using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LocalTool
{
    /// <summary>
    /// 用户控件辅助类
    /// </summary>
    public class UserControlLibraryHepler
    {
        #region 获取dll下的用户控件对象
        /// <summary>
        /// 获取当前运行程序路径下指定dll中的用户控件对象
        /// </summary>
        /// <param name="dllName">dll名称（Test.dll）</param>
        /// <param name="Namespace">控件命名空间(Test.test)</param>
        /// <returns></returns>
        public UserControl Get_PathDll_Usercontrol(string dllName,string Namespace)
        {
            if (!string.IsNullOrEmpty(dllName) && !string.IsNullOrEmpty(Namespace))
            {
                Assembly ass = Assembly.LoadFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + dllName);
                Type tp = ass.GetType(Namespace, false);
                UserControl usercontrolobj = (UserControl)Activator.CreateInstance(tp);
                return usercontrolobj;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取当前运行程序路径下指定dll中的窗体对象
        /// </summary>
        /// <param name="dllName">dll名称（Test.dll）</param>
        /// <param name="Namespace">窗体命名空间(Test.test)</param>
        /// <returns></returns>
        public Window Get_PathDll_Window(string dllName, string Namespace)
        {
            if (!string.IsNullOrEmpty(dllName)&&!string.IsNullOrEmpty(Namespace))
            {
                Assembly ass = Assembly.LoadFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + dllName);
                Type tp = ass.GetType(Namespace, false);
                Window windowobj = (Window)Activator.CreateInstance(tp);
                return windowobj;
            }
            else
            {
                return null;
            }
        }
        #endregion

    }
}
