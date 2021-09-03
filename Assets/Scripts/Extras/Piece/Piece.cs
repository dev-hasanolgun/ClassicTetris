using UnityEngine;

namespace devRHS.ClassicTetris.Piece
{
    public abstract class Piece
    {
        public abstract Vector3[] CellPositions { get; set; }
        public abstract Vector3 CenterPos { get; set; }

        public virtual void Orient(int direction)
        {
            for (int i = 0; i < CellPositions.Length; i++)
            {
                var pos = CellPositions[i] - CenterPos;
                var x = pos.x;
                var y = pos.y;
                CellPositions[i] = new Vector3(y, -x, 0) * direction + CenterPos;
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