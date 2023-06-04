using System;
using System.Linq;
using System.Threading.Tasks;
using static RustFishingBot_GUI.Classes.Misc.Constants;
using RustFishingBot_GUI.Classes.Misc;
using RustFishingBot_GUI.Classes.DataProcessors;
using RustFishingBot_GUI.Classes.Emulation;

namespace RustFishingBot_GUI.Classes
{
    internal class BotMain
    {
        private static bool gotFish = false;
        private static async Task FishChecker()
        {
            while (true)
            {
                if (await Images.ComparePixelsAsync(newItemPixel, newItemColor))
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
                var primanka = slots.OrderBy(item => item.Info.Primanka).FirstOrDefault(item => item.Info.Types == ItemsTypes.Primanka);
                var udochka = slots.FirstOrDefault(item => item.Info.Types == ItemsTypes.Udochka);
                await MouseEmulation.MoveCursorWithRightClickAsync(primanka.position, udochka.position);
                await MouseEmulation.MoveCursorWithRightClickAsync(udochka.position, firstSlot);
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.Tab);
                await Task.Delay(100);
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.D1);
                await Task.Delay(3000);
                logsForm.AddLog("Начинаю закидывать удочку");
                await MouseEmulation.CastFishingRodAsync();
                gotFish = false;
                Task.Run(FishChecker);
                while (!gotFish)
                {
                    logsForm.AddLog("Рыба ещё не поймана");
                    await MouseEmulation.PullAsync();
                }
                logsForm.AddLog("Поймал рыбу!");
                await Task.Delay(4000);
                KeyboardEmulation.SimulateKeyPress(ConsoleKey.Tab);
                await Task.Delay(2000);
                await MouseEmulation.MoveCursorWithRightClickAsync(firstSlot, udochka.position);
            }
        }
    }
}
