using System.Collections.Generic;
using UnityEngine;

namespace devRHS.ClassicTetris.Level
{
    public class LevelManager
    {
        public Level CurrentLevel;
        private LevelDatabase _levelDb;
        public int StartLevel;

        public LevelManager(LevelDatabase levelDb, int startLevel = 0)
        {
            _levelDb = levelDb;
            StartLevel = startLevel;
            CurrentLevel = _levelDb.Levels[startLevel];
        }

        public Color32[] DefaultBlockColors = {new Color32(66,66,255,255), new Color32(181,49,33,255)};

        public void LevelUp(Player player)
        {
            if (CurrentLevel.LevelNumber == StartLevel)
            {
                if (player.PlayerStats.ClearedLines >= CurrentLevel.LinesToLevelUp)
                {
                    EventManager.TriggerEvent("updatingTextures", new Dictionary<string, object>{{"currentColors", CurrentLevel.LevelColors}, {"desiredColors", _levelDb.Levels[CurrentLevel.LevelNumber+1].LevelColors}});
                    CurrentLevel = _levelDb.Levels[CurrentLevel.LevelNumber+1];
                }
            }
            else if (player.PlayerStats.ClearedLines - _levelDb.Levels[StartLevel].LinesToLevelUp >= (CurrentLevel.LevelNumber - StartLevel) * 10)
            {
                CurrentLevel = _levelDb.Levels[CurrentLevel.LevelNumber+1];
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
