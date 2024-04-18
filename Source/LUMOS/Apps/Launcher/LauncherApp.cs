using LUMOS.Liv;
using System;

namespace LUMOS.Apps.Launcher
{
    public abstract class LauncherApp : AppBase
    {
        public String Icon;
        public String Name;

        protected LauncherApp(String icon, String name, String uuid) : base(uuid)
        {
            Icon = icon;
            Name = name;
        }
    }
}
