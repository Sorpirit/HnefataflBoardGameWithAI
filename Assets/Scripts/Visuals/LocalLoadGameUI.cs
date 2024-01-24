using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game;
using Game.Library;
using Game.Save;
using UnityEngine;
using Utilities;

namespace Visuals
{
    public class LocalLoadGameUI : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private SaveUI selectableSave;
        [SerializeField] private GameSceneLoader gameSceneLoader;
        
        private ProtoContainerDataManager<GameSave> _dataManager;
        private List<string> _saveList;


        private void OnEnable()
        {
            _dataManager ??= new ProtoContainerDataManager<GameSave>(Path.Combine(Application.persistentDataPath, Constants.SaveFolderName));
            _saveList = _dataManager.GetContainerNames().ToList();
            for (var i = 0; i < _saveList.Count; i++)
            {
                var saveUI = Instantiate(selectableSave, content);
                saveUI.Init(i, Path.GetFileNameWithoutExtension(_saveList[i]), LoadSave);
            }
        }
        
        private void OnDisable()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
        }
        
        private void LoadSave(string label)
        {
            SceneDataTransferComponent.Instance.WriteSingle(new LocalGameSettings(label));
            
            gameSceneLoader.StartLocalGame();
        }
    }
}