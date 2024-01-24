using System;
using Core;

namespace Game.Players
{
    public interface IPlayer
    {
        void Init(PlayerType type, MoveAssistant moveAssistant);
        void StartTurn();
        event Action<PawnMove> OnMakeMove;
    }
}