using System;
using Core;

namespace Game.Players
{
    public class DummyPlayer : IPlayer
    {
        public event Action<PawnMove> OnMakeMove;
        
        public void Init(PlayerType type, MoveAssistant moveAssistant) { }

        public void StartTurn() { }
    }
}