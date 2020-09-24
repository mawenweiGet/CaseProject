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

namespace TemplateLibrary.ComboBox
{
    /// <summary>
    /// ComboBoxTemplate1.xaml 的交互逻辑
    /// </summary>
    public partial class ComboBoxTemplate1 : UserControl
    {
        public ComboBoxTemplate1()
        {
            InitializeComponent();
            LodData();
        }
        private void LodData()
        {
            IList<empoyee> customList = new List<empoyee>();
            //项目文件中新建一个images文件夹，并上传了001.png，002.png,003.png
            //customList.Add(new empoyee() { ID = "001", Name = "Jack", Image = new BitmapImage(new Uri("pack://application:,,,/ResourceBase;component/ImageResources/ProjectUsing/wave.png")), Desc = "this is good gay!" });
            customList.Add(new empoyee() { ID = "001", Name = "Jack", Image = "pack://application:,,,/ResourceBase;component/ImageResources/ProjectUsing/wave.png", Desc = "this is good gay!" });
            customList.Add(new empoyee() { ID = "002", Name = "Smith", Image = "pack://application:,,,/ResourceBase;component/ImageResources/TemporaryUsing/BY@2x.png", Desc = "this is great gay!" });
            customList.Add(new empoyee() { ID = "003", Name = "Json", Image = "pack://application:,,,/ResourceBase;component/ImageResources/TemporaryUsing/BY@2x.png", Desc = "this is the bset gay!" });


            this.myColorComBox.ItemsSource = customList;//数据源绑定
            //this.myColorComBox.SelectedValue = customList[1];//默认选择项
        }
    }
    //定义员工类
    public class empoyee
    {
        public string ID { get; set; }
        public string Name { get; set; }
        //public ImageSource Image { get; set; }
        public string Image { get; set; }
        public string Desc { get; set; }

    }
}
