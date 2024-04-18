using Avalonia.Controls;

namespace LUMOS.Keyboard
{
    public interface ISoftKeyboard
    {
        public void Resize(double baseKeyWidth);
        public void ResizeFontSize(int keyFontSize);
        public void SetListener(IKeyboardListener listener);
        public UserControl GetView();
    }
}
