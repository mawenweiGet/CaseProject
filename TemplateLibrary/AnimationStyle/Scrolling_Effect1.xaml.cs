using LocalTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TemplateLibrary.AnimationStyle
{
    /// <summary>
    /// Scrolling_Effect.xaml 的交互逻辑
    /// </summary>
    public partial class Scrolling_Effect1 : UserControl
    {
        private ObservableCollection<ListBindData> bindItems = null;//绑定的资源元素集合
        private Timer UpdateScrollTimer = null;//消失状态计时器
        private Storyboard storyboard = new Storyboard();
        FontLanguage font = new FontLanguage();
        public Scrolling_Effect1()
        {
            InitializeComponent();
            this.Tip.Content = font.TryFindResource("Online") as string;
            this.Loaded += Scrolling_Effect1_Loaded;
            bindItems = new ObservableCollection<ListBindData>();
        }

        private void Scrolling_Effect1_Loaded(object sender, RoutedEventArgs e)
        {
            this.listPic.ItemsSource = bindItems;//数据绑定
            this.bindItems.CollectionChanged += ListData_CollectionChanged;
            UpdateScrollTimer = new Timer(UpdateScrollTimerCallBack, null, 1000, Timeout.Infinite);
        }
        /// <summary>
        /// 数据集改变时加入动画
        /// </summary>
        private void ListData_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    this.listPic.UpdateLayout();
                    var listItem = this.listPic.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
                    var animation = new DoubleAnimation() { Duration = TimeSpan.FromMilliseconds(500), From = listItem.ActualWidth };
                    Storyboard.SetTarget(animation, listItem);
                    Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.X"));
                    storyboard.Children.Add(animation);
                    storyboard.Begin();
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    storyboard.Children.RemoveAt(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 定时器回调
        /// </summary>
        private void UpdateScrollTimerCallBack(object sender)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (bindItems.Count > 3)
                    bindItems.Remove(bindItems[3]);
                bindItems.Insert(0, new ListBindData(FilePath_Tool.Current_Run_Path() + @"res/1.png"));
            });
            if (UpdateScrollTimer != null)
                UpdateScrollTimer.Change(1000, Timeout.Infinite);
        }
        public class ListBindData : INotifyPropertyChanged
        {
            public ListBindData(string path)
            {
                itemPath = path;
            }

            private string itemPath;

            public string ItemPath
            {
                get
                {
                    return itemPath;
                }
                set
                {
                    if (itemPath != value)
                    {
                        itemPath = value;
                        OnPropertyChanged("ItemPath");
                    }
                }
            }

            private void OnPropertyChanged(string info)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
