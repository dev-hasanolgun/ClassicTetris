using UnityEngine;

namespace devRHS.ClassicTetris.StateMachines.UIStates
{
    public class MenuState : IState<GameStateMachine>
    {
        private GameStateMachine _stateMachine;
        public MenuState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Tick()
        {
        }

        public void OnStateEnter()
        {
        }

        public void OnStateExit()
        {
        }
    }
}
