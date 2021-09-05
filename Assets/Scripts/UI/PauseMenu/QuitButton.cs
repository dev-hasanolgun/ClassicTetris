using UnityEngine.SceneManagement;

namespace ClassicTetris.UI
{
    public class QuitButton : ButtonUI
    {
        public override void ExecuteOption()
        {
            EventManager.TriggerEvent("OnQuitGame", null);
            SceneManager.LoadScene("MainMenuScene");
        }
    }

}