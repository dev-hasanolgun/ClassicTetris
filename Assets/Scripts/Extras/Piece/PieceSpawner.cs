using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace devRHS.ClassicTetris.Piece
{
    public class PieceSpawner
    {
        private readonly Vector3 _spawnPosition = new Vector3(4, 19, 0);
        private readonly Func<Piece>[] _tetrominoShapes = 
        {
            () => new PieceT(),
            () => new PieceI()
        };

        public Piece SpawnRandomTetromino()
        {
            var piece = _tetrominoShapes[Random.Range(0,_tetrominoShapes.Length)]();
            var positions = piece.CellPositions;
            piece.CenterPos += _spawnPosition;
            
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] += _spawnPosition;
            }
            return piece;
        }
    }
}
