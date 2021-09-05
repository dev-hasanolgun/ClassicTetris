using ClassicTetris.Commands;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicTetris.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public UIRoot UIRoot;

        [HideInInspector] public MainMenuUI MainMenuUI;
        [HideInInspector] public LevelSelectionUI LevelSelectUI;
        [HideInInspector] public InGameUI InGameUI;

        private readonly CommandHandler _commandHandler = new CommandHandler();

        public void SendUISwapCommand(Canvas currentUI, Canvas nextUI)
        {
            ICommand swapUI = new SwapUI(currentUI, nextUI);
            _commandHandler.AddCommand(swapUI);
        }

        public void ReturnPreviousUI()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _commandHandler.UndoCommand();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            switch (scene.name)
            {
                case "GameplayScene":
                    PoolManager.Instance.ClearPool();
                    InGameUI = Instantiate(UIRoot.InGameUI);
                    Instantiate(GameManager.Instance.TetrominoVisualManager);
                    GameManager.Instance.StartGame(LevelSelectUI.SelectedLevel);
                    break;
                case "MainMenuScene":
                    MainMenuUI = Instantiate(UIRoot.MainMenuUI);
                    LevelSelectUI = Instantiate(UIRoot.LevelSelectUI);
                    LevelSelectUI.gameObject.SetActive(false);
                    break;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
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

}