using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicTetris.UI
{
    public class GameOver : MonoBehaviour
    {
        public SavingScoreUI SavingScoreUI;

        private void SaveScore(SavingScoreUI savingScoreUI) 
        {
            var biggestScore = PlayerPrefs.GetInt("LastScore");
            var lastLevel = PlayerPrefs.GetInt("LastLevel", 0);
            
            for (int i = 0; i < savingScoreUI.HighScores.Count; i++)
            {
                var defaultValue = int.Parse(savingScoreUI.HighScores[i].TopScore.text);
                
                // Save current score as high score if it is higher than any of the current high scores
                if (biggestScore > PlayerPrefs.GetInt("HighScore " + i + 1, defaultValue))
                {
                    PlayerPrefs.SetInt("HighScore " + i + 1, biggestScore);
                    PlayerPrefs.SetInt("Level " + i + 1, lastLevel);

                    savingScoreUI.HighScores[i].TopScore.text = biggestScore.ToString("000000");
                    savingScoreUI.HighScores[i].Level.text = lastLevel.ToString("00");
                    savingScoreUI.Index = i;
                    savingScoreUI.gameObject.SetActive(true);
                    return;
                }
            }

            SceneManager.LoadScene("MainMenuScene");
        }

        private void Start()
        {
            var savingScoreUI = Instantiate(SavingScoreUI);
            savingScoreUI.gameObject.SetActive(false);
            
            // Show current high scores in UI
            for (int i = 0; i < savingScoreUI.HighScores.Count; i++)
            {
                var currentHighScore = savingScoreUI.HighScores[i];
                
                currentHighScore.TopScore.text = PlayerPrefs.GetInt("HighScore " + i + 1, int.Parse(currentHighScore.TopScore.text)).ToString("000000");
                currentHighScore.Level.text = PlayerPrefs.GetInt("Level " + i + 1, int.Parse(currentHighScore.Level.text)).ToString("00");
                currentHighScore.PlayerName.text = PlayerPrefs.GetString("Name " + i + 1, currentHighScore.PlayerName.text);
            }

            SaveScore(savingScoreUI);
        }
    }
}
