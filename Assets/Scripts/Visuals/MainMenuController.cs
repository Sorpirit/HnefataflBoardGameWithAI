using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private List<Selectable> selectables;
        [SerializeField] private GameObject loadingPanel;

        [SerializeField] private GameObject localGameLoadGameSetting;
        [SerializeField] private GameObject localGameNewGameSetting;
        [SerializeField] private GameObject onlineGameNewGameSetting;
        
        public void ShowLoading()
        {
            LockUI();
            loadingPanel.SetActive(true);
        }
        
        public void HideLoading()
        {
            UnlockUI();
            loadingPanel.SetActive(false);
        }

        private void LockUI()
        {
            selectables.ForEach(selectable => selectable.interactable = false);
        }
        
        private void UnlockUI()
        {
            selectables.ForEach(selectable => selectable.interactable = true);
        }

        public void ResetUI()
        {
            UnlockUI();
            localGameLoadGameSetting.SetActive(false);
            localGameNewGameSetting.SetActive(false);
            onlineGameNewGameSetting.SetActive(false);
        }
        
        public void ActivateLocalGameLoadGameSetting()
        {
            localGameLoadGameSetting.SetActive(true);
            LockUI();
        }
        
        public void ActivateLocalGameNewGameSetting()
        {
            localGameNewGameSetting.SetActive(true);
            LockUI();
        }
        
        public void ActivateOnlineGameNewGameSetting()
        {
            onlineGameNewGameSetting.SetActive(true);
            LockUI();
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}