using LUMOS.UICat.Controller;
using LUMOS.UICat.Core;
using System;
using System.Collections.Generic;

namespace LUMOS.Liv
{
    public abstract class AppBase : ILayoutDependant
    {
        public readonly String Uuid;

        protected AppBase(String uuid)
        {
            Uuid = uuid;
        }

        public abstract void Start();
        public abstract void Stop();
        public abstract List<CatApp> GetApp();
        public abstract void UpdateLayout(Double windowWidth, Double windowHeight);
        public abstract void OnPrevious();
    }
}
