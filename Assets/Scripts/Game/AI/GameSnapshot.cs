using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Utilities;

namespace Game.AI
{
    public class GameSnapshot : IDisposable
    {
        private readonly struct GameState : IDisposable
        {
            private readonly GameSnapshot _snapshot;

            public GameBoard GameBoard => _snapshot._globalGameBoard;
            public GameSimulator GameSimulator => _snapshot._globalGameSimulator;
            public MoveAssistant MoveAssistant => _snapshot._globalMoveAssistant;

            public GameState(GameSnapshot snapshot) : this()
            {
                _snapshot = snapshot;
                _snapshot.SimulateMoves();
            }

            public void Dispose()
            {
                _snapshot.Revers();
            }
        }

        public static GameBoard DebugGameBoard { get; set; }

        private static readonly SimpleObjectPool<List<PawnMove>> PawnMovePool = new(() => new List<PawnMove>(), 1600);

        public PawnMove LastMove => _moves.Value[^1];

        private readonly PlayerType _maximizingPlayer;
        private readonly PlayerType _simulatorStartingPlayer;

        private readonly GameBoard _originalGameBoard;
        
        private readonly GameBoard _globalGameBoard;
        private readonly GameSimulator _globalGameSimulator;
        private readonly MoveAssistant _globalMoveAssistant;

        private readonly GameSimulator _currentGame;
        
        private readonly PoolToken<List<PawnMove>> _moves;

        private GameSnapshot(
            GameBoard globalGameBoard,
            GameBoard originalGameBoard,
            GameSimulator globalGameSimulator, 
            MoveAssistant globalMoveAssistant,
            PoolToken<List<PawnMove>> moves,
            PlayerType maximizingPlayer, 
            PlayerType simulatorStartingPlayer, 
            GameSimulator currentGame)
        {
            _globalGameBoard = globalGameBoard;
            _originalGameBoard = originalGameBoard;
            _globalGameSimulator = globalGameSimulator;
            _globalMoveAssistant = globalMoveAssistant;
            _maximizingPlayer = maximizingPlayer;
            _simulatorStartingPlayer = simulatorStartingPlayer;
            _currentGame = currentGame;

            _moves = moves;
        }
        
        private GameSnapshot(GameSnapshot snapshot, PoolToken<List<PawnMove>> moves) : this(snapshot._globalGameBoard, 
            snapshot._originalGameBoard,
            snapshot._globalGameSimulator, 
            snapshot._globalMoveAssistant, 
            moves, 
            snapshot._maximizingPlayer,
            snapshot._simulatorStartingPlayer, 
            snapshot._currentGame) { }

        public static GameSnapshot CreateSnapshot(GameBoard globalGameBoard, GameSimulator simulator,
            PlayerType maximizingPlayer)
        {
            var gameBoard = new GameBoard(globalGameBoard);
            var gameSimulator = new GameSimulator(gameBoard, simulator.CurrentPlayer);
            var moveAssistant = new MoveAssistant(gameBoard, gameSimulator);
            var moveList = PawnMovePool.Borrow();
            moveList.Value.Clear();

            return new GameSnapshot(gameBoard, globalGameBoard, gameSimulator, moveAssistant, moveList, maximizingPlayer, simulator.CurrentPlayer,
                simulator);
        }

        private GameState InitState() => new(this);

        private void SimulateMoves()
        {
            foreach (var pawnMove in _moves.Value)
            {
                _globalGameSimulator.SimulateMove(pawnMove);
            }
        }

        private void Revers()
        {
            _globalGameBoard.CopyBoard(_originalGameBoard);
            _globalGameSimulator.ResetGame(_simulatorStartingPlayer);
            if (DebugGameBoard != null && !DebugGameBoard.Equals(_globalGameBoard))
                throw new ArgumentException("Debug game board expected to be equal to global game board");
        }

        public static List<GameSnapshot> GetChildren(GameSnapshot snapshot)
        {
            var children = new List<GameSnapshot>();

            using (var state = snapshot.InitState())
            {
                var validMoveList = PawnMovePool.Borrow();
                validMoveList.Value.Clear();
                state.MoveAssistant.GatPlayerValidMoves(state.GameSimulator.CurrentPlayer, validMoveList);

                foreach (var pawnMove in validMoveList.Value)
                {
                    var childMoveList = PawnMovePool.Borrow();
                    childMoveList.Value.Clear();
                    childMoveList.Value.AddRange(snapshot._moves.Value);
                    childMoveList.Value.Add(pawnMove);

                    var childSnapshot = new GameSnapshot(snapshot, childMoveList);
                    children.Add(childSnapshot);
                }
            }

            return children;
        }
        
        public static double Evaluate(GameSnapshot snapshot)
        {
            double resultScore = 0;
            
            using (var state = snapshot.InitState())
            {
                int moveNumber = state.GameSimulator.MoveList.Count + snapshot._currentGame.MoveList.Count;
                var maximizingPlayer = snapshot._maximizingPlayer;

                if (state.GameSimulator.IsFinished)
                {
                    var winner = state.GameSimulator.Winner;
                    if (winner == maximizingPlayer)
                        resultScore = int.MaxValue;
                    else if (winner == null)
                        resultScore += 2;
                    else
                        resultScore = int.MinValue;

                    return resultScore;
                }

                // Evaluate based on piece count
                int attackerCount = state.GameBoard.AttackerCount;
                int defenderCount = state.GameBoard.DefenderCount;

                if (maximizingPlayer == PlayerType.Attacker)
                    defenderCount *= -1;
                else
                    attackerCount *= -1;

                // Adjust the weights based on the importance of each factor
                resultScore += 2 * (attackerCount + defenderCount);
                
                // Evaluate based on king position
                resultScore += state.GameBoard.GetKingScore(maximizingPlayer);
                
                if (moveNumber < 8)
                {
                    for (int x = 0; x < state.GameBoard.Size; x++)
                    {
                        for (int y = 0; y < state.GameBoard.Size; y++)
                        {
                            resultScore += state.GameBoard.GetScore(x, y, maximizingPlayer) * (8 - moveNumber) / 8.0d;
                        }
                    }
                }
                
                var kingPosition = state.GameBoard.KingPosition;
                int kingSurrounding = 0;
                if (state.GameSimulator.IsHostileCell(kingPosition.x + 1, kingPosition.y, PlayerType.Attacker,
                        false))
                    kingSurrounding++;

                if (state.GameSimulator.IsHostileCell(kingPosition.x - 1, kingPosition.y, PlayerType.Attacker,
                        false))
                    kingSurrounding++;

                if (state.GameSimulator.IsHostileCell(kingPosition.x, kingPosition.y + 1, PlayerType.Attacker,
                        false))
                    kingSurrounding++;

                if (state.GameSimulator.IsHostileCell(kingPosition.x, kingPosition.y - 1, PlayerType.Attacker,
                        false))
                    kingSurrounding++;

                int kingSurroundingMultiplier = 1;
                if (maximizingPlayer == PlayerType.Defender)
                    kingSurroundingMultiplier = -4;
                else
                    kingSurroundingMultiplier = 3;

                resultScore += kingSurrounding * kingSurroundingMultiplier;
            }
            return resultScore;
        }

        public static double EvaluateSimple(GameSnapshot snapshot)
        {
            double resultScore = 0;

            using (var state = snapshot.InitState())
            {
                var maximizingPlayer = snapshot._maximizingPlayer;

                if (state.GameSimulator.IsFinished)
                {
                    var winner = state.GameSimulator.Winner;
                    if (winner == maximizingPlayer)
                        resultScore = int.MaxValue;
                    else if (winner == null)
                        resultScore += 2;
                    else
                        resultScore = int.MinValue;

                    return resultScore;
                }

                // Evaluate based on piece count
                int attackerCount = state.GameBoard.AttackerCount;
                int defenderCount = state.GameBoard.DefenderCount;

                if (maximizingPlayer == PlayerType.Attacker)
                    defenderCount *= -1;
                else
                    attackerCount *= -1;

                // Adjust the weights based on the importance of each factor
                resultScore += 2 * (attackerCount + defenderCount);

                // Evaluate based on king position
                resultScore += state.GameBoard.GetKingScore(maximizingPlayer);
            }

            return resultScore;
        }

        [Obsolete("For testing purposes only")]
        public static GameSnapshot CreateTestSnapshot(GameBoard globalGameBoard, GameSimulator simulator,
            PoolToken<List<PawnMove>> moves, PlayerType maximizingPlayer)
        {
            var gameSimulator = new GameSimulator(globalGameBoard, simulator.CurrentPlayer);
            var moveAssistant = new MoveAssistant(globalGameBoard, gameSimulator);
            return new GameSnapshot(globalGameBoard, globalGameBoard, gameSimulator, moveAssistant, moves, maximizingPlayer, simulator.CurrentPlayer,
                simulator);
        }

        [Obsolete("For testing purposes only")]
        public static void TestLoop(GameSnapshot snapshot)
        {
            using var state = snapshot.InitState();
        }

        public void Dispose()
        {
        }
    }
}