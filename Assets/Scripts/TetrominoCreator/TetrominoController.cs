using System.Collections.Generic;
using devRHS.ClassicTetris.StateMachines.PieceStates;
using devRHS.ClassicTetris.TetrominoCreator;
using UnityEngine;

public class TetrominoController
{
    public Tetromino CurrentTetromino;
    
    private CommandHandler _commandHandler = new CommandHandler();
    private int _holdDownPoints;
    private float _shiftingFrames;
    private float _softDroppingFrames;
    private float _gravityTimer;
    private bool _isHoldingLeft;
    private bool _isHoldingRight;
    private bool _isHoldingDown;
    
    public void ControlTetromino(Player player)
    {
        Gravity(player);
        Shift(player);
        SoftDrop(player, CurrentTetromino.CenterPos);
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            TryOrienting(player, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            TryOrienting(player, -1);
        }
    }
    private void Shift(Player player)
    {
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
        {
            return;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && !_isHoldingLeft)
        {
            _shiftingFrames = 0;
            _isHoldingLeft = true;
            
            if (!TryShifting(player, Vector3.left))
            {
                _isHoldingLeft = false;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !_isHoldingRight)
        {
            _shiftingFrames = 0;
            _isHoldingRight = true;
            
            if (!TryShifting(player, Vector3.right))
            {
                _isHoldingRight = false;
            }
        }
        if (!Input.GetKey(KeyCode.LeftArrow))
        {
            _isHoldingLeft = false;
        }
        if (!Input.GetKey(KeyCode.RightArrow))
        {
            _isHoldingRight = false;
        }

        _shiftingFrames += Time.deltaTime;
        if (_shiftingFrames < 0.23f)
        {
            return;
        }
        _shiftingFrames = 0.14f;
        if (_isHoldingLeft)
        {
            TryShifting(player, Vector3.left);
        }
        else if (_isHoldingRight)
        {
            TryShifting(player, Vector3.right);
        }
    }

    private bool TryShifting(Player player, Vector3 direction)
    {
        ICommand shifting = new MoveTetromino(CurrentTetromino, direction);
        _commandHandler.AddCommand(shifting);

        var isShiftable = player.GridManager.IsCellsAvailable(CurrentTetromino.CellPositions);
        if (!isShiftable)
        {
            _commandHandler.UndoCommand();
        }
        else
        {
            EventManager.TriggerEvent("updatingCellVisual", new Dictionary<string, object>{{"player", player}});
        }

        return isShiftable;
    }
    private bool TryOrienting(Player player, int direction)
    {
        ICommand orienting = new OrientTetromino(CurrentTetromino, direction);
        _commandHandler.AddCommand(orienting);

        var isOrientable = player.GridManager.IsCellsAvailable(CurrentTetromino.CellPositions);
        if (!isOrientable)
        {
            _commandHandler.UndoCommand();
        }
        else
        {
            EventManager.TriggerEvent("updatingCellVisual", new Dictionary<string, object>{{"player", player}});
        }

        return isOrientable;
    }
    private void SoftDrop(Player player, Vector3 position)
    {
        var heldPos = new Vector3();
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            _isHoldingDown = false;
            _holdDownPoints = 0;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !_isHoldingDown)
        {
            heldPos = position;
            _isHoldingDown = true;
        }
        if (!Input.GetKey(KeyCode.DownArrow))
        {
            _isHoldingDown = false;
            _holdDownPoints = 0;
        }
        _softDroppingFrames += Time.deltaTime;
        if (_softDroppingFrames < 0.033f)
        {
            return;
        }

        _softDroppingFrames = 0;
        if (_isHoldingDown)
        {
            if (!TryShifting(player, Vector3.down))
            {
                LockPiece(player);
                player.PieceStateMachine.SetState(new PieceDroppedState(player.PieceStateMachine));
            }
            _holdDownPoints = (int) (position.y - heldPos.y);
        }
    }
    private void Gravity(Player player)
    {
        if (!_isHoldingDown)
        {
            _gravityTimer += Time.deltaTime;

            if (_gravityTimer > player.LevelManager.CurrentLevel.LevelSpeed)
            {
                if (!TryShifting(player, Vector3.down))
                {
                    LockPiece(player);
                    player.PieceStateMachine.SetState(new PieceDroppedState(player.PieceStateMachine));
                }
                _gravityTimer = 0;
            }
        }
    }
    private void LockPiece(Player player)
    {
        for (int i = 0; i < CurrentTetromino.CellPositions.Length; i++)
        {
            var x = (int) CurrentTetromino.CellPositions[i].x;
            var y = (int) CurrentTetromino.CellPositions[i].y;
            player.GridManager.GridMap[x][y].IsCellEmpty = false;
        }
        player.PlayerStats.CurrentScore += _holdDownPoints;
        _isHoldingDown = false;
    }
}
