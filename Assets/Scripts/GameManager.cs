using System.Collections.Generic;
using devRHS.ClassicTetris.Level;
using devRHS.ClassicTetris.StateMachines.PieceStates;
using devRHS.ClassicTetris.StateMachines.UIStates;
using UnityEngine;
using devRHS.ClassicTetris.TetrominoCreator;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    
    public Player Player;
    public LevelDatabase LevelDatabase;
    public TetrominoDatabase TetrominoDatabase;
    public GameStateMachine GameStateMachine;
    public PieceVisualManager PieceVisualManager;
    public bool IsGameStarted;
    
    [HideInInspector] public Tetromino CurrentTetromino;
    [HideInInspector] public TetrominoSpawner Spawner;
    
    private float _initialDelay;
    public void StartGame(int startLevel = 0)
    {
        Player = new Player(new GridManager(), new LevelManager(LevelDatabase, startLevel),  new TetrominoController(),  new TetrominoSpawner(TetrominoDatabase), Vector3.zero, "s");
        Player.PieceStateMachine = new PieceStateMachine(Player, this);
        Player.PieceStateMachine.SetState(new PieceFallingState(Player.PieceStateMachine));
        EventManager.TriggerEvent("updatingTextures", new Dictionary<string, object>{{"currentColors", null}, {"desiredColors", LevelDatabase.Levels[startLevel].LevelColors}});
        _initialDelay = 0;
        GameStateMachine.SetState(new GameState(GameStateMachine));
    }

    private void Start()
    {
        GameStateMachine = new GameStateMachine();
        GameStateMachine.SetState(new MenuState(GameStateMachine));
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

    private void Update()
    {
        if (_initialDelay > 1.6f)
        {
            GameStateMachine.CurrentState.Tick();
        }
        else
        {
            _initialDelay += Time.deltaTime;
        }
    }
}
