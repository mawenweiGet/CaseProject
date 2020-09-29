using System;
using System.Collections.Generic;
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

namespace TestProject
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Testconntent
        { 
        public string TestConteon = "3423";
        }
        Testconntent t = new Testconntent();
        public MainWindow()
        {
            InitializeComponent();

            Binding b = new Binding("TestConteon")
            { 
             Source=t,
             Mode= BindingMode.TwoWay,
            };
            //this.ErrContent.SetBinding(TextBox.TextProperty,b);
        }

        private void ErrContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(">>>");
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            t.TestConteon = "4234534";
        }
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
