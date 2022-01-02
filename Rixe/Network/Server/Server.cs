using Rixe.Network;
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
    class Server : INetwork
    {
        private const int dataBufferedSize = 4096;

        private IPEndPoint ipep;
        private IPEndPoint sender;
        private UdpClient newsock;

        private byte[] data;

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

        public void Send(string _message)
        {
            data = Encoding.ASCII.GetBytes(_message);
            newsock.Send(data, data.Length, sender);
        }
    }
}
