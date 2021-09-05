using System.Collections.Generic;
using UnityEngine;

namespace ClassicTetris.GameLevel
{
    public class LevelController
    {
        public readonly LevelDatabase LevelDb;
        
        public Level CurrentLevel;
        
        private readonly int _startLevel;

        public LevelController(LevelDatabase levelDb, int startLevel = 0)
        {
            LevelDb = levelDb;
            _startLevel = startLevel;
            CurrentLevel = LevelDb.Levels[startLevel];
        }        

        public void LevelUp(Player player)
        {
            if (CurrentLevel.LevelNumber == _startLevel)
            {
                if (player.PlayerStats.ClearedLines >= CurrentLevel.LinesToLevelUp) // First level up differs to the starting level because of the bug in original game so it has to be static
                {
                    CurrentLevel = LevelDb.Levels[CurrentLevel.LevelNumber+1];
                }
            }
            else if (player.PlayerStats.ClearedLines - LevelDb.Levels[_startLevel].LinesToLevelUp >= (CurrentLevel.LevelNumber - _startLevel) * 10) // Leveling up after first level up is same which 10 line clear per level
            {
                CurrentLevel = LevelDb.Levels[CurrentLevel.LevelNumber+1];
            }

            player.PlayerStats.Level = CurrentLevel.LevelNumber;
        }

        public void UpdateScore(Player player, int fullLines)
        {
            var playerStats = player.PlayerStats;
            
            switch (fullLines)
            {
                case 1:
                    playerStats.CurrentScore += 40 * (CurrentLevel.LevelNumber + 1);
                    playerStats.Burn++;
                    playerStats.TotalClears++;
                    break;
                case 2:
                    playerStats.CurrentScore += 100 * (CurrentLevel.LevelNumber + 1);
                    playerStats.Burn += 2;
                    playerStats.TotalClears++;
                    break;
                case 3:
                    playerStats.CurrentScore += 300 * (CurrentLevel.LevelNumber + 1);
                    playerStats.Burn += 3;
                    playerStats.TotalClears++;
                    break;
                case 4:
                    playerStats.CurrentScore += 1200 * (CurrentLevel.LevelNumber + 1);
                    playerStats.Burn = 0;
                    playerStats.TotalClears++;
                    playerStats.TotalTetris++;
                    break;
            }

            playerStats.TetrisRate = Mathf.RoundToInt(playerStats.TotalTetris/playerStats.TotalClears*100f);
        }
    }
}
