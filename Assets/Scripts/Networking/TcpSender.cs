using Assets.Scripts.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;

namespace Assets.Scripts.Networking
{
    class TcpSender : ThreadedJobWithQueue
    {
        SslStream stream;
        public TcpSender(SslStream stream)
        {
            this.stream = stream;
        }

        internal void SendMessage(byte[] data)
        {
            Message = data;
        }

        protected override void ThreadFunction()
        {
            while (HasMessage)
            {
                stream.Write(Message);
            }
        }

        protected override void OnFinished()
        {
            throw new NotImplementedException();
        }
    }
}
