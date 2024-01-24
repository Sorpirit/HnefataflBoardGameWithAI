using System;
using Core;
using Game.Library;
using Game.Players;
using UnityEngine;

namespace Game.GameFactories
{
    public class NewGameFactory : MonoBehaviour, IGameFactory
    {
        [SerializeField] private PlayerPrefabCollection playerPrefabCollection;
        
        public void CreateGame(out GameSimulator simulator, out GameBoard board, out IPlayer[] players)
        {
            var settings = SceneDataTransferComponent.Instance.ReadSingle<LocalGameSettings>();
            
            var attacker = Instantiate(playerPrefabCollection.GetPrefab(settings.Attacker), transform);
            var defender = Instantiate(playerPrefabCollection.GetPrefab(settings.Defender), transform);
            
            players = new[]
            {
                attacker.GetComponent<IPlayer>(),
                defender.GetComponent<IPlayer>()
            };
            
            board = CreateGameBoard(settings.GameMode);
            simulator = new GameSimulator(board);
            
            SceneDataTransferComponent.Instance.WriteSingle(new LocalGameSettings(DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") ,settings));
        }

        private GameBoard CreateGameBoard(GameMode gameMode) => gameMode switch
        {
            GameMode.Board7 => GameBoardFactory.CreatGameBoard7(),
            GameMode.Board9 => GameBoardFactory.CreatGameBoard9(),
            GameMode.Board11 => GameBoardFactory.CreatGameBoard11(),
        };
    }
}