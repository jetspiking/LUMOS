using LUMOS.Navigation.Views;
using LUMOS.UICat;
using LUMOS.UICat.Controller;
using System;

namespace LUMOS.Navigation
{
    internal class NavigationManager : ILayoutDependant
    {
        private readonly ICatBridge _catBridge;
        private readonly ISoftKeyListener _softKeyListener;
        private readonly NavigationBar _navigationBar = new();

        public NavigationManager(ICatBridge catBridge, ISoftKeyListener softKeyListener)
        {
            _catBridge = catBridge;
            _softKeyListener = softKeyListener;
            _catBridge.Display(new UICat.Core.CatApp(_navigationBar, UICat.Core.CatDisplayMode.Navigation));

            _navigationBar.Previous.PointerPressed += (s, e) => _softKeyListener.PressedPrevious();
            _navigationBar.Home.PointerPressed += (s, e) => _softKeyListener.PressedHome();
            _navigationBar.Search.PointerPressed += (s, e) => _softKeyListener.PressedSearch();
        }

        public void UpdateLayout(Double windowWidth, Double windowHeight)
        {
            _navigationBar.UpdateLayout(windowWidth, windowHeight);
        }
    }
}
