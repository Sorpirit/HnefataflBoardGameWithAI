using System;

namespace Core
{
    public static class PlayerTypeHelpers
    {
        public static PlayerType GetOpponentType(this PlayerType type) => type == PlayerType.Attacker ? PlayerType.Defender : PlayerType.Attacker;
        
        public static PlayerType GetSide(this PawnType type)
        {
            if(type == PawnType.Empty)
                throw new InvalidOperationException("Invalid pawn type");
            return type == PawnType.Attacker ? PlayerType.Attacker : PlayerType.Defender;
        }
    }
}