using UnityEngine.SceneManagement;

public class QuitButton : ButtonUI
{
    public override void ExecuteOption()
    {
        EventManager.TriggerEvent("OnQuitGame", null);
        SceneManager.LoadScene("MainMenuScene");
    }
}
