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

namespace TemplateLibrary.ListBox
{
    /// <summary>
    /// MenuListBox1.xaml 的交互逻辑
    /// </summary>
    public partial class MenuListBox1 : UserControl
    {
        public MenuListBox1()
        {
            InitializeComponent();
            m_MenuInfoList = new List<MenuInfo>()
            {
                new MenuInfo(){
                    Associatedobjects="element1",
                    IconCode="\ue948",
                    m_IconPath = "pack://application:,,,/ResourceBase;component/Font/#icomoon",
                    Info = "组态",
                },
                new MenuInfo(){
                    Associatedobjects="element5",
                    IconCode="\ue94a",
                    m_IconPath = "pack://application:,,,/ResourceBase;component/Font/#icomoon",
                    Info="标定",
                },
                new MenuInfo(){
                    Associatedobjects="element10",
                    IconCode="\uea42",
                    m_IconPath = "pack://application:,,,/ResourceBase;component/Font/#icomoon",
                    Info="调试",
                },
                 new MenuInfo(){
                    Associatedobjects="element15",
                    IconCode="\ue99c",
                    m_IconPath = "pack://application:,,,/ResourceBase;component/Font/#icomoon",
                    Info="监控",
                },
            };
            UserInfoList.ItemsSource = m_MenuInfoList;
        }
        public System.Windows.Controls.ListBox LB
        {
            get
            {
                return this.UserInfoList;
            }
        }
        /// <summary>
        /// 菜单数据集合
        /// </summary>
        public List<MenuInfo> m_MenuInfoList { get; set; }
        public class MenuInfo
        {
            /// <summary>
            /// 关联对象（滚动条展示跳转对象）
            /// </summary>
            public string Associatedobjects { get; set; }
            /// <summary>
            /// 图标代码
            /// </summary>
            public string IconCode { get; set; }
            /// <summary>
            /// 图标文本路径
            /// </summary>
            public string m_IconPath { get; set; }
            /// <summary>
            /// 说明文本
            /// </summary>
            public string Info { get; set; }
        }
    }
}
