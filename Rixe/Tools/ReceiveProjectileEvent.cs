using System;
using System.Windows.Shapes;

namespace Rixe.Tools
{
    /// <summary>
    /// Message send by ProjectileListener
    /// </summary>
    public class ReceiveProjectileEvent: EventArgs
    {
        private static object syncObj = new object();
        private static string rectangleStr;
        public string Rectangle {
            get
            {
                lock (syncObj)
                {
                    return rectangleStr;
                }
            }
            set
            {
                lock (syncObj)
                {
                    rectangleStr = value;
                }
            }
        }
    }
}
