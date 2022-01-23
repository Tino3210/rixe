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
    /// <summary>
    /// Server is use when the user host the game
    /// Implement INetwork
    /// </summary>
    class Server : INetwork
    {
        //Size of the buffer use to stock the data receive
        private const int dataBufferedSize = 4096;

        //Contains th IP adresse
        private IPEndPoint ipep;
        private IPEndPoint sender;

        //Provide the UDP service
        private UdpClient newsock;

        //The reveive data
        private byte[] data;
        private Thread thread;
        private bool isAlive;

        //A lock use for Threads
        private static readonly object _lock = new object();

        //Event use to notify the game
        public event MyEventHandler eventSendProjectile;
        public event MyEventHandler1 eventSendWin;

        /// <summary>
        /// Constructor
        /// </summary>
        public Server()
        {
            //Init connection
            this.ipep = new IPEndPoint(IPAddress.Any, 9050);
            //Init the sock UDP
            this.newsock = new UdpClient(ipep);

            //Init the data with the buffered size
            data = new byte[dataBufferedSize];

            //Init connection
            sender = new IPEndPoint(IPAddress.Any, 9050);

            //Wait a message from the client
            data = newsock.Receive(ref sender);

            //Thread of the connection
            this.isAlive = true;
            this.thread = new Thread(Receive);
            this.thread.SetApartmentState(ApartmentState.STA);
            this.thread.Start();
        }


        /// <summary>
        /// Receive method is using to recive the messages of the client
        /// </summary>
        private void Receive()
        {
            while (isAlive)
            {
                try
                {
                    //Wait to have a message
                    data = newsock.Receive(ref sender);

                    //Transfonm to string
                    string message = Encoding.ASCII.GetString(data, 0, data.Length);

                    if (message == "win")
                    {
                        EventSendWin(); //Send a win to the game
                    }
                    else
                    {
                        EventSendProjectile(message); //Send a projectille to the game
                    }
                }
                catch
                {

                }
                
            }
        }

        /// <summary>
        /// Use to send message to the client
        /// </summary>
        /// <param name="_message"></param>
        public void Send(string _message)
        {
            data = Encoding.ASCII.GetBytes(_message);
            //Send the message
            newsock.Send(data, data.Length, sender);
        }

        public void Stop()
        {
            isAlive = false;
            newsock.Close();
        }

        /// <summary>
        /// Notify the game that the client send a projectille
        /// </summary>
        /// <param name="message"></param>
        public void EventSendProjectile(string message)
        {
            eventSendProjectile(this, new ReceiveProjectileEvent() { Rectangle = message });
        }

        /// <summary>
        /// Notify the game that the client send a win
        /// </summary>
        public void EventSendWin()
        {
            eventSendWin(this, new ReceiveWinEvent());
        }
    }
}
