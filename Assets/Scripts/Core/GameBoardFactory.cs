namespace Core
{
    public static class GameBoardFactory
    {
        public static GameBoard CreatGameBoard7()
        {
            var board = GameBoard.CreatGameBoard7Empty();
            
            //Setup attackers
            board.SetPawn(3, 0, PawnType.Attacker);
            board.SetPawn(3, 1, PawnType.Attacker);
            board.SetPawn(0, 3, PawnType.Attacker);
            board.SetPawn(1, 3, PawnType.Attacker);
            board.SetPawn(3, 5, PawnType.Attacker);
            board.SetPawn(3, 6, PawnType.Attacker);
            board.SetPawn(5, 3, PawnType.Attacker);
            board.SetPawn(6, 3, PawnType.Attacker);
            
            //Setup defenders
            board.SetPawn(3, 3, PawnType.King);
            board.SetPawn(2, 3, PawnType.Defender);
            board.SetPawn(4, 3, PawnType.Defender);
            board.SetPawn(3, 2, PawnType.Defender);
            board.SetPawn(3, 4, PawnType.Defender);
            
            return board;
        }
        
        public static GameBoard CreatGameBoard9()
        {
            var board = GameBoard.CreatGameBoard9Empty();
            
            //Setup attackers
            board.SetPawn(3, 0, PawnType.Attacker);
            board.SetPawn(4, 0, PawnType.Attacker);
            board.SetPawn(5, 0, PawnType.Attacker);
            board.SetPawn(4, 1, PawnType.Attacker);
            
            board.SetPawn(3, 8, PawnType.Attacker);
            board.SetPawn(4, 8, PawnType.Attacker);
            board.SetPawn(5, 8, PawnType.Attacker);
            board.SetPawn(4, 7, PawnType.Attacker);
            
            board.SetPawn(0, 3, PawnType.Attacker);
            board.SetPawn(0, 4, PawnType.Attacker);
            board.SetPawn(0, 5, PawnType.Attacker);
            board.SetPawn(1, 4, PawnType.Attacker);
            
            board.SetPawn(8, 3, PawnType.Attacker);
            board.SetPawn(8, 4, PawnType.Attacker);
            board.SetPawn(8, 5, PawnType.Attacker);
            board.SetPawn(7, 4, PawnType.Attacker);
            
            //Setup defenders
            board.SetPawn(4, 4, PawnType.King);

            board.SetPawn(4, 2, PawnType.Defender);
            board.SetPawn(4, 3, PawnType.Defender);
            board.SetPawn(4, 5, PawnType.Defender);
            board.SetPawn(4, 6, PawnType.Defender);
            
            board.SetPawn(2, 4, PawnType.Defender);
            board.SetPawn(3, 4, PawnType.Defender);
            board.SetPawn(5, 4, PawnType.Defender);
            board.SetPawn(6, 4, PawnType.Defender);
            
            return board;
        }
        
        public static GameBoard CreatGameBoard11()
        {
            var board = GameBoard.CreatGameBoard11Empty();
            
            //Setup attackers
            board.SetPawn(3, 0, PawnType.Attacker);
            board.SetPawn(4, 0, PawnType.Attacker);
            board.SetPawn(5, 0, PawnType.Attacker);
            board.SetPawn(6, 0, PawnType.Attacker);
            board.SetPawn(7, 0, PawnType.Attacker);
            board.SetPawn(5, 1, PawnType.Attacker);
            
            board.SetPawn(3, 10, PawnType.Attacker);
            board.SetPawn(4, 10, PawnType.Attacker);
            board.SetPawn(5, 10, PawnType.Attacker);
            board.SetPawn(6, 10, PawnType.Attacker);
            board.SetPawn(7, 10, PawnType.Attacker);
            board.SetPawn(5, 9, PawnType.Attacker);
            
            board.SetPawn(0, 3, PawnType.Attacker);
            board.SetPawn(0, 4, PawnType.Attacker);
            board.SetPawn(0, 5, PawnType.Attacker);
            board.SetPawn(0, 6, PawnType.Attacker);
            board.SetPawn(0, 7, PawnType.Attacker);
            board.SetPawn(1, 5, PawnType.Attacker);
            
            board.SetPawn(10, 3, PawnType.Attacker);
            board.SetPawn(10, 4, PawnType.Attacker);
            board.SetPawn(10, 5, PawnType.Attacker);
            board.SetPawn(10, 6, PawnType.Attacker);
            board.SetPawn(10, 7, PawnType.Attacker);
            board.SetPawn(9, 5, PawnType.Attacker);
            
            //Setup defenders
            board.SetPawn(5, 5, PawnType.King);

            board.SetPawn(5, 3, PawnType.Defender);
            board.SetPawn(5, 4, PawnType.Defender);
            board.SetPawn(5, 6, PawnType.Defender);
            board.SetPawn(5, 7, PawnType.Defender);
            
            board.SetPawn(3, 5, PawnType.Defender);
            board.SetPawn(4, 5, PawnType.Defender);
            board.SetPawn(6, 5, PawnType.Defender);
            board.SetPawn(7, 5, PawnType.Defender);
            
            board.SetPawn(4, 4, PawnType.Defender);
            board.SetPawn(4, 6, PawnType.Defender);
            board.SetPawn(6, 4, PawnType.Defender);
            board.SetPawn(6, 6, PawnType.Defender);
            
            return board;
        }
    }
}