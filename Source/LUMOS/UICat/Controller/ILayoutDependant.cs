using System;

namespace LUMOS.UICat.Controller
{
    public interface ILayoutDependant
    {
        public void UpdateLayout(Double windowWidth, Double windowHeight);
    }
}
