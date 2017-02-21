using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Networking.Modules
{
    abstract class TcpModule<T> where T : class
    {
        internal delegate void MessageCallback(T data);
        internal event MessageCallback OnMessageRecieved;
        internal abstract void SendMessage();
        internal virtual void SendMessage(MessageCallback callback)
        {
            SendMessage();
            callback(null);
        }
    }
}
