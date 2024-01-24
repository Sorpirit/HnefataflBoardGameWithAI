using Core;
using Game.Library;
using Game.Network;
using UnityEngine;

namespace Game
{
    public class GameStarterTestComponent : MonoBehaviour
    {
        private enum TestGameMode
        {
            Local,
            Host,
            Client
        }
        
        [SerializeField] private TestGameMode testGameMode;
        [SerializeField] private GameObject networkManager;
        [SerializeField] private GameMode mode;
        
        private void Awake()
        {
            switch (testGameMode)
            {
                case TestGameMode.Local:
                    SceneDataTransferComponent.Instance.AddTag<GameSceneLoader.LocalGameTag>();
                    SceneDataTransferComponent.Instance.WriteSingle(new LocalGameSettings(mode, 
                        LocalPlayerType.LocalPlayer, LocalPlayerType.LocalPlayer));
                    break;
                case TestGameMode.Host:
                    SceneDataTransferComponent.Instance.AddTag<GameSceneLoader.OnlineGameTag>();
                    SceneDataTransferComponent.Instance.WriteSingle(new NetworkHostCookie());
                    SceneDataTransferComponent.Instance.WriteSingle(new OnlineGameSettings(mode, PlayerType.Attacker));
                    Instantiate(networkManager);
                    break;
                case TestGameMode.Client:
                    SceneDataTransferComponent.Instance.AddTag<GameSceneLoader.OnlineGameTag>();
                    SceneDataTransferComponent.Instance.WriteSingle(new OnlineGameSettings(mode, PlayerType.Attacker));
                    Instantiate(networkManager);
                    break;
            }
        }
    }
}