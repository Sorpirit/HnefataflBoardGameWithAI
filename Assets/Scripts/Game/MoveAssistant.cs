using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Game
{
    public class MoveAssistant
    {
        private GameBoard _board;
        private GameSimulator _gameSimulator;

        public MoveAssistant(GameBoard board, GameSimulator gameSimulator)
        {
            _board = board;
            _gameSimulator = gameSimulator;
        }
        
        public bool CanMove(PlayerType playerType)
        {
            return _gameSimulator.CurrentPlayer == playerType;
        }
        
        public bool IsValidStartingTile(Vector2Int tile, PlayerType playerType)
        {
            var pawn = _board.GetPawn(tile.x, tile.y);
            if (pawn == PawnType.Empty)
                return false;

            return pawn.GetSide() == playerType;
        }
        
        public bool IsValidFinishTile(Vector2Int tile)
        {
            var pawn = _board.GetPawn(tile.x, tile.y);
            if (pawn != PawnType.Empty)
                return false;

            return true;
        }
        
        public bool IsValidMove(Vector2Int startTile, Vector2Int finishTile, PlayerType playerType)
        {
            return _gameSimulator.IsMoveValid(new PawnMove(startTile.x, startTile.y, finishTile.x, finishTile.y), playerType);
        }
        
        public void GatPlayerValidMoves(PlayerType targetPlayer, List<PawnMove> moves)
        {
            Span<PawnMove> pawnMoves = stackalloc PawnMove[_board.Size * 2 - 1];
            int lastMoveIndex = 0;
            
            for (int x = 0; x < _board.Size; x++)
            {
                for (int y = 0; y < _board.Size; y++)
                {
                    //reset the last move index
                    lastMoveIndex = 0;
                    
                    GetPawnValidMoves(x, y, pawnMoves, ref lastMoveIndex, targetPlayer);

                    //Add all valid moves
                    for (int i = 0; i < lastMoveIndex; i++)
                    {
                        moves.Add(pawnMoves[i]);
                    }
                }
            }
        }

        public void GetPawnValidMoves(int x, int y, Span<PawnMove> pawnMoves, ref int lastMoveIndex, PlayerType currentPlayer)
        {
            if(_board.GetPawn(x, y) == PawnType.Empty || _board.GetPawn(x, y).GetSide() != currentPlayer)
                return;
            
            //Horizontal moves
            for (int i = 0; i < _board.Size; i++)
            {
                if (i == x || _board.GetPawn(i, y) != PawnType.Empty)
                    continue;

                var move = new PawnMove(x, y, i, y);

                if (_gameSimulator.IsMoveValid(move, currentPlayer))
                    pawnMoves[lastMoveIndex++] = move;

            }

            //Vertical moves
            for (int i = 0; i < _board.Size; i++)
            {
                if (i == y || _board.GetPawn(x, i) != PawnType.Empty)
                    continue;

                var move = new PawnMove(x, y, x, i);

                if (_gameSimulator.IsMoveValid(move, currentPlayer))
                    pawnMoves[lastMoveIndex++] = move;

            }
        }
    }
}