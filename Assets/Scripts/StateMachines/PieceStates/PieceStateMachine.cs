namespace ClassicTetris.StateMachines.PieceStates
{
    public class PieceStateMachine : BaseStateMachine<PieceStateMachine>
    {
        public readonly Player Player;

        public PieceStateMachine(Player player)
        {
            Player = player;
        }
    }
}