using Core;
using Game.Library;
using Game.Players;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class GameNetworkAdapter : NetworkBehaviour
    {
        [SerializeField] private HumanPlayer playerPrefab;
        [SerializeField] private Transform spawnRoot;

        private static GameNetworkAdapter MainAdapter;
        
        private IPlayer _controlledPlayer;
        
        private NetworkHumanPlayer _attacker;
        private NetworkHumanPlayer _defender;

        public override void OnNetworkSpawn()
        {
            var playerObj = Instantiate(playerPrefab.gameObject, spawnRoot);
            _controlledPlayer = playerObj.GetComponent<IPlayer>();
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
            NetworkManager.Singleton.Shutdown();
        }
        
        private void OnClientConnected(ulong obj)
        {
            if(!IsServer)
                return;
            
            var settings = SceneDataTransferComponent.Instance.ReadSingle<OnlineGameSettings>();
            
            Debug.Log($"[{OwnerClientId}]: Client connected {obj}");
            if(NetworkManager.ConnectedClientsList.Count == 2)
                StartGameClientRpc((OnlineGameSettingsRPC) settings);
        }

        private void OnClientDisconnect(ulong obj)
        {
            GameController.Instance.ForceFinishGame(PlayerType.Attacker);
        }
        
        [ClientRpc]
        private void StartGameClientRpc(OnlineGameSettingsRPC settingsRPC)
        {
            if(!IsOwner)
                return;
            
            Debug.Log($"[{OwnerClientId}]: Start game client rpc");
            _controlledPlayer.OnMakeMove += move => MakeMoveServerRpc(move);
            
            var settings = (OnlineGameSettings) settingsRPC;
            var currentPlayerType = OwnerClientId == 0 ? settings.HostType : settings.HostType.GetOpponentType();
            
            _attacker = new NetworkHumanPlayer(currentPlayerType == PlayerType.Attacker ? _controlledPlayer : null);
            _defender = new NetworkHumanPlayer(currentPlayerType == PlayerType.Defender ? _controlledPlayer : null);
            
            var factory = new NetworkGameFactory(settings.GameMode, _attacker, _defender);
            
            GameController.Instance.StartGame(factory, null);
            MainAdapter = this;
        }
        
        [ServerRpc]
        private void MakeMoveServerRpc(NetworkPawnMove move)
        {
            var playerType = OwnerClientId == 0 ? PlayerType.Attacker : PlayerType.Defender;
            //validate move
            //execute move
            //send move to clients
            Debug.Log($"[{OwnerClientId}]: Make move {move} : {playerType}");
            MakeMoveClientRpc(move, playerType, new ClientRpcParams() {Send = new ClientRpcSendParams() {TargetClientIds = NetworkManager.ConnectedClientsIds}});
        }
        
        [ClientRpc]
        private void MakeMoveClientRpc(NetworkPawnMove move, PlayerType playerType, ClientRpcParams rpcParams)
        {
            //execute move
            if(IsOwner)
                ExecuteMove(move, playerType);
            else
            {
                MainAdapter.ExecuteMove(move, playerType);
            }
        }

        private void ExecuteMove(PawnMove move, PlayerType playerType)
        {
            Debug.Log($"[{OwnerClientId}]: Client execute move {move} : {playerType} ");
            switch (playerType)
            {
                case PlayerType.Attacker:
                    _attacker.ExecuteMove(move);
                    break;
                case PlayerType.Defender:
                    _defender.ExecuteMove(move);
                    break;
            }
        }
    }
}