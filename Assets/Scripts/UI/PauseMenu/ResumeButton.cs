public class ResumeButton : ButtonUI
{
    public PauseMenuUI PauseMenuUI;
    public override void ExecuteOption()
    {
        EventManager.TriggerEvent("OnResumeGame", null);
        PauseMenuUI.gameObject.SetActive(false);
    }
}
