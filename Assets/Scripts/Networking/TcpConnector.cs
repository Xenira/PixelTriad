using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Assets.Scripts.Networking
{
    class TcpConnector : ThreadedJob
    {
        public delegate void TcpEvent(object sender, object data);
        public event TcpEvent OnConnected;

        private TcpClient client;
        private SslStream stream;

        protected override void ThreadFunction()
        {
            if (client != null && stream != null) return;
            client = new TcpClient("192.168.178.5", 3000);
            stream = new SslStream(client.GetStream(), false,
                new RemoteCertificateValidationCallback((s, c, chain, ssl) => true), null);
            stream.AuthenticateAsClient("192.168.178.5");
        }

        protected override void OnFinished()
        {
            OnConnected(this, "Connected to " + client.Client.RemoteEndPoint);
        }

        public void Connect()
        {
            Start();
        }
    }
}
