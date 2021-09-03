using System;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public Canvas Canvas;
    private int exitCounter;

    private void Play()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            exitCounter = 0;
            UIManager.Instance.SendUISwapCommand(Canvas, UIManager.Instance.LevelSelectUI.Canvas);
        }
    }

    private void Exit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitCounter++;
        }

        if (exitCounter > 1)
        {
            Application.Quit();
        }
    }

    private void Update()
    {
        Play();
        Exit();
    }
}
