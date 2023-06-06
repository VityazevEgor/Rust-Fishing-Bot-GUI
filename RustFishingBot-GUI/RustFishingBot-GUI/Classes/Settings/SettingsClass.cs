using System;
using System.Text.Json;
using System.IO;
namespace RustFishingBot_GUI.Classes.Settings
{
    internal class SettingsClass
    {
        public string TelegramBotToken { get; set; } = null;
        public int MaxFishingTime { get; set; } = 4;
        public bool UseChest { get; set; } = true;
        public int FishPullUpTime { get; set; } = 200;
        public bool UseTelegramBot {get; set;} = false;

        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(this, options);
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filePath = Path.Combine(appDataPath, "FishingBotSettings.txt");
            File.WriteAllText(filePath, jsonString);
        }

        public SettingsClass Load()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filePath = Path.Combine(appDataPath, "FishingBotSettings.txt");
            if (!File.Exists(filePath))
                return new SettingsClass();
            var jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<SettingsClass>(jsonString);
        }
    }
}