using BaseClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace TestProject
{
    /// <summary>
    /// DivdingTableControl.xaml 的交互逻辑
    /// </summary>
    public partial class DivdingTableControl : UserControl
    {
        private DataTable _dt = new DataTable();
        private CData_CustomTable _dtobj = new CData_CustomTable();
        public DataTable ReturnDataTable()
        {
            return _dt;
        }
        public DivdingTableControl()
        {
            InitializeComponent();

            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);

            _dt.Columns.Add(new DataColumn("NO", typeof(string)));
            _dt.Columns.Add(new DataColumn("Temperature", typeof(string)));
            _dt.Columns.Add(new DataColumn("Resistance", typeof(string)));

            //DataRow dr = _dt.NewRow();
            //dr[0] = "测量值Ω";
            //dr[1] = "温度值℃";
            //_dt.Rows.Add(dr);
            Init();
        }
        public void Init()
        {
            ObservableCollection<DataModel> dataModels = new ObservableCollection<DataModel>();
            for (int i = 0; i < 48; i++)
            {
                DataModel data = new DataModel()
                {
                    NO = i + 1,
                    Temperature = i + 1,
                    Resistance = i + 1
                };
                dataModels.Add(data);
            }
            _dtobj.customTable = dataModels;
            this.dgData.ItemsSource = _dtobj.customTable;

            //for (int i = 0; i < 48; i++)
            //{
            //    DataRow dr = _dt.NewRow();
            //    dr[0] = i+1;
            //    dr[1] = "";
            //    dr[2] = "";
            //    _dt.Rows.Add(dr);
            //}
            //this.dgData.ItemsSource = null;
            //this.dgData.ItemsSource = _dt.DefaultView;
        }
        #region 自定义依赖项属性
        public DataTable DataSource
        {
            get { return (DataTable)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(DataTable), typeof(DivdingTableControl), new PropertyMetadata(new DataTable(), DataSourceChanged));

        private static void DataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DivdingTableControl control = d as DivdingTableControl;
            if (e.NewValue != e.OldValue)
            {
                DataTable dt = e.NewValue as DataTable;
            }
        }

        #endregion

        #region 粘贴数据
        private void DataGirdViewCellPaste()
        {
            //获取获取当前选中单元格所在的行序号  
            int rowindex = dgData.SelectedIndex;
            string str =Clipboard.GetText();//获取剪切板数据
            //  数据载体 data = new 数据载体();
            if (string.IsNullOrEmpty(str.Trim()) == false)
            {
                string[] rows = str.Split('\n');//分行
                //循环行
                foreach (var item in rows)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] cols = item.Replace("\r", "").Split('\t');//分列
                        if (cols.Length==2)
                        {
                            _dtobj.customTable[rowindex].Temperature = Convert.ToSingle(cols[0]);
                            _dtobj.customTable[rowindex].Resistance = Convert.ToSingle(cols[1]);
                        }
                        else if(cols.Length==3)
                        {
                            _dtobj.customTable[rowindex].Temperature = Convert.ToSingle(cols[1]);
                            _dtobj.customTable[rowindex].Resistance = Convert.ToSingle(cols[2]);
                        }
                        rowindex++;
                    }
                }
            }
            //  list.Add(data);
            //绑定datagrid
        }
        #endregion
        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & (ModifierKeys.Control)) == (ModifierKeys.Control))
            {
                DataGirdViewCellPaste();
            }
        }
    }
    public class CData_CustomTable : BaseModel
    {
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
}
