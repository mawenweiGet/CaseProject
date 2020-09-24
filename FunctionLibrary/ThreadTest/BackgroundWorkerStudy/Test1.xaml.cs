using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FunctionLibrary.ThreadTest.BackgroundWorkerStudy
{
    /// <summary>
    /// Test1.xaml 的交互逻辑
    /// </summary>
    public partial class Test1 : UserControl
    {
        private BackgroundWorker bgWorker = new BackgroundWorker();
        BackgroundWorker m_worker = new BackgroundWorker();//线程
        public delegate void Event_Handler();
        public event Event_Handler Event_StartCalib;
        public ProgressBar getProgressBarobj()
        {
            return this.progressBar2;
        }
        public Test1()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
        }
        #region 自身方法 通知处理进度
        private void InitializeBackgroundWorker()
        {
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgessChanged);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_WorkerCompleted);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt.Tag.Equals("Start"))
            {
                if (bgWorker.IsBusy)
                    return;
                this.progressBar1.Maximum = 100;
                this.btnStart.IsEnabled = false;
                this.btnStop.IsEnabled = true;
                bgWorker.RunWorkerAsync("hello");
            }
            else
            {
                this.btnStart.IsEnabled = true;
                this.btnStop.IsEnabled = false;
                bgWorker.CancelAsync();
            }
        }
        public void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool result = true;
            int number = 0;
            while (result)
            {
                if (bgWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Thread.Sleep(50);
                number++;
                bgWorker.ReportProgress(number, "Working");
                if (number == 100)
                {
                    result = false;
                }
            }
        }
        public void bgWorker_ProgessChanged(object sender, ProgressChangedEventArgs e)
        {
            //string state = (string)e.UserState;//接收ReportProgress方法传递过来的userState
            this.progressBar1.Value = e.ProgressPercentage;
            this.label1.Content = "处理进度:" + Convert.ToString(e.ProgressPercentage) + "%";
        }
        public void bgWorker_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
                return;
            }
            if (!e.Cancelled)
                this.label1.Content = "处理完毕!";
            else
                this.label1.Content = "处理终止!";

        }
        #endregion

        #region 外部通过委托方法 通知处理进度
        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_worker.DoWork += M_worker_DoWork;
                m_worker.RunWorkerCompleted += M_worker_RunWorkerCompleted;
                m_worker.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void M_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Event_StartCalib != null)
            {
                Event_StartCalib();
            }
        }
        private void M_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            do
            {
                if (e.Cancelled == true)
                {//说明是正常手工退出
                    break;
                }
                if (e.Error == null)
                {
                    break;
                }
                MessageBox.Show(e.Error.Message);

            } while (false);

            m_worker.DoWork -= M_worker_DoWork;
            m_worker.RunWorkerCompleted -= M_worker_RunWorkerCompleted;
            this.label2.Content = "处理完成！";
        }
        #endregion

        #region 外部测试方法
        //ProgressBar_getProgressBarobj = this.bw_Test.getProgressBarobj();

        //void method()
        //{
        //    bool result = true;
        //   int number = 0;
        //       while (result)
        //       {
        //           Thread.Sleep(50);
        //           number++;
        //           _getProgressBarobj.Dispatcher.Invoke(new Action(delegate {
        //               _getProgressBarobj.Value = number;
        //           }));
        //           if (number == 100)
        //           {
        //               result = false;
        //           }
        //       }
        //}
        #endregion
    }
}
