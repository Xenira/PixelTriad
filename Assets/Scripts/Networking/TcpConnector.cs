using Assets.Scripts.Threading;
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
        public delegate void TcpConnected(TcpClient client, SslStream stream);
        public event TcpConnected OnConnected;

        private TcpClient client;
        internal SslStream stream;

        string url;
        int port;

        internal TcpConnector(string url, int port)
        {
            this.url = url;
            this.port = port;
        }

        protected override void ThreadFunction()
        {
            if (client != null && stream != null) return;
            client = new TcpClient(url, port);
            stream = new SslStream(client.GetStream(), false,
                new RemoteCertificateValidationCallback((s, c, chain, ssl) => true), null);
            stream.AuthenticateAsClient(url);
        }

        protected override void OnFinished()
        {
            OnConnected(client, stream);
        }

        public void Connect()
        {
            Start();
        }
    }
}
