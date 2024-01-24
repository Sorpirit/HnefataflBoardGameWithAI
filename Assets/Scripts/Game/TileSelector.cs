using UnityEngine;

namespace Game
{
    public class TileSelector : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private LayerMask tileLayerMask;
        [SerializeField] private GameBoardSpawner gameBoardSpawner;

        public bool CastMouseTile(out Vector2Int tile)
        {
            var mousePos = Input.mousePosition;
            var ray = camera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out var hit, 100, tileLayerMask))
            {
                var tileObject = hit.collider.gameObject;
                tile = gameBoardSpawner.ToTilePosition(tileObject.transform.position);
                return true;
            }

            tile = Vector2Int.zero;
            return false;
        }
    }
}