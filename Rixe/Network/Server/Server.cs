using Rixe.Network;
using Rixe.Tools;
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

        // We now have a lock object that will be used to synchronize threads
        // during first access to the Singleton.
        private static readonly object _lock = new object();

        public event MyEventHandler eventSendProjectile;
        public event MyEventHandler1 eventSendWin;

        public Server()
        {
            this.ipep = new IPEndPoint(IPAddress.Any, 9050);
            this.newsock = new UdpClient(ipep);

            data = new byte[dataBufferedSize];

            Console.WriteLine("Waiting for a client...");

            sender = new IPEndPoint(IPAddress.Any, 9050);

            data = newsock.Receive(ref sender);

            Thread thread = new Thread(Receive);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

        }

        
        private void Receive()
        {
            // Using Listen() method we create
            // the Client list that will want
            // to connect to Server

            while (true)
            {
                data = newsock.Receive(ref sender);

                Console.WriteLine("Serveur à reçu : "  + Encoding.ASCII.GetString(data, 0, data.Length));
                //newsock.Send(data, data.Length, sender);

                string message = Encoding.ASCII.GetString(data, 0, data.Length);

                if (message == "win")
                {
                    EventSendWin();
                }
                else
                {
                    EventSendProjectile(message);
                }

            }
        }

        public void Send(string _message)
        {
            data = Encoding.ASCII.GetBytes(_message);
            newsock.Send(data, data.Length, sender);
        }

        public void EventSendProjectile(string message)
        {            
            eventSendProjectile(this, new ReceiveProjectileEvent() { Rectangle = message});
        }
        public void EventSendWin()
        {
            eventSendWin(this, new ReceiveWinEvent());
        }
    }
}
