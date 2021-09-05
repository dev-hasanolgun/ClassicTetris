using System.Collections.Generic;
using UnityEngine;

namespace ClassicTetris.TetrominoBase
{
    public class TetrominoSpawner
    {
        public readonly int[] TetrominoCounter;
        
        public Tetromino NextPiece;
        public Tetromino CurrentPiece;
        
        private readonly TetrominoDatabase _tetrominoDatabase;
        private readonly Vector3 _spawnPosition = new Vector3(4, 19, 0);

        public TetrominoSpawner(TetrominoDatabase tetrominoDatabase)
        {
            _tetrominoDatabase = tetrominoDatabase;
            TetrominoCounter = new int[tetrominoDatabase.Tetrominoes.Count];
        }
        
        public Tetromino SpawnNextTetromino()
        {
            if (CurrentPiece == null) // Spawn current piece since there isn't any next piece to assign into current piece
            {
                var randomPiece = GetRandomTetromino();
                CurrentPiece = new Tetromino(new List<Orientation>(randomPiece.Orientations), randomPiece.CellSprite, randomPiece.TetrominoName);
            }
            else
            {
                CurrentPiece = NextPiece; // Assign next piece into current piece
            }
            
            var positions = CurrentPiece.CellPositions;
            CurrentPiece.CenterPos += _spawnPosition;

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] += _spawnPosition;
            }
            
            // Spawn next piece
            var nextRandomPiece = GetRandomTetromino();
            NextPiece = new Tetromino(new List<Orientation>(nextRandomPiece.Orientations), nextRandomPiece.CellSprite, nextRandomPiece.TetrominoName);
            
            return CurrentPiece;
        }
        private Tetromino GetRandomTetromino()
        {
            var randomIndex = Random.Range(0, _tetrominoDatabase.Tetrominoes.Count);
            var randomPiece = _tetrominoDatabase.Tetrominoes[randomIndex];
            TetrominoCounter[randomIndex]++;
            
            return randomPiece;
        }
    }
}
