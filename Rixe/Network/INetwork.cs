using Rixe.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rixe.Network
{
    public delegate void MyEventHandler(object source, ReceiveProjectileEvent args);
    interface INetwork
    {
        void Send(string _msg);
        
        event MyEventHandler eventSendProjectile;
    }
}
