using Core;
using Unity.Netcode;

namespace Game
{
    public struct NetworkPawnMove : INetworkSerializable
    {
        private int _fromX;
        private int _fromY;
        private int _toX;
        private int _toY;

        private NetworkPawnMove(int fromX, int fromY, int toX, int toY)
        {
            _fromX = fromX;
            _fromY = fromY;
            _toX = toX;
            _toY = toY;
        }

        public static implicit operator NetworkPawnMove(PawnMove move)
        {
            return new NetworkPawnMove(move.FromX, move.FromY, move.ToX, move.ToY);
        } 
        public static implicit operator PawnMove(NetworkPawnMove move)
        {
            return new PawnMove(move._fromX, move._fromY, move._toX, move._toY);
        }
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            
            serializer.SerializeValue(ref _fromX);
            serializer.SerializeValue(ref _fromY);
            serializer.SerializeValue(ref _toX);
            serializer.SerializeValue(ref _toY);
        }

        public override string ToString()
        {
            return $"{nameof(_fromX)}: {_fromX}, {nameof(_fromY)}: {_fromY}, {nameof(_toX)}: {_toX}, {nameof(_toY)}: {_toY}";
        }
    }
}