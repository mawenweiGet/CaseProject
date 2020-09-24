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

namespace TemplateLibrary.GroupBox
{
    /// <summary>
    /// GroupBox1.xaml 的交互逻辑
    /// </summary>
    public partial class GroupBox1 : UserControl
    {
        private string _strLableTitle;
        /// <summary>
        /// 标题说明文本
        /// </summary>
        public string m_strLableTitle
        {
            get => _strLableTitle;
            set
            {
                _strLableTitle = value;
                lableTitletip.Content = _strLableTitle;
            }
        }
        /// <summary>
        /// 内容填充
        /// </summary>
        /// <param name="ctrl">内容放置WrapPanel传过来即可</param>
        public void AddControl(WrapPanel ctrl)
        {
            Content_StackPanel.Children.Add((WrapPanel)ctrl);
        }
        public GroupBox1()
        {
            InitializeComponent();
        }
    }
}
