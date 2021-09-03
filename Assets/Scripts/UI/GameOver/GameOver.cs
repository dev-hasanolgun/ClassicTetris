using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public SavingScoreUI SavingScoreUI;
    public void SaveScore(SavingScoreUI savingScoreUI)
    {
        var biggestScore = PlayerPrefs.GetInt("LastScore");
        var lastLevel = PlayerPrefs.GetInt("LastLevel", 0);
        for (int i = 0; i < savingScoreUI.HighScores.Count; i++)
        {
            if (biggestScore > PlayerPrefs.GetInt("HighScore " + i+1, int.Parse(savingScoreUI.HighScores[i].TopScore.text)))
            {
                PlayerPrefs.SetInt("HighScore " + i+1, biggestScore);
                PlayerPrefs.SetInt("Level " + i+1, lastLevel);
                
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
        for (int i = 0; i < savingScoreUI.HighScores.Count; i++)
        {
            savingScoreUI.HighScores[i].TopScore.text = PlayerPrefs.GetInt("HighScore " + i + 1, int.Parse(savingScoreUI.HighScores[i].TopScore.text)).ToString("000000");
            savingScoreUI.HighScores[i].Level.text = PlayerPrefs.GetInt("Level " + i + 1, int.Parse(savingScoreUI.HighScores[i].Level.text)).ToString("00");
            savingScoreUI.HighScores[i].PlayerName.text = PlayerPrefs.GetString("Name " + i + 1, savingScoreUI.HighScores[i].PlayerName.text);
        }
        SaveScore(savingScoreUI);
    }
}
