using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Rixe.Network;
using System.Windows.Shapes;
using Rixe.Tools;

namespace Rixe
{

    class Client : INetwork
    {
        private const int dataBufferedSize = 4096;

        private IPEndPoint localEndPoint;

        private Socket server;

        private byte[] data;

        // We now have a lock object that will be used to synchronize threads
        // during first access to the Singleton.
        private static readonly object _lock = new object();

        public event MyEventHandler eventSendProjectile;

        public Client()
        {
            try
            {
                this.localEndPoint = new IPEndPoint(IPAddress.Parse("157.26.66.49"), 9050);

                this.server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Console.WriteLine("Receive");

                Send("Connected");

                Thread thread = new Thread(Receive);
                thread.Start();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }

        public void Send(string _message)
        {
            Console.WriteLine("Send");
            try
            {
                byte[] data = new byte[dataBufferedSize];
                data = Encoding.ASCII.GetBytes(_message);
                server.SendTo(data, data.Length, SocketFlags.None, localEndPoint);
            }
            // Manage of Socket's Exceptions
            catch (ArgumentNullException ane)
            {

                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }

            catch (SocketException se)
            {

                Console.WriteLine("SocketException : {0}", se.ToString());
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }

        private void Receive()
        {
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint tmpRemote = (EndPoint)sender;

            while (this.server.Connected)
            {

                data = new byte[1024];
                int recv = server.ReceiveFrom(data, ref tmpRemote);

                Console.WriteLine("Message received from {0}:", tmpRemote.ToString());
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
                EventSendProjectile();
            }
        }



        public void Stop()
        {
            Console.WriteLine("Stopping client");
            this.server.Shutdown(SocketShutdown.Both);
        }

        public void EventSendProjectile()
        {
            eventSendProjectile(this, new ReceiveProjectileEvent() { newRectangle = Serializable.StringToRectangle(BitConverter.ToString(data)) });
        }
    }
}
