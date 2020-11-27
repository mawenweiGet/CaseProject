using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionLibrary.MessageProcessing
{
    public class NotificationWindowApp
    {
        public static List<NotificationWindow> _dialogs = new List<NotificationWindow>();
        public void AddNotification(string title, string content)
        {
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content))
            {
                NotificationWindow dialog = new NotificationWindow()
                {
                    m_Title = title,
                    m_Content = content
                };
                EventHandler _EventHandler = (senderobj, obj) =>
                {
                    var closedDialog = senderobj as NotificationWindow;
                    RemoveNotification(closedDialog);
                };

                dialog.Closed += _EventHandler;
                dialog.TopFrom = GetTopFrom();
                AddNotification(dialog);
                dialog.Show();
            }
        }
        public bool AddNotification(NotificationWindow dialog)
        {
            _dialogs.Add(dialog);
            return true;
        }
        public bool RemoveNotification(NotificationWindow dialog)
        {
            _dialogs.Remove(dialog);
            return true;
        }
        /// <summary>
        /// 获取停靠位置信息
        /// </summary>
        /// <returns></returns>
        public double GetTopFrom()
        {
            //屏幕的高度-底部TaskBar的高度。
            double topFrom = System.Windows.SystemParameters.WorkArea.Bottom - 10;
            bool isContinueFind = _dialogs.Any(o => o.TopFrom == topFrom);

            while (isContinueFind)
            {
                topFrom = topFrom - 110;//此处100是NotifyWindow的高 110-100剩下的10  是通知之间的间距
                isContinueFind = _dialogs.Any(o => o.TopFrom == topFrom);
            }

            if (topFrom <= 0)
                topFrom = System.Windows.SystemParameters.WorkArea.Bottom - 10;

            return topFrom;
        }
    }
}
