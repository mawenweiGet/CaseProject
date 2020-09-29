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

namespace WPF_BaseStudy.Event
{
    /// <summary>
    /// EventExplain.xaml 的交互逻辑
    /// </summary>
    public partial class EventExplain : UserControl
    {
        //定义猫叫委托
        public delegate string CatCallEventHandler();

        Cat cat = new Cat();
        Mouse m = new Mouse();
        People p = new People();

        public EventExplain()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //关联绑定 
            cat.CatCall += new CatCallEventHandler(GetMouseRun);
            cat.CatCall += new CatCallEventHandler(GetWakeUp);
            this.TB_Content.Text+=cat.OnCatCall() + "\n\r"; 

        }
        public string GetMouseRun()
        {
            string val = m.MouseRun();
            this.TB_Content.Text += val+"\n\r";
            return val;
        }
        public string GetWakeUp()
        {
            string val = p.WakeUp();
            this.TB_Content.Text += val + "\n\r";
            return val;
        }
        public class Cat
        {
            //定义猫叫事件
            public event CatCallEventHandler CatCall;
            public string OnCatCall()
            {
                CatCall?.Invoke();
                return "猫叫了一声";
            }
        }
        public class Mouse
        {
            //定义老鼠跑掉方法
            public string MouseRun()
            {
                return "老鼠跑了";
            }
        }
        public class People
        {
            //定义主任起床方法
            public string WakeUp()
            {
                return "主任起床";
            }
        }
    }
}
