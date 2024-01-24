using System;
using Game.GameFactories;
using Game.Library;
using Game.Save;
using UnityEngine;

namespace Game
{
    public class LocalGameAdapter : MonoBehaviour
    {
        [SerializeField] private GameObject loadSavedGameFactory;
        [SerializeField] private GameObject loadNewGameFactory;
        
        [SerializeField] private GameObject gameSaveService;
        
        private IGameFactory _loadSavedGame;
        private IGameFactory _loadNewGame;
        
        private IGameSaveService _gameSaveService;

        private void Start()
        {
            _loadSavedGame = loadSavedGameFactory.GetComponent<IGameFactory>();
            _loadNewGame = loadNewGameFactory.GetComponent<IGameFactory>();
            
            _gameSaveService = gameSaveService.GetComponent<IGameSaveService>();
            
            var settings = SceneDataTransferComponent.Instance.ReadSingle<LocalGameSettings>();
            GameController.Instance.StartGame(settings.GameSaveName != null ? _loadSavedGame : _loadNewGame, _gameSaveService);
        }
    }
}