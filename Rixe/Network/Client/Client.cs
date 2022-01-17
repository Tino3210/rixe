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
        private IPEndPoint sender;
        private EndPoint tmpRemote;

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
<<<<<<< HEAD
                this.localEndPoint = new IPEndPoint(IPAddress.Parse("157.26.105.37"), 9050);
=======
                this.localEndPoint = new IPEndPoint(IPAddress.Parse("157.26.104.212"), 9050);
>>>>>>> 4f228df38544c48ad1807b73420a7772c78dc8ff

                this.server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Console.WriteLine("Receive");

                this.sender = new IPEndPoint(IPAddress.Any, 0);
                this.tmpRemote = (EndPoint)sender;
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

            while (this.server.Connected)
            {
                int recv = server.Receive(data);

                Console.WriteLine("Message received from {0}:", tmpRemote.ToString());
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
    
                EventSendProjectile(Encoding.ASCII.GetString(data, 0, data.Length));
            }
        }



        public void Stop()
        {
            Console.WriteLine("Stopping client");
            this.server.Shutdown(SocketShutdown.Both);
        }

        public void EventSendProjectile(string message)
        {
            eventSendProjectile(this, new ReceiveProjectileEvent() { newRectangle = Serializable.StringToRectangle(message) });
        }
    }
}
