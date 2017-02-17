using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment
{
    public static readonly Environment ENV = new Environment();
    public readonly string API_PATH = "";

#if UNITY_EDITOR
    public Environment()
    {
        API_PATH = "localhost:3010/socket.io/?EIO=3&transport=polling";
    }
#endif
}