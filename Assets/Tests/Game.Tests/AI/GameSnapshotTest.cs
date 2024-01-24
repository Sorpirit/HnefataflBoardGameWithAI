using System.Collections.Generic;
using Core;
using Game.AI;
using NUnit.Framework;
using Unity.PerformanceTesting;
using Utilities;

namespace Tests.Game.Tests.AI
{
    public class GameSnapshotTest
    {
        [Test]
        public void ReverseMoveTest()
        {
            var originalBoard = GameBoard.CreatGameBoard7Empty();
            originalBoard.SetPawn(3, 3, PawnType.King);
            originalBoard.SetPawn(3, 2, PawnType.Defender);
            originalBoard.SetPawn(3, 0, PawnType.Defender);
            originalBoard.SetPawn(5, 2, PawnType.Defender);
            
            originalBoard.SetPawn(5, 1, PawnType.Attacker);
            originalBoard.SetPawn(1, 0, PawnType.Attacker);
            originalBoard.SetPawn(1, 5, PawnType.Attacker);
            
            var simulator = new GameSimulator(originalBoard);
            simulator.SimulateMove(new PawnMove(1, 5, 1, 6));
            simulator.SimulateMove(new PawnMove(3, 3, 3, 4));
            
            int attackerCount = simulator.AttackerCount;
            int defenderCount = simulator.DefenderCount;
            
            List<PawnMove> moves = new List<PawnMove>()
            {
                new(5, 1, 3, 1),
                new(5, 2, 3, 2),
                new(1, 0, 1, 4),
                new(3, 4, 3, 6),
                new(1, 4, 3, 4),
                new(3, 6, 6, 6),
            };
            var copyBoard = new GameBoard(originalBoard);
            
            var simplePool = new SimpleObjectPool<List<PawnMove>>(() => new List<PawnMove>(), 100);
            using var token = simplePool.Borrow();
            
            
#pragma warning disable CS0618 // Type or member is obsolete
            var snapshot = GameSnapshot.CreateTestSnapshot(copyBoard, simulator, token, PlayerType.Attacker);
            GameSnapshot.TestLoop(snapshot);
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.True(originalBoard.Equals(copyBoard));
            
            Assert.AreEqual(PlayerType.Attacker, simulator.CurrentPlayer);
            Assert.AreEqual(2, simulator.MoveList.Count);
            
            Assert.AreEqual(attackerCount, simulator.AttackerCount);
            Assert.AreEqual(defenderCount, simulator.DefenderCount);
            
            Assert.IsNull(simulator.Winner);
            Assert.False(simulator.IsFinished);
        }

        [Test]
        public void ChildPositionGeneration()
        {
            var originalBoard = GameBoardFactory.CreatGameBoard7();
            var sim = new GameSimulator(originalBoard);
            sim.SimulateMove(new PawnMove(3, 0, 2, 0));
            
            var snapshot = GameSnapshot.CreateSnapshot(originalBoard, sim, PlayerType.Defender);
            var children = GameSnapshot.GetChildren(snapshot);
            
            Assert.AreEqual(23, children.Count);
        } 

        [Test]
        public void GlobalGameStateTest()
        {
            var originalBoard = GameBoard.CreatGameBoard7Empty();
            originalBoard.SetPawn(5, 4, PawnType.King);
            originalBoard.SetPawn(1, 0, PawnType.Defender);
            
            originalBoard.SetPawn(3, 0, PawnType.Attacker);
            originalBoard.SetPawn(0, 1, PawnType.Attacker);
            originalBoard.SetPawn(1, 1, PawnType.Attacker);
            originalBoard.SetPawn(2, 1, PawnType.Attacker);
            originalBoard.SetPawn(5, 2, PawnType.Attacker);
            originalBoard.SetPawn(1, 3, PawnType.Attacker);
            originalBoard.SetPawn(4, 3, PawnType.Attacker);
            originalBoard.SetPawn(6, 3, PawnType.Attacker);
            originalBoard.SetPawn(4, 4, PawnType.Attacker);
            originalBoard.SetPawn(6, 4, PawnType.Attacker);
            originalBoard.SetPawn(5, 5, PawnType.Attacker);
            
            var sim = new GameSimulator(originalBoard);
            sim.SimulateMove(new PawnMove(1, 3, 2, 3));
            
            var copyBoard = new GameBoard(originalBoard);
            var snapshot = GameSnapshot.CreateSnapshot(copyBoard, sim, PlayerType.Defender);
            var children = GameSnapshot.GetChildren(snapshot);
            
            Assert.AreEqual(2, children.Count);
            Assert.AreEqual(new PawnMove(1, 0, 2, 0), children[0].LastMove);
            Assert.AreEqual(new PawnMove(5, 4, 5, 3), children[1].LastMove);
            
#pragma warning disable CS0618 // Type or member is obsolete
            GameSnapshot.TestLoop(children[1]);
#pragma warning restore CS0618 // Type or member is obsolete
            
            Assert.True(originalBoard.Equals(copyBoard));
        }

        [Test, Performance]
        public void EvaluatePrefTest()
        {
            var gameBoard = GameBoardFactory.CreatGameBoard11();
            var gameSimulator = new GameSimulator(gameBoard);
            MinMax<GameSnapshot> minMax = new MinMax<GameSnapshot>(GameSnapshot.Evaluate, GameSnapshot.GetChildren);
            var rootSnapshot = GameSnapshot.CreateSnapshot(gameBoard, gameSimulator, PlayerType.Attacker);
            
            Measure.Method(() => { minMax.FindBestMove(rootSnapshot, 4, true); })
                .WarmupCount(5)
                .MeasurementCount(5)
                .IterationsPerMeasurement(3)
                .GC()
                .Run();
        }
    }
}