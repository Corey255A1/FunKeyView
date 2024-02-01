// WunderVision 2024
using FunKeyViewer.Windows;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FunKeyViewer.KeyboardUtils
{
    public static class KeyboardHookMananger
    {
        private static nint _hookId = nint.Zero;
        private static event EventHandler<IKeyState>? _keyStateChanged;
        public static event EventHandler<IKeyState>? KeyStateChanged
        {
            add
            {
                _keyStateChanged += value;
                AddHook();
            }
            remove
            {
                _keyStateChanged -= value;
                if (_keyStateChanged == null)
                {
                    RemoveHook();
                }
            }
        }

        private static nint HookCallback(int nCode, nint wParam, nint lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == Win32.WM_KEYDOWN)
                {
                    int virtualKeyCode = Marshal.ReadInt32(lParam);
                    var keyState = new VKKeyState((VK_KeyCode)virtualKeyCode, true);
                    _keyStateChanged?.Invoke(null, keyState);
                }
                else if (wParam == Win32.WM_KEYUP)
                {
                    int virtualKeyCode = Marshal.ReadInt32(lParam);
                    var keyState = new VKKeyState((VK_KeyCode)virtualKeyCode, false);
                    _keyStateChanged?.Invoke(null, keyState);
                }
            }
            return Win32.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        public static bool AddHook()
        {
            if (_hookId != nint.Zero) { return false; }
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule? curModule = curProcess.MainModule)
                {
                    if (curModule == null) { return false; }

                    _hookId = Win32.SetWindowsHookEx(Win32.WH_KEYBOARD_LL, HookCallback, Win32.GetModuleHandle(curModule.ModuleName), 0);

                    if (_hookId == nint.Zero) { return false; }
                }
            }
            return true;
        }

        public static void RemoveHook()
        {
            Win32.UnhookWindowsHookEx(_hookId);
            _hookId = nint.Zero;
        }
    }
}
