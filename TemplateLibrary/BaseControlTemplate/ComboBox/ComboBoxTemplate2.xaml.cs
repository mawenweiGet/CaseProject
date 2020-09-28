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
    }
}
