using System.Collections.Generic;

namespace ClassicTetris.Commands
{
    public class CommandHandler
    {
        private int _index;
        private readonly List<ICommand> _commandList = new List<ICommand>();

        public void AddCommand(ICommand command)
        {
            _commandList.Add(command);
            command.Execute();
            _index++;
        }

        public void UndoCommand()
        {
            if (_commandList.Count == 0)
            {
                return;
            }

            if (_index > 0)
            {
                _commandList[_index - 1].Undo();
                _commandList.RemoveAt(_index - 1);
                _index--;
            }
        }

        public void RedoCommand()
        {
            if (_commandList.Count == 0)
            {
                return;
            }

            if (_index < _commandList.Count)
            {
                _index++;
                _commandList[_index - 1].Execute();
            }
        }
    }
}
