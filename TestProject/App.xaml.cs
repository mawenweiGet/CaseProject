﻿using LocalTool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TestProject
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        Log_Record log_Record = new Log_Record();
        public App()
        {
            // 在异常由应用程序引发但未进行处理时发生。主要指的是UI线程。
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            //  当某个异常未被捕获时出现。主要指的是非UI线程
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //可以记录日志并转向错误bug窗口友好提示用户
            if (e.ExceptionObject is System.Exception)
            {
                Exception ex = (System.Exception)e.ExceptionObject;
                log_Record.WriteErrorLog(ex);
                MessageBox.Show(ex.Message);
            }
        }
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //可以记录日志并转向错误bug窗口友好提示用户
            e.Handled = true;
            log_Record.WriteErrorLog(e.Exception);
            MessageBox.Show("消息:" + e.Exception.Message + "\r\n" + e.Exception.StackTrace);
        }
    }
}
