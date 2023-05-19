using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;
using System.Text;

namespace FISAcops
{
    internal partial class Checker
    {
        private static Checker? instance;
        private static readonly object lockObject = new();

        private readonly TcpListener server;
        private readonly TcpClientPool tcpClientPool;
        private bool ServerOnline;

        public static TcpClient? LastClient;
        public string ReceivedMessage = "";

        private Checker()
        {
            server = new TcpListener(IPAddress.Any, 8080);
            tcpClientPool = TcpClientPool.GetInstance();
            ServerOnline = false;
        }

        public static Checker Instance
        {
            get
            {
                lock (lockObject)
                {
                    instance ??= new Checker();
                    return instance;
                }
            }
        }
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

                    // Ajouter le client à la pool
                    tcpClientPool.Release(client);

                    // Créer un nouveau thread pour gérer la communication avec le client
                    Thread clientThread = new(() => HandleClient(client))
                    {
                        IsBackground = true
                    };
                    clientThread.Start();
                }
            }
            catch (Exception) {}
        }


        public static void SendResponseToClient(TcpClient client, string response)
        {
            if (client.Connected)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(response);
                NetworkStream stream = client.GetStream();
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        private void HandleClient(TcpClient client)
        {
            if (client.Connected)
            {
                byte[] data = new byte[1024];
                NetworkStream stream = client.GetStream();

                try
                {
                    while (true)
                    {
                        int bytesRead = stream.Read(data, 0, data.Length);
                        ReceivedMessage = Encoding.UTF8.GetString(data, 0, bytesRead);
                        LastClient = client;

                        // Condition de sortie
                        if (bytesRead == 0)
                        {
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    // Fermer le flux réseau du client
                    stream.Close();

                    // Libérer le client en le remettant dans le pool
                    tcpClientPool.Release(client);
                }
            }
        }

    }
}
