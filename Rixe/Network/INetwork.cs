using Rixe.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rixe.Network
{
    public delegate void MyEventHandler(object source, ReceiveProjectileEvent args);
    public delegate void MyEventHandler1(object source, ReceiveWinEvent args);

    /// <summary>
    /// Interface use by the server and the client
    /// </summary>
    interface INetwork
    {
        void Send(string _msg);
        void Stop();

        event MyEventHandler eventSendProjectile;
        event MyEventHandler1 eventSendWin;
    }
}
