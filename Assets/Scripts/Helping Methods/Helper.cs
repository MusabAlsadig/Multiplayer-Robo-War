using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public static class Helper
{
    public static int Increase_in_a_Loop(int value, int min, int max)
    {
        if (value > max)
            Debug.LogError("How ?.....");

        value++;

        if (value > max)
            value = min;

        return value;
    }

    public static int Decrease_in_a_Loop(int value, int min, int max)
    {
        if (value < min)
            Debug.LogError("How ?.....");

        value--;

        if (value < min)
            value = max;

        return value;
    }

    public static int GetIndex<T>(T of, T[] from) where T : IEquatable<T>
    {

        for (int i = 0; i < from.Length; i++)
        {
            if (of.Equals(from[i]))
                return i;
        }

        Debug.LogError($"no Index for <color=red>{of}</color> from type <color=red>{typeof(T)}</color>");
        return -1;// did not find it

    }

    public static int GetIndex<T>(T of, NetworkList<T> from) where T : unmanaged, IEquatable<T>
    {

        if (!from.Contains(of))
        {
            Debug.LogError($"No index for <color=red>{of}</color> from type <color=red>{typeof(T)}</color>\n Count is <color=yellow>{from.Count}</color>");
            return -1;
        }

        return from.IndexOf(of);
    }

    public static bool TryGetAddress(out string ipAddress)
    {
        try
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", port: 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                ipAddress = endPoint.Address.ToString();
                return true;
            }

        }
        catch
        {
            ipAddress = null;
            return false;
        }

    }

    /// <summary>
    /// still un finised
    /// </summary>
    /// <param name="message"></param>
    /// <param name="color"></param>
    /// <param name="valuesToHighLight"></param>
    public static void HighLitedDebug(string message, Color color, params object[] valuesToHighLight)
    {
        Debug.Log("Yet to finish");
    }
}
