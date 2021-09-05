using System.Collections.Generic;
using UnityEngine;

namespace ClassicTetris.StateMachines.PieceStates
{
    public class PieceDroppedState : IState<PieceStateMachine>
    {
        private readonly PieceStateMachine _stateMachine;
        
        private List<int> _fullLines;
        private bool _isLineClearing;
        private float _pieceDropDelay;
        
        public PieceDroppedState(PieceStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

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
                EventManager.TriggerEvent("updatingGridVisual", new Dictionary<string, object>{{"fullLines", _fullLines}, {"player", _stateMachine.Player}});
                _pieceDropDelay = 0;
                _stateMachine.SetState(new PieceFallingState(_stateMachine));
                _isLineClearing = false;
            }
        }
        public void OnStateEnter()
        {
            var player = _stateMachine.Player;
            var piecePos = player.TetrominoController.CurrentTetromino.CellPositions;

            player.PlayerStats.DroppedPieces++;
            
            if (player.GridController.TryClearFullLines(piecePos, out var fullLines))
            {
                _fullLines = fullLines;
                _isLineClearing = true;
                player.PlayerStats.ClearedLines += fullLines.Count;
                player.LevelController.LevelUp(player);
                player.LevelController.UpdateScore(player, fullLines.Count);

                EventManager.TriggerEvent("onLineClear", new Dictionary<string, object>{{"fullLines", fullLines}});
            }
            EventManager.TriggerEvent("UpdatingUI", new Dictionary<string, object>{{"player", player}});
        }
        public void OnStateExit()
        {
        }
    }
}
