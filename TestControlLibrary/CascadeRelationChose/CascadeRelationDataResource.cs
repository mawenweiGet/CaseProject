using LocalTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestControlLibrary.CascadeRelationChose
{
    public class CascadeRelationDataResource: BaseModel
    {
        public CGraduation cGraduation;
        public CData_NumOfWires cData_NumOfWires;
        public delegate void Event_Handler();
        public event Event_Handler Event_ParameterChanged;
        private void OnParameterChanged()
        {
            if (Event_ParameterChanged != null)
            {
                Event_ParameterChanged();
            }
        }
        public CascadeRelationDataResource()
        {
            string strJson = "";
            FileTool.ReadFile(@"TestJson\Graduation.Json", ref strJson);
            cGraduation = Json_Operation.JsonToObject<CGraduation>(strJson);
            cData_NumOfWires = new CData_NumOfWires();
            m_TransducerSelected = cGraduation.listTransducers[0];
            m_SubTypeSelected = cGraduation.listTransducers[0].listSubType[0];
            m_UnitSelected = cGraduation.listTransducers[0].listSubType[0].listUnit[0];
            m_NumOfWiresSelected = cData_NumOfWires.listNumOfWires[0];

            OnParameterChanged();
        }
        #region 属性
        private CNumOfWires _NumOfWiresSelected;
        public CNumOfWires m_NumOfWiresSelected
        {
            get => _NumOfWiresSelected;
            set
            {
                _NumOfWiresSelected = value;
                if (value != null)
                {
                    m_bIfShow_CableResist = value.bIfShowCableResister;
                }
                OnPropertyChanged();
            }
        }
        private CTransducer _transducerSelected;
        public CTransducer m_TransducerSelected
        {
            get => _transducerSelected;
            set
            {
                _transducerSelected = value;
                OnPropertyChanged();
                OnParameterChanged();
                //顺序不能变
                if (value == null)
                {
                    m_SubTypeSelected = null;
                }
                else
                {
                    m_SubTypeSelected = _transducerSelected.listSubType[0];
                }
            }
        }
        private CSubType _subTypeSelected;
        public CSubType m_SubTypeSelected
        {
            get => _subTypeSelected;
            set
            {
                _subTypeSelected = value;
                OnPropertyChanged();
                OnParameterChanged();
                //顺序不能变
                if (value == null)
                {
                    m_UnitSelected = null;
                }
                else
                {
                    m_UnitSelected = _subTypeSelected.listUnit[0];
                    m_bIfShow_NumOfWires = _subTypeSelected.bIfShow_NumOfWires;
                    m_bIfShow_TypeCJC = _subTypeSelected.bIfShow_TypeCJC;
                    m_bIfShow_CustomTable = _subTypeSelected.bIfShow_CustomTable;
                }
            }
        }
        private CUnit _UnitSelected;
        public CUnit m_UnitSelected
        {
            get => _UnitSelected;
            set
            {
                _UnitSelected = value;
                OnPropertyChanged();
                OnParameterChanged();
            }
        }
        private bool _bIfShow_NumOfWires;
        public bool m_bIfShow_NumOfWires
        {
            get => _bIfShow_NumOfWires;
            set
            {
                _bIfShow_NumOfWires = value;
                OnPropertyChanged();
                OnPropertyChanged("m_bIfShow_CableResist");
            }
        }
        private bool _bIfShow_TypeCJC;
        public bool m_bIfShow_TypeCJC
        {
            get => _bIfShow_TypeCJC;
            set
            {
                _bIfShow_TypeCJC = value;
                OnPropertyChanged();
                OnPropertyChanged("m_bIfShow_fixedTcjc");
            }
        }
        private bool _bIfShow_CustomTable;
        public bool m_bIfShow_CustomTable
        {
            get => _bIfShow_CustomTable;
            set
            {
                _bIfShow_CustomTable = value;
                OnPropertyChanged();
            }
        }
        private bool _bIfShow_CableResist;
        public bool m_bIfShow_CableResist
        {

            get
            {
                if ((_bIfShow_CableResist == true) &&
                    (_bIfShow_NumOfWires == true))
                {
                    return true;
                }
                return false;
            }
            set
            {
                _bIfShow_CableResist = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public class CData_NumOfWires
        {
            public ObservableCollection<CNumOfWires> listNumOfWires { get; set; }
            public CData_NumOfWires()
            {
                listNumOfWires = new ObservableCollection<CNumOfWires>()
                    {

                        new CNumOfWires
                        {
                            typeCode = 0x03,
                            strNumOfWires = "3-Wires",
                            bIfShowCableResister=false
                        },
                        new CNumOfWires
                        {
                            typeCode = 0x04,
                            strNumOfWires = "4-Wires",
                            bIfShowCableResister=false
                        },
                        new CNumOfWires
                        {
                            typeCode = 0x02,
                            strNumOfWires = "2-Wires",
                            bIfShowCableResister=true
                        }
                    };
            }

        }
        public class CNumOfWires
        {
            public Byte typeCode { get; set; }
            public string strNumOfWires { get; set; }
            public bool bIfShowCableResister { get; set; }
        }
        public class CGraduation
        {
            public List<CTransducer> listTransducers { get; set; } = new List<CTransducer>();
        }
        public class CTransducer
        {
            public string strTransducerType { get; set; }

            public ObservableCollection<CSubType> listSubType { get; set; } = new ObservableCollection<CSubType>();

            public byte typeCode { get; set; }
        }
        public class CSubType
        {


            public Byte typeCode { get; set; }
            public string strSubType { get; set; }
            public bool bIfLimitCheck { get; set; }
            public float pvUpperLimit { get; set; }
            public float pvLowerLimit { get; set; }
            public float rangeMin { get; set; }
            public float pvOffsetMin { get; set; }
            public bool bIfShow_TypeCJC { get; set; }
            public bool bIfShow_CustomTable { get; set; }
            public bool bIfShow_NumOfWires { get; set; }

            public ObservableCollection<CUnit> listUnit { get; set; } = new ObservableCollection<CUnit>();
        }
        public class CUnit
        {
            public UInt16 typeCode { get; set; }
            public string strUnit { get; set; }
            public const UInt16 DEGREEC_UNIT = 0x20;
            public const UInt16 DEGREEF_UNIT = 0x21;
            public const UInt16 DEGREER_UNIT = 0x22;
            public const UInt16 DEGREEK_UNIT = 0x23;
            public const UInt16 MVOLTS_UNIT = 0x31;
            public const UInt16 VOLTSU_UNIT = 0x32;
            public const UInt16 MAMPERE_UNIT = 0x40;
            public const UInt16 AMPERE_UNIT = 0x41;
            public const UInt16 OHMSRES_UNIT = 0xA1;
            public const UInt16 KOHMS_UNIT = 0xA2;
            public const UInt16 MOHMS_UNIT = 0xA3;


            static public float UnitConvertToDefault(UInt16 typecodeUnit, float pvOrigin)
            {
                float pv = pvOrigin;

                /*标准为mV*/
                if (typecodeUnit == MVOLTS_UNIT)
                {
                    pv = pvOrigin;
                }
                else if (typecodeUnit == VOLTSU_UNIT) /*V转换*/
                {
                    pv = pvOrigin / 1000.0f;
                }
                else
                {/*不做任何操作*/
                }

                /*电位器标准为Ω*/
                if (typecodeUnit == OHMSRES_UNIT)
                {
                    pv = pvOrigin;
                }
                else if (typecodeUnit == KOHMS_UNIT)
                {
                    pv = pvOrigin / 1000.0f;
                }
                else if (typecodeUnit == MOHMS_UNIT)
                {
                    pv = pvOrigin / 1000000.0f;
                }
                else
                {/*不做任何操作*/
                }


                /*热电阻输入 热电偶输入 标准为℃*/
                if (typecodeUnit == DEGREEC_UNIT)
                {
                    pv = pvOrigin;
                }
                else if (typecodeUnit == DEGREER_UNIT)
                {   /*R ＝ F ＋ 495.69 ；*/
                    pv = pvOrigin * 9.0f / 5.0f + 527.69f;
                }
                else if (typecodeUnit == DEGREEK_UNIT)
                {   /*K ＝ C ＋ 273.16*/
                    pv = pvOrigin + 273.16f;
                }
                else if (typecodeUnit == DEGREEF_UNIT)
                {
                    /*F ＝ (C × 9 ／ 5) ＋ 32*/
                    pv = pvOrigin * 9.0f / 5.0f + 32.0f;
                }
                else
                {/*不做任何操作*/
                }


                /*标准为mA*/
                if (typecodeUnit == MAMPERE_UNIT)
                {
                    pv = pvOrigin;
                }
                else if (typecodeUnit == AMPERE_UNIT) /*V转换*/
                {
                    pv = pvOrigin / 1000.0f;
                }
                else
                {/*不做任何操作*/
                }

                return pv;
            }

            static public float UnitConvertFromDefault(UInt16 typecodeUnit, float pvOrigin)
            {
                float pv = pvOrigin;

                if (typecodeUnit == DEGREEC_UNIT)
                {
                    pv = pvOrigin;
                }
                else if (typecodeUnit == DEGREER_UNIT)
                {   /*R ＝ F ＋ 495.69 ；*/
                    pv = (pvOrigin - 527.69f) * 5.0f / 9.0f;
                }
                else if (typecodeUnit == DEGREEK_UNIT)
                {   /*C ＝ K - 273.16*/
                    pv = pvOrigin - 273.16f;
                }
                else if (typecodeUnit == DEGREEF_UNIT)
                {
                    /*F ＝ (C × 9 ／ 5) ＋ 32*/
                    pv = (pvOrigin - 32.0f) * 5.0f / 9.0f;
                }
                else
                {
                    /*不做任何操作*/
                }

                return pv;

            }
        }
    }
}
