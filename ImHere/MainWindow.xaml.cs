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
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (message == null)
                    {
                        Disconnect();
                        Dispatcher.Invoke(() => tbState.Text = "Connexion interrompue");
                    }
                    else if (message == "disconnect")
                    {
                        Disconnect();
                        Dispatcher.Invoke(() => tbState.Text = "Déconnecté par le serveur");
                    }
                    else
                    {
                        // Traitez le message reçu ici
                        Dispatcher.Invoke(() => tbState.Text = "Message reçu : " + message);
                    }
                }
                catch (IOException)
                {
                    Disconnect();
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
