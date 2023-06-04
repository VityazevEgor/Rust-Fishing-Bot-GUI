using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RustFishingBot_GUI
{
    /// <summary>
    /// Логика взаимодействия для LogsForm.xaml
    /// </summary>
    public partial class LogsForm : Window
    {
        public LogsForm()
        {
            InitializeComponent();
            Task.Run(UpdateMemoryUsageAsync);
        }
        public void AddLog(string message)
        {
            if (logPanel.Children.Count > 4) logPanel.Children.Clear();
            var textBlock = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 10)
            };
            logPanel.Children.Add(textBlock);
        }

        private async Task UpdateMemoryUsageAsync()
        {
            while (true)
            {
                Process currentProcess = Process.GetCurrentProcess();

                // Получаем количество используемой памяти в байтах
                long memoryUsageBytes = currentProcess.PrivateMemorySize64;

                // Преобразуем байты в мегабайты
                double memoryUsageMb = memoryUsageBytes / (1024.0 * 1024.0);
                // Обновляем содержимое текстового поля "Bot Logs"
                await Dispatcher.InvokeAsync(() =>
                {
                    Title.Text = $"Bot Logs | Использовано RAM: {memoryUsageMb}";
                });
                // Ждем 1 секунду перед выполнением следующей итерации цикла
                await Task.Delay(1000);
            }
        }
    }
}
