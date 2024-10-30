using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace NetServices
{
    public class RelayManager : MonoBehaviour
    {
        public static RelayManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        #region Internal Methods
        internal async Task<string> CreateRelay(int maxPlayers)
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1); // host don't count on relay

                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);


                RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                NetworkManager.Singleton.StartHost();

                return joinCode;
            }
            catch (RelayServiceException ex)
            {
                Logger.LogException(ex);
                return null;
            }

        }

        internal async Task JoinRelay(string joinCode)
        {
            try
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);


                RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                NetworkManager.Singleton.StartClient();
            }
            catch (RelayServiceException ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion
    }
}