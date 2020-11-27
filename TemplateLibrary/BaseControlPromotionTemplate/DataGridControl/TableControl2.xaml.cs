using BaseClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace TemplateLibrary.BaseControlPromotionTemplate.DataGridControl
{
    /// <summary>
    /// TableControl2.xaml 的交互逻辑
    /// </summary>
    public partial class TableControl2 : UserControl
    {
        private CData_CustomTable _dtobj = new CData_CustomTable();

        public TableControl2()
        {
            InitializeComponent();
            Init();
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
