using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace TestControlLibrary.InformationEntry
{
    /// <summary>
    /// IdentityInfo.xaml 的交互逻辑
    /// </summary>
    public partial class IdentityInfo : UserControl
    {
        ErrContent err = null;
        public IdentityInfo()
        {
            InitializeComponent();
            err= new ErrContent() { ErrConteont="数据无误！"};
            Binding b_err = new Binding("ErrConteont");
            b_err.Source = err;
            this.ErrContent.SetBinding(TextBlock.TextProperty,b_err);
        }
    }
    public class ErrContent : INotifyPropertyChanged
    {
        private string _errstring;

        public string ErrConteont
        {
            get { return _errstring; }
            set
            {
                _errstring = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("_errstring"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class RangeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double myValue = 0;
            if (double.TryParse(value.ToString(), out myValue))
            {
                if (myValue >= 0 && myValue <= 100)
                {
                    return new ValidationResult(true, null);
                }
            }
            return new ValidationResult(false, "Input should between 0 and 100");
        }
    }
}
