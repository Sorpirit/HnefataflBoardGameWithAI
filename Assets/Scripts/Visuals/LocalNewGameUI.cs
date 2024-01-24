using System;
using Game;
using Game.Library;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Visuals
{
    public class LocalNewGameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown modeDropdown;
        [SerializeField] private TMP_Dropdown attackerDropdown;
        [SerializeField] private TMP_Dropdown defenderDropdown;
        [SerializeField] private TMP_Dropdown attackerAIDifficultyDropdown;
        [SerializeField] private TMP_Dropdown defenderAIDifficultyDropdown;
        
        [SerializeField] private GameSceneLoader gameSceneLoader;
        
        private LocalPlayerType _attacker;
        private LocalPlayerType _defender;
        
        private void OnEnable()
        {
            attackerDropdown.onValueChanged.AddListener(OnAttackerChanged);
            defenderDropdown.onValueChanged.AddListener(OnDefenderChanged);
            
            attackerAIDifficultyDropdown.gameObject.SetActive(false);
            defenderAIDifficultyDropdown.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            attackerDropdown.onValueChanged.RemoveListener(OnAttackerChanged);
            defenderDropdown.onValueChanged.RemoveListener(OnDefenderChanged);
        }

        private void OnDefenderChanged(int arg0)
        {
            _defender = (LocalPlayerType) arg0;
            defenderAIDifficultyDropdown.gameObject.SetActive(_defender == LocalPlayerType.AI);
        }

        private void OnAttackerChanged(int arg0)
        {
            _attacker = (LocalPlayerType) arg0;
            attackerAIDifficultyDropdown.gameObject.SetActive(_attacker == LocalPlayerType.AI);
        }

        public void StartGame()
        {
            var mode = (GameMode) modeDropdown.value;
            var attackerAIDifficulty = (AIDifficulty) attackerAIDifficultyDropdown.value;
            var defenderAIDifficulty = (AIDifficulty) defenderAIDifficultyDropdown.value;
            
            SceneDataTransferComponent.Instance.WriteSingle(
                new LocalGameSettings(mode, _attacker, _defender, attackerAIDifficulty, defenderAIDifficulty));
            
            gameSceneLoader.StartLocalGame();
        }
    }
}