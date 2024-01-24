using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Game.Network
{
    public class RelayLauncher
    {
        public RelayLauncher(List<Task> tasks)
        {
            tasks.Add(StartRelay());
        }

        private async Task StartRelay()
        {
            await UnityServices.InitializeAsync();
            AuthenticationService.Instance.SignedIn += OnSignedIn;
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        private void OnSignedIn()
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        }
        
        public async Task<string> CreateRelay()
        {
            var relay = await RelayService.Instance.CreateAllocationAsync(2);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(relay.AllocationId);
            Debug.Log("Relay created : join code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(relay, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            return joinCode;
        }
        
        public async Task JoinRelay(string joinCode)
        {
            var relay = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            RelayServerData relayServerData = new RelayServerData(relay, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            Debug.Log("Relay joined : " + relay.AllocationId);
        }
    }
}