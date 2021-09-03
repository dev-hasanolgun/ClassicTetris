using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    public List<TextMeshProUGUI> Levels;
    public List<HighScore> HighScores;
    public Image LevelSelector;
    public Canvas Canvas;
    [HideInInspector] public int SelectedLevel;
    private int _index;
    private float _highlightDelay;

    private void HighlightSelector()
    {
        _highlightDelay += Time.deltaTime;
        if (_highlightDelay > 0.025f)
        {
            LevelSelector.gameObject.SetActive(!LevelSelector.gameObject.activeInHierarchy);
            _highlightDelay = 0;
        }
    }
    private void ChoosingLevel()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _index = Mathf.Clamp(_index-1, 0, Levels.Count-1);
            LevelSelector.rectTransform.position = Levels[_index].rectTransform.position;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _index = Mathf.Clamp(_index+1, 0, Levels.Count-1);
            LevelSelector.rectTransform.position = Levels[_index].rectTransform.position;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _index = Mathf.Clamp(_index+5, 0, Levels.Count-1);
            LevelSelector.rectTransform.position = Levels[_index].rectTransform.position;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _index = Mathf.Clamp(_index-5, 0, Levels.Count-1);
            LevelSelector.rectTransform.position = Levels[_index].rectTransform.position;
        }
    }

    private void Start()
    {
        for (int i = 0; i < HighScores.Count; i++)
        {
            HighScores[i].TopScore.text = PlayerPrefs.GetInt("HighScore " + i + 1, int.Parse(HighScores[i].TopScore.text)).ToString("000000");
            HighScores[i].Level.text = PlayerPrefs.GetInt("Level " + i + 1, int.Parse(HighScores[i].Level.text)).ToString("00");
            HighScores[i].PlayerName.text = PlayerPrefs.GetString("Name " + i + 1, HighScores[i].PlayerName.text);
        }
    }

    private void Update()
    {
        ChoosingLevel();
        HighlightSelector();
        UIManager.Instance.ReturnPreviousUI();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Input.GetKey(KeyCode.X))
            {
                _index += 10;
            }

            SelectedLevel = _index;
            SceneManager.LoadScene("GameplayScene");
        }
    }
}
