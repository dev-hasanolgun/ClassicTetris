using System.Collections.Generic;
using ClassicTetris.Commands;
using ClassicTetris.StateMachines.PieceStates;
using ClassicTetris.TetrominoBase;
using UnityEngine;

public class TetrominoController
{
    public Tetromino CurrentTetromino;
    
    private readonly CommandHandler _commandHandler = new CommandHandler();
    
    private int _holdDownPoints;
    private float _shiftingFrames;
    private float _softDroppingFrames;
    private float _gravityTimer;
    private bool _isHoldingLeft;
    private bool _isHoldingRight;
    private bool _isHoldingDown;
    
    public void ControlTetromino(Player player)
    {
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
    public void Gravity(Player player)
    {
        if (!_isHoldingDown)
        {
            _gravityTimer += Time.deltaTime;

            if (_gravityTimer > player.LevelController.CurrentLevel.LevelSpeed)
            {
                if (!TryShifting(player, Vector3.down))
                {
                    LockTetromino(player);
                    player.PieceStateMachine.SetState(new PieceDroppedState(player.PieceStateMachine));
                }
                _gravityTimer = 0;
            }
        }
    }
    private void Shift(Player player) // Actual shifting algorithm in original game that made with assembly
    {
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) return;
        
        if (Input.GetKey(KeyCode.LeftArrow) && !_isHoldingLeft)
        {
            TryShifting(player, Vector3.left);
            
            _shiftingFrames = 0;
            _isHoldingLeft = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !_isHoldingRight)
        {
            TryShifting(player, Vector3.right);
            
            _shiftingFrames = 0;
            _isHoldingRight = true;
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
        
        if (_shiftingFrames < 0.23f) return;
        
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
    private void SoftDrop(Player player, Vector3 position) // Actual soft drop algorithm in original game that made with assembly
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
                LockTetromino(player);
                player.PieceStateMachine.SetState(new PieceDroppedState(player.PieceStateMachine));
            }
            _holdDownPoints = (int) (position.y - heldPos.y);
        }
    }
    private void LockTetromino(Player player) // Lock tetromino into the grid if tetromino is dropped
    {
        for (int i = 0; i < CurrentTetromino.CellPositions.Length; i++)
        {
            var x = (int) CurrentTetromino.CellPositions[i].x;
            var y = (int) CurrentTetromino.CellPositions[i].y;
            player.GridController.Grid.GridMap[x][y].IsCellEmpty = false;
        }
        player.PlayerStats.CurrentScore += _holdDownPoints;
        _isHoldingDown = false;
    }
    private bool TryShifting(Player player, Vector3 direction) // Try to shift tetromino and return if shifting was possible
    {
        ICommand shifting = new MoveTetromino(CurrentTetromino, direction);
        _commandHandler.AddCommand(shifting);

        var isShiftable = player.GridController.IsCellsAvailable(CurrentTetromino.CellPositions);
        if (!isShiftable)
        {
            _commandHandler.UndoCommand();
        }
        else
        {
            EventManager.TriggerEvent("OnTetrominoMovement", new Dictionary<string, object>{{"player", player}});
        }

        return isShiftable;
    }
    private bool TryOrienting(Player player, int direction) // Try to orienting tetromino and return if orienting was possible
    {
        ICommand orienting = new OrientTetromino(CurrentTetromino, direction);
        _commandHandler.AddCommand(orienting);

        var isOrientable = player.GridController.IsCellsAvailable(CurrentTetromino.CellPositions);
        if (!isOrientable)
        {
            _commandHandler.UndoCommand();
        }
        else
        {
            EventManager.TriggerEvent("OnTetrominoMovement", new Dictionary<string, object>{{"player", player}});
        }

        return isOrientable;
    }
}
