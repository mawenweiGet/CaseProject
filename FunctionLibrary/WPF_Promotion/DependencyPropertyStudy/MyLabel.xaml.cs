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

namespace FunctionLibrary.WPF_Promotion.DependencyPropertyStudy
{
    /// <summary>
    /// MyLabel.xaml 的交互逻辑
    /// </summary>
    public partial class MyLabel : UserControl
    {
        public MyLabel()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty MyPropertyProperty =
    DependencyProperty.Register("Caption", typeof(int), typeof(MyLabel));

        public static readonly DependencyProperty MyShowProperty =
    DependencyProperty.Register("MyShow", typeof(bool), typeof(MyLabel),
        new PropertyMetadata(new PropertyChangedCallback(MyShowPropertyv)));

        private static void MyShowPropertyv(DependencyObject d,
                                    DependencyPropertyChangedEventArgs e)
        {
            MyLabel cCtrlBase = (MyLabel)d;
            bool bIfVisible = (bool)e.NewValue;
            if (bIfVisible == true)
            {
                cCtrlBase.Visibility = Visibility.Visible;
            }
            else
            {
                cCtrlBase.Visibility = Visibility.Collapsed;
            }
            return;
        }

        public bool MyShow
        {
            set 
            { 
                SetValue(MyShowProperty, value);
                if (value==false)
                {
                    Visibility= Visibility.Collapsed;
                }
                else
                {
                    Visibility= Visibility.Visible;
                }
            }
        }

        public int Caption
        {
            get { return (int)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }


    }
}
