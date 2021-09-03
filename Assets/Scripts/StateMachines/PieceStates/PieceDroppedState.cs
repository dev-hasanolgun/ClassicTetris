using System.Collections.Generic;
using UnityEngine;

namespace devRHS.ClassicTetris.StateMachines.PieceStates
{
    public class PieceDroppedState : IState<PieceStateMachine>
    {
        private PieceStateMachine _stateMachine;
        public PieceDroppedState(PieceStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private List<int> fullLines;
        private bool _isLineClearing;
        private float _pieceDropDelay;
    
        public void Tick()
        {
            _pieceDropDelay += Time.deltaTime;
            if (_pieceDropDelay > 0.25f && !_isLineClearing)
            {
                _pieceDropDelay = 0;
                _stateMachine.SetState(new PieceFallingState(_stateMachine));
            }
            else if (_pieceDropDelay > 0.5f && _isLineClearing)
            {
                EventManager.TriggerEvent("updatingGridVisual", new Dictionary<string, object>{{"fullLines", fullLines}, {"player", _stateMachine.Player}});
                _pieceDropDelay = 0;
                _stateMachine.SetState(new PieceFallingState(_stateMachine));
                _isLineClearing = false;
            }
        }
        public void OnStateEnter()
        {
            var player = _stateMachine.Player;
            var piecePos = player.TetrominoController.CurrentTetromino.CellPositions;
            fullLines = new List<int>(player.GridManager.GetFullLines(piecePos));
        
            if (fullLines.Count != 0)
            {
                _isLineClearing = true;
                player.GridManager.ClearFullLines(piecePos);
                player.PlayerStats.ClearedLines += fullLines.Count;
                player.LevelManager.LevelUp(player);
                player.LevelManager.UpdateScore(player, fullLines.Count);

                EventManager.TriggerEvent("onLineClear", new Dictionary<string, object>{{"fullLines", fullLines}});
            }
            EventManager.TriggerEvent("UpdatingUI", new Dictionary<string, object>{{"player", player}});
        }
        public void OnStateExit()
        {
        }
    }
}
