using UnityEngine.SceneManagement;

namespace ClassicTetris.StateMachines.GameStates
{
    public class GameOverState : IState<GameStateMachine>
    {
        private readonly GameStateMachine _stateMachine;
        public GameOverState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Tick()
        {
        }

        public void OnStateEnter()
        {
            SceneManager.LoadScene("GameOverScene");
            _stateMachine.SetState(new MenuState(_stateMachine));
        }

        public void OnStateExit()
        {
        }
    }
}