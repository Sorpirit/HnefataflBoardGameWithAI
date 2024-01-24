using System;

namespace Core
{
    [Serializable]
    public struct PawnMove
    {
        public int FromX => _fromX;
        public int FromY => _fromY;
        public int ToX => _toX;
        public int ToY => _toY;

        private int _fromX;
        private int _fromY;
        private int _toX;
        private int _toY;
        
        public PawnMove(int fromX, int fromY, int toX, int toY)
        {
            _fromX = fromX;
            _fromY = fromY;
            _toX = toX;
            _toY = toY;
        }

        public override string ToString()
        {
            return $"{nameof(FromX)}: {FromX}, {nameof(FromY)}: {FromY}, {nameof(ToX)}: {ToX}, {nameof(ToY)}: {ToY}";
        }
        
        public bool Equals(PawnMove other)
        {
            return _fromX == other._fromX && _fromY == other._fromY && _toX == other._toX && _toY == other._toY;
        }
    }
}