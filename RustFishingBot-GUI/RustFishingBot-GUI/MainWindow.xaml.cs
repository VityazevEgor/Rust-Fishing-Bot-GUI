using System.Windows;
using RustFishingBot_GUI.Classes.Misc;
using System.IO;
using RustFishingBot_GUI.Classes;
using RustFishingBot_GUI.Classes.Settings;

namespace RustFishingBot_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KeyInterceptor hook = new KeyInterceptor();
        SettingsClass settings = new SettingsClass();
        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists("rus.traineddata"))
            {
                File.WriteAllBytes("rus.traineddata", RustFishingBot_GUI.Properties.Resources.rus);
            }
            settings = settings.Load();
            MaxFishingTimeTextBox.Text = settings.MaxFishingTime.ToString();
            FishPullTimeTextBox.Text = settings.FishPullUpTime.ToString();
            SaveLootToChestCheckBox.IsChecked = settings.UseChest;
            TelegramBotTokenTextBox.Text = settings.TelegramBotToken;
            UseTelegramBotCheckBox.IsChecked = settings.UseTelegramBot;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            settings.MaxFishingTime = int.Parse(MaxFishingTimeTextBox.Text);
            settings.FishPullUpTime = int.Parse(FishPullTimeTextBox.Text);
            settings.UseChest = (bool)SaveLootToChestCheckBox.IsChecked;
            settings.UseTelegramBot = (bool)UseTelegramBotCheckBox.IsChecked;
            settings.TelegramBotToken = TelegramBotTokenTextBox.Text;
            settings.Save();
            LogsForm logsForm = new LogsForm();
            logsForm.Show();
            BotMain.MainThread(logsForm, settings);
            //BotMain.TestMethod(logsForm);
        }
    }
}
