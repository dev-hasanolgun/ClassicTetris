using ClassicTetris.TetrominoBase;
using UnityEngine;

namespace ClassicTetris.Commands
{
    public class MoveTetromino : ICommand
    {
        private readonly Tetromino _tetromino;
        private readonly Vector3 _direction;

        public MoveTetromino(Tetromino tetromino, Vector3 direction)
        {
            _tetromino = tetromino;
            _direction = direction;
        }
        public void Execute()
        {
            Move(_tetromino, _direction);
        }

        public void Undo()
        {
            Move(_tetromino, -_direction);
        }
        private static void Move(Tetromino tetromino, Vector3 direction) // Move every cell block of the tetromino
        {
            var positions = tetromino.CellPositions;
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] += direction;
            }

            tetromino.CenterPos += direction;
        }
    }
}
