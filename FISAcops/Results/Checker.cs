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
        private readonly TcpClientPool clientPool;
        private bool ServerOnline;

        public static TcpClient? LastClient;
        public string ReceivedMessage = "";

        private Checker()
        {
            server = new TcpListener(IPAddress.Any, 8080);
            clientPool = new TcpClientPool();
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
                    TcpClient client = clientPool.Acquire();

                    // Créer un nouveau thread pour gérer la communication avec le client
                    Thread clientThread = new(() =>
                    {
                        try
                        {
                            HandleClient(client);
                        }
                        finally
                        {
                            clientPool.Release(client);
                        }
                    })
                    {
                        IsBackground = true
                    };
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
            // Boucle de lecture de messages du client
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
        }
    }

}
