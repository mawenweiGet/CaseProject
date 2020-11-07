using LocalTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
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

namespace TestProject
{
    /// <summary>
    /// Test.xaml 的交互逻辑
    /// </summary>
    public partial class Test : UserControl
    {
        //private DataTable _dt = new DataTable();
        private CData_CustomTable _dtobj = new CData_CustomTable();
        public Test()
        {
            InitializeComponent();
            Init();

            //MailboxOperation.SendEmail("smtp.163.com", "925634430@qq.com", "mww_a_syp@163.com","异常数据","有附件请查收！", @"D:\资源文件\公司资源\邮件邮箱\服务器配置信息.png", "DKITNSDRTOCBTOZR");

        }
        public void Init()
        {
            ObservableCollection<CData_CustomTable.DataModel> dataModels = new ObservableCollection<CData_CustomTable.DataModel>();
            for (int i = 0; i < 48; i++)
            {
                CData_CustomTable.DataModel data = new CData_CustomTable.DataModel()
                {
                    NO = i + 1,
                    Temperature = i + 1,
                    Resistance = i + 1
                };
                dataModels.Add(data);
            }
            _dtobj.customTable = dataModels;
            this.dataGridAlarm.ItemsSource = null;
            this.dataGridAlarm.ItemsSource = dataModels;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (CData_CustomTable.DataModel row in _dtobj.customTable)
            {
                string res1 = row.NO.ToString();
                string res2 = row.Temperature.ToString();
                string res3 = row.Resistance.ToString();
            }
        }
    }
    public class CData_CustomTable : BaseModel
    {
        public class DataModel
        {
            public int NO { get; set; }
            /// <summary>
            /// 温度
            /// </summary>
            public float Temperature { get; set; }
            /// <summary>
            /// 电阻
            /// </summary>
            public float Resistance { get; set; }
        }
        private ObservableCollection<DataModel> _customTable;
        public ObservableCollection<DataModel> customTable
        {
            get => _customTable;
            set
            {
                _customTable = value;
                OnPropertyChanged();
            }
        }
    }
}
