using UnityEngine;

namespace ClassicTetris.Piece
{
    public class PieceT : Piece
    {
        public override Vector3[] CellPositions { get; set; } =
        {
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, -1, 0)
        };

        public override Vector3 CenterPos { get; set; } = new Vector3(0, 0, 0);
    }
}