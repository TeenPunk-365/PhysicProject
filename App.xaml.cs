using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows;

namespace PhysicProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Запускаем ImGui приложение в отдельном потоке
            Task.Run(() =>
            {
                using (var imguiApp = new ImGuiViewModel())
                {
                    imguiApp.Run();
                }

                // Когда ImGui приложение закрывается, закрываем WPF приложение
                Dispatcher.Invoke(() =>
                {
                    Shutdown();
                });
            });
        }
    }

}
