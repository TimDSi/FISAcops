using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FISAcops
{
    internal partial class Checker
    {
        // Création d'un serveur de socket
        private readonly TcpListener server = new(IPAddress.Any, 8080);
        public string ReceivedMessage = "";
        public Checker()
        {
            try
            {

                // Démarrage du serveur
                server.Start();

                // Attendre une connexion de client
                TcpClient client = server.AcceptTcpClient();

                // Récupération des données envoyées par le client
                byte[] data = new byte[1024];
                NetworkStream stream = client.GetStream();
                int bytesRead = stream.Read(data, 0, data.Length);
                ReceivedMessage = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);

                // Stockage du message reçu
                

                // Fermeture de la connexion avec le client
                client.Close();

                // Arrêt du serveur
                server.Stop();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
        }

        public void CheckerStop()
        {
            // Arrêt du serveur
            server.Stop();
        }
    }
}
