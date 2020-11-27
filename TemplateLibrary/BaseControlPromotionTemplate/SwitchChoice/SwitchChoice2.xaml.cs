using BaseClassLibrary;
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

namespace TemplateLibrary.BaseControlPromotionTemplate.SwitchChoice
{
    /// <summary>
    /// SwitchChoice2.xaml 的交互逻辑
    /// </summary>
    public partial class SwitchChoice2 : UserControl
    {
        TestDataModel testDataModel = new TestDataModel();
        public SwitchChoice2()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            m_chkBoxContent.SetBinding(
              CheckBox.IsCheckedProperty,
              new Binding("m_bSwitchEBD")
              {
                  Source = testDataModel
              }
              );
            m_chkBoxContent.SetBinding(
             CheckBox.ContentProperty,
             new Binding("m_CDataSwitchEBD.strDisplay")
             {
                 Source = testDataModel
             }
             );
            //m_Labe.SetBinding(TestDataModel.customVisibleProperty,
            //new Binding("m_cDataSwitchInputLimit.bIfShow")
            //{
            //    Source = testDataModel
            //});
        }
    }
    public class TestDataModel : BaseModel
    {
        public TestDataModel()
        {
            cSwitchEBD = new CSwitchEBD();
        }
        public class CDataSwitchEBD
        {
            public UInt16 typeCode { get; set; }
            public string strDisplay { get; set; }
            public bool bCheckValue { get; set; }
            public bool bIfShow { get; set; }
        }
        class CSwitchEBD
        {
            public List<CDataSwitchEBD> listSwitchEBD;
            public CSwitchEBD()
            {
                listSwitchEBD = new List<CDataSwitchEBD>
                {
                    new CDataSwitchEBD
                    {
                        typeCode=0x00,
                        strDisplay="关闭",
                        bCheckValue=false,
                        bIfShow=false

                    },
                    new CDataSwitchEBD
                    {
                        typeCode=0x01,
                        strDisplay="开启",
                        bCheckValue=true,
                        bIfShow=true
                    }
                };
            }
        }
        private CSwitchEBD cSwitchEBD;
        private CDataSwitchEBD _cDataSwitchEBD;
        public CDataSwitchEBD m_CDataSwitchEBD
        {
            get => _cDataSwitchEBD;
            set
            {
                _cDataSwitchEBD = value;
                _bSwitchEBD = value.bCheckValue;
                OnPropertyChanged();
                OnPropertyChanged("m_bSwitchEBD");
            }
        }
        private bool _bSwitchEBD;
        /// <summary>
        /// 断线诊断开关
        /// </summary>
        public bool m_bSwitchEBD
        {
            get => _bSwitchEBD;
            set
            {
                _bSwitchEBD = value;
                SetSwitchEBDByCheckValue(value);
                OnPropertyChanged();
                //OnParameterChanged();
            }
        }
        public void SetSwitchEBDByCheckValue(bool bCheckValue)
        {
            foreach (var item in cSwitchEBD.listSwitchEBD)
            {
                if (item.bCheckValue == bCheckValue)
                {
                    m_CDataSwitchEBD = item;
                    return;
                }
            }
            m_CDataSwitchEBD = null;
        }
        public void SetSwitchEBDByTypecode(UInt16 TypeCode)
        {
            foreach (var item in cSwitchEBD.listSwitchEBD)
            {
                if (item.typeCode == TypeCode)
                {
                    m_CDataSwitchEBD = item;
                    return;
                }
            }
            m_CDataSwitchEBD = null;
        }
    }
}
