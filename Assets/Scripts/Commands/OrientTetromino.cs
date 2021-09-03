using devRHS.ClassicTetris.TetrominoCreator;
using UnityEngine;

public class OrientTetromino : ICommand
{
    private Tetromino _tetromino;
    private int _direction;
    
    public OrientTetromino(Tetromino tetromino, int direction)
    {
        _tetromino = tetromino;
        _direction = direction;
    }
    public void Orient(Tetromino tetromino, int direction)
    {
        if (tetromino.Orientations.Count >= 2) 
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
        else
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
    public void Execute()
    {
        Orient(_tetromino,_direction);
    }

    public void Undo()
    {
        Orient(_tetromino,-_direction);
    }
}
