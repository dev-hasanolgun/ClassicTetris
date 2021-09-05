namespace ClassicTetris.Commands
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
