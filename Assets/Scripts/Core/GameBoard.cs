using System;

namespace Core
{
    public class GameBoard
    {
        public int Size => _size;
        
        public (int x, int y) KingPosition { get; private set; } = (0,0);
        public int AttackerCount { get; private set; } = 0;
        public int DefenderCount { get; private set; } = 0;

        private PawnType[,] _board;
        private int _size;

        public static GameBoard CreatGameBoard7Empty() => new(7);
        public static GameBoard CreatGameBoard9Empty() => new(9);
        public static GameBoard CreatGameBoard11Empty() => new(11);
        
        private GameBoard(int size)
        {
            _size = size;
            _board = new PawnType[size, size];
        }
        
        public GameBoard(GameBoard gameBoard)
        {
            _size = gameBoard.Size;
            _board = new PawnType[_size, _size];
            for (int x = 0; x < _size; x++)
            for (int y = 0; y < _size; y++)
            {
                _board[x, y] = gameBoard._board[x, y];
            }
            
            KingPosition = gameBoard.KingPosition;
            AttackerCount = gameBoard.AttackerCount;
            DefenderCount = gameBoard.DefenderCount;
        }
        
        public void SetPawn(int x, int y, PawnType pawnType)
        {
            if (!IsMoveValid(x, y, pawnType))
            {
                throw new System.Exception("Invalid move");
            }

            SetPawnInternal(x, y, pawnType);
        }
        
        public bool TrySetPawn(int x, int y, PawnType pawnType)
        {
            if (!IsMoveValid(x, y, pawnType))
            {
                return false;
            }
            
            SetPawnInternal(x, y, pawnType);
            return true;
        }
        
        public PawnType GetPawn(int x, int y)
        {
            return _board[x, y];
        }
        
        public void CopyBoard(GameBoard gameBoard)
        {
            for (int x = 0; x < _size; x++)
            for (int y = 0; y < _size; y++)
            {
                _board[x, y] = gameBoard._board[x, y];
            }
            
            KingPosition = gameBoard.KingPosition;
            AttackerCount = gameBoard.AttackerCount;
            DefenderCount = gameBoard.DefenderCount;
        }
        
        public PawnType GetPawnOrDefault(int x, int y)
        {
            return IsPositionInBoard(x, y) ? _board[x, y] : PawnType.Empty;
        }
        
        public bool IsKingSquare(int x, int y) => x == 0 && y == 0 || 
                                                  x == 0 && y == _size - 1 || 
                                                  x == _size - 1 && y == 0 ||
                                                  x == _size - 1 && y == _size - 1 ||
                                                  x == _size / 2 && y == _size / 2;

        public bool IsEscapeSquare(int x, int y) => x == 0 && y == 0 ||
                                                    x == 0 && y == _size - 1 ||
                                                    x == _size - 1 && y == 0 ||
                                                    x == _size - 1 && y == _size - 1;
        
        public bool IsKingThrone(int x, int y) => x == _size / 2 && y == _size / 2;

        public bool IsSquareEmpty(int x, int y) => _board[x, y] == PawnType.Empty;
        
        public bool IsPositionInBoard(int x, int y) => x >= 0 && x < _size && y >= 0 && y < _size;

        private void SetPawnInternal(int x, int y, PawnType pawnType)
        {
            switch (pawnType)
            {
                case PawnType.King:
                    KingPosition = (x, y);
                    break;
                case PawnType.Attacker:
                    AttackerCount++;
                    break;
                case PawnType.Defender:
                    DefenderCount++;
                    break;
                case PawnType.Empty:
                    if (_board[x, y] == PawnType.Attacker)
                        AttackerCount--;
                    else if (_board[x, y] == PawnType.Defender)
                        DefenderCount--;
                    break;
            }
            
            _board[x, y] = pawnType;
        }
        
        private bool IsMoveValid(int x, int y, PawnType pawnType)
        {
            return IsPositionInBoard(x, y) &&
                   (pawnType == PawnType.Empty || (pawnType == PawnType.King || !IsKingSquare(x, y)) && IsSquareEmpty(x, y));
        }

        protected bool Equals(GameBoard other)
        {
            return _size == other._size && _board.SequenceEqual(other._board);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GameBoard)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_board, _size);
        }
    }
}