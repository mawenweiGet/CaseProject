using LocalTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FunctionLibrary.LoadConfigMenu.Model
{
    public class MenuData
    {
        public MenuInfo menuInfo;
        public List<MenuItemInfo> menuItemList = null;

        private const string resName = "MenuInfo";
        public MenuData()
        {
            this.menuItemList = new List<MenuItemInfo>();
            this.SerializerByStream(resName);
        }
        private void SerializerByStream(string value)
        {        
            Stream stream = FileTool.GetXmlobj("ResourceBase.dll", "ResourceBase.DataSurce.Xml.MenuInfo.xml");
            if (stream.Length > 0)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MenuInfo));
                try
                {
                     menuInfo = (MenuInfo)serializer.Deserialize(stream);
                    if (menuInfo != null && menuInfo.MenuItemList.Count > 0)
                    {
                        foreach (MenuItemInfo info in menuInfo.MenuItemList)
                        {
                            this.menuItemList.Add(info);
                            AddChild(info);
                        }
                    }
                }
                catch
                {
                    throw new Exception("Can't serializer '" + value + "' stream");
                }
            }
        }
        private void AddChild(MenuItemInfo item)
        {
            if (item.MenuItemList != null && item.MenuItemList.Count > 0)
            {
                foreach (MenuItemInfo info in item.MenuItemList)
                {
                    this.menuItemList.Add(info);
                    AddChild(info);
                }
            }
        }
    }
}
