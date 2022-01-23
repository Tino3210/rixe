using System;

namespace Rixe.Network
{
    /// <summary>
    /// Singleton that create a server or a client
    /// </summary>
    class MyNetwork
    {
        // Type enum to select Server or Client
        public enum Type
        {
            Client,
            Server
        }

        // instance of server or client
        private static INetwork _instance;

        // Lock
        private static readonly object _lock = new object();

        /// <summary>
        /// Create a Server or Client
        /// </summary>
        /// <param name="type">Connection type</param>
        /// <param name="ip">Server Ip</param>
        public static INetwork GetInstance(Type type, string ip)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        if (type == Type.Server)
                        {
                            _instance = new Server();
                        }
                        else if (type == Type.Client)
                        {
                            _instance = new Client(ip);
                        }
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Create a server if no instance
        /// </summary>
        public static INetwork GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Server();
                    }
                }
            }
            return _instance;
        }

        public static void Disconnect()
        {
            _instance.Stop();
            _instance = null;
        }

    }
}
