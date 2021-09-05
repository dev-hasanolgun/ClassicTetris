using System.Collections.Generic;
using UnityEngine;

namespace ClassicTetris.TetrominoBase
{
    [System.Serializable]
    public class Orientation
    {
        public Vector3[] Positions;

        public Orientation(Vector3[] positions)
        {
            Positions = positions;
        }
    }
    [System.Serializable]
    public class Tetromino
    {
        public List<Orientation> Orientations;
        public Sprite CellSprite;
        public Vector3[] CellPositions;
        public Vector3 CenterPos = Vector3.zero;
        public int CurrentOrientation;
        public bool Orientable;
        public string TetrominoName;
        
        public Tetromino(List<Orientation> orientations, Sprite cellSprite, string tetrominoName)
        {
            Orientations = new List<Orientation>(orientations);
            CellPositions = new Vector3[Orientations[0].Positions.Length];
            Orientations[0].Positions.CopyTo(CellPositions, 0);
            CellSprite = cellSprite;
            TetrominoName = tetrominoName;
        }
    }
}
