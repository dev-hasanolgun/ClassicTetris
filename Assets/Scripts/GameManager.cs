using System.Collections.Generic;
using ClassicTetris.GameplayVisual;
using ClassicTetris.GameLevel;
using ClassicTetris.StateMachines.PieceStates;
using ClassicTetris.StateMachines.GameStates;
using UnityEngine;
using ClassicTetris.TetrominoBase;

public class GameManager : MonoBehaviour
{
    public GameStateMachine GameStateMachine;
    public Player Player;
    public TetrominoDatabase TetrominoDatabase;
    public TetrominoVisualManager TetrominoVisualManager;
    public LevelDatabase LevelDatabase;
    
    public static GameManager Instance { get; private set; }
    
    public void StartGame(int startLevel = 0) // Create the player and start the game from selected level.
    {
        Player = new Player(new GridController(Vector3.zero,10,22), new LevelController(LevelDatabase, startLevel),  new TetrominoController(),  new TetrominoSpawner(TetrominoDatabase));
        
        Player.PieceStateMachine.SetState(new PieceFallingState(Player.PieceStateMachine));
        GameStateMachine.SetState(new GameState(GameStateMachine));
        
        EventManager.TriggerEvent("updatingTextures", new Dictionary<string, object>{{"currentColors", null}, {"desiredColors", LevelDatabase.Levels[startLevel].LevelColors}});
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
    private void Start()
    {
        GameStateMachine = new GameStateMachine();
        GameStateMachine.SetState(new MenuState(GameStateMachine));
    }
    private void Update()
    {
        GameStateMachine.CurrentState.Tick();
    }
}
