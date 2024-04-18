using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using LUMOS.UICat;
using LUMOS.UICat.Controller;
using LUMOS.UICat.Core;
using LUMOS.UICat.Keyboard;
using System;

namespace LUMOS.Keyboard
{
    public class KeyboardManager : IKeyboardListener, ILayoutDependant
    {
        private ISoftKeyboard _softKeyboard;
        private readonly ICatBridge _catBridge;
        private double _buttonWidth = 50;
        private TextBox? _focussedInputBox = null;

        public KeyboardManager(ICatBridge catBridge)
        {
            _catBridge = catBridge;
            InitializeKeyboard(new LumosKeyboardNormal());
            _softKeyboard.GetView().IsVisible = false;
        }

        public void TriggerFocus(TextBox textBox)
        {
            HandleTextBox(textBox);
            if (textBox.Text == null) return;
            textBox.CaretIndex = textBox.Text.Length;
            textBox.SelectionStart = 0;
            textBox.SelectionEnd = textBox.Text.Length;
        }

        public bool LoseFocus()
        {
            Boolean wasKeyboardVisible = _softKeyboard.GetView().IsVisible;
            _softKeyboard.GetView().IsVisible = false;
            return wasKeyboardVisible;
        }

        public void InitializeKeyboard(ISoftKeyboard softKeyboard)
        {
            softKeyboard.SetListener(this);
            _softKeyboard = softKeyboard;
            _catBridge.Display(new CatApp(softKeyboard.GetView(), CatDisplayMode.Keyboard));
            AutoSize(_buttonWidth);
        }

        public void AutoSize(double width)
        {
            _buttonWidth = width;
            int targetSize = (int)(width / 10.0) - 2;

            if (targetSize > 25 && targetSize < 100)
                _softKeyboard.Resize(targetSize);
            else if (targetSize > 25) _softKeyboard.Resize(99);
            else if (targetSize < 100) _softKeyboard.Resize(26);

            if (targetSize < 40) _softKeyboard.ResizeFontSize(Constants.KeyboardSmallFontSize);
            else if (targetSize > 100) _softKeyboard.ResizeFontSize(Constants.KeyboardLargeFontSize);
            else _softKeyboard.ResizeFontSize(Constants.KeyboardDefaultFontSize);
        }

        public void PressedKey(string keyName, string keyContent, KeyboardTypes keyboardType)
        {
            if (keyName == Constants.KeyShift)
            {
                switch (keyboardType)
                {
                    case KeyboardTypes.Default:
                        InitializeKeyboard(new LumosKeyboardShift());
                        break;
                    case KeyboardTypes.Shift:
                        InitializeKeyboard(new LumosKeyboardCaps());
                        break;
                    case KeyboardTypes.Caps:
                        InitializeKeyboard(new LumosKeyboardNormal());
                        break;
                }
            }
            else if (keyName == Constants.KeyNumeric) InitializeKeyboard(new LumosKeyboardNumeric());
            else if (keyName == Constants.KeyNormal) InitializeKeyboard(new LumosKeyboardNormal());
            else if (keyName == Constants.KeyEmoticons) InitializeKeyboard(new LumosKeyboardEmoticons());
            else if (keyName == Constants.KeyHide) LoseFocus();
            else if (keyName == Constants.KeySpace) InputSymbol(" ");
            else if (keyName == Constants.KeyEnter) RaiseEnter();
            else if (keyName == Constants.KeyDelete) HandleDelete();
            else if (keyName == Constants.KeyBackSpace) HandleBackspace();
            else InputSymbol(keyContent);

            if (keyName != Constants.KeyShift && keyboardType == KeyboardTypes.Shift) InitializeKeyboard(new LumosKeyboardNormal());
        }

        private void RaiseEnter()
        {
            if (_focussedInputBox == null) return;
            _focussedInputBox.RaiseEvent(new KeyEventArgs()
            {
                RoutedEvent = InputElement.KeyDownEvent,
                Source = _focussedInputBox,
                Key = Key.Enter
            });
        }

        private void HandleBackspace()
        {
            if (_focussedInputBox == null || _focussedInputBox.CaretIndex == 0) return;

            int caretIndex = _focussedInputBox.CaretIndex;
            int selectionLength = _focussedInputBox.SelectionEnd - _focussedInputBox.SelectionStart;

            if (selectionLength > 0)
            {
                _focussedInputBox.Text = _focussedInputBox.Text.Remove(_focussedInputBox.SelectionStart, selectionLength);
                _focussedInputBox.CaretIndex = caretIndex - selectionLength;
            }
            else
            {
                _focussedInputBox.Text = _focussedInputBox.Text.Remove(caretIndex - 1, 1);
                _focussedInputBox.CaretIndex = caretIndex - 1;
            }
        }

        private void HandleDelete()
        {
            if (_focussedInputBox == null || _focussedInputBox.CaretIndex >= _focussedInputBox.Text.Length) return;

            int selectionLength = _focussedInputBox.SelectionEnd - _focussedInputBox.SelectionStart;
            if (selectionLength > 0)
            {
                _focussedInputBox.Text = _focussedInputBox.Text.Remove(_focussedInputBox.SelectionStart, selectionLength);
                _focussedInputBox.CaretIndex = _focussedInputBox.SelectionStart;
            }
            else
            {
                _focussedInputBox.Text = _focussedInputBox.Text.Remove(_focussedInputBox.CaretIndex, 1);
            }
        }

        private void InputSymbol(string keyContent)
        {
            if (_focussedInputBox == null) return;

            int selectionStart = _focussedInputBox.SelectionStart;
            int selectionLength = _focussedInputBox.SelectionEnd - _focussedInputBox.SelectionStart;

            if (selectionLength > 0)
            {
                _focussedInputBox.Text = _focussedInputBox.Text.Remove(selectionStart, selectionLength);
                _focussedInputBox.CaretIndex = selectionStart;
            }
            else if (_focussedInputBox.Text.Length == 0)
            {
                _focussedInputBox.CaretIndex = 0;
            }

            _focussedInputBox.Text = _focussedInputBox.Text.Insert(_focussedInputBox.CaretIndex, keyContent);
            _focussedInputBox.CaretIndex += keyContent.Length;
        }

        private TextBox? FindParentTextBox(Control control)
        {
            while (control != null && !(control is TextBox))
                control = control.Parent as Control;
            return control as TextBox;
        }

        private void HandleInput(TextPresenter textPresenter)
        {
            TextBox? textBox = FindParentTextBox(textPresenter);
            if (textBox == null) return;
            HandleTextBox(textBox);
        }

        private void HandleTextBox(TextBox textBox)
        {
            _softKeyboard.GetView().IsVisible = true;
            _focussedInputBox = textBox;
        }

        public void InputClickEvent(object sender, PointerReleasedEventArgs e)
        {
            if (e.Source is TextPresenter)
            {
                HandleInput(e.Source as TextPresenter);
            }
        }

        public void InputTapEvent(object sender, TappedEventArgs e)
        {
            if (e.Source is TextPresenter)
            {
                HandleInput(e.Source as TextPresenter);
            }
        }

        public void UpdateLayout(double windowWidth, double windowHeight)
        {
            AutoSize(windowWidth);
        }
    }
}
