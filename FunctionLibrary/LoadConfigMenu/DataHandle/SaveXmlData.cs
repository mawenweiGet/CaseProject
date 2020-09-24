using FunctionLibrary.LoadConfigMenu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace FunctionLibrary.LoadConfigMenu.DataHandle
{
    /// <summary>
    /// 保存菜单xml数据对象
    /// </summary>
    public class SaveXmlData
    {
        public bool Savexml(MenuData md)
        {
            //TODO: 弊端（只保存两级菜单）
            try
            {
                XmlDocument doc = new XmlDocument();

                XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmldecl, root);

                XmlElement ele = doc.CreateElement("MenuList");
                doc.AppendChild(ele);

                XmlElement language = doc.CreateElement("language");
                language.InnerText = md.menuInfo.Language;
                ele.AppendChild(language);

                XmlElement displayName = doc.CreateElement("displayName");
                displayName.InnerText = md.menuInfo.DisplayName;
                ele.AppendChild(displayName);

                XmlElement version = doc.CreateElement("version");
                version.InnerText = md.menuInfo.Version;
                ele.AppendChild(version);

                XmlElement author = doc.CreateElement("author");
                author.InnerText = md.menuInfo.Author;
                ele.AppendChild(author);

                List<MenuItemInfo> parentlist = md.menuItemList.FindAll(p => p.ParentMenuId == 0);
                foreach (var MenuItemInfo in parentlist)
                {
                    XmlElement menuItem = doc.CreateElement("MenuItem");
                    menuItem.SetAttribute("Name", MenuItemInfo.Name);
                    menuItem.SetAttribute("HeaderText", MenuItemInfo.HeaderText);
                    menuItem.SetAttribute("MenuId", MenuItemInfo.MenuId.ToString());
                    menuItem.SetAttribute("ParentMenuId", MenuItemInfo.ParentMenuId.ToString());
                    List<MenuItemInfo> childrenlist = md.menuItemList.FindAll(p => p.ParentMenuId == MenuItemInfo.MenuId);
                    foreach (var item in childrenlist)
                    {
                        XmlElement cmenuItem = doc.CreateElement("MenuItem");
                        cmenuItem.SetAttribute("Name", item.Name);
                        cmenuItem.SetAttribute("HeaderText", item.HeaderText);
                        cmenuItem.SetAttribute("MenuId", item.MenuId.ToString());
                        cmenuItem.SetAttribute("ParentMenuId", item.ParentMenuId.ToString());
                        cmenuItem.SetAttribute("FormName", item.FormName);
                        cmenuItem.SetAttribute("MenuName", item.MenuName);
                        cmenuItem.SetAttribute("OpenType", item.OpenType.ToString());
                        menuItem.AppendChild(cmenuItem);
                    }
                    ele.AppendChild(menuItem);
                }

                doc.Save("TestMenuInfo.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }
    }
}
