using ClassicTetris.TetrominoBase;
using UnityEngine;

namespace ClassicTetris.Commands
{
    public class OrientTetromino : ICommand
    {
        private readonly Tetromino _tetromino;
        private readonly int _direction;
    
        public OrientTetromino(Tetromino tetromino, int direction)
        {
            _tetromino = tetromino;
            _direction = direction;
        }
        public void Execute()
        {
            Orient(_tetromino,_direction);
        }

        public void Undo()
        {
            Orient(_tetromino,-_direction);
        }
        private static void Orient(Tetromino tetromino, int direction) // Change orientation of the tetromino to the previous or the next one
        {
            if (tetromino.Orientations.Count >= 2) // Change orientation from the orientation list
            {
                if (direction == 1)
                {
                    tetromino.CurrentOrientation = tetromino.CurrentOrientation + 1 >= tetromino.Orientations.Count ? 0 : tetromino.CurrentOrientation + 1;
                }
                else
                {
                    tetromino.CurrentOrientation = tetromino.CurrentOrientation - 1 < 0 ? tetromino.Orientations.Count - 1 : tetromino.CurrentOrientation - 1;
                }
                for (int i = 0; i < tetromino.CellPositions.Length; i++)
                {
                    tetromino.CellPositions[i] = tetromino.Orientations[tetromino.CurrentOrientation].Positions[i] + tetromino.CenterPos;
                }
            }
            else // Change orientation just by the center of the piece if there is no orientation
            {
                for (int i = 0; i < tetromino.CellPositions.Length; i++)
                {
                    var pos = tetromino.CellPositions[i] - tetromino.CenterPos;
                    var x = pos.x;
                    var y = pos.y;
                    tetromino.CellPositions[i] = new Vector3(y, -x, 0) * direction + tetromino.CenterPos;
                }
            }
        }
    }
}
