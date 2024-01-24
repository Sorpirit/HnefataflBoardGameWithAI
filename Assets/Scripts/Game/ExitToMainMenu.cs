using UnityEngine;

namespace Game
{
    public class ExitToMainMenu : MonoBehaviour
    {
        [SerializeField] private GameSceneLoader gameSceneLoader;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameSceneLoader.GoToMainMenu();
            }
        }
        
    }
}