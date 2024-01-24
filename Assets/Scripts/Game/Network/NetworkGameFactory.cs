using Core;
using Game.GameFactories;
using Game.Players;

namespace Game
{
    public class NetworkGameFactory : IGameFactory
    {
        private IPlayer[] _players;
        private GameMode _mode;

        public NetworkGameFactory(GameMode mode, IPlayer attacker, IPlayer defender)
        {
            _players = new IPlayer[2]
            {
                attacker,
                defender
            };
        }

        public void CreateGame(out GameSimulator simulator, out GameBoard board, out IPlayer[] players)
        {
            board = _mode == GameMode.Board9 ? GameBoardFactory.CreatGameBoard9() : GameBoardFactory.CreatGameBoard11();
            simulator = new GameSimulator(board);
            players = _players;
        }
    }
}