using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;
using LUMOS.UICat.Controller;
using System;
using System.Timers;

namespace LUMOS.Notification.Views
{
    public partial class NotificationBar : UserControl, ILayoutDependant
    {
        private Timer? _updateTimer = null;

        public NotificationBar()
        {
            InitializeComponent();

            StartUpdateNotificationTimer();
            UpdateTime();
        }

        private void StartUpdateNotificationTimer()
        {
            _updateTimer?.Dispose();
            _updateTimer = new Timer(60000 - (DateTime.Now.Second * 1000 + DateTime.Now.Millisecond));
            _updateTimer.Elapsed += (sender, e) =>
            {
                _updateTimer.Interval = 60000;
                Dispatcher.UIThread.Post(() => UpdateTime());
            };
            _updateTimer.AutoReset = true;
            _updateTimer.Start();
        }

        private void UpdateTime()
        {
            this.TimeLabel.Content = GetTime();
        }

        private String GetTime()
        {
            return DateTime.Now.ToShortTimeString();
        }

        public void UpdateLayout(Double windowWidth, Double windowHeight)
        {
            this.NotificationDock.HorizontalAlignment = windowWidth > windowHeight ? HorizontalAlignment.Center : HorizontalAlignment.Stretch;
        }
    }
}
