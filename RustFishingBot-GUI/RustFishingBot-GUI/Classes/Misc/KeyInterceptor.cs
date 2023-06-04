using Gma.System.MouseKeyHook;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace RustFishingBot_GUI.Classes.Misc
{
    internal class KeyInterceptor : IDisposable
    {
        private IKeyboardMouseEvents _globalHook;

        public KeyInterceptor()
        {
            _globalHook = Hook.GlobalEvents();
            _globalHook.KeyDown += OnKeyDown;
        }

        public void Dispose()
        {
            _globalHook.KeyDown -= OnKeyDown;
            _globalHook.Dispose();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.End)
            {
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
