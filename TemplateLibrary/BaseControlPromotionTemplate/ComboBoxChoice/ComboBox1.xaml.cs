using System;
using System.Collections.Generic;
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

namespace TemplateLibrary.BaseControlPromotionTemplate.ComboBoxChoice
{
    /// <summary>
    /// ComboBox1.xaml 的交互逻辑
    /// </summary>
    public partial class ComboBox1 : UserControl
    {
        /// <summary>
        /// 图标字符
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string explain { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string details { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        /// 获取ComboBox对象
        /// </summary>
        /// <returns></returns>
        public System.Windows.Controls.ComboBox GetComboBoxobj()
        {
            return this.m_cmbBoxContent;
        }
        public ComboBox1()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
