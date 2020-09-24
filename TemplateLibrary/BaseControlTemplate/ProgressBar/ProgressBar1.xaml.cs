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

namespace TemplateLibrary.ProgressBar
{
    /// <summary>
    /// ProgressBar1.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressBar1 : UserControl
    {
        public System.Windows.Controls.ProgressBar GetProgressBarobj()
        {
            return this.progressBar1;
        }
        public ProgressBar1()
        {
            InitializeComponent();
        }
    }
}
