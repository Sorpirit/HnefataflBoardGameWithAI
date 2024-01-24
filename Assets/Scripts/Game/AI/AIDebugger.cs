using UnityEngine;
using Utilities;

namespace Game.AI
{
    public class AIDebugger : MonoBehaviour
    {
        [SerializeField] private GameObject debugPointPrefab;
        
        //private SimpleObjectPool<GameObject> _debugPointPool = new SimpleObjectPool<GameObject>();
        private GameBoardSpawner _gameBoardSpawner;
        
        private void Start()
        {
            _gameBoardSpawner = GameDependencyResolver.Instance.GameBoardSpawner;
        }

        public void DebugSnapshot()
        {
            
        }
        
        public void ShowDebugPoint(Vector2Int tilePosition)
        {
            var point = Instantiate(debugPointPrefab, _gameBoardSpawner.ToWorldPositionTile(tilePosition), Quaternion.identity);
            //Destroy(point, 1f);
        }
    }
}