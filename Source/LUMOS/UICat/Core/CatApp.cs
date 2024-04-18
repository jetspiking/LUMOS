using Avalonia.Controls;

namespace LUMOS.UICat.Core
{
    public class CatApp
    {
        public Control? Control;
        public CatDisplayMode DisplayMode;

        public CatApp(Control? control, CatDisplayMode displayMode)
        {
            Control = control;
            DisplayMode = displayMode;
        }
    }
}
