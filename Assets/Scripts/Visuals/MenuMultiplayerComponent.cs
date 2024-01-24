using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Game;
using Game.Library;
using Game.Network;
using TMPro;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.UI;

namespace Visuals
{
    public class MenuMultiplayerComponent : MonoBehaviour
    {
        [SerializeField] private GameSceneLoader gameSceneLoader;
        
        [SerializeField] private TMP_InputField _joinCodeInput;
        [SerializeField] private GameObject networkComponentPrefab;
        
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        
        [SerializeField] private MainMenuController mainMenuController;
        
        [SerializeField] private TMP_Dropdown modeDropdown;
        [SerializeField] private TMP_Dropdown hostTypeDropdown;
        
        private static RelayLauncher _relayLauncher;
        
        private GameObject _networkComponent;

        private void OnEnable()
        {
            hostButton.onClick.AddListener(Host);
            joinButton.onClick.AddListener(Join);
        }
        
        private void OnDisable()
        {
            hostButton.onClick.RemoveListener(Host);
            joinButton.onClick.RemoveListener(Join);
        }

        private async void Host()
        {
            mainMenuController.ShowLoading();
            CreateNetworkComponent();
            //TODO remove error handling in ui
            try
            {
                await InitRelay();
                var joinCode = await _relayLauncher.CreateRelay();

                var cookie = new NetworkHostCookie(joinCode);
                SceneDataTransferComponent.Instance.WriteSingle(cookie);
                
                SceneDataTransferComponent.Instance.WriteSingle(new OnlineGameSettings((GameMode) modeDropdown.value, hostTypeDropdown.value < 2 ? (PlayerType) hostTypeDropdown.value : null));
                
                gameSceneLoader.StartOnlineGame();
            }
            catch (RelayServiceException e)
            {
                Debug.LogError(e);
            }
            finally
            {
                mainMenuController.HideLoading();
            }
        }

        private async void Join()
        {
            mainMenuController.ShowLoading();
            
            string joinCode = _joinCodeInput.text;
            if(string.IsNullOrEmpty(joinCode) || joinCode.Length < 4)
            {
                Debug.LogError("Invalid join code:" + joinCode);
                return;
            }

            CreateNetworkComponent();
            try
            {
                await InitRelay();
                await _relayLauncher.JoinRelay(joinCode);
                gameSceneLoader.StartOnlineGame();
            }
            catch (RelayServiceException e)
            {
                Debug.LogError(e);
            }
            finally
            {
                mainMenuController.HideLoading();
            }
        }

        private async Task InitRelay()
        {
            if (_relayLauncher != null) 
                return;
            List<Task> tasks = new List<Task>();
            _relayLauncher ??= new RelayLauncher(tasks);
            await Task.WhenAll(tasks);
        }

        private void CreateNetworkComponent()
        {
            if(_networkComponent != null || _relayLauncher != null)
                return;
            
            _networkComponent = Instantiate(networkComponentPrefab);
        }
    }
}