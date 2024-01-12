using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FunKeyViewer
{
    public class KeyState
    {
        public Key Key { get; private set; }
        public string Name { get; private set; }
        public bool IsPressed { get; private set; }
        public KeyState(Key key, string name)
        {
            Key = key;
            Name = name;
            IsPressed = false;
            UpdateState();
        }
        public bool UpdateState()
        {
            var state = Keyboard.GetKeyStates(Key);
            bool hasChanged = IsPressed != state.HasFlag(KeyStates.Down);
            if (!hasChanged) { return false; }

            IsPressed = !IsPressed;
            return true;
        }
    }
}
