using System;
using System.Linq;
using System.Threading.Tasks;
using static RustFishingBot_GUI.Classes.Misc.Constants;
using RustFishingBot_GUI.Classes.Misc;
using RustFishingBot_GUI.Classes.DataProcessors;
using RustFishingBot_GUI.Classes.Emulation;
using System.Drawing;
using Point = System.Drawing.Point;
using System.Reflection.Metadata;
using WindowsInput;
using System.Windows.Documents;
using System.Collections.Generic;

namespace RustFishingBot_GUI.Classes
{
    internal class BotMain
    {
        private static bool gotFish = false;

        // флаги для поиска сундука
        private static bool triedToFindChest = false;
        private static int chestX = -1, chestY = -1;

        // метод который проверяет не являеться ли пиксель зелынм в том месте где появляеться увдомления о новом предмете
        private static async Task FishChecker(Rectangle notoficationPosition)
        {
            Point newItemPixelPos = new Point(notoficationPosition.X + 5, notoficationPosition.Y + 5);
            while (true)
            {
                if (await Images.ComparePixelsAsync(newItemPixelPos, newItemColor))
                {
                    gotFish = true;
                    break;
                }
            }
        }
        public static async Task MainThread(LogsForm logsForm)
        {
            logsForm.AddLog("Бот начал свою работу!");
            WindowActivator.ActivateWindow("Rust");
            await Task.Delay(2000);
            while (true)
            {
                var slots = await Inventory.GetInventorySlots(logsForm);
                // подготовка
                await ProcessPlayerStats(slots, logsForm);

                // режим рыбу на приманку, которую поймали
                var toCut = slots.Where(item=>item.Info.Types == ItemsTypes.ToCut).ToList();
                await Inventory.DoJobWithItems(toCut, "Потрошить");

                // выбрасываем мусор
                var toThrowOut = slots.Where(item => item.Info.Types == ItemsTypes.Junk).ToList();
                await Inventory.DoJobWithItems(toThrowOut, "Выбросить");

                // ищим рыбу которую надо засейвить 
                var toSave = slots.Where(item => item.Info.Types == ItemsTypes.Safe).ToList();
                await SaveFish(toSave, logsForm);

                if (toCut.Count > 0)
                {
                    slots = await Inventory.GetInventorySlots(logsForm); // получаем информацию заново т.к могла появиться приманка получше
                }

                // начало процесса рыбалки
                var primanka = slots.OrderByDescending(item => item.Info.Primanka).FirstOrDefault(item => item.Info.Types == ItemsTypes.Primanka);
                var udochka = slots.FirstOrDefault(item => item.Info.Types == ItemsTypes.Udochka);
                if (udochka is null | primanka is null)
                {
                    logsForm.AddLog("Боту нечем рыбачить!");
                    return;
                }

                await MouseEmulation.MoveCursorWithRightClickAsync(primanka.position, udochka.position);
                await MouseEmulation.MoveCursorWithRightClickAsync(udochka.position, firstSlot);
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.Tab);
                await Task.Delay(100);
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.D1);
                await Task.Delay(3000);
                logsForm.AddLog("Начинаю закидывать удочку");
                await MouseEmulation.CastFishingRodAsync();
                gotFish = false;
                FishChecker(await Inventory.GetNewItemNotificationPosition(logsForm));
                while (!gotFish)
                {
                    logsForm.AddLog("Рыба ещё не поймана");
                    await MouseEmulation.PullAsync();
                }
                logsForm.AddLog("Поймал рыбу!");
                await Task.Delay(5000);

                // подождал анимацию поимки рыбки теперь ищем сундук
                if (!triedToFindChest)
                {
                    (chestX, chestY) = await Inventory.FindChest(logsForm);
                    triedToFindChest = true;
                }

                KeyboardEmulation.SimulateKeyPress(ConsoleKey.Tab);
                await Task.Delay(2000);
                await MouseEmulation.MoveCursorWithRightClickAsync(firstSlot, udochka.position);
            }
        }

        private static async Task ProcessPlayerStats(List<Item> slots, LogsForm logsForm)
        {
            (int hp, int water, int food) = await Inventory.GetPlayerStats(logsForm);

            if (hp <= 20 || (food <= 20 && food != 0))
            {
                var foodItem = slots.FirstOrDefault(item => item.Info.Types == ItemsTypes.Food);
                if (foodItem is not null)
                {
                    await Inventory.DoJobWithItem(foodItem, "Съесть");
                }
                else
                {
                    logsForm.AddLog("Я хочу кушац, но мне нечего кушац");
                }
            }
            
            if (water <= 20)
            {
                var waterItem = slots.FirstOrDefault(item => item.Info.Types == ItemsTypes.Drinks);
                if (waterItem is not null)
                {
                    await Inventory.DoJobWithItem(waterItem, "Выпить");
                }
                else
                {
                    logsForm.AddLog("Я хочу пить, но мне нечего пить");
                }
            }
        }
        private static async Task SaveFish(List<Item> toSave, LogsForm logsForm)
        {
            if (chestX != -1 && chestY != -1 && toSave.Count > 0)
            {
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.Tab);
                await Task.Delay(100);
                await MouseEmulation.MoveCamera(chestX, chestY);
                await Task.Delay(100);
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.E);
                await Task.Delay(300);
                foreach (var item in toSave)
                {
                    MouseEmulation.RightClick(item.position);
                    logsForm.AddLog($"Засейвил {item.Info.Name}| {item.position.X} {item.position.Y}");
                    await Task.Delay(500);
                }
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.Escape);
                await Task.Delay(100);
                await MouseEmulation.MoveCamera(-chestX, -chestY);
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.Tab);
                await Task.Delay(100);
            }
            else
            {
                logsForm.AddLog($"Мне нечего сейвить: {toSave.Count}| {chestX}: {chestY}");
            }
        }

        public static async Task TestMethod(LogsForm logsForm = null)
        {
            WindowActivator.ActivateWindow("Rust");
            await Task.Delay(2000);

            (chestX, chestY) = await Inventory.FindChest(logsForm);
            KeyboardEmulation.SimulateKeyPress(ConsoleKey.Tab);
            await Task.Delay(1000);
            var slots = await Inventory.GetInventorySlots(logsForm);
            var toSave = slots.Where(item => item.Info.Types == ItemsTypes.Safe).ToList();
            if (chestX != -1 && chestY != -1 && toSave.Count > 0)
            {
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.Tab);
                await Task.Delay(100);
                await MouseEmulation.MoveCamera(chestX, chestY);
                await Task.Delay(100);
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.E);
                await Task.Delay(300);
                foreach (var item in toSave)
                {
                    MouseEmulation.RightClick(item.position);
                    logsForm.AddLog($"Нажал на {item.Info.Name}");
                    await Task.Delay(500);
                }
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.Escape);
                await Task.Delay(100);
                await MouseEmulation.MoveCamera(-chestX, -chestY);
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.Tab);
                await Task.Delay(100);
            }
            else
            {
                logsForm.AddLog($"Мне нечего сейвить: {toSave.Count}| {chestX}: {chestY}");
            }


        }
    }
}
