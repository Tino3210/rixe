using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rixe
{
    class Server
    {
        private const int dataBufferedSize = 4096;

        private static Server _instance;

        private IPEndPoint ipep;
        private IPEndPoint sender;
        private UdpClient newsock;

        private byte[] data;

        // We now have a lock object that will be used to synchronize threads
        // during first access to the Singleton.
        private static readonly object _lock = new object();

        public static Server GetInstance()
        {
            // This conditional is needed to prevent threads stumbling over the
            // lock once the instance is ready.
            if (_instance == null)
            {
                // Now, imagine that the program has just been launched. Since
                // there's no Singleton instance yet, multiple threads can
                // simultaneously pass the previous conditional and reach this
                // point almost at the same time. The first of them will acquire
                // lock and will proceed further, while the rest will wait here.
                lock (_lock)
                {
                    // The first thread to acquire the lock, reaches this
                    // conditional, goes inside and creates the Singleton
                    // instance. Once it leaves the lock block, a thread that
                    // might have been waiting for the lock release may then
                    // enter this section. But since the Singleton field is
                    // already initialized, the thread won't create a new
                    // object.
                    if (_instance == null)
                    {
                        _instance = new Server();
                    }
                }
            }
            return _instance;
        }

        public Server()
        {
            this.ipep = new IPEndPoint(IPAddress.Any, 9050);
            this.newsock = new UdpClient(ipep);

            data = new byte[dataBufferedSize];

            Console.WriteLine("Waiting for a client...");

            sender = new IPEndPoint(IPAddress.Any, 9050);

            data = newsock.Receive(ref sender);

            Thread thread = new Thread(Receive);
            thread.Start();

        }

        private void Receive()
        {
            // Using Listen() method we create
            // the Client list that will want
            // to connect to Server

            Console.WriteLine("Message received from {0}:", sender.ToString());
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

            string welcome = "Welcome to my test server";
            data = Encoding.ASCII.GetBytes(welcome);
            newsock.Send(data, data.Length, sender);

            while (true)
            {
                data = newsock.Receive(ref sender);

                Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
                newsock.Send(data, data.Length, sender);
            }
        }
    }
}
