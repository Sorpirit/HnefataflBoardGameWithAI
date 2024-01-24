using Core;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "PlayerPrefabs", menuName = "CreatePlayerPrefabCollection", order = 0)]
    public class PlayerPrefabCollection : ScriptableObject
    {
        [SerializeField] private GameObject humanPlayerPrefab;
        [SerializeField] private GameObject aiPlayerPrefab;
        
        public GameObject GetPrefab(LocalPlayerType playerType)
        {
            switch (playerType)
            {
                case LocalPlayerType.LocalPlayer:
                    return humanPlayerPrefab;
                case LocalPlayerType.AI:
                    return aiPlayerPrefab;
                default:
                    return null;
            }
        }
    }
}