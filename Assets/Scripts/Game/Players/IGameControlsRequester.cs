using Core;

namespace Game.Players
{
    public interface IGameControlsRequester
    {
        void ProvideControls(GameBoard gameBoard, GameSimulator gameSimulator);
    }
}