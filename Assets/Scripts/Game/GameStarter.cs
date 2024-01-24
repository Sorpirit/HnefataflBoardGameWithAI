using Game.Library;
using Game.Network;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private GameObject localGameAdapter;
        
        private void Start()
        {
            if(SceneDataTransferComponent.Instance.HasTag<GameSceneLoader.OnlineGameTag>())
                StartOnlineGame();
            else if (SceneDataTransferComponent.Instance.HasTag<GameSceneLoader.LocalGameTag>())
                StartLocalGame();
            else
                throw new System.Exception("No game mode selected");
            
        }

        private void StartOnlineGame()
        {
            if (SceneDataTransferComponent.Instance.Contains<NetworkHostCookie>())
            {
                //Start host game
                NetworkManager.Singleton.StartHost();
            }
            else
            {
                //Start client game
                NetworkManager.Singleton.StartClient();
            }
        }
        
        private void StartLocalGame()
        {
            Instantiate(localGameAdapter);
        }
    }
}