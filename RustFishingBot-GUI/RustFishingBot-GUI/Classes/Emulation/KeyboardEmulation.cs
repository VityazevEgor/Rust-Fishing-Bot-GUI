using System;
using System.Runtime.InteropServices;

namespace RustFishingBot_GUI.Classes.Emulation
{
    internal class KeyboardEmulation
    {
        private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const int KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void SimulateKeyPress(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Tab:
                    SimulateKeyDown(Keys.Tab);
                    SimulateKeyUp(Keys.Tab);
                    break;
                case ConsoleKey.D1:
                    SimulateKeyDown(Keys.D1);
                    SimulateKeyUp(Keys.D1);
                    break;
                default:
                    throw new NotSupportedException("This key is not supported for simulation.");
            }
        }

        private static void SimulateKeyDown(Keys key)
        {
            keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
        }

        private static void SimulateKeyUp(Keys key)
        {
            keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        public enum Keys : byte
        {
            Tab = 0x09,
            D1 = 0x31
        }
    }
}
