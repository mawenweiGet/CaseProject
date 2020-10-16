using LocalTool;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
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
        public MainWindow()
        {
            InitializeComponent();

            ////1.对象-->JSON
            //UserInfo info = new UserInfo
            //{
            //    Age = 10,
            //    Gender = 1,
            //    UserName = "刘德华",
            //    Lover = new List<string> { "美女1", "美女2", "美女3" },
            //    Address = new ContactAddress
            //    {
            //        Province = "湖南省",
            //        City = "长沙市",
            //        Country = "望城县",
            //        Details = "某旮旯快递找不到的地方"
            //    },
            //    DailyRecord = new Dictionary<string, string> { { "星期一", "吃饭" }, { "星期二", "洗衣服" }, { "星期三", "好事情" } }
            //};
            MonitorDataConfig obj = new MonitorDataConfig
            {
                ListConfig = new List<ConfigData>()
                {
                    new ConfigData
                    {
                        Typecode = 0,
                        ProductType_Image = "1",
                        Collecting_Image = "2",
                        Computes_Image = "3",
                        Transmutes_Image = "4",
                        OutputType_Image = "5",
                        Parameter_one_Explain = "电阻值:",
                        TcjcparmeterVisibility=false,
                        Parameter_one_unit= "Ω",
                        Parameter_two_Explain= "PV值:",
                        Parameter_two_unit= "Ω"
                    },
                    new ConfigData
                    {
                        Typecode=1,
                        ProductType_Image = "5",
                        Collecting_Image = "4",
                        Computes_Image = "3",
                        Transmutes_Image = "2",
                        OutputType_Image = "1",
                        Parameter_one_Explain = "mV值:",
                        TcjcparmeterVisibility=true,
                        Parameter_one_unit= "Ω",
                        Parameter_two_Explain= "PV值:",
                        Parameter_two_unit= "Ω"
                    },
                }
            };

            string json = ObjectToJSON<MonitorDataConfig>(obj);//MonitorConfig.json
            FileTool.WriteFile(@"Json\MonitorConfig.json", json);

            string refstring = null;
            FileTool.ReadFile(@"Json\MonitorConfig.json", ref refstring);

            MonitorDataConfig MD = JsonToObject<MonitorDataConfig>(refstring);
            MD.ListConfig[0].Typecode = 100;

            FileTool.WriteFile(@"Json\MonitorConfig.json", ObjectToJSON<MonitorDataConfig>(MD));




            //string json = ObjectToJSON<UserInfo>(info);

            //FileTool.WriteFile(@"C:\Users\maww\Desktop\usbTest\T.json", json);

            //string refstring = null;
            //FileTool.ReadFile(@"C:\Users\maww\Desktop\usbTest\T.json",ref refstring);

            //UserInfo uI = JsonToObject<UserInfo>(refstring);
            //uI.Age = 100;

            //FileTool.WriteFile(@"C:\Users\maww\Desktop\usbTest\T.json", ObjectToJSON<UserInfo>(uI));
        }
        /// <summary>
        /// 对象转换成JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJSON<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            string result = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                ms.Position = 0;
                using (StreamReader read = new StreamReader(ms))
                {
                    result = read.ReadToEnd();
                }
            }
            return result;
        }
        /// <summary>
        /// Json转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string jsonText)
        {
            DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonText));
            T obj = (T)s.ReadObject(ms);
            ms.Dispose();
            return obj;
        }
        #region  类对象
        //[DataContract]
        public class UserInfo
        {
            //[DataMember(Order = 0)]
            public string UserName { get; set; }
            //[DataMember(Order = 1)]
            public int Age { get; set; }
            //[DataMember(Order = 2)]
            public int Gender { get; set; }
            //[DataMember(Order = 3)]
            public List<string> Lover { get; set; }
            //[DataMember(Order = 4)]
            public ContactAddress Address { get; set; }
            //[DataMember(Order = 5)]
            public Dictionary<string, string> DailyRecord
            {
                get; set;
            }
        }
        //[DataContract]
        public class ContactAddress
        {
            //[DataMember(Order = 0)]
            public string Province { get; set; }
            //[DataMember(Order = 1)]
            public string City { get; set; }
            //[DataMember(Order = 2)]
            public string Country { get; set; }
            //[DataMember(Order = 3)]
            public string Details { get; set; }
        }
        #endregion
        public class MonitorDataConfig
        {
            public List<ConfigData> ListConfig { get; set; }
        }
        public class ConfigData
        {
            public double Typecode { get; set; }
            public string ProductType_Image { get; set; }
            public string Collecting_Image { get; set; }
            public string Computes_Image { get; set; }
            public string Transmutes_Image { get; set; }
            public string OutputType_Image { get; set; }
            public string Parameter_one_Explain { get; set; }
            public bool TcjcparmeterVisibility { get; set; }
            public string Parameter_one_unit { get; set; }
            public string Parameter_two_Explain { get; set; }
            public string Parameter_two_unit { get; set; }
        }
    }
}
