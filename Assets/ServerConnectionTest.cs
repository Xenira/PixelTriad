using Assets.Scripts.Networking.Modules.Login;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerConnectionTest : MonoBehaviour
{

    public Text log;
    public Text messageCount;
    public InputField serverUrl;
    public InputField serverPort;
    public Toggle stepByStep;
    private Queue<string> messages = new Queue<string>();

    // Use this for initialization
    void Start()
    {
        log.text = "Ready...";
        serverUrl.text = Environment.ENV.API_PATH;
        serverPort.text = Environment.ENV.API_PORT.ToString();
    }

    private void Update()
    {
        messageCount.text = messages.Count.ToString();
    }

    public void Connect()
    {
        var connector = GetComponent<TcpConnection>();
        connector.StartClient(serverUrl.text, Convert.ToInt32(serverPort.text));
        //ServerConnection.OpenSocket(serverUrl.text);
    }

    public void Login()
    {
        LoginModule.Instance.Login("lasse.sprengel@gmail.com", "abcdef", (success, user) =>
        {
            Log(success ? null : "failed to authenticate", JsonUtility.ToJson(user));
        });
    }

    private void Connector_OnConnected(object sender, object data)
    {
        Debug.Log(data);
        Log(null, data);
    }

    public void PrintNext()
    {
        if (messages.Count > 0)
        {
            log.text += messages.Dequeue();
        }
    }

    public void Flush()
    {
        while (messages.Count > 0)
        {
            log.text += messages.Dequeue();
        }
    }

    public void Log(string error, object data)
    {
        messages.Enqueue("\n" + (error ?? data.ToString()));
        if (!stepByStep.isOn)
        {
            Flush();
        }
    }
}
