using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FunctionLibrary.LoadConfigMenu.Model
{
    [Serializable]
    public class MenuItemInfo
    {
        [XmlAttribute("Icon")]
        public string Icon { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("HeaderText")]
        public string HeaderText { get; set; }

        [XmlAttribute("MenuId")]
        public int MenuId { get; set; }

        [XmlAttribute("ParentMenuId")]
        public int ParentMenuId { get; set; }

        [XmlAttribute("FormName")]
        public string FormName { get; set; }

        [XmlAttribute("MenuName")]
        public string MenuName { set; get; }

        [XmlAttribute("OpenType")]
        public int OpenType { get; set; }

        [XmlElement("MenuItem")]
        public List<MenuItemInfo> MenuItemList { get; set; }
    }

    public enum OpenType
    {
        NewWindow = 0,              //窗口
        NewUsercontrol = 1,         //用户控件
    }
}
