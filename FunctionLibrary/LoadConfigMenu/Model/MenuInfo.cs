using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FunctionLibrary.LoadConfigMenu.Model
{
    [Serializable]
    [XmlRoot("MenuList")]
    public class MenuInfo
    {
        [XmlElement("language")]
        public string Language { get; set; }

        [XmlElement("displayName")]
        public string DisplayName { get; set; }

        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("author")]
        public string Author { get; set; }

        [XmlElement("MenuItem")]
        public List<MenuItemInfo> MenuItemList { get; set; }
    }
}
