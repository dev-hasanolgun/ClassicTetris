namespace ClassicTetris.StateMachines.GameStates
{
    public class MenuState : IState<GameStateMachine>
    {
        private readonly GameStateMachine _stateMachine;
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
