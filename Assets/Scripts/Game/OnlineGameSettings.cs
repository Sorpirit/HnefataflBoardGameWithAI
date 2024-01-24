using Core;
using Unity.Netcode;
using UnityEngine;

namespace Game
{
    public struct OnlineGameSettings
    {
        public readonly GameMode GameMode;
        public readonly PlayerType HostType;
        
        public OnlineGameSettings(GameMode gameMode, PlayerType? hostType)
        {
            GameMode = gameMode;
            HostType = hostType ?? (PlayerType) Random.Range(0, 2);
        }
    }

    public struct OnlineGameSettingsRPC : INetworkSerializable
    {
        private int GameMode;
        private int HostType;
        
        public OnlineGameSettingsRPC(OnlineGameSettings settings)
        {
            GameMode = (int) settings.GameMode;
            HostType = (int) settings.HostType;
        }
        
        public static implicit operator OnlineGameSettings(OnlineGameSettingsRPC settings) => new((GameMode) settings.GameMode, (PlayerType) settings.HostType); 
        
        public static explicit operator OnlineGameSettingsRPC(OnlineGameSettings settings) => new(settings);
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref GameMode);
            serializer.SerializeValue(ref HostType);
        }
    }
}