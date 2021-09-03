using devRHS.ClassicTetris.TetrominoCreator;
using UnityEngine;

public class MoveTetromino : ICommand
{
    private Tetromino _tetromino;
    private Vector3 _direction;

    public MoveTetromino(Tetromino tetromino, Vector3 direction)
    {
        _tetromino = tetromino;
        _direction = direction;
    }
    public void Move(Tetromino tetromino, Vector3 direction)
    {
        var positions = tetromino.CellPositions;
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] += direction;
        }

        tetromino.CenterPos += direction;
    }
    public void Execute()
    {
        Move(_tetromino, _direction);
    }

    public void Undo()
    {
        Move(_tetromino, -_direction);
    }
}
