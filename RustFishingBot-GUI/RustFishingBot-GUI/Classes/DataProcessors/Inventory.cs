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
using System.Windows.Forms;

namespace RustFishingBot_GUI.Classes.DataProcessors
{
    internal class Inventory
    {
        // метод, который получает информацию о предметах в инвенторе
        public static async Task<List<Item>> GetInventorySlots(LogsForm logsForm)
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
                        logsForm.AddLog($"{row}:{col} - {itemInfo.Name} | x={slotPoint.X}, y={slotPoint.Y}");
                    }
                    else
                    {
                        //logsForm.AddLog(itemText);
                    }

                    screenshot.Dispose();
                    slotImage.Dispose();
                }
            }
            return slots;
        }

        // метод, который определяет где будет появляться увдомление об новых предметах
        public static async Task<Rectangle> GetNewItemNotificationPosition(LogsForm logsForm)
        {
            Bitmap screen = await Images.CaptureScreenAsync();

            int currentY = firstNotificationPixel.Y;
            while (currentY>0)
            {
                Rectangle notificationRect = new Rectangle(firstNotificationPixel.X + 38, currentY, notificationWidth - 38, notificationHeight);
                Bitmap notificationPic = screen.Clone(notificationRect, PixelFormat.Format32bppArgb);
                string notficationText = await DataProcessors.Images.ReadTextFromImageAsync(notificationPic);
                if (string.IsNullOrEmpty(notficationText))
                {
                    logsForm.AddLog($"Уведомление о рыбе будет тут - {notificationRect.X}:{notificationRect.Y}");
                    screen.Dispose();
                    return notificationRect;
                }
                else
                {
                    logsForm.AddLog(notficationText + $" в {notificationRect.X}:{notificationRect.Y}");
                }
                currentY -= notificationPadding + notificationHeight;
                notificationPic.Dispose();
            }
            return errorRectangel;
        }

        // метод который получает состояние игрока
        public static async Task<(int,int,int)> GetPlayerStats(LogsForm logsForm)
        {
            Bitmap screen = await Images.CaptureScreenAsync();
            Bitmap healthPic = screen.Clone(healthRect, PixelFormat.Format32bppArgb);
            Bitmap waterPic = screen.Clone(waterRect, PixelFormat.Format32bppArgb);
            Bitmap foodPic = screen.Clone(foodRect, PixelFormat.Format32bppArgb);

            int hp = -1;
            int water = -1;
            int food = -1;
            int.TryParse(await Images.ReadTextFromImageAsync(healthPic), out hp);
            int.TryParse(await Images.ReadTextFromImageAsync(waterPic), out water);
            int.TryParse(await Images.ReadTextFromImageAsync(foodPic), out food);
            logsForm.AddLog($"HP = {hp}; Water = {water}; Food = {food}");

            screen.Dispose();
            healthPic.Dispose(); waterPic.Dispose(); foodPic.Dispose();

            return (hp, water, food);
        }

        // метод который выполняет действия с предметами, такие как потрошить, есть, выбросить и т.д
        public static async Task DoJobWithItems(List<Item> slots, string expectedActionName)
        {
            foreach (Item item in slots)
            {
                await DoJobWithItem(item, expectedActionName);
            }
        }

        public static async Task DoJobWithItem(Item item, string expectedActionName)
        {
            await MouseEmulation.SafeLeftClick(item.position);
            await Task.Delay(200);

            Bitmap screen = await Images.CaptureScreenAsync();
            foreach (var rect in buttonsPositions)
            {
                Bitmap cut = screen.Clone(rect, PixelFormat.Format32bppArgb);
                string actualActionName = await Images.ReadTextFromImageAsync(cut);
                cut.Dispose();
                if (actualActionName.ToUpper().Contains(expectedActionName.ToUpper()))
                {
                    for (int i = 0; i < 10; i++) // прокликиваем несколько раз, иначе можнт не сработать
                    {
                        await MouseEmulation.SafeLeftClick(new Point(rect.X, rect.Y));
                    }
                    break;
                }
            }
            screen.Dispose();
        }

        // метод который ищет на сколько надо повернуть камеру чтобы получть доступ к сундуку
        public static async Task<(int,int)> FindChest(LogsForm logsForm)
        {
            for (int moveX = 10; moveX < 1500; moveX += 100)
            {
                for (int moveY = -300; moveY < 300; moveY += 100)
                {
                    await MouseEmulation.MoveCamera(moveX, moveY);
                    await Task.Delay(200);
                    Bitmap scren = await Images.CaptureScreenAsync();
                    Bitmap actionPic = scren.Clone(actionRect, PixelFormat.Format32bppArgb);
                    string text = await Images.ReadTextFromImageAsync(actionPic);
                    scren.Dispose();
                    actionPic.Dispose();
                    if (!string.IsNullOrEmpty(text) && text.ToLower().Contains("открыть"))
                    {
                        logsForm.AddLog($"Нашёл сундук {moveX} {moveY}");
                        await MouseEmulation.MoveCamera(-moveX, -moveY);
                        return (moveX, moveY);
                    }
                    await MouseEmulation.MoveCamera(-moveX, -moveY);
                }
            }
            return (-1, -1);
        }
    }
}
