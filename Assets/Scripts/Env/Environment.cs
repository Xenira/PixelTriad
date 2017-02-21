using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment
{
    public static readonly Environment ENV = new Environment();
    public readonly string API_PATH = "";
    public readonly int API_PORT;

#if UNITY_EDITOR
    public Environment()
    {
        API_PATH = "192.168.178.5";
        API_PORT = 3000;
    }
#endif
}