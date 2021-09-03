using devRHS.ClassicTetris.Level;
using devRHS.ClassicTetris.TetrominoCreator;
using devRHS.ClassicTetris.StateMachines.PieceStates;
using UnityEngine;

public class Player
{
    public PlayerStats PlayerStats;
    public GridManager GridManager;
    public LevelManager LevelManager;
    public TetrominoController TetrominoController;
    public TetrominoSpawner Spawner;
    public Vector3 PlayerPosition;
    public string name;

    public PieceStateMachine PieceStateMachine;

    public Player(GridManager gridManager, LevelManager levelManager, TetrominoController tetrominoController, TetrominoSpawner spawner, Vector3 playerPosition, string _name)
    {
        GridManager = gridManager;
        LevelManager = levelManager;
        TetrominoController = tetrominoController;
        Spawner = spawner;
        PlayerPosition = playerPosition;
        name = _name;
        
        PlayerStats = new PlayerStats(levelManager.CurrentLevel.LevelNumber);
    }
}