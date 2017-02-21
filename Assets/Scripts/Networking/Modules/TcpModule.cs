using Assets.Scripts.Model;
using Assets.Scripts.Util;
using MiniMessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Networking.Modules
{
    abstract class TcpModule<T> where T : class, new()
    {
        private static CyclingByte id = new CyclingByte();
        private static MiniMessagePacker packer = new MiniMessagePacker();

        private Dictionary<byte, MessageCallback> callbacks = new Dictionary<byte, MessageCallback>();

        internal delegate void MessageCallback(bool success, Message<T> data);

        public TcpModule(TcpListener listener)
        {
            listener.OnMessageRecived += Listener_OnMessageRecived;
        }

        private void Listener_OnMessageRecived(Message message)
        {
            if (typeof(T) is IDictionary<string, object>)
            {
                OnMessage(message as Message<T>);
                return;
            }

            var parsedMessage = new Message<T>
            {
                id = message.id,
                cmd = message.cmd,
                data = DictionarySerializer.ToObject<T>(message.data)
            };

            OnMessage(parsedMessage);
        }

        protected virtual void OnMessage(Message<T> message) { }

        internal void SendMessage(T data) {
            packer.Pack(new Message<T>()
            {
                cmd = 0,
                data = data
            });
        }

        internal void SendMessage(T data, MessageCallback callback)
        {
            var packageId = id.GetNext();
            if (callbacks.ContainsKey(packageId))
            {
                callbacks[packageId](false, null);
            }
            callbacks.Add(packageId, callback);
            SendMessage(data);
        }
    }

    abstract class TcpModule : TcpModule<Dictionary<string, object>>
    {
        public TcpModule(TcpListener listener) : base(listener) { }
    }
}
