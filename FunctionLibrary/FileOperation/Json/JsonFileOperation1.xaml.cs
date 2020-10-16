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

namespace FunctionLibrary.FileOperation.Json
{
    /// <summary>
    /// JsonFileOperation1.xaml 的交互逻辑
    /// </summary>
    public partial class JsonFileOperation1 : UserControl
    {
        public JsonFileOperation1()
        {
            InitializeComponent();

            //1.对象-->JSON
            UserInfo info = new UserInfo
            {
                Age = 10,
                Gender = 1,
                UserName = "刘德华",
                Lover = new List<string> { "美女1", "美女2", "美女3" },
                Address = new ContactAddress
                {
                    Province = "湖南省",
                    City = "长沙市",
                    Country = "望城县",
                    Details = "某旮旯快递找不到的地方"
                },
                DailyRecord = new Dictionary<string, string> { { "星期一", "吃饭" }, { "星期二", "洗衣服" }, { "星期三", "好事情" } }
            };

            string json = Json_Operation.ObjectToJSON<UserInfo>(info);

            FileTool.WriteFile(@"TestJson\TuserInfo.json", json);

            string refstring = null;
            FileTool.ReadFile(@"TestJson\TuserInfo.json", ref refstring);

            UserInfo uI = Json_Operation.JsonToObject<UserInfo>(refstring);
            uI.Age = 100;

            FileTool.WriteFile(@"TestJson\T.json", Json_Operation.ObjectToJSON<UserInfo>(uI));
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
    }
}
