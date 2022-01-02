﻿using System;

namespace Rixe.Network
{

    

    class MyNetwork
    {
        public enum Type
        {
            Client,
            Server
        }

        private static INetwork _instance;

        private static readonly object _lock = new object();
        public static INetwork GetInstance(Type type)
        {
            Console.WriteLine(_instance);
            // This conditional is needed to prevent threads stumbling over the
            // lock once the instance is ready.
            if (_instance == null)
            {
                Console.WriteLine("HELLO1");
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
                        Console.WriteLine("HELLO2");
                        if (type == Type.Server)
                        {
                            Console.WriteLine("HELLO3");
                            _instance = new Server();
                        }
                        else if (type == Type.Client)
                        {
                            Console.WriteLine("HELLO4");
                            _instance = new Client();
                        }
                            
                    }
                }
            }
            return _instance;
        }

        public static INetwork GetInstance()
        {
            Console.WriteLine(_instance);
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
    }
}
