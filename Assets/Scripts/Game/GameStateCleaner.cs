using Game.Library;
using UnityEngine;

namespace Game
{
    public class GameStateCleaner : MonoBehaviour
    {
        private void Start()
        {
            SceneDataTransferComponent.Instance.Clear();
        }
    }
}