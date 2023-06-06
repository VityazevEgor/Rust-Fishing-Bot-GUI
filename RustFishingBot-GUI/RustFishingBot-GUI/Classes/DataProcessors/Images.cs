using System;
using System.Collections.Generic;
using System.Drawing;
using Tesseract;
using Tesseract.Interop;
using System.IO;
using System.Threading.Tasks;

namespace RustFishingBot_GUI.Classes.DataProcessors
{
    internal class Images
    {

        public static async Task<Bitmap> CaptureScreenAsync()
        {
            // FULL HD
            var screenBounds = new Rectangle(0, 0, 1920, 1080);

            return await Task.Run(() =>
            {
                // Создаем скриншот экрана
                Bitmap bitmap = new Bitmap(screenBounds.Width, screenBounds.Height);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(screenBounds.Left, screenBounds.Top, 0, 0, screenBounds.Size);
                }
                return bitmap;
            });
        }

        public static async Task<string> ReadTextFromImageAsync(Bitmap image)
        {
            using (var engine = new TesseractEngine(Directory.GetCurrentDirectory(), "rus", EngineMode.Default))
            {
                using (var pix = PixConverter.ToPix(image))
                {
                    var text = await Task.Run(() =>
                    {
                        using (var page = engine.Process(pix))
                        {
                            return page.GetText();
                        }
                    });
                    return text.Replace("\n", "").Replace(" ", "");
                }
            }
        }

        public static async Task<bool> ComparePixelsAsync(Point PixelPosition, Color ExpectedColor)
        {
            Bitmap screenshot = await CaptureScreenAsync();
            Color ActualColor = screenshot.GetPixel(PixelPosition.X, PixelPosition.Y);
            screenshot.Dispose();
            return ActualColor == ExpectedColor;
        }
        public static async Task<bool> ComparePixelsWithToleranceAsync(Point PixelPosition, Color ExpectedColor, int Tolerance=0)
        {
            Bitmap screenshot = await CaptureScreenAsync();
            Color ActualColor = screenshot.GetPixel(PixelPosition.X, PixelPosition.Y);
            screenshot.Dispose();
            int redDiff = Math.Abs(ActualColor.R - ExpectedColor.R);
            int greenDiff = Math.Abs(ActualColor.G - ExpectedColor.G);
            int blueDiff = Math.Abs(ActualColor.B - ExpectedColor.B);
            return redDiff <= Tolerance && greenDiff <= Tolerance && blueDiff <= Tolerance;
        }
    }
}
