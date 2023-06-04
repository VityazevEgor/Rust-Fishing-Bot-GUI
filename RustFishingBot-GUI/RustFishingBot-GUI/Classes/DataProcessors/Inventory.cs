using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static RustFishingBot_GUI.Classes.Misc.Constants;
using RustFishingBot_GUI.Classes.Emulation;
using RustFishingBot_GUI.Classes.DataProcessors;
using System.Drawing.Imaging;
using System.Windows;
using Point = System.Drawing.Point;

namespace RustFishingBot_GUI.Classes.DataProcessors
{
    internal class Inventory
    {
        public static async Task<List<Item>> GetInventorySlots(LogsForm logsForm = null)
        {
            var slots = new List<Item>();
            int inventoryWidth = 6;
            int inventoryHeight = 4;
            int slotSize = 90;
            int slotSpacing = 6;
            int startX = 660;
            int startY = 574;
            for (int row = 0; row < inventoryHeight; row++)
            {
                for (int col = 0; col < inventoryWidth; col++)
                {
                    int slotX = startX + col * (slotSize + slotSpacing);
                    int slotY = startY + row * (slotSize + slotSpacing);
                    Point slotPoint = new Point(slotX + 20, slotY + 20);
                    MouseEmulation.LeftClick(slotPoint);
                    await Task.Delay(200);

                    Bitmap screenshot = await Images.CaptureScreenAsync();
                    Rectangle slotRect = new Rectangle(655, 102, 350, 30);
                    Bitmap slotImage = screenshot.Clone(slotRect, PixelFormat.Format32bppArgb);

                    string itemText = await Images.ReadTextFromImageAsync(slotImage);
                    var itemInfo = itemsInfo.FirstOrDefault(item => itemText.Contains(item.Name));
                    if (itemInfo is not null)
                    {
                        slots.Add(new Item(itemInfo, slotPoint));
                        if (logsForm is not null)
                        {
                            logsForm.AddLog($"{row}:{col} - {itemInfo.Name} | x={slotPoint.X}, y={slotPoint.Y}");
                        }
                    }

                    screenshot.Dispose();
                    slotImage.Dispose();
                }
            }
            return slots;
        }
    }
}
