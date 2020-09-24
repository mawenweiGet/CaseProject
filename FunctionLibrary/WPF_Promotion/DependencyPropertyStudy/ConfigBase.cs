using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FunctionLibrary.WPF_Promotion.DependencyPropertyStudy
{
    public partial class ConfigBase: CBaseControl
    {

        //自定义绑定，将bool量绑定为是否显示
        public static readonly DependencyProperty customVisibleProperty = DependencyProperty.Register(
        "MyVisibilityProperty",
        typeof(bool),
        typeof(ConfigBase),
        new PropertyMetadata(new PropertyChangedCallback(MyVisibilityPropertyChanged))
        );

        private static void MyVisibilityPropertyChanged(DependencyObject d,
                                    DependencyPropertyChangedEventArgs e)
        {
            ConfigBase cCtrlBase = (ConfigBase)d;
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
    }
    public class CBaseControl : TextBlock, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
