using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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
using System.Threading;

namespace FISAcops
{
    /// <summary>
    /// Interaction logic for Checker.xaml
    /// </summary>
    public partial class Checker : Page
    {
        private void BtnMainPage(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.frame.Navigate(new MainPage());
        }

        public Checker()
        {
            InitializeComponent();
            MainChecker();

        }


        private void MainChecker()
        {
            try
            {
                // Création d'un serveur de socket
                TcpListener server = new TcpListener(IPAddress.Any, 8080);

                // Démarrage du serveur
                server.Start();

                // Attendre une connexion de client
                TcpClient client = server.AcceptTcpClient();

                // Récupération des données envoyées par le client
                byte[] data = new byte[1024];
                NetworkStream stream = client.GetStream();
                int bytesRead = stream.Read(data, 0, data.Length);
                string receivedMessage = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);

                // Fermeture de la connexion avec le client
                client.Close();

                // Arrêt du serveur
                server.Stop();

                // Stockage du message reçu
                tbStatus.Text = receivedMessage;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
        }
    }
}
