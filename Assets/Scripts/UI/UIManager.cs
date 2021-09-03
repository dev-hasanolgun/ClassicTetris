using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    public UIRoot UIRoot;
    
    [HideInInspector] public MainMenuUI MainMenuUI;
    [HideInInspector] public LevelSelectionUI LevelSelectUI;
    [HideInInspector] public InGameUI InGameUI;
    [HideInInspector] public SavingScoreUI SavingScoreUI;
    [HideInInspector] public CommandHandler CommandHandler = new CommandHandler();

    public void SendUISwapCommand(Canvas currentUI, Canvas nextUI)
    {
        ICommand swapUI = new SwapUI(currentUI, nextUI);
        CommandHandler.AddCommand(swapUI);
    }

    public void ReturnPreviousUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CommandHandler.UndoCommand();
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "GameplayScene")
        {
            PoolManager.Instance.ClearPool();
            InGameUI = Instantiate(UIRoot.InGameUI);
            Instantiate(GameManager.Instance.PieceVisualManager);
            GameManager.Instance.StartGame(LevelSelectUI.SelectedLevel);
        }

        if (scene.name == "MainMenuScene")
        {
            MainMenuUI = Instantiate(UIRoot.MainMenuUI);
            LevelSelectUI = Instantiate(UIRoot.LevelSelectUI);
            LevelSelectUI.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
