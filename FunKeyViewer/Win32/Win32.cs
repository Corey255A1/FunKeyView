// WunderVision 2024
using System.Runtime.InteropServices;

namespace FunKeyViewer.Windows
{
    public static class Win32
    {
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        // Declare the LowLevelKeyboardProc delegate
        public delegate nint LowLevelKeyboardProc(int nCode, nint wParam, nint lParam);

        // Import the necessary Win32 functions
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern nint SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, nint hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(nint hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern nint CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern nint GetModuleHandle(string lpModuleName);
    }
}
