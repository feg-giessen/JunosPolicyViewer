using System;
using System.Configuration;
using System.Windows;
using JunosPolicyViewer.Junos;

namespace JunosPolicyViewer
{
    /// <summary>
    /// Interaktionslogik für LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly Client client;

        public LoginWindow(Client client)
        {
            this.client = client;
            this.InitializeComponent();

            this.TextHost.Text = ConfigurationManager.AppSettings["host"];
            this.TextPort.Text = ConfigurationManager.AppSettings["port"];
            this.TextUser.Text = ConfigurationManager.AppSettings["user"];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.client.Load(this.TextHost.Text, Convert.ToInt32(this.TextPort.Text), this.TextUser.Text, this.TextPassword.Password);

            this.DialogResult = true;
            this.Close();
        }
    }
}
