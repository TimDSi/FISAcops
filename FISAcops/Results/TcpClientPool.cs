using System.Collections.Generic;
using System.Net.Sockets;

namespace FISAcops
{
    internal class TcpClientPool
    {
        private static TcpClientPool? instance;
        private static readonly object lockObject = new();
        private readonly Stack<TcpClient> pool;


        public static TcpClientPool GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    instance ??= new TcpClientPool();
                }
            }
            return instance;
        }

        private TcpClientPool()
        {
            pool = new Stack<TcpClient>();
        }

        public TcpClient Acquire()
        {
            lock (pool)
            {
                if (pool.Count > 0)
                {
                    return pool.Pop();
                }
            }

            return new TcpClient();
        }

        public void Release(TcpClient client)
        {
            if (client != null && client.Connected)
            {
                lock (pool)
                {
                    pool.Push(client);
                }
            }
        }
    }
}