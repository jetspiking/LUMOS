using LUMOS.UICat.Core;
using System;

namespace LUMOS.Liv
{
    public interface IDisplayManager
    {
        public void Display(CatApp catApp);
        public Boolean SwitchApp(String uuid);

    }
}
