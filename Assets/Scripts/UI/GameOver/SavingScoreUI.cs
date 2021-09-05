using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicTetris.UI
{
    public class SavingScoreUI : MonoBehaviour
    {
        public List<HighScore> HighScores;
        public int Index;

        private void Start()
        {
            HighScores[Index].PlayerName.placeholder.GetComponent<TextMeshProUGUI>().text = "------";
        }

        private void Update()
        {
            HighScores[Index].PlayerName.caretWidth = 0;
            HighScores[Index].PlayerName.ActivateInputField();
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PlayerPrefs.SetString("Name " + Index + 1, HighScores[Index].PlayerName.text);
                SceneManager.LoadScene("MainMenuScene");
            }
        }
    }
}
