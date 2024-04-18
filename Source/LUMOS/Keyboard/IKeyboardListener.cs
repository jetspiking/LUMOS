namespace LUMOS.Keyboard
{
    public interface IKeyboardListener
    {
        public void PressedKey(string keyName, string keyContent, KeyboardTypes keyboardType);
    }
}
