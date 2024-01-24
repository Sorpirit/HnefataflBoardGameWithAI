using Core;
using ProtoBuf;

namespace Game.Save
{
    [ProtoContract]
    public class PawnMoveInternal
    {
        [ProtoMember(1)]
        public int _fromX { get; set; }
        [ProtoMember(2)]
        public int _fromY { get; set; }
        [ProtoMember(3)]
        public int _toX { get; set; }
        [ProtoMember(4)]
        public int _toY { get; set; }

        public PawnMoveInternal(int fromX, int fromY, int toX, int toY)
        {
            _fromX = fromX;
            _fromY = fromY;
            _toX = toX;
            _toY = toY;
        }

        public PawnMoveInternal()
        {
        }

        public static implicit operator PawnMoveInternal(PawnMove move) => new(move.FromX, move.FromY, move.ToX, move.ToY);
        public static implicit operator PawnMove(PawnMoveInternal move) => new(move._fromX, move._fromY, move._toX, move._toY);
    }
}