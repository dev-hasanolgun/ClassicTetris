using System.Collections.Generic;
using UnityEngine;

namespace devRHS.ClassicTetris.TetrominoCreator
{
    public class TetrominoSpawner
    {
        public int[] TetrominoCounter;
        public Vector3 SpawnPosition = new Vector3(4, 19, 0);
        private TetrominoDatabase _tetrominoDatabase;
        public Tetromino NextPiece;
        public Tetromino CurrentPiece;

        public TetrominoSpawner(TetrominoDatabase tetrominoDatabase)
        {
            _tetrominoDatabase = tetrominoDatabase;
            TetrominoCounter = new int[tetrominoDatabase.Tetrominoes.Count];
        }

        public Tetromino GetRandomTetromino()
        {
            var randomIndex = Random.Range(0, _tetrominoDatabase.Tetrominoes.Count);
            var randomPiece = _tetrominoDatabase.Tetrominoes[randomIndex];
            TetrominoCounter[randomIndex]++;
            return randomPiece;
        }
        public Tetromino SpawnNextTetromino()
        {
            if (CurrentPiece == null)
            {
                var randomPiece = GetRandomTetromino();
                CurrentPiece = new Tetromino(new List<Orientation>(randomPiece.Orientations), randomPiece.TetrominoName);
                CurrentPiece.CellSprite = randomPiece.CellSprite;
            }
            else
            {
                CurrentPiece = NextPiece;
            }
            
            var positions = CurrentPiece.CellPositions;
            CurrentPiece.CenterPos += SpawnPosition;

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] += SpawnPosition;
            }

            var nextRandomPiece = GetRandomTetromino();
            NextPiece = new Tetromino(new List<Orientation>(nextRandomPiece.Orientations), nextRandomPiece.TetrominoName);
            NextPiece.CellSprite = nextRandomPiece.CellSprite;
            return CurrentPiece;
        }
    }
}
