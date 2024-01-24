using System;

namespace Game
{
    [Serializable]
    public struct LocalGameSettings
    {
        [NonSerialized]
        public readonly string GameSaveName;
        public readonly GameMode GameMode;
        public readonly LocalPlayerType Attacker;
        public readonly LocalPlayerType Defender;
        
        public AIDifficulty AttackerAIDifficulty => Attacker == LocalPlayerType.AI ? _attackerAIDifficulty : 
            throw new ArgumentException("Attacker is not an ai");
        
        public AIDifficulty DefenderAIDifficulty => Defender == LocalPlayerType.AI ? _defenderAIDifficulty : 
            throw new ArgumentException("Defender is not an ai");
        
        private readonly AIDifficulty _attackerAIDifficulty;
        private readonly AIDifficulty _defenderAIDifficulty;

        public LocalGameSettings(GameMode gameMode, LocalPlayerType attacker, LocalPlayerType defender, 
            AIDifficulty attackerAIDifficulty = AIDifficulty.Easy, 
            AIDifficulty defenderAIDifficulty = AIDifficulty.Easy)
        {
            GameMode = gameMode;
            GameSaveName = null;
            Attacker = attacker;
            Defender = defender;
            _attackerAIDifficulty = attackerAIDifficulty;
            _defenderAIDifficulty = defenderAIDifficulty;
        }

        public LocalGameSettings(string gameSaveName) : this()
        {
            GameSaveName = gameSaveName;
        }
        
        public LocalGameSettings(string gameSaveName, LocalGameSettings settings) : this(settings.GameMode, settings.Attacker, settings.Defender, settings._attackerAIDifficulty, settings._defenderAIDifficulty)
        {
            GameSaveName = gameSaveName;
        }
    }
}