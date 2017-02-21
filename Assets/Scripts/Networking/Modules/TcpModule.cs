using Assets.Scripts.Model;
using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Networking.Modules
{
    abstract class TcpModule<T> where T : class
    {
        static ResettingUint id = new ResettingUint();

        private Dictionary<ushort, MessageCallback> callbacks = new Dictionary<ushort, MessageCallback>();
        private Dictionary<ushort, DateTime> timeouts = new Dictionary<ushort, DateTime>();

        internal delegate void MessageCallback(T data);
        internal event MessageCallback OnMessageRecieved;
        internal abstract void SendMessage(T data);
        internal virtual void SendMessage(MessageCallback callback)
        {
            var packageId = id.GetNext();
            SendMessage(packageId);
            callback(null);
        }
        private void SendMessage(ushort? packageId = null)
        {

        }
        internal virtual void RecieveMessage(Message message)
        {

        }
    }
}
