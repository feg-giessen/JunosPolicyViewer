using System.Windows;
using JunosPolicyViewer.Junos;

namespace JunosPolicyViewer
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private readonly Client client = new Client();

        private void AppStart(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            Window window = new LoginWindow(this.client);

            if (window.ShowDialog() == true)
            {
                window = new PolicyOverviewWindow(this.client);
                window.ShowDialog();
            }

            Current.Shutdown(0);
        }
    }
}
