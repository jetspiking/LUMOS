using Avalonia.Controls;
using Avalonia.Layout;
using LUMOS.UICat.Controller;
using System;

namespace LUMOS.Navigation.Views
{
    public partial class NavigationBar : UserControl, ILayoutDependant
    {
        public NavigationBar()
        {
            InitializeComponent();
        }

        public void UpdateLayout(Double windowWidth, Double windowHeight)
        {
            this.Menu.HorizontalAlignment = windowWidth > windowHeight ? HorizontalAlignment.Center : HorizontalAlignment.Stretch;
        }
    }
}
