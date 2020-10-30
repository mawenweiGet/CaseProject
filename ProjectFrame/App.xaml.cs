using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectFrame
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static string Language { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            GetLanguage();
        }
        #region Language Method

        private void GetLanguage()
        {
            Language = string.Empty;
            try
            {
                Language = ProjectFrame.Properties.Settings.Default.Language.Trim();
            }
            catch
            {
                ;
            }
            Language = string.IsNullOrEmpty(Language) ? "Font-zh-CN" : Language;

            UpdateLanguage();
        }

        private void SaveLanguage()
        {
            try
            {
                ProjectFrame.Properties.Settings.Default.Language = Language;
                ProjectFrame.Properties.Settings.Default.Save();
            }
            catch
            {
                ;
            }
        }

        private static void UpdateLanguage()
        {
            ResourceDictionary newRD = new ResourceDictionary();
            newRD.Source = new Uri("pack://application:,,,/ResourceBase;component/Language/" +
               Language + ".xaml", UriKind.RelativeOrAbsolute);
            Current.Resources.MergedDictionaries[0] = newRD;

        }

        public static void ChooseLanguage(eLanguage language)
        {
            switch (language)
            {
                case eLanguage.Chinese:
                    Language = "Font-zh-CN";
                    break;
                case eLanguage.English:
                    Language = "Font-en-US";
                    break;
            }
            UpdateLanguage();
        }
        public enum eLanguage
        {
            Chinese,
            English
        }
        #endregion
    }
}
