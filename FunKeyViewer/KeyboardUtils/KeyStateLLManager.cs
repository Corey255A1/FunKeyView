// WunderVision 2024
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace FunKeyViewer.KeyboardUtils
{
    public class KeyStateLLManager : KeyStateManager
    {
        private readonly Dictionary<VK_KeyCode, IKeyState> _keyMap = new();
        public KeyStateLLManager(Dispatcher? dispatcher) : base(dispatcher)
        {
            KeyboardHookMananger.KeyStateChanged += KeyStateChanged;
            foreach (var key in Enum.GetValues<VK_KeyCode>())
            {
                VKKeyState keyState = new(key, false);
                _keyMap[key] = keyState;
            }
        }

        private void KeyStateChanged(object? sender, IKeyState key)
        {
            if (!(key is VKKeyState keyState)) { return; }
            var currentState = _keyMap[keyState.Key];
            if (currentState.IsPressed == keyState.IsPressed) { return; }

            currentState.IsPressed = keyState.IsPressed;
            if (currentState.IsPressed)
            {
                AddKeyPressed(currentState);
            }
            else
            {
                RemoveKeyPressed(currentState);
            }
        }
    }
}