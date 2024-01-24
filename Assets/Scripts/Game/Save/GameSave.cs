using System;
using Core;
using ProtoBuf;

namespace Game.Save
{
    [Serializable]
    [ProtoContract]
    public class GameSave
    {
        [ProtoMember(1)]
        private readonly int _gameMode;
        [ProtoMember(2)]
        private readonly int _attacker;
        [ProtoMember(3)]
        private readonly int _defender;
        [ProtoMember(4)]
        public readonly PawnMove[] MoveList;

        /// <summary>
        /// Constructor for protobuf-net
        /// </summary>
        private GameSave() { }
        
        public GameSave(PawnMove[] moveList, LocalGameSettings localGameSettings)
        {
            MoveList = moveList;
            _gameMode = (int) localGameSettings.GameMode;
            _attacker = (int) localGameSettings.Attacker;
            _defender = (int) localGameSettings.Defender;
        }
        
        public LocalGameSettings GetLocalGameSettings() =>
            new((GameMode)_gameMode, (LocalPlayerType)_attacker, (LocalPlayerType)_defender);
        
        
    }
}