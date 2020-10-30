using LocalTool;
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
    /// GIFAnimationTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class GIFAnimationTemplate : UserControl
    {
        private GifImage gifImage;
        public GIFAnimationTemplate()
        {
            InitializeComponent();
            this.Loaded += GIFAnimationTemplate_Loaded;
        }

        private void GIFAnimationTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            this.gifImage = new GifImage(@"D:\资源文件\学习资料\WPF\wpf练习册\CaseProject\ResourceBase\ImageResources\ProjectUsing\Animation\Dynamic_Arrow.gif");//此处路径必须是绝对路径
            this.gifImage.Width = 100;
            this.gifImage.Height = 100;
            GifTest.Content = this.gifImage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (StartOrStop.Content.Equals("开始"))
            {
                gifImage.StartAnimate();
                StartOrStop.Content = "结束";
            }
            else
            {
                gifImage.StopAnimate();
                StartOrStop.Content = "开始";
            }
        }
    }
}
