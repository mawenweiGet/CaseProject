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
using TemplateLibrary.BaseControlPromotionTemplate.ComboBoxChoice;

namespace TestControlLibrary.CascadeRelationChose
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : UserControl
    {
        DataApp dataApp = new DataApp();

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            ComboBox1 comboBox1 = new ComboBox1();
            comboBox1.icon = "\ued3e";
            comboBox1.explain = "传感器类型";
            comboBox1.details = "选择对应的传感器类型才能正常后续操作！";
            //指定列表来源
            comboBox1.GetComboBoxobj().ItemsSource =
                dataApp.dataResource.cGraduation.listTransducers;
            //指定列表选中对象的显示值
            comboBox1.GetComboBoxobj().DisplayMemberPath = "strTransducerType";
            //绑定数据源
            comboBox1.SetBinding(ComboBox.SelectedValueProperty,
                new Binding("m_TransducerSelected")
                { Source = dataApp.dataResource });
            comboBox1.GetComboBoxobj().SelectedIndex = 0;
            this.stackPanel_Ctrl.Children.Add(comboBox1);


            ComboBox1 comboBox2 = new ComboBox1();
            comboBox2.icon = "\ued23";
            comboBox2.explain = "分度号";
            comboBox2.details = "分度号信息！";
            //指定列表来源
            comboBox2.GetComboBoxobj().SetBinding(ComboBox.ItemsSourceProperty,
                new Binding("m_TransducerSelected.listSubType")
                {
                    Source = dataApp.dataResource
                });
            //指定列表选中对象的显示值
            comboBox2.GetComboBoxobj().DisplayMemberPath = "strSubType";
            //绑定数据源
            comboBox2.SetBinding(ComboBox.SelectedValueProperty,
                new Binding("m_SubTypeSelected")
                { Source = dataApp.dataResource });
            comboBox2.GetComboBoxobj().SelectedIndex = 0;
            this.stackPanel_Ctrl.Children.Add(comboBox2);
        }
    }
}
