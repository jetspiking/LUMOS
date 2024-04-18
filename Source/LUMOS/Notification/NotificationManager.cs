using LUMOS.Notification.Views;
using LUMOS.UICat;
using LUMOS.UICat.Controller;
using System;

namespace LUMOS.Notification
{
    public class NotificationManager : ILayoutDependant
    {
        private readonly ICatBridge _catBridge;
        private readonly NotificationBar _notificationBar = new();

        public NotificationManager(ICatBridge catBridge)
        {
            _catBridge = catBridge;
            _catBridge.Display(new UICat.Core.CatApp(_notificationBar, UICat.Core.CatDisplayMode.Notification));
        }

        public void UpdateLayout(Double windowWidth, Double windowHeight)
        {
            _notificationBar.UpdateLayout(windowWidth, windowHeight);
        }
    }
}
