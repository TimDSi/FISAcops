using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
