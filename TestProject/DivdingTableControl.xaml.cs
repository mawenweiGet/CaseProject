using System;
using System.Collections.Generic;
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
        public DataTable ReturnDataTable()
        {
            return _dt;
        }
        public DivdingTableControl()
        {
            InitializeComponent();
            _dt.Columns.Add(new DataColumn("NO", typeof(string)));
            _dt.Columns.Add(new DataColumn("ParamKey", typeof(string)));
            _dt.Columns.Add(new DataColumn("ParamValue", typeof(string)));

            //DataRow dr = _dt.NewRow();
            //dr[0] = "测量值Ω";
            //dr[1] = "温度值℃";
            //_dt.Rows.Add(dr);
            Init();
        }
        public void Init()
        {
            for (int i = 0; i < 48; i++)
            {
                DataRow dr = _dt.NewRow();
                dr[0] = i+1;
                dr[1] = "";
                dr[2] = "";
                _dt.Rows.Add(dr);
            }
            this.dgData.ItemsSource = null;
            this.dgData.ItemsSource = _dt.DefaultView;
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
    }
}
