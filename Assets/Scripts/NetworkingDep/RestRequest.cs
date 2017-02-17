using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class RestRequest {

    private const string QUERY_PARAM_CHECK = @"\?\w+?=";
    public delegate void Callback(string error, UnityWebRequest request = null);

    public enum HTTP_METHOD
    {
        GET, POST, PUT, DELETE
    }

    internal static IEnumerator Execute(Callback callback, string url, HTTP_METHOD method, object data = null, Dictionary<string, string> headers = null)
    {
        if (ServerConnection.session != null)
        {
            Regex r = new Regex(QUERY_PARAM_CHECK);
            url += r.IsMatch(url) ? "&" : "?" + "access_token=" + ServerConnection.session.token;
            Debug.Log(url);
        }

        UnityWebRequest req = new UnityWebRequest(url, method.ToString());
        req.downloadHandler = new DownloadHandlerBuffer();
        if (data != null && (method == HTTP_METHOD.POST || method == HTTP_METHOD.PUT))
        {
            UploadHandler uploader = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
            uploader.contentType = "application/json";
            req.uploadHandler = uploader;
        }

        if (headers != null)
        {
            foreach (var entry in headers)
            {
                req.SetRequestHeader(entry.Key, entry.Value);
            }
        }

        yield return req.Send();

        if (req.isError)
        {
            Debug.LogError(req.error);
            callback(req.error);
        }
        else if (req.responseCode >= 400 && req.responseCode < 600)
        {
            var text = req.downloadHandler.text;
            var error = string.IsNullOrEmpty(text) ? req.responseCode.ToString() : text;
            Debug.LogError(error);
            callback(error);
        }
        else
        {
            callback(null, req);
        }
    }
}
