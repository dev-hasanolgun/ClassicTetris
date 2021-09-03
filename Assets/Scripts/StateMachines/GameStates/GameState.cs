using devRHS.ClassicTetris.StateMachines.PieceStates;
using UnityEngine;

namespace devRHS.ClassicTetris.StateMachines.UIStates
{
    public class GameState : IState<GameStateMachine>
    {
        private GameStateMachine _stateMachine;
        public GameState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Tick()
        {
            var player = GameManager.Instance.Player;
            
            player.PieceStateMachine.CurrentState.Tick();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _stateMachine.SetState(new PauseState(_stateMachine));
            }
        }
        public void OnStateEnter()
        {
        }
        public void OnStateExit()
        {
        }
    }
}
