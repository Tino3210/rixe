using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Rixe.Network.Client
{

    class Client
    {


        private static Client _instance;


        private const int dataBufferedSize = 4096;

        private IPEndPoint localEndPoint;

        private Socket server;

        private byte[] data;

        // We now have a lock object that will be used to synchronize threads
        // during first access to the Singleton.
        private static readonly object _lock = new object();

        public Client()
        {
            try
            {
                this.localEndPoint = new IPEndPoint(IPAddress.Parse("157.26.66.60"), 9050);

                this.server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                Thread thread = new Thread(Receive);
                thread.Start();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }

        public static Client GetInstance()
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
                        _instance = new Client();
                    }
                }
            }
            return _instance;
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
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint tmpRemote = (EndPoint)sender;

            while (this.server.Connected)
            {

                data = new byte[1024];
                int recv = server.ReceiveFrom(data, ref tmpRemote);

                Console.WriteLine("Message received from {0}:", tmpRemote.ToString());
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
            }
        }

        public void Stop()
        {
            Console.WriteLine("Stopping client");
            this.server.Shutdown(SocketShutdown.Both);
        }

    }
}
