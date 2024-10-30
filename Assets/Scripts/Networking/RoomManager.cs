using Unity.Netcode;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    MyNetworkManager networkManager;

    [SerializeField] private NetworkObject holder_Prefab;

    private void Start()
    {
        networkManager = MyNetworkManager.singleton;
    }

# if UNITY_EDITOR
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.J))
        {
            Join_SameDevice();
        }
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            LocalHost();
        }

        if (Input.GetKeyDown(KeyCode.Q))
            networkManager.QuitMatch();
    }
#endif



    private void LocalHost()
    {
        networkManager.M_StartLocalHosting();
        Instantiate(holder_Prefab).Spawn();
        networkManager.LoadScene(GameManager.Prematch);
    }

    public void GotoPrematch()
    {
        Instantiate(holder_Prefab).Spawn();

        networkManager.LoadScene(GameManager.Prematch);
    }

    #region UI Buttons

    public void Solo()
    {
        LocalHost();
    }

    public void Join_SameDevice()
    {
        networkManager.StartClient();
    }

    public void Host()
    {
        string hostName = Data.GameData.Current.playerInfo.name;

        if (networkManager.M_TryStartHosting(hostName))
        {
            GotoPrematch();
        }
        else
        {
            Dialogue.ShowPanel("you aren't connected to Wi-Fi or the internet,\n" +
                "do you want to test solo?",
                onConfirm: LocalHost, canCancel: true);
        }
    }

    #endregion
}
