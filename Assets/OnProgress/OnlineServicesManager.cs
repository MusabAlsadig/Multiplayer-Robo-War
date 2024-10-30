using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetServices
{
    public class OnlineServicesManager : MonoBehaviour
    {
        [SerializeField] private RoomManager roomManager;
        [SerializeField] private int refreshDelay = 2;

        private float time;

        private LobbyManager lobbyManager;
        private RelayManager relayManager;

        public static OnlineServicesManager Instance { get; private set; }

        internal const string KEY_FOR_RELAY = "Relay";

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        private async void Start()
        {
            lobbyManager = LobbyManager.Instance;
            relayManager = RelayManager.Instance;

            time = refreshDelay;
            CheckInternet();
        }

        private void Update()
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                time = refreshDelay;
                CheckInternet();
            }
        }

        #region Public methods
        public async void Host(string lobbyName, int maxPlayers)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            Dialogue.ShowWaitingPanel("Trying to host a match", cancellationTokenSource);

            string relayCode = await relayManager.CreateRelay(maxPlayers);
            if (string.IsNullOrEmpty(relayCode))
            {
                Dialogue.ShowPanel("couldn't host a match");
                Dialogue.HideWaitingPanel();
                return;
            }

            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            await lobbyManager.CreateLobbyAsync(lobbyName, maxPlayers, relayCode);

            if (token.IsCancellationRequested)
            {
                lobbyManager.LeaveLobbyAsync();
                token.ThrowIfCancellationRequested();
            }

            Dialogue.HideWaitingPanel();

            roomManager.GotoPrematch();
        }

        public async void Join(Lobby lobby)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            Dialogue.ShowWaitingPanel($"joining with {lobby.Name}", cancellationTokenSource);

            bool isJoinedLobby = await lobbyManager.TryJoinLobbyByCodeAsync(lobby.Id);
            if (isJoinedLobby)
            {
                if (token.IsCancellationRequested)
                {
                    lobbyManager.LeaveLobbyAsync();
                    token.ThrowIfCancellationRequested();
                }

                await relayManager.JoinRelay(lobby.Data[KEY_FOR_RELAY].Value);
            }
            else
            {
                Dialogue.ShowPanel("couldn't join to the lobby");
                Dialogue.HideWaitingPanel();
            }
        }
        #endregion


        private async void CheckInternet()
        {
            await TryLogin();
        }

        private async Task TryLogin()
        {
            try
            {
                if (UnityServices.State == ServicesInitializationState.Uninitialized)
                    await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}