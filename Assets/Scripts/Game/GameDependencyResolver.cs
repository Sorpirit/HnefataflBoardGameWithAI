using Game.Library;
using UnityEngine;

namespace Game
{
    public class GameDependencyResolver : SingleBehaviour<GameDependencyResolver>
    {
        [SerializeField] private GameBoardSpawner gameBoardSpawner;
        [SerializeField] private TileSelector tileSelector;

        public GameBoardSpawner GameBoardSpawner => gameBoardSpawner;
        public TileSelector TileSelector => tileSelector;
    }
}