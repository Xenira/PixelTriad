using Assets.Scripts.Model;
using Assets.Scripts.Util;
using MiniMessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Networking.Modules
{
    abstract class TcpModule<Base, In, Out>
        where Base : TcpModule<Base, In, Out>, new()
        where In : class, new()
        where Out : class, new()
    {
        private static CyclingByte id = new CyclingByte();
        private static MiniMessagePacker packer = new MiniMessagePacker();

        private Dictionary<byte, MessageCallback> callbacks = new Dictionary<byte, MessageCallback>();
        private TcpSender sender;

        internal delegate void MessageCallback(bool success, Message<Out> data);

        private static Base _instance;
        internal static Base Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Base();
                }
                return _instance;
            }
        }

        public TcpModule()
        {
            sender = TcpConnection.sender;
            TcpConnection.listener.OnMessageRecived += Listener_OnMessageRecived;
        }

        private void Listener_OnMessageRecived(Message message)
        {
            if (typeof(In) is IDictionary<string, object>)
            {
                OnMessage(message as Message<Out>);
                return;
            }

            var parsedMessage = new Message<Out>
            {
                id = message.id,
                cmd = message.cmd,
                data = message.data.ToObject<Out>()
            };

            OnMessage(parsedMessage);
        }

        protected virtual void OnMessage(Message<Out> message)
        {
            if (message.id.HasValue && callbacks.ContainsKey(message.id.Value))
            {
                callbacks[message.id.Value](true, message);
            }
        }

        internal void SendMessage(In data) {
            sender.SendMessage(packer.Pack(DictionarySerializer.AsDictionary(new Message<In>()
            {
                cmd = 0,
                data = data
            })));
        }

        internal void SendMessage(In data, MessageCallback callback)
        {
            var packageId = id.GetNext();
            if (callbacks.ContainsKey(packageId))
            {
                callbacks[packageId](false, null);
            }
            callbacks.Add(packageId, callback);

            sender.SendMessage(packer.Pack(DictionarySerializer.AsDictionary(new Message<In>()
            {
                id = packageId,
                cmd = 0,
                data = data
            })));
        }
    }

    abstract class TcpModule<Base, In> : TcpModule<Base, In, Dictionary<string, object>>
        where In : class, new()
        where Base : TcpModule<Base, In, Dictionary<string, object>>, new() { }

    abstract class TcpModule<Base> : TcpModule<Base, Dictionary<string, object>, Dictionary<string, object>>
        where Base : TcpModule<Base, Dictionary<string, object>, Dictionary<string, object>>, new() { }
}
