using UnityEngine;

public class SwapUI : ICommand
{
    private Canvas _currentUI, _nextUI;
    public SwapUI(Canvas currentUI, Canvas nextUI)
    {
        _currentUI = currentUI;
        _nextUI = nextUI;
    }
    public void Execute()
    {
        _currentUI.gameObject.SetActive(false);
        _nextUI.gameObject.SetActive(true);
    }

    public void Undo()
    {
        _currentUI.gameObject.SetActive(true);
        _nextUI.gameObject.SetActive(false);
    }
}
