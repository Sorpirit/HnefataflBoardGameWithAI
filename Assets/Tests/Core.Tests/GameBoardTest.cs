using Core;
using NUnit.Framework;

namespace Tests.Core.Tests
{
    public class GameBoardTest
    {
        [Test]
        public void KingSquareTest()
        {
            var board9 = GameBoard.CreatGameBoard9Empty();
            
            Assert.IsTrue(board9.IsKingSquare(0, 0));
            Assert.IsTrue(board9.IsKingSquare(8, 0));
            Assert.IsTrue(board9.IsKingSquare(8, 8));
            Assert.IsTrue(board9.IsKingSquare(4, 4));
            
            Assert.IsFalse(board9.IsKingSquare(1, 8));
            Assert.IsFalse(board9.IsKingSquare(5, 5));
            
            var board11 = GameBoard.CreatGameBoard11Empty();
            Assert.IsTrue(board11.IsKingSquare(0, 10));
            Assert.IsTrue(board11.IsKingSquare(5, 5));
            Assert.IsFalse(board11.IsKingSquare(6, 6));
        }
        
        [Test]
        public void SetPawnTest()
        {
            var board9 = GameBoard.CreatGameBoard9Empty();
            
            Assert.IsTrue(board9.TrySetPawn(2, 3, PawnType.Attacker));
            Assert.IsTrue(board9.TrySetPawn(6, 8, PawnType.Defender));
            Assert.IsTrue(board9.TrySetPawn(8, 8, PawnType.King));

            Assert.AreEqual(PawnType.Attacker, board9.GetPawn(2, 3));
            Assert.AreEqual(PawnType.Defender, board9.GetPawn(6, 8));
            
            Assert.False(board9.TrySetPawn(2, 3, PawnType.Defender));
            Assert.False(board9.TrySetPawn(-1, 3, PawnType.Defender));
            Assert.False(board9.TrySetPawn(0, 0, PawnType.Defender));
            
            var board11 = GameBoard.CreatGameBoard11Empty();
            Assert.IsTrue(board11.TrySetPawn(9, 2, PawnType.Attacker));
            Assert.IsTrue(board11.TrySetPawn(5, 5, PawnType.King));
            Assert.IsFalse(board11.TrySetPawn(0, 0, PawnType.Defender));
        }

        [Test]
        public void BoardValidationTest()
        {
            var board9 = GameBoard.CreatGameBoard9Empty();
            
            //Move outside the board
            Assert.IsFalse(board9.TrySetPawn(-1, 0, PawnType.Attacker));
            Assert.IsFalse(board9.TrySetPawn(0, -1, PawnType.Attacker));
            Assert.IsFalse(board9.TrySetPawn(9, 0, PawnType.Attacker));
            Assert.IsFalse(board9.TrySetPawn(0, 9, PawnType.Attacker));
            
            var board11 = GameBoard.CreatGameBoard11Empty();
            Assert.IsFalse(board11.TrySetPawn(-1, 0, PawnType.Attacker));
            Assert.IsFalse(board11.TrySetPawn(0, -1, PawnType.Attacker));
            Assert.IsFalse(board11.TrySetPawn(11, 0, PawnType.Attacker));
            Assert.IsFalse(board11.TrySetPawn(0, 11, PawnType.Attacker));
            
            //Move to king square
            Assert.IsFalse(board9.TrySetPawn(0, 0, PawnType.Attacker));
            Assert.IsFalse(board9.TrySetPawn(8, 0, PawnType.Defender));
            Assert.IsFalse(board9.TrySetPawn(8, 8, PawnType.Defender));
            Assert.IsFalse(board9.TrySetPawn(4, 4, PawnType.Defender));
            
            Assert.IsFalse(board11.TrySetPawn(0, 10, PawnType.Defender));
            Assert.IsFalse(board11.TrySetPawn(5, 5, PawnType.Defender));
        }
        
        [Test]
        public void BoardCopyTest()
        {
            var board9 = GameBoard.CreatGameBoard9Empty();
            board9.SetPawn(2, 3, PawnType.Attacker);
            board9.SetPawn(6, 8, PawnType.Defender);
            board9.SetPawn(8, 8, PawnType.King);
            
            var board9Copy = new GameBoard(board9);
            Assert.AreEqual(PawnType.Attacker, board9Copy.GetPawn(2, 3));
            Assert.AreEqual(PawnType.Defender, board9Copy.GetPawn(6, 8));
            Assert.AreEqual(PawnType.King, board9Copy.GetPawn(8, 8));
            
            board9Copy.SetPawn(2, 3, PawnType.Empty);
            board9Copy.SetPawn(2, 3, PawnType.Defender);
            
            Assert.AreEqual(PawnType.Attacker, board9.GetPawn(2, 3));
            Assert.AreEqual(PawnType.Defender, board9Copy.GetPawn(2, 3));
        }
    }
}
