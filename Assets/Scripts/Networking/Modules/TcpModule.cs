using Assets.Scripts.Model;
using Assets.Scripts.Util;
using MiniMessagePack;
using System.Collections.Generic;
using System;

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
        protected ushort cmd;

        internal delegate void MessageCallback(Message<Error> error, Message<Out> data);

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

        protected TcpModule()
        {
            sender = TcpConnection.sender;
            TcpConnection.listener.OnMessageRecived += Listener_OnMessageRecived;
        }

        private void Listener_OnMessageRecived(Message message)
        {
            if (message.cmd == 0 && message.id.HasValue && callbacks.ContainsKey(message.id.Value))
            {
                callbacks[message.id.Value](ParseMessage<Error>(message), null);
                callbacks.Remove(message.id.Value);
            }
            if (message.cmd != cmd) { return; }

            OnMessage(ParseMessage<Out>(message));
        }

        private Message<T> ParseMessage<T>(Message message) where T : class, new()
        {
            if (typeof(T) is IDictionary<string, object>)
            {
                return message as Message<T>;
            }

            T data = null;
            try
            {
                data = message.data.ToObject<T>();
            }
            catch (Exception e)
            {
            }

            return new Message<T>
            {
                id = message.id,
                cmd = message.cmd,
                data = data
            };
        }

        protected virtual void OnMessage(Message<Out> message)
        {
            if (message.id.HasValue && callbacks.ContainsKey(message.id.Value))
            {
                callbacks[message.id.Value](null, message);
                callbacks.Remove(message.id.Value);
            }
        }

        internal void SendMessage(In data)
        {
            sender.SendMessage(packer.Pack(DictionarySerializer.AsDictionary(new Message<In>()
            {
                cmd = cmd,
                data = data
            })));
        }

        internal void SendMessage(In data, MessageCallback callback)
        {
            var packageId = id.GetNext();
            if (callbacks.ContainsKey(packageId))
            {
                callbacks[packageId](ParseMessage<Error>(MessageSerializer.createErrorMessage("Message timed out", packageId)), null);
                callbacks.Remove(packageId);
            }
            callbacks.Add(packageId, callback);

            sender.SendMessage(packer.Pack(DictionarySerializer.AsDictionary(new Message<In>()
            {
                id = packageId,
                cmd = cmd,
                data = data
            })));
        }
    }

    abstract class TcpModule<Base, In> : TcpModule<Base, In, Dictionary<string, object>>
        where In : class, new()
        where Base : TcpModule<Base, In, Dictionary<string, object>>, new()
    { }

    abstract class TcpModule<Base> : TcpModule<Base, Dictionary<string, object>, Dictionary<string, object>>
        where Base : TcpModule<Base, Dictionary<string, object>, Dictionary<string, object>>, new()
    { }
}
