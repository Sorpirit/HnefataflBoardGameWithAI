using System;
using System.Collections.Generic;
using Core;
using Game;
using UnityEngine;

namespace Visuals
{
    public class PawnUIMover : MonoBehaviour
    {
        [SerializeField] private LayerMask pawnLayerMask;
        [SerializeField] private GameBoardSpawner gameBoardSpawner;
        [SerializeField] private GameController gameController;

        private Collider[] _collidersCached = new Collider[1];

        private void Start()
        {
            gameController.OnPawnMoved += MovePawn;
            gameController.OnPawnsCaptured += RemovePawn;
        }

        public void MovePawn(PawnMove pawnMove)
        {
            var startTile = new Vector2Int(pawnMove.FromX, pawnMove.FromY);
            var finishTile = new Vector2Int(pawnMove.ToX, pawnMove.ToY);

            var pawn = GetPawn(startTile);
            if (pawn == null)
                return;

            pawn.transform.position = gameBoardSpawner.ToWorldPositionPawn(finishTile);
        }
        
        public void RemovePawn(List<(int x, int y)> captures)
        {
            foreach ((int x, int y) in captures)
            {
                var pawn = GetPawn(new Vector2Int(x, y));
                Destroy(pawn);
            }
        }
        
        private GameObject GetPawn(Vector2Int tile)
        {
            var worldPos = gameBoardSpawner.ToWorldPositionPawn(tile);
            var colliders = Physics.OverlapSphereNonAlloc(worldPos, 0.1f, _collidersCached, pawnLayerMask);
            if (colliders == 0)
                throw new ArgumentException("No pawn found on tile");

            return _collidersCached[0].gameObject;
        }
    }
}