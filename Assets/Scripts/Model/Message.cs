using Assets.Scripts.Util;
using MsgPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [Serialize]
    class Message<T> where T : class, new()
    {
        public byte? id { get; set; }
        public int cmd { get; set; }
        public T data { get; set; }
    }

    [Serialize]
    sealed class Message : Message<Dictionary<string, object>> { }

    class MessageSerializer
    {
        public static Message Serialize(Dictionary<string, object> data)
        {
            if (!data.ContainsKey("cmd") || !(data["cmd"] is long)) { return createErrorMessage("Cmd currupted or missing", data); }
            if (!data.ContainsKey("data") || !(data["data"] is Dictionary<string, object>)) { return createErrorMessage("Data currupted or missing", data); }

            var id = data.ContainsKey("id") && data["id"] is long ? Convert.ToByte(data["id"]) : (byte?)null;
            var cmd = Convert.ToInt32(data["cmd"]);
            var d = (Dictionary<string, object>)data["data"];

            return new Message()
            {
                id = id,
                cmd = cmd,
                data = d
            };
        }

        internal static Message createErrorMessage(string error, byte? id = null)
        {
            return new Message()
            {
                id = id,
                cmd = 0,
                data = new Dictionary<string, object>() { { "message", error } }
            };
        }

        internal static Message createErrorMessage(string error, Dictionary<string, object> data)
        {
            return createErrorMessage(error, data.ContainsKey("id") && data["id"] is long ? Convert.ToByte(data["id"]) : (byte?)null);
        }

        private static Type GetTypeFromId(int? id)
        {
            throw new NotImplementedException();
        }
    }

    internal class Welcome
    {
        public string message { get; set; }
    }
}
