using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Threading.Tasks;
using WindowsInput;

namespace RustFishingBot_GUI.Classes.Emulation
{
    internal class MouseEmulation
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;

        public static void MoveCursorWithRightClick(Point startPoint, Point endPoint)
        {
            SetCursorPos(startPoint.X, startPoint.Y);

            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            Thread.Sleep(300);

            int dx = endPoint.X - startPoint.X;
            int dy = endPoint.Y - startPoint.Y;

            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

            float xIncrement = (float)dx / steps;
            float yIncrement = (float)dy / steps;

            float x = startPoint.X;
            float y = startPoint.Y;

            for (int i = 0; i < steps; i++)
            {
                x += xIncrement;
                y += yIncrement;
                SetCursorPos((int)x, (int)y);
                System.Threading.Thread.Sleep(2); // Задержка между шагами 
            }

            // Отпускаем правую кнопку мыши
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
            Thread.Sleep(100);
        }

        public static async Task MoveCursorWithRightClickAsync(Point startPoint, Point endPoint)
        {
            await Task.Run(async () =>
            {
                SetCursorPos(startPoint.X, startPoint.Y);

                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                await Task.Delay(300);

                int dx = endPoint.X - startPoint.X;
                int dy = endPoint.Y - startPoint.Y;

                int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

                float xIncrement = (float)dx / steps;
                float yIncrement = (float)dy / steps;

                float x = startPoint.X;
                float y = startPoint.Y;

                for (int i = 0; i < steps; i++)
                {
                    x += xIncrement;
                    y += yIncrement;
                    SetCursorPos((int)x, (int)y);
                    await Task.Delay(1); // Задержка между шагами 
                }

                // Отпускаем правую кнопку мыши
                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                await Task.Delay(100);
            });
        }

        // кушает довольно много перативки. Надо шо то придумать
        public static async Task MoveCamera(int x=0, int y = 0)
        {
            await Task.Run(() =>
            {
                var simulator = new InputSimulator();
                simulator.Mouse.MoveMouseBy(x, y);
            });
        }

        public static void RightClick(Point point)
        {
            // Устанавливаем позицию курсора в указанную точку
            SetCursorPos(point.X, point.Y);

            // Нажимаем правую кнопку мыши
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);

            // Отпускаем правую кнопку мыши
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public static void LeftClick(Point point)
        {
            SetCursorPos(point.X, point.Y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static async Task SafeLeftClick(Point point)
        {
            SetCursorPos(point.X, point.Y);
            await Task.Delay(50);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            await Task.Delay(50);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static async Task CastFishingRodAsync()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            await Task.Delay(5000);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            await Task.Delay(5000);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public static async Task PullAsync()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            await Task.Delay(1000);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            await Task.Delay(1000);
        }

    }
}
