using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;


namespace ImHere
{
    public partial class MainWindow : Window
    {
        private readonly TcpClient client = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!client.Connected)
                {
                    client.Connect("127.0.0.1", 8080);
                    tbState.Text = "Connexion établie";
                }
                else
                {
                    tbState.Text = "Connexion déjà établie";
                }
            }
            catch (Exception ex)
            {
                tbState.Text = "Erreur lors de la connexion : " + ex.Message;
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    tbState.Text = "Erreur : aucune connexion établie";
                    return;
                }

                byte[] data = Encoding.ASCII.GetBytes(tbCode.Text);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                tbState.Text = "Message envoyé : " + tbCode.Text;
            }
            catch (Exception ex)
            {
                tbState.Text = "Erreur lors de l'envoi du message : " + ex.Message;
            }
        }
    }
}
