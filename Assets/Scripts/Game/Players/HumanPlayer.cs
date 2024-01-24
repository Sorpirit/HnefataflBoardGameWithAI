using System;
using Core;
using UnityEngine;

namespace Game.Players
{
    public class HumanPlayer : MonoBehaviour, IPlayer
    {
        

        public Vector2Int? StartTile
        {
            get => _startTile;
            private set
            {
                _startTile = value;
                OnStartTileChanged?.Invoke(_startTile);
            }
        }
        
        public bool IsTurnInProgress { get; private set; }
        
        public event Action<PawnMove> OnMakeMove;
        public event Action<Vector2Int?> OnStartTileChanged;

        private TileSelector _tileSelector;
        
        private PlayerType _type;
        private Vector2Int? _startTile;
        private Vector2Int? _finishTile;
        private MoveAssistant _moveAssistant;

        private void Start()
        {
            _tileSelector = GameDependencyResolver.Instance.TileSelector;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0) || !IsTurnInProgress || !_moveAssistant.CanMove(_type)) 
                return;
            
            if (!_tileSelector.CastMouseTile(out var tile)) 
                return;
                
                
            if (StartTile == null)
            {
                if (_moveAssistant.IsValidStartingTile(tile, _type))
                {
                    StartTile = tile;
                }
            }
            else
            {
                if(StartTile.Value == tile)
                {
                    StartTile = null;
                    return;
                }
                
                if (_moveAssistant.IsValidStartingTile(tile, _type))
                {
                    StartTile = tile;
                    return;
                }
                
                if (!_moveAssistant.IsValidFinishTile(tile)) 
                    return;

                _finishTile = tile;
                if (_moveAssistant.IsValidMove(StartTile.Value, _finishTile.Value, _type))
                {
                    OnMakeMove?.Invoke(new PawnMove(StartTile.Value.x, StartTile.Value.y, _finishTile.Value.x, _finishTile.Value.y));
                    StartTile = null;
                    IsTurnInProgress = false;
                }
            }
        }

        public void Init(PlayerType type, MoveAssistant moveAssistant)
        {
            Debug.Log($"Human player {type} initialized");
            _type = type;
            _moveAssistant = moveAssistant;
        }

        public void StartTurn()
        {
            Debug.Log($"Human player {_type} started turn");
            IsTurnInProgress = true;
        }
    }
}