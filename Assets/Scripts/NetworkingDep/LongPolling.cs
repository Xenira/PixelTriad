using System;
using System.Collections.Generic;
using UnityEngine;

public class LongPolling {

    private static LinkedList<string> longPolling = new LinkedList<string>();

    public delegate void Callback(string error, EmitterResult result = null);

    internal static void Start(MonoBehaviour behaviour, Callback callback, string url, RestRequest.HTTP_METHOD method, string data = "{}", Dictionary<string, string> header = null)
    {
        if (longPolling.Contains(url)) throw new ArgumentException("Already performing long polling for " + url);
        longPolling.AddLast(url);

        RestRequest.Callback resultCallback = (err, request) =>
        {
            Debug.LogError("Undeclared result callback called.");
        };

        resultCallback = (err, request) =>
        {
            if (err != null)
            {
                callback(err);
                longPolling.Remove(url);
                return;
            }

            var result = new JSONObject(request.downloadHandler.text);
            callback(null, new EmitterResult(result));

            if (longPolling.Contains(url))
            {
                behaviour.StartCoroutine(RestRequest.Execute(resultCallback, request.url, RestRequest.HTTP_METHOD.GET));
            }
        };
        behaviour.StartCoroutine(RestRequest.Execute(resultCallback, url, method, data, header));
    }

    internal static void Stop(string url)
    {
        longPolling.Remove(url);
    }

    internal static void StopAll()
    {
        longPolling.Clear();
    }
}

[Serializable]
public class EmitterResult
{
    public string id;
    public LinkedList<EmitterEvent> events = new LinkedList<EmitterEvent>();

    public EmitterResult() { }
    public EmitterResult(JSONObject result)
    {
        id = result["id"].str;
        foreach(var e in result["events"].list)
        {
            if (e.list.Count == 2)
            {
                events.AddLast(new EmitterEvent(e.list[0].str, e.list[1]));
            }
        }
    }
}

[Serializable]
public class EmitterEvent
{
    public string type;
    public JSONObject data;

    public EmitterEvent() { }
    public EmitterEvent(string type, JSONObject data)
    {
        this.type = type;
        this.data = data;
    }
}

