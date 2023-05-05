using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace FISAcops
{
    internal partial class Checker
    {
        private readonly TcpListener server = new(IPAddress.Any, 8080);
        public string ReceivedMessage = "";
        private bool ServerOnline = false;

        public Checker() { }
        public void CheckerStart()
        {
            try
            {
                server.Start();

                ServerOnline = true;
                while (ServerOnline)
                {
                    // Attendre une connexion de client
                    TcpClient client = server.AcceptTcpClient();

                    // Créer un nouveau thread pour gérer la communication avec le client
                    Thread clientThread = new(() => HandleClient(client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
        }

        public void CheckerStop()
        {
            ServerOnline = false;
            server.Stop();
            ReceivedMessage = "server stopped";
        }

        private void HandleClient(TcpClient client)
        {
            // Boucle de lecture de messages du client
            byte[] data = new byte[1024];
            NetworkStream stream = client.GetStream();

            try
            {
                while (true)
                {
                    int bytesRead = stream.Read(data, 0, data.Length);
                    ReceivedMessage = Encoding.ASCII.GetString(data, 0, bytesRead);

                    // Stockage du message reçu
                    // ...

                    // Condition de sortie
                    if (ReceivedMessage == "stop")
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception lors de la lecture du message: {0}", ex.Message);
            }

            // Fermeture de la connexion avec le client
            client.Close();
        }
    }
}
