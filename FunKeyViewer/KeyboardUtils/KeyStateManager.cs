// WunderVision 2024
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace FunKeyViewer.KeyboardUtils
{
    public class KeyStateManager
    {
        public List<IKeyState> PressedKeys { get; private set; } = new();
        public List<IKeyState> ComboKeys { get; private set; } = new();

        public event EventHandler? FirstKeyPressed;
        public event EventHandler<IKeyState>? KeyPressed;
        public event EventHandler? AllKeysReleased;

        private Dispatcher? _dispatcher;

        public KeyStateManager(Dispatcher? dispatcher)
        {
            _dispatcher = dispatcher;
        }

        private void OnFirstKeyPressed()
        {
            if (FirstKeyPressed == null) { return; }
            if (_dispatcher != null)
            {
                _dispatcher.Invoke(FirstKeyPressed, this, EventArgs.Empty);
            }
            else
            {
                FirstKeyPressed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnKeyPressed(IKeyState keyState)
        {
            if (KeyPressed == null) { return; }
            if (_dispatcher != null)
            {
                _dispatcher.Invoke(KeyPressed, this, keyState);
            }
            else
            {
                KeyPressed?.Invoke(this, keyState);
            }
        }

        private void OnAllKeysReleased()
        {
            if (AllKeysReleased == null) { return; }
            if (_dispatcher != null)
            {
                _dispatcher.Invoke(AllKeysReleased, this, EventArgs.Empty);
            }
            else
            {
                AllKeysReleased?.Invoke(this, EventArgs.Empty);
            }
        }

        protected void AddKeyPressed(IKeyState keyState)
        {
            PressedKeys.Add(keyState);
            OnKeyPressed(keyState);
            ComboKeys.Add(keyState);
            if (PressedKeys.Count == 1)
            {
                OnFirstKeyPressed();
            }
        }

        protected void RemoveKeyPressed(IKeyState keyState)
        {
            PressedKeys.Remove(keyState);
            if (PressedKeys.Count == 0)
            {
                OnAllKeysReleased();
                ComboKeys.Clear();
            }
        }

    }
}
