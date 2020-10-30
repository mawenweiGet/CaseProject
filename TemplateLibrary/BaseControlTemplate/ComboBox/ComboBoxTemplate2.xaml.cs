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

namespace TemplateLibrary.ComboBox
{
    /// <summary>
    /// ComboxTemplate2.xaml 的交互逻辑
    /// </summary>
    public partial class ComboBoxTemplate2 : UserControl
    {
        public ComboBoxTemplate2()
        {
            InitializeComponent();
            List<string> testComboxlist = new List<string>()
            {
                "选项A",
                "选项B",
                "选项C",
                "选项D",
                "选项E",
                "选项F",
            };
            this.m_cmbBoxContent.ItemsSource = testComboxlist;
            this.m_cmbBoxContent.SelectedIndex = 1;
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
