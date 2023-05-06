using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;


namespace ImHere
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private bool isConnected = false;
        private Thread? monitoringThread;

        public MainWindow()
        {
            InitializeComponent();
            client = new TcpClient();
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isConnected)
                {
                    client = new TcpClient();
                    client.Connect("127.0.0.1", 8080);
                    isConnected = true;
                    tbState.Text = "Connexion établie";

                    // Démarrer le thread de surveillance uniquement si ce n'est pas déjà fait
                    if (monitoringThread==null || !monitoringThread.IsAlive)
                    {
                        monitoringThread = new Thread(ReceiveMessagesFromServer)
                        {
                            IsBackground = true
                        };
                        monitoringThread.Start();
                    }
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

        private void Disconnect()
        {
            isConnected = false;
            if (client.Connected)
            {
                NetworkStream stream = client.GetStream();
                stream.Close(); // Ferme le flux du client
                client.Close(); // Ferme le client
                client.Dispose();
            }
        }

        private void ReceiveMessagesFromServer()
        {
            while (isConnected)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1];

                    // Modifier la ligne suivante pour lire au moins 1 octet du flux
                    bool isStillConnected = stream.Read(buffer, 0, buffer.Length) > 0;

                    if (!isStillConnected)
                    {
                        Disconnect();
                        Dispatcher.Invoke(() => tbState.Text = "Connexion interrompue");
                    }
                    //Thread.Sleep(100);
                }
                catch (IOException)
                {
                    Disconnect();
                    // arrive quand on ferme l'application FISA COPS sans déconnecter au préalable le serveur
                    Dispatcher.Invoke(() => tbState.Text = "Connexion interrompue erreur");
                }
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isConnected)
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
