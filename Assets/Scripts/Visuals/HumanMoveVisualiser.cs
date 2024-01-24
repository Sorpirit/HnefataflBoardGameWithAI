using Game;
using Game.Players;
using UnityEngine;

namespace Visuals
{
    public class HumanMoveVisualiser : MonoBehaviour
    {
        [SerializeField] private GameObject startMovePoint;
        [SerializeField] private HumanPlayer humanPlayer;
        
        private GameBoardSpawner _gameBoardSpawner;
        
        private void Start()
        {
            startMovePoint.SetActive(false);
            _gameBoardSpawner = GameDependencyResolver.Instance.GameBoardSpawner;
            humanPlayer.OnStartTileChanged += OnStartTileChanged;
        }

        private void OnStartTileChanged(Vector2Int? tilePosition)
        {
            if (tilePosition == null)
            {
                startMovePoint.SetActive(false);
            }
            else
            {
                startMovePoint.SetActive(true);
                startMovePoint.transform.position = _gameBoardSpawner.ToWorldPositionTile(tilePosition.Value);
            }
        }
    }
}