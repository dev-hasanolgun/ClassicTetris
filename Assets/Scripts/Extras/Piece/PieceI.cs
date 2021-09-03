using UnityEngine;

namespace devRHS.ClassicTetris.Piece
{
    public class PieceI : Piece
    {
        public override Vector3[] CellPositions { get; set; } =
        {
            new Vector3(-2, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0)
        };

        public override Vector3 CenterPos { get; set; } = new Vector3(0, 0, 0);

        private int _orientation = 1;

        public override void Orient(int direction)
        {
            for (int i = 0; i < CellPositions.Length; i++)
            {
                var pos = CellPositions[i] - CenterPos;
                var x = pos.x;
                var y = pos.y;
                CellPositions[i] = new Vector3(y, -x, 0) * (direction * _orientation) + CenterPos;
            }

            _orientation *= -1;
        }
    }
}