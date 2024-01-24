using System;
using System.IO;
using Core;
using Game.Library;
using UnityEngine;
using Utilities;

namespace Game.Save
{
    public class ProtoGameSaveService : MonoBehaviour, IGameSaveService
    {
        private IContainerDataManager<GameSave> _dataManager;
        
        private void Start()
        {
            string targetPath = Path.Combine(Application.persistentDataPath, Constants.SaveFolderName);
            //_dataManager = new ProtoContainerDataManager<GameSave>(targetPath);
            _dataManager = new CustomBinarySerialisator<GameSave>(targetPath);
            Debug.Log(targetPath);
        }

        public void SaveGame(GameSimulator simulator, string overrideName = null)
        {
            var settings = SceneDataTransferComponent.Instance.ReadSingle<LocalGameSettings>();
            _dataManager.SaveDataContainerAsync(settings.GameSaveName ?? overrideName ?? DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss"),
                new GameSave(simulator.MoveList.ToArray(), settings)).Wait();
        }
    }
}