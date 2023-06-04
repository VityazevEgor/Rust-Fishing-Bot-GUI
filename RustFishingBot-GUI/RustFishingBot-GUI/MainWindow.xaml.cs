using System.Windows;
using RustFishingBot_GUI.Classes.Misc;
using System.IO;
using System.Reflection;
using Path = System.IO.Path;
using RustFishingBot_GUI.Classes;
using System.Threading.Tasks;
using RustFishingBot_GUI.Classes.DataProcessors;
using System.Threading;

namespace RustFishingBot_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KeyInterceptor hook = new KeyInterceptor();

        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists("rus.traineddata"))
            {
                File.WriteAllBytes("rus.traineddata", RustFishingBot_GUI.Properties.Resources.rus);
            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            LogsForm logsForm = new LogsForm();
            logsForm.Show();
            BotMain.MainThread(logsForm);
        }
    }
}
