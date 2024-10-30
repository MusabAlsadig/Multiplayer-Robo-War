using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public abstract class NetworkDiscovery : MonoBehaviour
{
    private const int DEFAULT_PORT = 47777;
    private readonly WaitForSecondsRealtime delay = new WaitForSecondsRealtime(2);

    private Coroutine sendingCoroutine;

    private readonly IPEndPoint broadcast_Address = new IPEndPoint(IPAddress.Broadcast, DEFAULT_PORT);

    protected UdpClient udpClient;
    private byte[] broadcastData;

    protected readonly Queue<byte[]> buffer = new Queue<byte[]>();


    protected void Awake()
    {
        udpClient = new UdpClient(DEFAULT_PORT);
    }

    private void Update()
    {
        if (buffer.Count > 0)
        {
            byte[] data = buffer.Dequeue();
            string value = ByteToString(data);
            OnReceivedBroadcast(LanConnectionInfo.FromString(value));
        }
    }

    protected bool StartBroadcasting(LanConnectionInfo lanConnectionInfo)
    {
        try
        {
            
            broadcastData = StringToByte(lanConnectionInfo);
            sendingCoroutine = StartCoroutine(LoopSending());
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected void ListenToBroadcast()
    {
        BeginListening();
    }

    protected void StopBroadcasting()
    {
        if (sendingCoroutine != null)
            StopCoroutine(sendingCoroutine);

        broadcastData = null;
    }

    

    private IEnumerator LoopSending()
    {
        while (true)
        {
            udpClient.Send(broadcastData, broadcastData.Length, broadcast_Address);
            yield return delay;
        }
    }
    
    private void BeginListening()
    {
        udpClient.BeginReceive(new AsyncCallback(OnPacketReceived), new object());
    }

    private void OnPacketReceived(IAsyncResult result)
    {
        IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Any, DEFAULT_PORT);
        byte[] data = udpClient.EndReceive(result, ref localEndpoint);
        BeginListening();
        buffer.Enqueue(data);
        Debug.Log("packet received");
    }

    protected abstract void OnReceivedBroadcast(LanConnectionInfo lanConnectionInfo);


    #region Not Really needed
    private string ByteToString(byte[] bytes) => Encoding.ASCII.GetString(bytes);
    
    private byte[] StringToByte(string value) => Encoding.ASCII.GetBytes(value);
    #endregion
}