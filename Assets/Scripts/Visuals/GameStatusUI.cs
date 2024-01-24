using System;
using Core;
using Game;
using TMPro;
using UnityEngine;

namespace Visuals
{
    public class GameStatusUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentPlayerMove;
        [SerializeField] private TMP_Text moveCount;
        [SerializeField] private TMP_Text pwanCount;

        private void Start()
        {
            GameController.Instance.OnPawnMoved += OnPawnMoved;
        }

        private void OnDisable()
        {
            var instance = GameController.Instance;
            if (instance != null) instance.OnPawnMoved -= OnPawnMoved;
        }

        private void OnPawnMoved(PawnMove obj)
        {
            currentPlayerMove.text = GetCurrentPlayerLabel(GameController.Instance.CurrentPlayer);
            moveCount.text = "Move count: " + GameController.Instance.MoveCount;
            pwanCount.text = GetCurrentPawnCountLabel(GameController.Instance.AttackerCount, GameController.Instance.DefenderCount);
        }
        
        private string GetCurrentPlayerLabel(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Attacker:
                    return "Attacker move";
                case PlayerType.Defender:
                    return "Defender move";
                default:
                    throw new ArgumentOutOfRangeException(nameof(playerType), playerType, null);
            }
        }
        
        public string GetCurrentPawnCountLabel(int attackerCount, int defenderCount)
        {
            return "Attacker pawns: " + attackerCount + "\nDefender pawns: " + defenderCount;
        }
    }
}