// WunderVision 2024
using System.Windows.Input;

namespace FunKeyViewer.KeyboardUtils
{
    public interface IKeyState
    {
        string Name { get; }
        bool IsPressed { get; set; }
    }

    public class KeyState : IKeyState
    {
        public Key Key { get; private set; }
        public string Name { get; private set; }
        public bool IsPressed { get; set; }

        public KeyState(Key key, bool isPressed)
        {
            Key = key;
            Name = key.ToString();
            IsPressed = isPressed;
        }
    }

    public class VKKeyState : IKeyState
    {
        public VK_KeyCode Key { get; private set; }
        public string Name { get; private set; }
        public bool IsPressed { get; set; }

        public VKKeyState(VK_KeyCode key, bool isPressed)
        {
            Key = key;
            Name = key.ToString();
            IsPressed = isPressed;
        }
    }
}
