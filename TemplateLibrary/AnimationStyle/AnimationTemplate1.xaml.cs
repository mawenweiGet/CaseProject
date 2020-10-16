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

namespace TemplateLibrary.AnimationStyle
{
    /// <summary>
    /// AnimationTemplate1.xaml 的交互逻辑
    /// </summary>
    public partial class AnimationTemplate1 : UserControl
    {
        public AnimationTemplate1()
        {
            InitializeComponent();
            //单独控制是否启动动画属性
            this.txtBlockWaiting.IsEnabled = true;
        }
    }
}
