using System;
using System.IO;
using System.Linq;
using Core;
using Game.Library;
using Game.Players;
using Game.Save;
using UnityEngine;
using Utilities;

namespace Game.GameFactories
{
    public class LoadGameFactory : MonoBehaviour, IGameFactory
    {
        [SerializeField] private PlayerPrefabCollection playerPrefabCollection;
        
        private IContainerDataManager<GameSave> _dataManager;
        
        private void Awake()
        {
            //_dataManager = new ProtoContainerDataManager<GameSave>(Path.Combine(Application.persistentDataPath, Constants.SaveFolderName));
            _dataManager = new CustomBinarySerialisator<GameSave>(Path.Combine(Application.persistentDataPath, Constants.SaveFolderName));
        }

        public void CreateGame(out GameSimulator simulator, out GameBoard board, out IPlayer[] players)
        {
            var settings = SceneDataTransferComponent.Instance.ReadSingle<LocalGameSettings>();
            var save = _dataManager.LoadDataContainerAsync(settings.GameSaveName);
            save.Wait();
            
            if(!save.IsCompletedSuccessfully)
                throw new InvalidOperationException("Unable to load game save");

            var attacker = Instantiate(playerPrefabCollection.GetPrefab(save.Result.GetLocalGameSettings().Attacker), transform);
            var defender = Instantiate(playerPrefabCollection.GetPrefab(save.Result.GetLocalGameSettings().Defender), transform);
            
            players = new[]
            {
                attacker.GetComponent<IPlayer>(),
                defender.GetComponent<IPlayer>()
            };
            
            board = CreateGameBoard(save.Result.GetLocalGameSettings().GameMode);
            simulator = new GameSimulator(board, save.Result.MoveList.ToList());
            SceneDataTransferComponent.Instance.WriteSingle(new LocalGameSettings(settings.GameSaveName, save.Result.GetLocalGameSettings()));
        }
        
        private GameBoard CreateGameBoard(GameMode gameMode) => gameMode switch
        {
            GameMode.Board7 => GameBoardFactory.CreatGameBoard7(),
            GameMode.Board9 => GameBoardFactory.CreatGameBoard9(),
            GameMode.Board11 => GameBoardFactory.CreatGameBoard11(),
        };
    }
}