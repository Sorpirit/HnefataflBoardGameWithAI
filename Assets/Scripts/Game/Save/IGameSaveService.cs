using Core;
using Game.Players;

namespace Game.Save
{
    public interface IGameSaveService
    {
        void SaveGame(GameSimulator simulator, string overrideName = null);
    }
}