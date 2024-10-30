using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace NetServices
{
    public class OnlineButtonsManager : MonoBehaviour
    {
        [SerializeField] private ScrollRect roomsScrollRect; 
        [SerializeField] private Button host_btn;
        [SerializeField] private Button refresh_btn;

        private LobbyManager lobbyManager;
        [SerializeField] private LobbyRoomButton roomButton_Prefab;

        private List<Lobby> lobbies;

        private const int Search_Delay = 3;
        private void Awake()
        {
            host_btn.onClick.AddListener(Host);
            refresh_btn.onClick.AddListener(Search);
        }

        private void Start()
        {
            lobbyManager = LobbyManager.Instance;
        }

        private void OnEnable()
        {
            InvokeRepeating(nameof(Search), 0, Search_Delay);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(Search));
        }

        private void Host()
        {
            OnlineServicesManager.Instance.Host(Data.GameData.Current.playerInfo.name, 16);
        }

        private async void Search()
        {
            lobbies = await lobbyManager.ListLobbiesAsync();

            foreach (Transform child in roomsScrollRect.content)
            {
                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbies)
            {
                Instantiate(roomButton_Prefab, roomsScrollRect.content).Setup(lobby);
            }

        }
    }
}