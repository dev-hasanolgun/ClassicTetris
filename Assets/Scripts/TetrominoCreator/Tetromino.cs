using System.Collections.Generic;
using UnityEngine;

namespace devRHS.ClassicTetris.TetrominoCreator
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
        
        public Tetromino(List<Orientation> orientations, string tetrominoName)
        {
            Orientations = new List<Orientation>(orientations);
            CellPositions = new Vector3[Orientations[0].Positions.Length];
            Orientations[0].Positions.CopyTo(CellPositions, 0);
            TetrominoName = tetrominoName;
        }

        public void Orient(int direction)
        {
            if (Orientations.Count >= 2) 
            {
                if (direction == 1)
                {
                    CurrentOrientation = CurrentOrientation + 1 >= Orientations.Count ? 0 : CurrentOrientation + 1;
                }
                else
                {
                    CurrentOrientation = CurrentOrientation - 1 < 0 ? Orientations.Count - 1 : CurrentOrientation - 1;
                }
                for (int i = 0; i < CellPositions.Length; i++)
                {
                    CellPositions[i] = Orientations[CurrentOrientation].Positions[i] + CenterPos;
                }
            }
            else
            {
                for (int i = 0; i < CellPositions.Length; i++)
                {
                    var pos = CellPositions[i] - CenterPos;
                    var x = pos.x;
                    var y = pos.y;
                    CellPositions[i] = new Vector3(y, -x, 0) * direction + CenterPos;
                }
            }
        }

        public void Move(Vector3 direction)
        {
            for (int i = 0; i < CellPositions.Length; i++)
            {
                CellPositions[i] += direction;
            }
        }
    }
}
