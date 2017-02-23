using Assets.Scripts.Model;
using Assets.Scripts.Networking;
using MsgPack;
using System.Net.Security;
using System.Net.Sockets;
using UnityEngine;

public class TcpConnection : MonoBehaviour {

    internal static TcpClient client;
    internal static SslStream stream;
    internal static Assets.Scripts.Networking.TcpListener listener;
    internal static TcpSender sender;

    internal static bool IsInitialized
    {
        get { return client != null && stream != null; }
    }

    internal static bool IsListening
    {
        get { return IsInitialized && listener != null && sender != null; }
    }

    private void Start()
    {
        if (!IsListening)
        {
            StartClient(Environment.ENV.API_PATH, Environment.ENV.API_PORT);
        }
    }

    internal void StartClient(string address, int port)
    {
        var connector = new TcpConnector(address, port);
        connector.OnConnected += Connector_OnConnected;
        connector.Connect();
        StartCoroutine(connector.WaitFor());
    }

    private void Connector_OnConnected(TcpClient client, SslStream stream)
    {
        TcpConnection.client = client;
        TcpConnection.stream = stream;
        StartListening();
        PrepareSending();
    }

    internal void PrepareSending()
    {
        sender = new TcpSender(stream);
        sender.Start();
        StartCoroutine(sender.WaitFor());
    }

    internal void StartListening()
    {
        listener = new Assets.Scripts.Networking.TcpListener(stream);
        listener.OnMessageRecived += Listener_OnMessageRecived;
        listener.Start();
        StartCoroutine(listener.WaitFor());
    }

    private void Listener_OnMessageRecived(Message message)
    {
    }

    internal bool IsConnected()
    {
        return stream != null && client != null && client.Connected;
    }
}
