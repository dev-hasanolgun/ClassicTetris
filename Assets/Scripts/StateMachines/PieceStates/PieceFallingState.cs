using System.Collections.Generic;
using devRHS.ClassicTetris.StateMachines.UIStates;
using UnityEngine;

namespace devRHS.ClassicTetris.StateMachines.PieceStates
{
    public class PieceFallingState : IState<PieceStateMachine>
    {
        private PieceStateMachine _stateMachine;
        public PieceFallingState(PieceStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
    
        public void Tick()
        {
            var player = _stateMachine.Player;
            player.TetrominoController.ControlTetromino(player);
        }
        public void OnStateEnter()
        {
            var player = _stateMachine.Player;
            player.TetrominoController.CurrentTetromino = player.Spawner.SpawnNextTetromino();
            EventManager.TriggerEvent("UpdatingPieceCounters", new Dictionary<string, object>{{"player", player}});
            var piecePos = player.TetrominoController.CurrentTetromino.CellPositions;
            if (!player.GridManager.IsCellsAvailable(piecePos))
            {
                _stateMachine.GameManager.GameStateMachine.SetState(new GameOverState(_stateMachine.GameManager.GameStateMachine));
                PlayerPrefs.SetInt("LastScore", player.PlayerStats.CurrentScore);
                PlayerPrefs.SetInt("LastLevel", player.PlayerStats.Level);
            }
            
            EventManager.TriggerEvent("addingCellVisual", new Dictionary<string, object>{{"player", player}});
        }
        public void OnStateExit()
        {
        }
    }
}