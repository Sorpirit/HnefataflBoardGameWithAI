using System;
using System.Collections.Generic;
using Core;
using Game.GameFactories;
using Game.Library;
using Game.Players;
using Game.Save;
using UnityEngine;

namespace Game
{
    public class GameController : SingleBehaviour<GameController>
    {
        [SerializeField] private GameBoardSpawner gameBoardSpawner;

        public event Action OnGameStarted;
        public event Action<List<(int x, int y)>> OnPawnsCaptured;
        public event Action<PawnMove> OnPawnMoved;
        public event Action<PlayerType?> OnGameFinished;
        
        public int MoveCount => _gameSimulator.MoveList.Count;
        public int DefenderCount => _gameBoard.DefenderCount;
        public int AttackerCount => _gameBoard.AttackerCount;
        public PlayerType CurrentPlayer => _gameSimulator.CurrentPlayer;
        
        
        private bool _isGameStarted;
        private GameBoard _gameBoard;
        private GameSimulator _gameSimulator;
        private IPlayer _attacker;
        private IPlayer _defender;
        private bool _turnInProgress;

        private IGameSaveService _gameSaveService;

        public void StartGame(IGameFactory gameFactory, IGameSaveService gameSaveService)
        {
            _gameSaveService = gameSaveService;
            
            gameFactory.CreateGame(out _gameSimulator, out _gameBoard, out var players);
            _attacker = players[0];
            _defender = players[1];
            
            _gameSimulator.OnCaptures += CapturePawns;

            gameBoardSpawner.SpawnBoard(_gameBoard);
            
            var moveAssistant = new MoveAssistant(_gameBoard, _gameSimulator);
            
            _attacker.Init(PlayerType.Attacker, moveAssistant);
            _defender.Init(PlayerType.Defender, moveAssistant);

            if(_attacker is IGameControlsRequester attackerControlsRequester)
                attackerControlsRequester.ProvideControls(_gameBoard, _gameSimulator);
            
            if(_defender is IGameControlsRequester defenderControlsRequester)
                defenderControlsRequester.ProvideControls(_gameBoard, _gameSimulator);
            
            _attacker.OnMakeMove += OnMakeMove;
            _defender.OnMakeMove += OnMakeMove;
            
            _isGameStarted = true;
            OnGameStarted?.Invoke();
        }
        
        public void ForceFinishGame(PlayerType winner)
        {
            _gameSimulator.ForceFinishGame(winner);
            OnGameFinished?.Invoke(winner);
        }

        private void CapturePawns(List<(int x, int y)> captures)
        {
            OnPawnsCaptured?.Invoke(captures);
        }

        private void Update()
        {
            if(!_isGameStarted)
                return;
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                _gameSaveService.SaveGame(_gameSimulator);
            }
            
            if (_gameSimulator.IsFinished)
                return;

            if(_turnInProgress)
                return;

            _turnInProgress = true;
            
            if (_gameSimulator.CurrentPlayer == PlayerType.Attacker)
            {
                _attacker.StartTurn();
            }
            else
            {
                _defender.StartTurn();
            }
        }

        private void OnMakeMove(PawnMove move)
        {
            try
            {
                _gameSimulator.SimulateMove(move);
            }
            catch (InvalidOperationException e)
            {
                Debug.LogError(e.Message);
            }
            
            OnPawnMoved?.Invoke(move);
            if(_gameSimulator.IsFinished)
            {
                Debug.Log("Game finished, winner: " + _gameSimulator.Winner);
                OnGameFinished?.Invoke(_gameSimulator.Winner);
            }
            else
            {
                _turnInProgress = false;
            }
        }
    }
}