using Core;
using NUnit.Framework;

namespace Tests.Core.Tests
{
    public class SimulatorTest
    {
        [Test]
        public void SimpleMoveTest()
        {
            GameBoard board = GameBoard.CreatGameBoard9Empty();
            board.SetPawn(4, 4, PawnType.King);
            board.SetPawn(4, 2, PawnType.Defender);
            board.SetPawn(2, 2, PawnType.Attacker);
            
            var simulator = new GameSimulator(board);
            
            simulator.SimulateMove(new PawnMove(2, 2, 2, 6));
            Assert.AreEqual(PawnType.Attacker, board.GetPawn(2, 6));
            Assert.AreEqual(PawnType.Empty, board.GetPawn(2, 2));
            
            simulator.SimulateMove(new PawnMove(4, 2, 4, 0));
            Assert.AreEqual(PawnType.Defender, board.GetPawn(4, 0));
            Assert.AreEqual(PawnType.Empty, board.GetPawn(4, 2));
            
            simulator.SimulateMove(new PawnMove(2, 6, 4, 6));
            Assert.AreEqual(PawnType.Attacker, board.GetPawn(4, 6));
            Assert.AreEqual(PawnType.Empty, board.GetPawn(2, 6));
            
            simulator.SimulateMove(new PawnMove(4, 4, 0, 4));
            Assert.AreEqual(PawnType.King, board.GetPawn(0, 4));
            Assert.AreEqual(PawnType.Empty, board.GetPawn(4, 4));
            
            simulator.SimulateMove(new PawnMove(4, 6, 4, 3));
            Assert.AreEqual(PawnType.Attacker, board.GetPawn(4, 3));
            Assert.AreEqual(PawnType.Empty, board.GetPawn(4, 6));
            
            simulator.SimulateMove(new PawnMove(0, 4, 0, 8));
            Assert.AreEqual(PawnType.King, board.GetPawn(0, 8));
        }

        [Test]
        public void MoveOrderTest()
        {
            GameBoard board = GameBoard.CreatGameBoard9Empty();
            board.SetPawn(4, 4, PawnType.King);
            board.SetPawn(4, 2, PawnType.Defender);
            board.SetPawn(2, 2, PawnType.Attacker);
            
            var simulator = new GameSimulator(board);
            
            Assert.AreEqual(PlayerType.Attacker, simulator.CurrentPlayer);
            simulator.SimulateMove(new PawnMove(2, 2, 2, 6));
            Assert.AreEqual(PlayerType.Defender, simulator.CurrentPlayer);
            simulator.SimulateMove(new PawnMove(4, 2, 4, 0));
            Assert.AreEqual(PlayerType.Attacker, simulator.CurrentPlayer);
            simulator.SimulateMove(new PawnMove(2, 6, 4, 6));
            //Break move order
            Assert.Catch(() => simulator.SimulateMove(new PawnMove(4, 6, 4, 5)));
        }

        [Test]
        public void MoveValidationTest()
        {
            GameBoard board = GameBoard.CreatGameBoard9Empty();
            board.SetPawn(5, 4, PawnType.King);
            board.SetPawn(4, 2, PawnType.Defender);
            board.SetPawn(2, 2, PawnType.Attacker);
            
            var simulator = new GameSimulator(board);
            
            //Move out the board
            Assert.Catch(() => simulator.SimulateMove(new PawnMove(2, 2, 2, -1)));
            Assert.Catch(() => simulator.SimulateMove(new PawnMove(2, 2, 2, 9)));
            Assert.Catch(() => simulator.SimulateMove(new PawnMove(2, 2, -1, 2)));
            Assert.Catch(() => simulator.SimulateMove(new PawnMove(2, 2, 9, 2)));
            
            //Move diagonally
            Assert.Catch(() => simulator.SimulateMove(new PawnMove(2, 2, 3, 3)));
            
            //Move empty square
            Assert.Catch(() => simulator.SimulateMove(new PawnMove(4, 3, 4, 4)));
            
            //Move to occupied square
            Assert.Catch(() => simulator.SimulateMove(new PawnMove(2, 2, 4, 2)));

            //Break clear path movement
            Assert.Catch(() => simulator.SimulateMove(new PawnMove(2, 2, 6, 2)));
            
            //Pass turn to defender
            simulator.SimulateMove(new PawnMove(2, 2, 2, 8));
            
            //Move to the king square
            Assert.Catch(() => simulator.SimulateMove(new PawnMove(4, 2, 4, 4)));
            simulator.SimulateMove(new PawnMove(5, 4, 4, 4));
            
        }
        

        [Test]
        public void CaptureTest()
        {
            var scenario1Board = SetupScenario1();
            var simulator = new GameSimulator(scenario1Board);
            
            int defenderCount = simulator.DefenderCount;
            int attackerCount = simulator.AttackerCount;
            
            simulator.SimulateMove(new PawnMove(4, 7, 4, 3));
            defenderCount -= 1;
            Assert.AreEqual(PawnType.Empty, scenario1Board.GetPawn(4, 2));
            CheckCount();

            simulator.SimulateMove(new PawnMove(6, 1, 5, 1));
            attackerCount -= 1;
            Assert.AreEqual(PawnType.Empty, scenario1Board.GetPawn(5, 2));
            CheckCount();

            simulator.SimulateMove(new PawnMove(2, 7, 2, 3));
            defenderCount -= 3;
            Assert.AreEqual(PawnType.Empty, scenario1Board.GetPawn(1, 3));
            Assert.AreEqual(PawnType.Empty, scenario1Board.GetPawn(2, 2));
            Assert.AreEqual(PawnType.Empty, scenario1Board.GetPawn(3, 3));
            CheckCount();
            
            simulator.SimulateMove(new PawnMove(7, 2, 4, 2));
            attackerCount -= 1;
            Assert.AreEqual(PawnType.Empty, scenario1Board.GetPawn(4, 3));
            CheckCount();

            void CheckCount()
            {
                Assert.AreEqual(defenderCount, simulator.DefenderCount);
                Assert.AreEqual(attackerCount, simulator.AttackerCount);
            }   
            
            GameBoard SetupScenario1()
            {
                var board = GameBoard.CreatGameBoard9Empty();
                board.SetPawn(6, 1, PawnType.King);
                board.SetPawn(1, 3, PawnType.Defender);
                board.SetPawn(2, 2, PawnType.Defender);
                board.SetPawn(3, 3, PawnType.Defender);
                board.SetPawn(4, 2, PawnType.Defender);
                board.SetPawn(5, 3, PawnType.Defender);
                board.SetPawn(7, 2, PawnType.Defender);
                
                board.SetPawn(0, 3, PawnType.Attacker);
                board.SetPawn(2, 1, PawnType.Attacker);
                board.SetPawn(2, 7, PawnType.Attacker);
                board.SetPawn(4, 1, PawnType.Attacker);
                board.SetPawn(4, 7, PawnType.Attacker);
                board.SetPawn(5, 2, PawnType.Attacker);
                return board;
            }
        }
    }
}