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
            Debug.Log(data["cmd"].GetType().ToString());
            if (!data.ContainsKey("cmd") || !(data["cmd"] is long) || !data.ContainsKey("data")) { return null; }

            var id = data.ContainsKey("id") && data["id"] is byte ? (byte)data["id"] : (byte?)null;
            var cmd = Convert.ToInt32(data["cmd"]);
            var d = (Dictionary<string, object>)data["data"];

            return new Message()
            {
                id = id,
                cmd = cmd,
                data = d
            };
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
