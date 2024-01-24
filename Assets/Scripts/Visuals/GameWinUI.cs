using System;
using Core;
using Game;
using TMPro;
using UnityEngine;

namespace Visuals
{
    public class GameWinUI : MonoBehaviour
    {
        [SerializeField] private GameObject winPanel;
        [SerializeField] private TMP_Text winLabel;
        
        
        private void Awake()
        {
            winPanel.SetActive(false);
        }

        private void Start()
        {
            var instance = GameController.Instance;
            if(instance != null) instance.OnGameFinished += ShowWinPanel;
        }
        
        private void OnDisable()
        {
            GameController.Instance.OnGameFinished -= ShowWinPanel;
        }

        public void ShowWinPanel(PlayerType? playerType)
        {
            winPanel.SetActive(true);
            winLabel.text = GetWinLabel(playerType);
        }
        
        private string GetWinLabel(PlayerType? playerType)
        {
            switch (playerType)
            {
                case PlayerType.Attacker:
                    return "Attacker win!";
                case PlayerType.Defender:
                    return "Defender win!";
                default:
                    return "Draw!";
            }
        }
    }
}