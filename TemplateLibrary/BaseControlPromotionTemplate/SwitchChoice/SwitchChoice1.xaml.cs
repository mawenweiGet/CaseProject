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

namespace TemplateLibrary.BaseControlPromotionTemplate.SwitchChoice
{
    /// <summary>
    /// SwitchChoice1.xaml 的交互逻辑
    /// </summary>
    public partial class SwitchChoice1 : UserControl
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
        /// 返回开关对象
        /// </summary>
        /// <returns></returns>
        public CheckBox GetCheckBoxobj()
        {
            return this.m_chkBoxContent;
        }
        public SwitchChoice1()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
