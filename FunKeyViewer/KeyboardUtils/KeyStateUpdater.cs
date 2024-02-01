// WunderVision 2024
using System.Windows.Input;

namespace FunKeyViewer.KeyboardUtils
{
    public static class KeyStateUpdater
    {
        public static bool UpdateState(KeyState state)
        {
            var keyboarState = Keyboard.GetKeyStates(state.Key);
            bool hasChanged = state.IsPressed != keyboarState.HasFlag(KeyStates.Down);
            if (!hasChanged) { return false; }

            state.IsPressed = !state.IsPressed;
            return true;
        }
    }
}
