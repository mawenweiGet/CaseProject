using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace TemplateLibrary.BaseControlPromotionTemplate.Input_Information
{
    /// <summary>
    /// InputInformation1.xaml 的交互逻辑
    /// </summary>
    public partial class InputInformation1 : UserControl
    {
        /// <summary>
        /// 图标字符
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string explain { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string details { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        /// 填充文本输入文本框对象（外部绑定数据验证等）
        /// </summary>
        /// <returns></returns>
        public TextBox AddContentobj()
        {
            TextBox textBox = new TextBox()
            {
                Name = "testcontoin",
                MinWidth = 150,
                Height = 35,
                MaxWidth = 300,
                Margin =new Thickness(15 ,0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment=  VerticalAlignment.Center
            };
            textBox.Style = Resources["TipsInputStyle"] as Style;

            Binding b_d = new Binding("testcontoin");
            b_d.Source = this;
            b_d.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            b_d.ValidatesOnDataErrors = true;
            b_d.NotifyOnValidationError = true;
            b_d.ValidationRules.Add(new LocalTool.RangeValidationRules { checkType = LocalTool.RangeValidationRules.DataCheckType.Float_One });
            textBox.SetBinding(TextBox.TextProperty, b_d);


            //this.TextContentobj.Children.Add(textBox);
            return textBox;
        }
        public InputInformation1()
        {
            InitializeComponent();

            DataContext = this;
        }
        private void m_txtBoxContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                DummyBox.Focus();
            }
            else if (e.Key == Key.Escape)
            {
                DummyBox.Focus();
            }
        }

        private void m_txtBoxContent_MouseMove(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.IBeam;
        }

        private void Thumb_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
    public class InputDataRuleResult : ValidationRule
    {
        public enum DataCheckType
        {
            /// <summary>
            /// 整数
            /// </summary>
            Integer,
            /// <summary>
            /// 浮点型（小数后一位）
            /// </summary>
            Float_One,
            /// <summary>
            /// 浮点型（小数后三位）
            /// </summary>
            Float_Three,
            /// <summary>
            /// 文本（除特俗字符）
            /// </summary>
            NonspecificChar,
            /// <summary>
            /// 文本（无限制）
            /// </summary>
            PureText,
        }
        public DataCheckType checkType { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                switch (checkType)
                {
                    case DataCheckType.Integer:
                        if (!string.IsNullOrEmpty(value.ToString().Trim()) && !Is_integer(value.ToString().Trim()))
                        {
                            return new ValidationResult(false, "输入整数有误！");
                        }
                        break;
                    case DataCheckType.Float_One:
                        if (!string.IsNullOrEmpty(value.ToString().Trim()) && !IsFloat_One(value.ToString().Trim(), 1))
                        {
                            return new ValidationResult(false, "输入浮点数据有误，小数点后只保留一位！");
                        }
                        break;
                    case DataCheckType.Float_Three:
                        if (!string.IsNullOrEmpty(value.ToString().Trim()) && !IsFloat_One(value.ToString().Trim(), 3))
                        {
                            return new ValidationResult(false, "输入浮点数据有误，小数点后只保留三位！");
                        }
                        break;
                    case DataCheckType.NonspecificChar:
                        if (!string.IsNullOrEmpty(value.ToString().Trim()) && !IsNonspecificChar(value.ToString().Trim()))
                        {
                            return new ValidationResult(false, "输入文本含有特殊字符！");
                        }
                        break;
                    case DataCheckType.PureText:
                        if (!string.IsNullOrEmpty(value.ToString().Trim()) && !IsPureText(value.ToString().Trim()))
                        {
                            return new ValidationResult(false, "请检查文本是否为空！");
                        }
                        break;
                    default:
                        break;
                }
                return new ValidationResult(true, null);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 整数
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        private bool Is_integer(string con)
        {
            bool Result = true;
            if (!Regex.IsMatch(con, @"^-?\d+$"))
            {
                Result = false;
            }
            return Result;
        }
        /// <summary>
        /// 浮点型（小数后一位）
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        private bool IsFloat_One(string con, int mantissa)
        {
            bool Result = true;
            con = con.Replace("-", "");
            if ((!Regex.IsMatch(con, "^([0-9]{1,}[.][0-9]*)$")) &&
                       (!Regex.IsMatch(con, "^[0-9]{1,}$")))
            {
                return false;
            }
            else
            {
                if (con.Split('.').Length > 1 && con.Split('.')[1].Length > mantissa)
                {
                    return false;
                }
            }
            return Result;
        }

        /// <summary>
        /// 非特殊字符
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        private bool IsNonspecificChar(string con)
        {
            bool Result = true;
            if ((Regex.IsMatch(con, @"[^%&’,;=?$\x22]+")))
            {
                return false;
            }
            return Result;
        }
        private bool IsPureText(string con)
        {
            return true;
        }
    }
}
