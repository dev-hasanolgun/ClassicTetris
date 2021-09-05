using System.Collections.Generic;
using UnityEngine;

namespace ClassicTetris.StateMachines.PieceStates
{
    public class PieceDroppedState : IState<PieceStateMachine>
    {
        private readonly PieceStateMachine _stateMachine;
        
        private List<int> _fullLines;
        private bool _isLineCleared;
        private bool _isLeveledUp;
        private float _pieceDropDelay;
        
        public PieceDroppedState(PieceStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Tick()
        {
            _pieceDropDelay += Time.deltaTime;
            if (_pieceDropDelay > 0.25f && !_isLineCleared)
            {
                _pieceDropDelay = 0;
                _stateMachine.SetState(new PieceFallingState(_stateMachine));
            }
            else if (_pieceDropDelay > 0.5f && _isLineCleared)
            {
                _pieceDropDelay = 0;
                _stateMachine.SetState(new PieceFallingState(_stateMachine));
            }
        }
        public void OnStateEnter()
        {
            var player = _stateMachine.Player;
            var piecePos = player.TetrominoController.CurrentTetromino.CellPositions;
            var currentLevelNumber = player.LevelController.CurrentLevel.LevelNumber;

            player.PlayerStats.DroppedPieces++;
            
            if (player.GridController.TryClearFullLines(piecePos, out var fullLines))
            {
                _fullLines = fullLines;
                _isLineCleared = true;
                player.PlayerStats.ClearedLines += fullLines.Count;
                player.LevelController.LevelUp(player);
                _isLeveledUp = currentLevelNumber != player.LevelController.CurrentLevel.LevelNumber;
                player.LevelController.UpdateScore(player, fullLines.Count);
                EventManager.TriggerEvent("ClearingLines", new Dictionary<string, object>{{"fullLines", _fullLines}});
            }
        }
        public void OnStateExit()
        {
            var player = _stateMachine.Player;
            
            if (_isLineCleared)
            {
                EventManager.TriggerEvent("OnLineClear", new Dictionary<string, object>{{"fullLines", _fullLines}});
                
                if (_isLeveledUp)
                {
                    var currentLevel = player.LevelController.CurrentLevel;
                    var currentColors = player.LevelController.LevelDb.Levels[currentLevel.LevelNumber - 1].LevelColors;
                    var desiredColors = player.LevelController.LevelDb.Levels[currentLevel.LevelNumber].LevelColors;
                    
                    EventManager.TriggerEvent("OnLevelChange", new Dictionary<string, object>{{"currentColors", currentColors}, {"desiredColors", desiredColors}});

                    _isLeveledUp = false;
                }
                
                _isLineCleared = false;
            }
            
            EventManager.TriggerEvent("OnTetrominoDrop", new Dictionary<string, object>{{"player", player}});
        }
    }
}
