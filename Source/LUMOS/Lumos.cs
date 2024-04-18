using Avalonia.Controls;
using LUMOS.Apps.Boot;
using LUMOS.Apps.Camera;
using LUMOS.Apps.Launcher;
using LUMOS.Keyboard;
using LUMOS.Liv;
using LUMOS.Navigation;
using LUMOS.Notification;
using LUMOS.UICat;
using LUMOS.UICat.Controller;
using System;
using System.Collections.Generic;

namespace LUMOS
{
    public class Lumos : ILayoutDependant, IInputEssentials, ISoftKeyListener
    {
        private readonly KeyboardManager _keyboardManager;
        private readonly NotificationManager _notificationManager;
        private readonly NavigationManager _navigationManager;
        private readonly AppManager _appManager;
        private readonly ICatBridge _catBridgeService;

        private readonly Starport _starport;

        public Lumos(ICatBridge catBridgeService)
        {
            _catBridgeService = catBridgeService;
            _keyboardManager = new(_catBridgeService);
            _notificationManager = new(_catBridgeService);
            _navigationManager = new(_catBridgeService, this as ISoftKeyListener);
            _appManager = new(_catBridgeService);

            Boot boot = new(_appManager as IDisplayManager);

            List<LauncherApp> launcherApps = new();
            launcherApps.Add(boot);
            launcherApps.Add(new Terminal(_appManager as IDisplayManager));

            boot.Start();

            _starport = new(launcherApps, _appManager);
        }

        public void FocussedSearchBox(TextBox textBox)
        {
            _keyboardManager.TriggerFocus(textBox);
        }

        public void PressedHome()
        {
            _keyboardManager.LoseFocus();
            _appManager.SwitchApp(_starport.Uuid);
        }

        public void PressedPrevious()
        {
            if (_keyboardManager.LoseFocus()) return;

            _appManager.OnPrevious();
        }

        public void PressedSearch()
        {
            _keyboardManager.LoseFocus();
            _starport.OnLiv();
            _appManager.SwitchApp(_starport.Uuid);
        }

        public void UpdateLayout(Double windowWidth, Double windowHeight)
        {
            _keyboardManager.UpdateLayout(windowWidth, windowHeight);
            _notificationManager.UpdateLayout(windowWidth, windowHeight);
            _navigationManager.UpdateLayout(windowWidth, windowHeight);
            _appManager.UpdateLayout(windowWidth, windowHeight);
        }
    }
}
