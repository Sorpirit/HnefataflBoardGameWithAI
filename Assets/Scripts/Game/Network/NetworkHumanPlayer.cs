using System;
using Core;

namespace Game.Players
{
    public class NetworkHumanPlayer : IPlayer
    {
        private IPlayer _parentPlayer;
        
        public event Action<PawnMove> OnMakeMove;
        
        public NetworkHumanPlayer(IPlayer parentPlayer)
        {
            _parentPlayer = parentPlayer;
        }

        public void Init(PlayerType type, MoveAssistant moveAssistant)
        {
            _parentPlayer?.Init(type, moveAssistant);   
        }

        public void StartTurn()
        {
            _parentPlayer?.StartTurn();
        }
        
        public void ExecuteMove(PawnMove move)
        {
            OnMakeMove?.Invoke(move);
        }
    }
}