using Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class GameBoardSpawner : MonoBehaviour
    {
        [SerializeField] private Vector3 boardOffset;
        [SerializeField] private Vector2 tileOffset;
        [SerializeField] private GameObject tileWhitePrefab;
        [SerializeField] private GameObject tileDarkPrefab;
        [SerializeField] private GameObject tileKingPrefab;
        
        [SerializeField] private Vector3 pawnOffset;
        [SerializeField] private GameObject pawnAttackerPrefab;
        [SerializeField] private GameObject pawnDefenderPrefab;
        [SerializeField] private GameObject pawnKingPrefab;

        [SerializeField] private Transform tileRoot;
        [SerializeField] private Transform attackerPawnRoot;
        [SerializeField] private Transform defenderPawnRoot;

        [SerializeField] private Transform boardCenter;
        
        public void SpawnBoard(GameBoard board)
        {
            bool whiteTile = true;
            
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    Vector2Int tilePosition = new Vector2Int(i, j);
                    var tilePrefab = board.IsKingSquare(i, j) ? tileKingPrefab : whiteTile ? tileWhitePrefab : tileDarkPrefab;
                    Instantiate(tilePrefab, ToWorldPositionTile(tilePosition), Quaternion.identity, tileRoot);
                    whiteTile = !whiteTile;
                    
                    var pawnType = board.GetPawn(i, j);
                    if(pawnType == PawnType.Empty)
                        continue;
                    
                    var pawnPrefab = pawnType == PawnType.Attacker ? pawnAttackerPrefab : pawnType == PawnType.Defender ? pawnDefenderPrefab : pawnKingPrefab;
                    var root = pawnType == PawnType.Attacker ? attackerPawnRoot : defenderPawnRoot;
                    Instantiate(pawnPrefab, ToWorldPositionPawn(tilePosition), Quaternion.identity, root);
                }
            }
            
            boardCenter.position = ToWorldPositionTile(new Vector2Int(board.Size / 2, board.Size / 2));
        }
        
        public Vector3 ToWorldPositionTile(Vector2Int tilePosition)
        {
            return boardOffset + new Vector3(tilePosition.x * tileOffset.x, 0, tilePosition.y * tileOffset.y);
        }
        
        public Vector3 ToWorldPositionPawn(Vector2Int tilePosition)
        {
            return boardOffset + pawnOffset + new Vector3(tilePosition.x * tileOffset.x, 0, tilePosition.y * tileOffset.y);
        }
        
        public Vector2Int ToTilePosition(Vector3 worldPosition)
        {
            var relativePosition = worldPosition - boardOffset;
            var x = Mathf.RoundToInt(relativePosition.x / tileOffset.x);
            var y = Mathf.RoundToInt(relativePosition.z / tileOffset.y);
            return new Vector2Int(x, y);
        }

    }
}