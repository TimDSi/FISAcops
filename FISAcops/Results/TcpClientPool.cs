using System.Collections.Generic;
using System.Net.Sockets;

namespace FISAcops
{
    internal class TcpClientPool
    {
        private readonly Stack<TcpClient> pool;

        public TcpClientPool()
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
                else
                {
                    return new TcpClient();
                }
            }
        }

        public void Release(TcpClient client)
        {
            lock (pool)
            {
                pool.Push(client);
            }
        }
    }
}