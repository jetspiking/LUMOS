using LUMOS.UICat;
using LUMOS.UICat.Controller;
using LUMOS.UICat.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LUMOS.Liv
{
    public class AppManager : ILayoutDependant, IDisplayManager
    {
        private readonly ICatBridge _catBridge;

        private AppBase? _focussedApp;
        private readonly List<AppBase> _apps = new();
        private readonly List<AppBase> _activeApps = new();

        public AppManager(ICatBridge catBridge)
        {
            _catBridge = catBridge;
        }

        public void Register(AppBase app)
        {
            _apps.Add(app);
        }

        public void Unregister(AppBase app)
        {
            _apps.Remove(app);
        }

        public Boolean SwitchApp(String uuid)
        {
            _focussedApp = _apps.Where(app => app.Uuid == uuid).ToList().FirstOrDefault();
            if (_focussedApp == null) return false;
            _focussedApp.GetApp()?.ForEach(app => _catBridge.Display(app));
            if (!_activeApps.Where(app => app.Uuid == uuid).Any())
            {
                _activeApps.Add(_focussedApp);
                _focussedApp.Start();
            }
            return true;
        }

        public void TerminateApp(String uuid)
        {
            AppBase? appToStop = _activeApps.Where(app => app.Uuid == uuid).ToList().FirstOrDefault();
            if (appToStop == null) return;
            _activeApps.Remove(appToStop);
            appToStop.Stop();
        }

        public List<AppBase> GetActiveApps()
        {
            return _activeApps;
        }

        public Boolean IsApp(String uuid)
        {
            return _focussedApp?.Uuid == uuid;
        }

        public void Display(CatApp catApp)
        {
            _catBridge.Display(catApp);
        }

        public void UpdateLayout(Double windowWidth, Double windowHeight)
        {
            _apps.ForEach(app => app.UpdateLayout(windowWidth, windowHeight));
        }

        public void OnPrevious()
        {
            _focussedApp?.OnPrevious();
        }
    }
}
