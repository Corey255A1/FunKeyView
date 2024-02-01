// WunderVision 2024
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;

namespace FunKeyViewer.KeyboardUtils
{
    public class KeyStateThreadLooper : KeyStateManager
    {
        private readonly Dictionary<Key, IKeyState> _keyMap = new();
        private Thread? _keyMonitorThread;
        private bool _threadRunning = false;
        private const int THREAD_MS = 25;

        public KeyStateThreadLooper(Dispatcher? dispatcher) : base(dispatcher)
        {
            foreach (var key in Enum.GetValues<Key>())
            {
                if (key == Key.None) { continue; }
                KeyState keyState = new(key, false);
                KeyStateUpdater.UpdateState(keyState);
                _keyMap[key] = keyState;
                if (keyState.IsPressed) { PressedKeys.Add(keyState); }
            }
        }


        public void StartThread()
        {
            if (_keyMonitorThread != null) { throw new Exception("Thread is already running."); }

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


        private void ThreadLoop()
        {
            _threadRunning = true;
            while (_threadRunning)
            {
                Update();
                Thread.Sleep(THREAD_MS);
            }
        }

        private void Update()
        {
            foreach (var keyMapState in _keyMap.Values)
            {
                if (!KeyStateUpdater.UpdateState((KeyState)keyMapState)) { continue; }

                if (keyMapState.IsPressed) { AddKeyPressed(keyMapState); }
                else { RemoveKeyPressed(keyMapState); }
            }
        }
    }
}
