using System;
using System.Threading.Tasks;
using Core;
using Game.Library;
using Game.Players;
using UnityEngine;
using Utilities;

namespace Game.AI
{
    public class AIPlayer : MonoBehaviour, IPlayer, IGameControlsRequester
    {
        [SerializeField] private AIDebugger debugger;
        
        public event Action<PawnMove> OnMakeMove;
        
        private GameBoard _gameBoard;
        private PlayerType _targetPlayerType;
        private GameSimulator _gameSimulator;
        private AIDifficulty _difficulty;
        
        public void Init(PlayerType type, MoveAssistant moveAssistant)
        {
            _targetPlayerType = type;
            
            var settings = SceneDataTransferComponent.Instance.ReadSingle<LocalGameSettings>();
            _difficulty = _targetPlayerType == PlayerType.Attacker ? settings.AttackerAIDifficulty : settings.DefenderAIDifficulty;
        }

        public async void StartTurn()
        {
            MinMax<GameSnapshot> minMax = new MinMax<GameSnapshot>(_difficulty == AIDifficulty.Easy ? GameSnapshot.EvaluateSimple : GameSnapshot.Evaluate, GameSnapshot.GetChildren);
            var rootSnapshot = GameSnapshot.CreateSnapshot(_gameBoard, _gameSimulator, _targetPlayerType);

            var task = Task.Run(() => minMax.FindBestMove(rootSnapshot, GetDepthDifficulty(_difficulty, _gameSimulator.MoveList.Count), true));
            await task;
            if (!task.IsCompletedSuccessfully)
            {
                Debug.LogError(task.Exception);
                return;
            }
            
            OnMakeMove?.Invoke(task.Result.LastMove);
        }
        
        public void ProvideControls(GameBoard gameBoard, GameSimulator gameSimulator)
        {
            _gameBoard = gameBoard;
            _gameSimulator = gameSimulator;
            GameSnapshot.DebugGameBoard = gameBoard;
        }

        public int GetDepthDifficulty(AIDifficulty difficulty, int currentMove)
        {
            return difficulty switch
            {
                AIDifficulty.Easy => 2,
                AIDifficulty.Medium => 3,
                AIDifficulty.Hard => currentMove < 5 ? 3 : 4,
                _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
            };
        }
    }
}