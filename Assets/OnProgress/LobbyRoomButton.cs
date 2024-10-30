using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace NetServices
{
    internal class LobbyRoomButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI slotsText;
        [SerializeField] private Button button;

        private Lobby lobby;
        private void Awake()
        {
            button.onClick.AddListener(Join);
        }

        internal void Setup(Lobby lobby)
        {
            this.lobby = lobby;
            nameText.SetText(lobby.Name);
            slotsText.SetText($"{lobby.Players.Count}/{lobby.MaxPlayers}");

        }

        private void Join()
        {
            OnlineServicesManager.Instance.Join(lobby);
        }
    }
}
