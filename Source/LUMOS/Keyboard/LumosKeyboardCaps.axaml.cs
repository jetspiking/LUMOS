using Avalonia.Controls;
using Avalonia.Interactivity;
using LUMOS.Keyboard;

namespace LUMOS.UICat.Keyboard
{
    public partial class LumosKeyboardCaps : UserControl, ISoftKeyboard
    {
        private IKeyboardListener? _keyboardListener;

        public LumosKeyboardCaps()
        {
            InitializeComponent();
        }

        public UserControl GetView()
        {
            return this;
        }

        public void Resize(double baseKeyWidth)
        {
            foreach (StackPanel keyRow in this.Keys.Children)
            {
                foreach (Button button in keyRow.Children)
                {
                    double keyWidth = baseKeyWidth;
                    if (button.Name == Constants.KeySpace || button.Name == Constants.KeyEnter)
                    {
                        int fillCount = (10 - keyRow.Children.Count);
                        keyWidth = ((fillCount + 1) * baseKeyWidth) + (fillCount * 2);
                    }
                    button.Width = keyWidth;
                }
            }
        }

        public void ResizeFontSize(int keyFontSize)
        {
            foreach (StackPanel keyRow in this.Keys.Children)
                foreach (Button button in keyRow.Children)
                    button.FontSize = keyFontSize;
        }

        public void SetListener(IKeyboardListener listener)
        {
            this._keyboardListener = listener;
        }

        private void OnKeyPress(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                this._keyboardListener?.PressedKey(button.Name, button.Content.ToString(), KeyboardTypes.Caps);
            }
        }
    }
}
