public class PlayerStats
{
    public int ClearedLines;
    public int TopScore;
    public int CurrentScore;
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
