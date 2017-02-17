using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using pomeloUnityClient;
using SimpleJson;
using SocketIOClient;
using System.Net.Sockets;
using System.Net.Security;

public class ServerConnection
{
    #region API Paths
    private const string AUTH = "/auth";
    private const string GAME = "/games";
    private const string FIND_GAME = GAME + "/normal";
    #endregion
    private static PomeloClient c;

    internal static Session session { get; private set; }

    internal delegate void Result(string error, object data = null);
    internal static Result logger;

    internal static void Connect()
    {
        using (var client = new TcpClient("192.168.178.5", 3000))
        using (var sslStream = new SslStream(client.GetStream(), false,
            new RemoteCertificateValidationCallback((s, c, chain, ssl) => true), null))
        {
            sslStream.AuthenticateAsClient("192.168.178.5");
            sslStream.Write(Encoding.UTF8.GetBytes("test"));
        }
    }

    internal static void Login(MonoBehaviour context, string mail, string password, Result result)
    {
        string svcCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(mail + ":" + password));

        var header = new Dictionary<string, string>();
        header.Add("Authorization", "Basic " + svcCredentials);
        context.StartCoroutine(RestRequest.Execute((error, request) =>
        {
            if (error != null)
            {
                result(error);
                return;
            }

            var data = request.downloadHandler.text;
            Debug.Log(data);
            session = JsonUtility.FromJson<Session>(data);
            result(null, session);
        }, Environment.ENV.API_PATH + AUTH, RestRequest.HTTP_METHOD.POST, headers: header));
    }

    internal static void CancelQueue(MonoBehaviour context, Result result)
    {
        LongPolling.Stop(Environment.ENV.API_PATH + FIND_GAME);
        context.StartCoroutine(RestRequest.Execute((error, data) =>
        {

        }, Environment.ENV.API_PATH + FIND_GAME, RestRequest.HTTP_METHOD.DELETE));
    }

    internal static void FindGame(MonoBehaviour context, Result result)
    {
        try
        {
            LongPolling.Start(context, (error, data) =>
            {
                if (error != null)
                {
                    Debug.Log(error);
                    result(error);
                    return;
                }

                if (data.events.Count > 0)
                {
                    var match = data.events.FirstOrDefault(e => e.type == "match");
                    if (match != null)
                    {
                        result(error, JsonUtility.FromJson<Game>(match.data.Print()));
                    }
                }

            }, Environment.ENV.API_PATH + FIND_GAME, RestRequest.HTTP_METHOD.GET);
        }
        catch (ArgumentException e)
        {
            result(e.Message);
        }

    }
    internal static void OpenSocket()
    {
        OpenSocket(Environment.ENV.API_PATH);
    }

    internal static void OpenSocket(string url)
    {
        if (c == null)
        {
            c = new PomeloClient(url);
            c.PomeloError += Pomelo_Error;
            c.init((data) =>
            {
                if (logger != null)
                {
                    logger(null, "Connected");
                }
            });
            
            var msg = new JsonObject();
            msg.Add("body", "hallo");
            c.request("connector.entryHandler.entry", msg, (data) =>
            {
                if (logger != null)
                {
                    logger(null, data);
                }
            });
        }
    }

    private static void Pomelo_Error(object sender, ErrorEventArgs e)
    {
        if (logger != null)
        {
            logger(e.Message);
        }
    }
}
