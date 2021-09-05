using ClassicTetris.GameLevel;
using ClassicTetris.StateMachines.PieceStates;
using ClassicTetris.TetrominoBase;
public class Player
{
    public readonly PlayerStats PlayerStats;
    public readonly GridController GridController;
    public readonly LevelController LevelController;
    public readonly TetrominoController TetrominoController;
    public readonly TetrominoSpawner Spawner;
    public readonly PieceStateMachine PieceStateMachine;

    public Player(GridController gridController, LevelController levelController, TetrominoController tetrominoController, TetrominoSpawner spawner)
    {
        GridController = gridController;
        LevelController = levelController;
        TetrominoController = tetrominoController;
        Spawner = spawner;
    
        PieceStateMachine = new PieceStateMachine(this);
        PlayerStats = new PlayerStats(levelController.CurrentLevel.LevelNumber);
    }
}
public class PlayerStats
{
    public int ClearedLines;
    public int TopScore;
    public int CurrentScore;
    public int DroppedPieces;
    public int Level;
    public int Burn;
    public int TetrisRate;
    public float TotalTetris;
    public float TotalClears;

    public PlayerStats(int currentLevel = 0)
    {
        Level = currentLevel;
    }
}