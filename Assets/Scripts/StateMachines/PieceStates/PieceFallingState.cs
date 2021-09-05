using System.Collections.Generic;
using ClassicTetris.StateMachines.GameStates;
using UnityEngine;

namespace ClassicTetris.StateMachines.PieceStates
{
    public class PieceFallingState : IState<PieceStateMachine>
    {
        private readonly PieceStateMachine _stateMachine;
        private float _initialDelay;
        private bool _isDownPressed;

        public PieceFallingState(PieceStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
    
        public void Tick()
        {
            var player = _stateMachine.Player;
            
            // Delay for initial tetromino gravity
            if (player.PlayerStats.DroppedPieces > 0 || _isDownPressed || _initialDelay > 2f)
            {
                player.TetrominoController.Gravity(player);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _isDownPressed = true;
            }
            else
            {
                _initialDelay += Time.deltaTime;
            }
            
            player.TetrominoController.ControlTetromino(player);
        }
        public void OnStateEnter()
        {
            var player = _stateMachine.Player;
            
            player.TetrominoController.CurrentTetromino = player.Spawner.SpawnNextTetromino();
            var piecePos = player.TetrominoController.CurrentTetromino.CellPositions;
            
            EventManager.TriggerEvent("OnPieceSpawn", new Dictionary<string, object>{{"player", player}});
            EventManager.TriggerEvent("OnTetrominoSpawn", new Dictionary<string, object>{{"player", player}});
            
            // If spawned tetromino position is full in grid, game is over
            if (!player.GridController.IsCellsAvailable(piecePos))
            {
                GameManager.Instance.GameStateMachine.SetState(new GameOverState(GameManager.Instance.GameStateMachine));
                PlayerPrefs.SetInt("LastScore", player.PlayerStats.CurrentScore);
                PlayerPrefs.SetInt("LastLevel", player.PlayerStats.Level);
            }
        }
        public void OnStateExit()
        {
        }
    }
}