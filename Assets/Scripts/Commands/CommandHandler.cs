using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler
{
    public List<ICommand> CommandList = new List<ICommand>();
    private int index;

    public void AddCommand(ICommand command)
    {
        CommandList.Add(command);
        command.Execute();
        index++;
    }

    public void UndoCommand()
    {
        if (CommandList.Count == 0)
        {
            return;
        }

        if (index > 0)
        {
            CommandList[index - 1].Undo();
            CommandList.RemoveAt(index - 1);
            index--;
        }
    }

    public void RedoCommand()
    {
        if (CommandList.Count == 0)
        {
            return;
        }

        if (index < CommandList.Count)
        {
            index++;
            CommandList[index - 1].Execute();
        }
    }
}
