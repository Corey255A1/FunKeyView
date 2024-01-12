using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace FunKeyViewer
{
    public class KeyStateManager
    {
        public readonly Dictionary<Key, KeyState> KeyMap = new();
        public List<KeyState> PressedKeys { get; private set; } = new();
        public List<KeyState> ComboKeys { get; private set; } = new();

        public event EventHandler? FirstKeyPressed;
        public event EventHandler<KeyState>? KeyPressed;
        public event EventHandler? AllKeysReleased;

        private Dispatcher? _dispatcher;
        public Thread? _keyMonitorThread;
        private bool _threadRunning = false;
        public KeyStateManager(Dispatcher? dispatcher)
        {
            _dispatcher = dispatcher;
            foreach (var key in Enum.GetValues<Key>())
            {
                if (key == Key.None) { continue; }
                KeyState keyState = new(key, key.ToString());
                KeyMap[key] = keyState;
                if (keyState.IsPressed) { PressedKeys.Add(keyState); }
            }
        }

        public void StartThread()
        {
            _keyMonitorThread = new Thread(ThreadLoop);
            _keyMonitorThread.SetApartmentState(ApartmentState.STA);
            _keyMonitorThread.Start();
        }

        public void StopThread()
        {
            if (_keyMonitorThread == null) { return; }
            _threadRunning = false;
            _keyMonitorThread.Join();
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

        private void OnKeyPressed(KeyState keyState)
        {
            if(KeyPressed == null) { return; }
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

        private void AddKeyPressed(KeyState keyState)
        {
            PressedKeys.Add(keyState);
            OnKeyPressed(keyState);
            ComboKeys.Add(keyState);
            if (PressedKeys.Count == 1)
            {
                OnFirstKeyPressed();
            }
        }

        private void RemoveKeyPressed(KeyState keyState)
        {
            PressedKeys.Remove(keyState);
            if (PressedKeys.Count == 0)
            {
                OnAllKeysReleased();
                ComboKeys.Clear();
            }
        }

        private void ThreadLoop()
        {
            _threadRunning = true;
            while (_threadRunning)
            {
                Update();
                Thread.Sleep(25);
            }
        }

        public void Update()
        {
            foreach (var keyMapState in KeyMap.Values)
            {
                if (!keyMapState.UpdateState()) { continue; }

                if (keyMapState.IsPressed) { AddKeyPressed(keyMapState); }
                else { RemoveKeyPressed(keyMapState); }
            }
        }
    }
}
