using FunctionLibrary.LoadConfigMenu.DataHandle;
using FunctionLibrary.LoadConfigMenu.Model;
using LocalTool;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace ProjectFrame
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MenuData md = new MenuData();
        UserControlLibraryHepler uch = new UserControlLibraryHepler();
        public MainWindow()
        {
            InitializeComponent();

            BindMenu(0,null);

        }
        private void BindMenu(int parentId, MenuItem item)
        {
            List<MenuItemInfo> list = md.menuItemList.FindAll(p => p.ParentMenuId == parentId);

            foreach (MenuItemInfo info in list)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Style = Resources["MenuItemsStyle"] as Style;
                menuItem.ToolTip = info.Icon;//info.Name;
                menuItem.Header = info.HeaderText;
                menuItem.Tag = info;
                menuItem.Click += new RoutedEventHandler(menuItem_Click);
                if (parentId == 0)
                {
                    this.MainMenu.Items.Add(menuItem);
                }
                else
                {
                    item.Items.Add(menuItem);
                }
                BindMenu(info.MenuId, menuItem);
            }
        }
        private void menuItem_Click(object sender, RoutedEventArgs e)
        {

            MenuItem item = sender as MenuItem;
            MenuItemInfo obj = (MenuItemInfo)(item.Tag);

            if (obj.OpenType == (int)OpenType.NewUsercontrol)
            {
                UserControl uc = uch.Get_PathDll_Usercontrol(obj.FormName, obj.MenuName);
                if (uc!=null)
                {
                    MainContent.Children.Clear();

                    MainContent.Children.Add(uc);
                }
            }
            else
            {
                Window window = uch.Get_PathDll_Window(obj.FormName, obj.MenuName);
                if (window!=null)
                {
                    window.Show();
                }
            }
        }

    }
}
