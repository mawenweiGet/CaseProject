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
    /// Test.xaml 的交互逻辑
    /// </summary>
    public partial class Test : UserControl
    {
        private DataTable _dt = new DataTable();

        public Test()
        {
            InitializeComponent();

            _dt.Columns.Add(new DataColumn("NO", typeof(string)));
            _dt.Columns.Add(new DataColumn("ParamKey", typeof(string)));
            _dt.Columns.Add(new DataColumn("ParamValue", typeof(string)));

            for (int i = 0; i < 48; i++)
            {
                DataRow dr = _dt.NewRow();
                dr[0] = i;
                dr[1] = i.ToString()+i.ToString();
                dr[2] = i.ToString() + i.ToString()+i.ToString();
                _dt.Rows.Add(dr);
            }
            this.TestDataGrid.ItemsSource = null;
            this.TestDataGrid.ItemsSource = _dt.DefaultView;
        }
    }
}
