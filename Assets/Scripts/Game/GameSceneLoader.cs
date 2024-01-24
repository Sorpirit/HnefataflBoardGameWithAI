using Game.Library;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameSceneLoader : MonoBehaviour
    {
        [SerializeField] private int gameSceneIndex;
        
        public void StartLocalGame()
        {
            SceneDataTransferComponent.Instance.AddTag<LocalGameTag>();
            SceneManager.LoadScene(Constants.GameSceneName, LoadSceneMode.Single);
        }
        
        public void StartOnlineGame()
        {
            SceneDataTransferComponent.Instance.AddTag<OnlineGameTag>();
            SceneManager.LoadScene(Constants.GameSceneName, LoadSceneMode.Single);
        }
        
        public void GoToMainMenu()
        {
            SceneManager.LoadScene(Constants.MainMenuSceneName, LoadSceneMode.Single);
        }
        
        public struct OnlineGameTag { }
        public struct LocalGameTag { }
    }
}