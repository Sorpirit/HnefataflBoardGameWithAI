using System;
using System.Collections.Generic;

namespace Core
{
    public class GameSimulator
    {
        public List<PawnMove> MoveList => _moveList;
        public int DefenderCount => _gameBoard.DefenderCount;
        public int AttackerCount => _gameBoard.AttackerCount;
        public PlayerType CurrentPlayer => _currentPlayer;
        public bool IsFinished { get; private set; }
        public PlayerType? Winner { get; private set; }
        
        public event Action<List<(int x, int y)>> OnCaptures; 

        private readonly GameBoard _gameBoard;
        private List<PawnMove> _moveList;
        private PlayerType _currentPlayer = PlayerType.Attacker;
        
        public GameSimulator(GameBoard gameBoard, PlayerType playerType = PlayerType.Attacker)
        {
            _gameBoard = gameBoard;
            _currentPlayer = playerType;
            _moveList = new List<PawnMove>();
        }
        
        public GameSimulator(GameBoard gameBoard, List<PawnMove> simMoves, PlayerType playerType = PlayerType.Attacker) : this(gameBoard, playerType)
        {
            foreach (var simMove in simMoves)
            {
                SimulateMove(simMove);
            }
        }

        public void ResetGame(PlayerType playerType = PlayerType.Attacker)
        {
            _moveList.Clear();
            _currentPlayer = playerType;
            IsFinished = false;
            Winner = null;
        }
        
        public void SimulateMove(PawnMove pawnMove)
        {
            if(!IsMoveValid(pawnMove, _currentPlayer))
                throw new InvalidOperationException("Invalid move: " + pawnMove);
            
            var originalPawn = _gameBoard.GetPawn(pawnMove.FromX, pawnMove.FromY);
            bool result = _gameBoard.TrySetPawn(pawnMove.ToX, pawnMove.ToY, originalPawn);
            if (!result)
                throw new InvalidOperationException("Invalid board move");
            
            _gameBoard.SetPawn(pawnMove.FromX, pawnMove.FromY, PawnType.Empty);
            _moveList.Add(pawnMove);

            var captures = GetCaptures(pawnMove);
            RemovePawn(captures);
            OnCaptures?.Invoke(captures);

            var currentPlayerWon = CheckWin();
            if (currentPlayerWon)
            {
                IsFinished = true;
                Winner = _currentPlayer;
            }
            
            if (CheckPerpetualMoves())
            {
                IsFinished = true;
                Winner = _currentPlayer.GetOpponentType();
            }
            
            var opponent = _currentPlayer.GetOpponentType();
            if(!HasAvailableMoves(opponent))
            {
                IsFinished = true;
                Winner = _currentPlayer;
            }
                
            //Pass turn to the next player
            _currentPlayer = opponent;
        }
        
        public bool IsMoveValid(PawnMove pawnMove, PlayerType playerType)
        {
            //Move in board
            if(!_gameBoard.IsPositionInBoard(pawnMove.FromX, pawnMove.FromY) || !_gameBoard.IsPositionInBoard(pawnMove.ToX, pawnMove.ToY))
                return false;
            
            //if diagonal move
            if (pawnMove.FromX != pawnMove.ToX && pawnMove.FromY != pawnMove.ToY)
                return false;
            
            //Check if moving a pawn and moving it to the empty square
            var originalPawn = _gameBoard.GetPawn(pawnMove.FromX, pawnMove.FromY);
            var targetPawn = _gameBoard.GetPawn(pawnMove.ToX, pawnMove.ToY);
            
            if (originalPawn == PawnType.Empty || targetPawn != PawnType.Empty)
                return false;

            //Check if player owns the pawn
            if (originalPawn.GetSide() != playerType)
                return false;
            
            //Check if pawn can occupy the king square
            if(originalPawn != PawnType.King && _gameBoard.IsKingSquare(pawnMove.ToX, pawnMove.ToY))
                return false;

            //Check if the path is clear
            int deltaX = pawnMove.ToX - pawnMove.FromX;
            int deltaY = pawnMove.ToY - pawnMove.FromY;

            int distance = Math.Abs(deltaX) + Math.Abs(deltaY);
            deltaX = Math.Sign(deltaX);
            deltaY = Math.Sign(deltaY);
            for (int i = 1; i < distance; i++)
            {
                if(_gameBoard.GetPawnOrDefault(pawnMove.FromX + deltaX * i, pawnMove.FromY + deltaY * i) != PawnType.Empty)
                    return false;
            }
            
            return true;
        }

        private void RemovePawn(List<(int x, int y)> captures)
        {
            foreach (var (x, y) in captures)
            {
                _gameBoard.SetPawn(x, y, PawnType.Empty);
            }
        }

        private List<(int x, int y)> GetCaptures(PawnMove pawnMove)
        {
            var lastMove = pawnMove;
            List<(int x, int y)> toDelete = new List<(int x, int y)>();
            var opponentType = _currentPlayer.GetOpponentType();

            if (CanCapture(lastMove.ToX, lastMove.ToY + 1, lastMove.ToX, lastMove.ToY + 2))
                toDelete.Add((lastMove.ToX, lastMove.ToY + 1));
            
            if (CanCapture(lastMove.ToX, lastMove.ToY - 1, lastMove.ToX, lastMove.ToY - 2))
                toDelete.Add((lastMove.ToX, lastMove.ToY - 1));
            
            if (CanCapture(lastMove.ToX + 1, lastMove.ToY, lastMove.ToX + 2, lastMove.ToY))
                toDelete.Add((lastMove.ToX + 1, lastMove.ToY));
            
            if (CanCapture(lastMove.ToX - 1, lastMove.ToY, lastMove.ToX - 2, lastMove.ToY))
                toDelete.Add((lastMove.ToX - 1, lastMove.ToY));

            return toDelete;
            
            bool CanCapture(int x, int y, int xN, int yN)
            {
                var capturePawn = _gameBoard.GetPawnOrDefault(x, y);
                
                if (capturePawn == PawnType.Empty || capturePawn == PawnType.King || capturePawn.GetSide() != opponentType)
                    return false;
                
                if (!_gameBoard.IsPositionInBoard(xN, yN))
                    return false;
                
                var type = _gameBoard.GetPawnOrDefault(xN, yN);
                return type == PawnType.Empty && _gameBoard.IsKingSquare(xN, yN) || type != PawnType.Empty && type.GetSide() == _currentPlayer;
            }
        }
        
        public bool IsHostileCell(int x, int y, PlayerType opponent, bool includeCorners = true) => 
            (_gameBoard.GetPawnOrDefault(x, y) != PawnType.Empty && _gameBoard.GetPawnOrDefault(x, y).GetSide() == opponent) ||
                                                _gameBoard.IsKingThrone(x, y) || (includeCorners && _gameBoard.IsKingSquare(x, y));
        
        private bool CheckWin()
        {
            var kingPosition = _gameBoard.KingPosition;
            
            if (_currentPlayer == PlayerType.Defender)
                return _gameBoard.IsEscapeSquare(kingPosition.x, kingPosition.y);
            
            int surroundedCount = 0;
            if(IsHostileCell(kingPosition.x, kingPosition.y + 1, PlayerType.Attacker, false))
                surroundedCount++;
                
            if(IsHostileCell(kingPosition.x, kingPosition.y - 1, PlayerType.Attacker, false))
                surroundedCount++;
                
            if(IsHostileCell(kingPosition.x + 1, kingPosition.y, PlayerType.Attacker, false))
                surroundedCount++;
                
            if(IsHostileCell(kingPosition.x - 1, kingPosition.y, PlayerType.Attacker, false))
                surroundedCount++;
                
            return surroundedCount == 4;
        }

        public bool CheckPerpetualMoves()
        {
            if (MoveList.Count <= 6) 
                return false;
            
            var yMove = _moveList[^1];
            var xMove = _moveList[^2];
            var yMove2 = _moveList[^3];
            var xMove2 = _moveList[^4];
            var yMove3 = _moveList[^5];
            var xMove3 = _moveList[^6];
                
            bool yMoveIsPerpetual = yMove.Equals(yMove3) && (yMove.ToX == yMove2.FromX && yMove.ToY == yMove2.FromY) && (yMove2.ToX == yMove.FromX && yMove2.ToY == yMove.FromY);
            bool xMoveIsPerpetual = xMove.Equals(xMove3) && (xMove.ToX == xMove2.FromX && xMove.ToY == xMove2.FromY) && (xMove2.ToX == xMove.FromX && xMove2.ToY == xMove.FromY);

            return yMoveIsPerpetual && xMoveIsPerpetual;
        }

        private bool HasAvailableMoves(PlayerType targetPlayer)
        {
            for (int i = 0; i < _gameBoard.Size; i++)
            {
                for (int j = 0; j < _gameBoard.Size; j++)
                {
                    var pawnType = _gameBoard.GetPawn(i, j);
                    if(pawnType == PawnType.Empty)
                        continue;
                    
                    if(pawnType.GetSide() != targetPlayer)
                        continue;

                    if (_gameBoard.IsPositionInBoard(i, j + 1) && _gameBoard.GetPawnOrDefault(i, j + 1) == PawnType.Empty)
                        return true;
                    
                    if (_gameBoard.IsPositionInBoard(i, j - 1) && _gameBoard.GetPawnOrDefault(i, j - 1) == PawnType.Empty)
                        return true;
                    
                    if (_gameBoard.IsPositionInBoard(i + 1, j) && _gameBoard.GetPawnOrDefault(i + 1, j) == PawnType.Empty)
                        return true;

                    if (_gameBoard.IsPositionInBoard(i - 1, j) && _gameBoard.GetPawnOrDefault(i - 1, j) == PawnType.Empty)
                        return true;
                }
            }

            return false;
        }

        public void ForceFinishGame(PlayerType winner)
        {
            IsFinished = true;
            Winner = winner;
        }
    }
}