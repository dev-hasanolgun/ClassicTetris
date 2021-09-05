using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassicTetris.GameLevel
{
    [Serializable]
    public class Level
    {
        public List<Color32> LevelColors;
        public int LevelNumber;
        public int LinesToLevelUp;
        public float LevelSpeed;

        public Level(int levelNumber, int linesToLevelUp, float levelSpeed, List<Color32> levelColors)
        {
            LevelNumber = levelNumber;
            LinesToLevelUp = linesToLevelUp;
            LevelSpeed = levelSpeed;
            LevelColors = levelColors;
        }
    }
}
