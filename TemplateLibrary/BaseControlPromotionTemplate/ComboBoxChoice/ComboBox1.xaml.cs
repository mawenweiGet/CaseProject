using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private string _icon;
        /// <summary>
        /// 图标字符
        /// </summary>
        public string icon 
        {
            get => _icon;
            set
            {
                _icon = value;
                this.IcoName_TextBlock.Text = value;
            }
        }
        private string _explain;
        /// <summary>
        /// 说明
        /// </summary>
        public string explain 
        {
            get => _explain;
            set
            {
                _explain = value;
                this.Title_TextBlock.Text = value;
            }
        }
        private string _details;
        /// <summary>
        /// 详情
        /// </summary>
        public string details 
        {
            get => _details;
            set
            {
                _details = value;
                this.Comment_TextBlock.Text = value;
            }
        }
        private string _unit;
        /// <summary>
        /// 单位
        /// </summary>
        public string unit 
        {
            get => _unit;
            set
            {
                _unit = value;
                this.m_txtBlockUnit.Text = value;
            }
        }
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

        private void Back_MouseMove(object sender, MouseEventArgs e)
        {
            ToggleButton border = m_cmbBoxContent.Template.FindName("toggleButton", this.m_cmbBoxContent) as ToggleButton;

            System.Windows.Shapes.Path path = border.Template.FindName("arrow", border) as System.Windows.Shapes.Path;

            path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#45C4ED"));

            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
            path.Data = (Geometry)(converter.ConvertFrom("M 15,0 L 0,15 30,15 z"));
        }

        private void Back_MouseLeave(object sender, MouseEventArgs e)
        {
            ToggleButton border = m_cmbBoxContent.Template.FindName("toggleButton", this.m_cmbBoxContent) as ToggleButton;

            System.Windows.Shapes.Path path = border.Template.FindName("arrow", border) as System.Windows.Shapes.Path;

            path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#898989"));

            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
            path.Data = (Geometry)(converter.ConvertFrom("M 50,50 L 0,0 100,0 z"));
        }
    }
}
