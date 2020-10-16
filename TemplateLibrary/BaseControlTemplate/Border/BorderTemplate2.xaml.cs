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

namespace TemplateLibrary.Border
{
    /// <summary>
    /// BorderTemplate2.xaml 的交互逻辑
    /// </summary>
    public partial class BorderTemplate2 : UserControl
    {
        public BorderTemplate2()
        {
            InitializeComponent();
        }
        private string _content;
        public string m_content
        {
            get => _content;
            set
            {
                _content = value;
                this.Lable_obj.Content = _content;
            }
        }
    }
}
