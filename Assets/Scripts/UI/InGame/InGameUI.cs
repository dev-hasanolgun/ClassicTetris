using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public Canvas Canvas;
    public PauseMenuUI PauseMenuUI;
    public TextMeshProUGUI Lines;
    public TextMeshProUGUI TopScore;
    public TextMeshProUGUI CurrentScore;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI Burn;
    public TextMeshProUGUI TetrisRate;
    
    public void UpdateUI(Dictionary<string,object> message)
    {
        var player = (Player) message["player"];
        Lines.text = player.PlayerStats.ClearedLines.ToString("000");
        TopScore.text = player.PlayerStats.TopScore.ToString("000000");
        CurrentScore.text = player.PlayerStats.CurrentScore.ToString("000000");
        Level.text = player.PlayerStats.Level.ToString("00");
        Burn.text = player.PlayerStats.Burn.ToString("00");
        TetrisRate.text = player.PlayerStats.TetrisRate.ToString("00");
    }
    private void Awake()
    {
        Canvas.worldCamera = Camera.main;
    }
    private void OnEnable()
    {
        EventManager.StartListening("UpdatingUI", UpdateUI);
    }

    private void OnDisable()
    {
        EventManager.StopListening("UpdatingUI", UpdateUI);
    }
}