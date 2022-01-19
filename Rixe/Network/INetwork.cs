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
    interface INetwork
    {
        void Send(string _msg);
        
        event MyEventHandler eventSendProjectile;
        event MyEventHandler1 eventSendWin;
    }
}
