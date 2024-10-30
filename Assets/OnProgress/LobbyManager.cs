using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetServices
{
    public class LobbyManager : MonoBehaviour
    {
        private bool isHost;
        private Lobby joinedLobby;

        private float heartbeatTimer = HeartbeatTimerMax;
        private const float HeartbeatTimerMax = 15;


        public static LobbyManager Instance { get; private set; }
        #region Unity methods
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Update()
        {
            HandleHeartbeat();
        }
        #endregion

        #region Internal Methods
        internal async Task CreateLobbyAsync(string lobbyName, int maxPlayers, string relayCode)
        {
            try
            {
                CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions()
                {
                    Data = new Dictionary<string, DataObject>()
                    {
                        [OnlineServicesManager.KEY_FOR_RELAY] = new DataObject(DataObject.VisibilityOptions.Public, relayCode),
                    },
                    IsPrivate = false,
                };

                isHost = true;
                joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            }
            catch (LobbyServiceException ex)
            {
                Logger.LogException(ex);
            }
        }

        /// <summary>
        /// Search and return found Lobbies
        /// </summary>
        internal async Task<List<Lobby>> ListLobbiesAsync()
        {
            try
            {
                QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions()
                {
                    Count = 25,
                    Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, 0.ToString(), QueryFilter.OpOptions.GT), // only show lobbies with atleast 1 empty slot
                },
                    Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created), // order by old to new
                }
                };
                
                QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
                return queryResponse.Results;
            }
            catch (LobbyServiceException ex)
            {
                Logger.LogException(ex);
                return null;
            }

        }

        internal async Task<bool> TryJoinLobbyByCodeAsync(string lobbyId)
        {
            try
            {
                Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
                joinedLobby = lobby;
                return true;
            }
            catch (LobbyServiceException ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }

        /// <returns>relay code</returns>
        internal async Task<string> QuickJoinLobbyAsync()
        {
            try
            {
                joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
                return joinedLobby.Data[OnlineServicesManager.KEY_FOR_RELAY].Value;
            }
            catch (LobbyServiceException ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        // TODO : Make player Leave after quitting or kicked
        internal async void LeaveLobbyAsync()
        {
            if (isHost)
                isHost = false;

            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            }
            catch (LobbyServiceException ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion
        private void HandleHeartbeat()
        {
            if (!isHost)
                return;
            if (joinedLobby == null)
                return;

            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                heartbeatTimer = HeartbeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }
}