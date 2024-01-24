using Core;
using Game.Players;

namespace Game.GameFactories
{
    public interface IGameFactory
    {
        void CreateGame(out GameSimulator simulator, out GameBoard board, out IPlayer[] players);
    }
}