using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TestProject
{
    public class ImageTextBox : TextBox
    {

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageTextBox), new PropertyMetadata(null));


        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImageTextBox), new PropertyMetadata(20d));

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImageTextBox), new PropertyMetadata(20d));


        public Thickness ImageMargin
        {
            get { return (Thickness)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageMarginProperty =
            DependencyProperty.Register("ImageMargin", typeof(Thickness), typeof(ImageTextBox), new PropertyMetadata(new Thickness()));


    }

    public class TextBoxHelper
    {
        #region 附加属性 Clearable
        public static readonly DependencyProperty ClearableProperty =
            DependencyProperty.RegisterAttached("Clearable", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, ClearText));


        public static bool GetClearable(DependencyObject obj)
        {
            return (bool)obj.GetValue(ClearableProperty);
        }

        public static void SetClearable(DependencyObject obj, bool value)
        {
            obj.SetValue(ClearableProperty, value);
        }

        #endregion

        #region 

        private static void ClearText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Button btn = d as Button;
            if (d != null && e.OldValue != e.NewValue)
            {
                btn.Click -= ClearTextClicked;
                if ((bool)e.NewValue)
                {
                    btn.Click += ClearTextClicked;
                }
            }
        }

        public static void ClearTextClicked(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                var parent = VisualTreeHelper.GetParent(btn);
                while (!(parent is TextBox))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
                TextBox txt = parent as TextBox;
                if (txt != null)
                {
                    txt.Clear();
                }
            }
        }

        #endregion
    }

    public class UserNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //if (string.IsNullOrEmpty(value.ToString()))
            //    return new ValidationResult(false, "userNameRequired".ToGlobal());
            //string rule = @"^[\u4e00-\u9fa5\w]{2,16}$";
            //bool result = Validator.IsMatch(value.ToString(), rule);
            //if (!result)
            //{
            //    return new ValidationResult(false, "userNameValidation".ToGlobal());
            //}
            return new ValidationResult(true, null);
        }
    }
}
