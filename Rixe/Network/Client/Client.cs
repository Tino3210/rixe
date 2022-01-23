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

    /// <summary>
    /// Client is use when the user join the game
    /// Implement INetwork
    /// </summary>
    class Client : INetwork
    {
        //Size of the buffer use to stock the data receive
        private const int dataBufferedSize = 4096;

        //Contains th IP adresse
        private IPEndPoint localEndPoint;
        private IPEndPoint sender;
        private EndPoint tmpRemote;

        //The socket server
        private Socket server;

        //The reveive data
        private byte[] data;

        //Thread isAlive
        private bool isAlive;
        private Thread thread;

        //A lock use for Threads
        private static readonly object _lock = new object();

        //Event use to notify the game
        public event MyEventHandler eventSendProjectile;
        public event MyEventHandler1 eventSendWin;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverIP"></param>
        public Client(string serverIP)
        {
            try
            {
                //Init connection
                this.localEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), 9050);

                //Create the socket
                this.server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                //Init the data with the buffered size
                data = new byte[dataBufferedSize];

                //Init connection
                this.sender = new IPEndPoint(IPAddress.Any, 0);
                this.tmpRemote = (EndPoint)sender;
                //Send to the client a message
                Send("Connected");

                //Thread of the connection
                this.isAlive = true;
                this.thread = new Thread(Receive);
                this.thread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Use to send message to the server
        /// </summary>
        /// <param name="_message"></param>
        public void Send(string _message)
        {
            try
            {
                byte[] data = new byte[dataBufferedSize];
                data = Encoding.ASCII.GetBytes(_message);
                //Send the message with the data
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

        /// <summary>
        /// Receive method is using to recive the messages of the server
        /// </summary>
        private void Receive()
        {

            while (isAlive)
            {
                try
                {
                    //Wait for a message
                    int recv = server.ReceiveFrom(data, ref tmpRemote);

                    //Transform the data to a string
                    string message = Encoding.ASCII.GetString(data, 0, recv);

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
        /// Stop the server
        /// </summary>
        /// <param name="_message"></param>
        public void Stop()
        {
            Console.WriteLine("Stopping client");
            //Shut down the sever
            isAlive = false;
            this.server.Shutdown(SocketShutdown.Both);
            server.Close();
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