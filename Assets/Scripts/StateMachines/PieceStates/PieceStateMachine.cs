namespace devRHS.ClassicTetris.StateMachines.PieceStates
{
    public class PieceStateMachine : BaseStateMachine<PieceStateMachine>
    {
        public Player Player;
        public GameManager GameManager;

        public PieceStateMachine(Player player, GameManager gameManager)
        {
            Player = player;
            GameManager = gameManager;
        }
    }
}