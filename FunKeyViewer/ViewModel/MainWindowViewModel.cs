using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FunKeyViewer.ViewModel
{
    public class MainWindowViewModel
    {
        private DispatcherTimer _itemRemoveTimer;
        private KeyStateManager _keyStateManager;
        public const int MAX_COMBO_HISTORY = 5;
        public ObservableCollection<List<KeyState>> KeyHistory { get; private set; } = new();
        public ObservableCollection<KeyState> CurrentKeys { get; private set; } = new();
        public MainWindowViewModel(Window window)
        {
            _keyStateManager = new KeyStateManager(Dispatcher.CurrentDispatcher);
            _keyStateManager.AllKeysReleased += KeyStateManagerAllKeysReleased;
            _keyStateManager.KeyPressed += KeyStateManagerKeyPressed;
            _itemRemoveTimer = new DispatcherTimer();
            _itemRemoveTimer.Interval = TimeSpan.FromSeconds(2);
            _itemRemoveTimer.Tick += ItemRemoveTimer;


            window.SourceInitialized += WindowSourceInitialized;
            window.Closed += WindowClosed;
        }

        private void KeyStateManagerKeyPressed(object? sender, KeyState e)
        {
            CurrentKeys.Add(e);
        }

        private void RemoveOldestKeyCombo()
        {
            if(KeyHistory.Count == 0) { return; }
            KeyHistory.RemoveAt(0);
        }

        private void ItemRemoveTimer(object? sender, EventArgs e)
        {
            RemoveOldestKeyCombo();
        }

        private void KeyStateManagerAllKeysReleased(object? sender, EventArgs e)
        {
            KeyHistory.Add(new List<KeyState>(_keyStateManager.ComboKeys));
            CurrentKeys.Clear();
            _itemRemoveTimer.Stop();
            _itemRemoveTimer.Start();
            if (KeyHistory.Count > MAX_COMBO_HISTORY)
            {
                RemoveOldestKeyCombo();
            }
        }

        private void WindowClosed(object? sender, EventArgs e)
        {
            _keyStateManager.StopThread();
        }

        private void WindowSourceInitialized(object? sender, EventArgs e)
        {
            _keyStateManager.StartThread();
        }


    }
}
