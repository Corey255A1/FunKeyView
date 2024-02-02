// WunderVision 2024
using FunKeyViewer.KeyboardUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace FunKeyViewer.ViewModel
{
    public class MainWindowViewModel
    {
        public int MaxComboElementHistory { get; } = 5;
        public int ItemRemoveSeconds { get; } = 2;

        private DispatcherTimer _itemRemoveTimer;
        private KeyStateManager _keyStateManager;

        public ObservableCollection<List<IKeyState>> KeyHistory { get; private set; } = new();
        public ObservableCollection<IKeyState> CurrentKeys { get; private set; } = new();
        public MainWindowViewModel(Window window)
        {
            _keyStateManager = new KeyStateLLManager(Dispatcher.CurrentDispatcher);
            _keyStateManager.AllKeysReleased += KeyStateManagerAllKeysReleased;
            _keyStateManager.KeyPressed += KeyStateManagerKeyPressed;

            _itemRemoveTimer = new DispatcherTimer();
            _itemRemoveTimer.Interval = TimeSpan.FromSeconds(ItemRemoveSeconds);
            _itemRemoveTimer.Tick += ItemRemoveTimer;
        }

        private void KeyStateManagerKeyPressed(object? sender, IKeyState e)
        {
            CurrentKeys.Add(e);
        }

        private void RemoveOldestKeyCombo()
        {
            if (KeyHistory.Count == 0) { return; }
            KeyHistory.RemoveAt(0);
        }

        private void ItemRemoveTimer(object? sender, EventArgs e)
        {
            RemoveOldestKeyCombo();
        }

        private void KeyStateManagerAllKeysReleased(object? sender, EventArgs e)
        {
            KeyHistory.Add(new List<IKeyState>(_keyStateManager.ComboKeys));
            CurrentKeys.Clear();
            _itemRemoveTimer.Stop();
            _itemRemoveTimer.Start();
            if (KeyHistory.Count > MaxComboElementHistory)
            {
                RemoveOldestKeyCombo();
            }
        }
    }
}
