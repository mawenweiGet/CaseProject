using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace TestProject
{
    /// <summary>
    /// ClockUserCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class ClockUserCtrl : UserControl
    {
        public ClockUserCtrl()
        {
            InitializeComponent();
        }

        static ClockUserCtrl()
        {
            SetCommands();
        }

        private static void SetCommands()
        {
            CommandBinding commandBinding =
                new CommandBinding(SpeakCommand, new ExecutedRoutedEventHandler(ExecuteSpeak),
                new CanExecuteRoutedEventHandler(CanExecuteSpeak));

            CommandManager.RegisterClassCommandBinding(typeof(ClockUserCtrl), commandBinding);


            InputBinding inputBinding = new InputBinding(SpeakCommand, new MouseGesture(MouseAction.LeftClick));
            CommandManager.RegisterClassInputBinding(typeof(ClockUserCtrl), inputBinding);
        }

        private DispatcherTimer innerTimer;
        //private SpeechSynthesizer speecher = new SpeechSynthesizer();

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.SetTimer();
            this.SetBinding();
            this.SpeakTheTime();
        }



        private static void ExecuteSpeak(object sender, ExecutedRoutedEventArgs arg)
        {
            ClockUserCtrl clock = sender as ClockUserCtrl;
            if (clock != null)
            {
                clock.SpeakTheTime();
            }
        }

        private static void CanExecuteSpeak(object sender, CanExecuteRoutedEventArgs arg)
        {
            ClockUserCtrl clock = sender as ClockUserCtrl;
            arg.CanExecute = (clock != null);
        }

        private void SpeakTheTime()
        {
            DateTime localTime = this.Time.ToLocalTime();
            string textToSpeak = "现在时刻," +
                localTime.ToShortDateString() + "," +
                localTime.ToShortTimeString() +
                ",星期" + (int)localTime.DayOfWeek;

            //this.speecher.SpeakAsync(textToSpeak);
        }

        private void SetTimer()
        {
            this.innerTimer = new DispatcherTimer(TimeSpan.FromSeconds(1.0),
                DispatcherPriority.Loaded, new EventHandler(this.InnerTimerCallback), this.Dispatcher);
            this.innerTimer.Start();
        }

        private void InnerTimerCallback(object obj, EventArgs arg)
        {
            this.Time = DateTime.Now;
        }

        private void SetBinding()
        {
            Binding timeBinding = new Binding();
            timeBinding.Source = this;
            timeBinding.Path = new PropertyPath(ClockUserCtrl.TimeProperty);
            timeBinding.Converter = new TimeConverter();
            this.textBlock_Time.SetBinding(TextBlock.TextProperty, timeBinding);

            Binding dateBinding = new Binding();
            dateBinding.Source = this;
            dateBinding.Path = new PropertyPath(ClockUserCtrl.TimeProperty);
            dateBinding.Converter = new DateConverter();
            this.textBlock_Date.SetBinding(TextBlock.TextProperty, dateBinding);

        }



        #region DP

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(DateTime), typeof(ClockUserCtrl),
            new FrameworkPropertyMetadata(DateTime.Now, new PropertyChangedCallback(TimePropertyChangedCallback)));

        [Description("获取或设置当前日期和时间")]
        [Category("Common Properties")]
        public DateTime Time
        {
            get
            {
                return (DateTime)this.GetValue(TimeProperty);
            }
            set
            {
                this.SetValue(TimeProperty, value);
            }
        }

        private static void TimePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            if (sender != null && sender is ClockUserCtrl)
            {
                ClockUserCtrl clock = sender as ClockUserCtrl;
                clock.OnTimeUpdated((DateTime)arg.OldValue, (DateTime)arg.NewValue);

            }
        }

        #endregion


        #region Event

        public static readonly RoutedEvent TimeUpdatedEvent =
            EventManager.RegisterRoutedEvent("TimeUpdated",
             RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(ClockUserCtrl));

        [Description("日期或时间被更新后发生")]
        public event RoutedPropertyChangedEventHandler<DateTime> TimeUpdated
        {
            add
            {
                this.AddHandler(TimeUpdatedEvent, value);
            }
            remove
            {
                this.RemoveHandler(TimeUpdatedEvent, value);
            }
        }

        protected virtual void OnTimeUpdated(DateTime oldValue, DateTime newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime> arg =
                new RoutedPropertyChangedEventArgs<DateTime>(oldValue, newValue, TimeUpdatedEvent);
            this.RaiseEvent(arg);
        }

        #endregion

        #region Commands

        public static readonly RoutedUICommand SpeakCommand = new RoutedUICommand("Speak", "Speak", typeof(ClockUserCtrl));

        #endregion
    }
    #region Converter

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class TimeConverter : IValueConverter
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime date = (DateTime)value;

            return date.ToLocalTime().ToLongTimeString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateConverter : IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime date = ((DateTime)value).ToLocalTime();

            return date.ToShortDateString() + " " + date.DayOfWeek;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    #endregion
}
