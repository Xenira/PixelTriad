using Assets.Scripts.Model;
using Assets.Scripts.Threading;
using MiniMessagePack;
using MsgPack;
using System;
using System.Collections.Generic;
using System.Net.Security;

namespace Assets.Scripts.Networking
{
    class TcpListener : ThreadedJobWithStatus
    {
        public delegate void TcpMessage(Message message);
        public event TcpMessage OnMessageRecived;

        SslStream stream;
        public TcpListener(SslStream stream)
        {
            this.stream = stream;
        }

        protected override void OnFinished()
        {
            throw new NotImplementedException();
        }

        protected override void OnStatusUpdate(Message status)
        {
            OnMessageRecived(status);
        }

        protected override void ThreadFunction()
        {
            var mini = new MiniMessagePacker();
            var unpacker = Unpacker.Create(stream);
            object message;

            while((message = mini.Unpack(stream)) != null && message is Dictionary<string, object>) {
                Status = MessageSerializer.Serialize((Dictionary<string, object>)message);
            }
        }
    }
}
