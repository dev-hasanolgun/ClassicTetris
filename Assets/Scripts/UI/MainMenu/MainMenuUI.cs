using UnityEngine;

namespace ClassicTetris.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        public Canvas Canvas;
        
        private int _exitCounter;

        private void Play()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _exitCounter = 0;
                UIManager.Instance.SendUISwapCommand(Canvas, UIManager.Instance.LevelSelectUI.Canvas);
            }
        }

        private void Exit()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _exitCounter++;
            }

            if (_exitCounter > 1)
            {
                _exitCounter = 0;
                Application.Quit();
            }
        }

        private void Update()
        {
            Play();
            Exit();
        }
    }
}
