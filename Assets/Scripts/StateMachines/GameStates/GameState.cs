using ClassicTetris.StateMachines.PieceStates;
using UnityEngine;

namespace ClassicTetris.StateMachines.GameStates
{
    public class GameState : IState<GameStateMachine>
    {
        private readonly GameStateMachine _stateMachine;
        private float _initialDelay;

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
