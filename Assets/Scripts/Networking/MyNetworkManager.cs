using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using LoadSceneMode = UnityEngine.SceneManagement.LoadSceneMode;
using Unity.Netcode.Transports.UTP;
using DialogueClasses;

public class MyNetworkManager : NetworkManager
{
    public List<LanConnectionInfo> lanConnrctions = new List<LanConnectionInfo>();

    public static MyNetworkManager singleton { get; private set; }

    /// <summary>
    /// to determine the difference between quitting and disconnection
    /// </summary>
    private bool quited;
    public void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
            SetSingleton();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        OnClientStopped += OnClientStoppedCallback;
        OnTransportFailure += OnTransportFailureCallback;
    }

    private void OnTransportFailureCallback()
    {
        Dialogue.ShowPanel("Transport failed", M_Shutdown);
    }

    private void OnClientStoppedCallback(bool isHost)
    {
        string message = !string.IsNullOrEmpty(DisconnectReason) ? DisconnectReason : "Looks like Host have stopped";

        if (quited)
            print("You quitted");
        else
            Dialogue.ShowPanel(message, M_Shutdown);

        quited = false;
    }


    #region Public Methods
    public void LoadScene(string _name)
    {
        SceneManager.LoadScene(_name, LoadSceneMode.Single);
    }


    public bool M_TryStartHosting(string roomName)
    {
        UnityTransport transport = GetComponent<UnityTransport>();

        if (!Helper.TryGetAddress(out string ipAddress))
            return false;

        ushort port = transport.ConnectionData.Port;
        transport.SetConnectionData(ipAddress, port);
        IPText.SetIPAddress(ipAddress);
        print(ipAddress);

        if (!NetworkDiscoveryManager.singleton.M_ShowMatch(ipAddress, port, roomName))
            return false;

        if (!StartHost())
            return false;

        return true;
    }

    public void M_StartLocalHosting()
    {
        UnityTransport transport = GetComponent<UnityTransport>();


        ushort roomPort = transport.ConnectionData.Port;
        transport.SetConnectionData("127.0.0.1", roomPort);
        IPText.LocalMatch();

        StartHost();
    }

    public void M_JoinMatch(LanConnectionInfo lanConnrction)
    {
        M_JoinCustomRoom(lanConnrction.ipAddress, lanConnrction.port);
    }
    
    public void M_JoinCustomRoom(string ipAddress, ushort? customPort = null, string roomName = null)
    {
        UnityTransport transport = ((UnityTransport)NetworkConfig.NetworkTransport);
        ushort defaultPort = transport.ConnectionData.Port;

        ushort port = customPort != null ? customPort.Value : defaultPort;


        transport.SetConnectionData(ipAddress, port);
        IPText.SetIPAddress(ipAddress);

        if (StartClient())
        {
            string text = "Trying to join ";
            text += roomName ?? ipAddress;
            Dialogue.ShowWaitingPanel(text, M_Shutdown); 
        }
        else
            Debug.LogError("Client Failed");
    }

    public void QuitMatch()
    {
        quited = true;
        M_Shutdown();
    }

    public void M_Shutdown()
    {
        Dialogue.HideWaitingPanel();

        if (IsServer || IsClient)
            Shutdown();
        else
        {
            Debug.Log("you don't quit if it's already not working");
            quited = false;
        }

        SceneChanger.ReturnToMainMenu();
    }

    public void M_StopBroadcast()
    {
        NetworkDiscoveryManager.singleton.M_StopBroadcast();
    }

    public void KickPlayer(ulong id)
    {
        DisconnectClient(id, "you were kicked");
    }
    #endregion
}
