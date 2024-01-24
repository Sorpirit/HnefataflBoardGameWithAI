using Core;

namespace Game.AI
{
    public static class ScoreBoard
    { 
        public static int GetAttackScore(int x, int y, int s) => s switch
        {
            7 => _attackScoreBoard7[x, y],
            9 => _attackScoreBoard9[x, y],
            11 => _attackScoreBoard11[x, y],
        };
        
        public static int GetDefenderScore(int x, int y, int s) => s switch
        {
            7 => _defenderScoreBoard7[x, y],
            9 => _defenderScoreBoard9[x, y],
            11 => _defenderScoreBoard11[x, y],
        };
        
        public static int GetScore(this GameBoard board, int x, int y, PlayerType playerType) => board.GetPawn(x, y) switch
        {
            PawnType.Attacker => GetAttackScore(x, y, board.Size) * (playerType == PlayerType.Attacker ?  1 : -1),
            PawnType.Defender => GetDefenderScore(x, y, board.Size) * (playerType == PlayerType.Defender ?  1 : -1),
            _ => 0
        };

        public static int GetKingScore(this GameBoard board, PlayerType playerType) => board.Size switch
        {
            7 => _kingScoreBoard7[board.KingPosition.x, board.KingPosition.y] *
                 (playerType == PlayerType.Defender ? 1 : -1),
            9 => _kingScoreBoard9[board.KingPosition.x, board.KingPosition.y] *
                 (playerType == PlayerType.Defender ? 1 : -1),
            11 => _kingScoreBoard11[board.KingPosition.x, board.KingPosition.y] *
                  (playerType == PlayerType.Defender ? 1 : -1),
        };
            
        
        private static readonly int[,] _attackScoreBoard7 =
        {
            {0, 0, 3, 2, 3, 0, 0},
            {0, 3, 1, 0, 1, 3, 0},
            {3, 1, 0, 0, 0, 1, 3},
            {2, 0, 0, 0, 0, 0, 2},
            {3, 1, 0, 0, 0, 1, 3},
            {0, 3, 1, 0, 1, 3, 0},
            {0, 0, 3, 2, 3, 0, 0},
        };
        
        private static readonly int[,] _defenderScoreBoard7 =
        {
            {0, 1, 1, 3, 1, 1, 0},
            {1, 2, 0, 0, 0, 2, 1},
            {1, 0, 0, 0, 0, 0, 1},
            {3, 0, 0, 0, 0, 0, 3},
            {1, 0, 0, 0, 0, 0, 1},
            {1, 2, 0, 0, 0, 2, 1},
            {0, 1, 1, 3, 1, 1, 0},
        };
        
        private static readonly int[,] _kingScoreBoard7 =
        {
            {30, 12, 3, 3, 3, 12, 30},
            {12, 3, 1, 1, 1, 3, 12},
            {3, 1, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 1, 3},
            {12, 3, 1, 1, 1, 3, 12},
            {30, 12, 3, 3, 3, 12, 30},
        };
        
        private static readonly int[,] _attackScoreBoard9 =
        {
            {0, 0, 3, 1, 2, 1, 3, 0, 0},
            {0, 3, 1, 0, 0, 0, 1, 3, 0},
            {3, 1, 0, 0, 0, 0, 0, 1, 3},
            {1, 0, 0, 0, 0, 0, 0, 0, 1},
            {2, 0, 0, 0, 0, 0, 0, 0, 2},
            {1, 0, 0, 0, 0, 0, 0, 0, 1},
            {3, 1, 0, 0, 0, 0, 0, 1, 3},
            {0, 3, 1, 0, 0, 0, 1, 3, 0},
            {0, 0, 3, 1, 2, 1, 3, 0, 0},
        };
        
        private static readonly int[,] _defenderScoreBoard9 =
        {
            {0, 1, 1, 1, 3, 1, 1, 1, 0},
            {1, 2, 0, 0, 0, 0, 0, 2, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 1},
            {3, 0, 0, 0, 0, 0, 0, 0, 3},
            {1, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 2, 0, 0, 0, 0, 0, 2, 1},
            {0, 1, 1, 1, 3, 1, 1, 1, 0},
        };
        
        private static readonly int[,] _kingScoreBoard9 =
        {
            {30, 12, 3, 3, 3, 3, 3, 12, 30},
            {12, 3, 1, 1, 1, 1, 1, 3, 12},
            {3, 1, 0, 0, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 0, 0, 1, 3},
            {12, 3, 1, 1, 1, 1, 1, 3, 12},
            {30, 12, 3, 3, 3, 3, 3, 12, 30},
        };
        
        private static readonly int[,] _attackScoreBoard11 =
        {
            {0, 0, 3, 1, 1, 2, 1, 1, 3, 0, 0},
            {0, 3, 1, 0, 0, 0, 0, 0, 1, 3, 0},
            {3, 1, 0, 0, 0, 0, 0, 0, 0, 1, 3},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {3, 1, 0, 0, 0, 0, 0, 0, 0, 1, 3},
            {0, 3, 1, 0, 0, 0, 0, 0, 1, 3, 0},
            {0, 0, 3, 1, 1, 2, 1, 1, 3, 0, 0},
        };
        
        private static readonly int[,] _defenderScoreBoard11 =
        {
            {0, 1, 1, 1, 1, 3, 1, 1, 1, 1, 0},
            {1, 2, 0, 0, 0, 0, 0, 0, 0, 2, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 2, 0, 0, 0, 0, 0, 0, 0, 2, 1},
            {0, 1, 1, 1, 1, 3, 1, 1, 1, 1, 0},
        };
        
        private static readonly int[,] _kingScoreBoard11 =
        {
            {30, 12, 3, 3, 3, 3, 3, 3, 3, 12, 30},
            {12, 3, 1, 1, 1, 1, 1, 1, 1, 3, 12},
            {3, 1, 0, 0, 0, 0, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 0, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 0, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 0, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 0, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 0, 0, 0, 0, 1, 3},
            {3, 1, 0, 0, 0, 0, 0, 0, 0, 1, 3},
            {12, 3, 1, 1, 1, 1, 1, 1, 1, 3, 12},
            {30, 12, 3, 3, 3, 3, 3, 3, 3, 12, 30},
        };
    }
}