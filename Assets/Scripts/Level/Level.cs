using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace devRHS.ClassicTetris.Level
{
    [Serializable]
    public class Level
    {
        public int LevelNumber;
        public int LinesToLevelUp;
        public float LevelSpeed;
        public List<Color32> LevelColors;

        public Level(int levelNumber, int linesToLevelUp, float levelSpeed, List<Color32> levelColors)
        {
            LevelNumber = levelNumber;
            LinesToLevelUp = linesToLevelUp;
            LevelSpeed = levelSpeed;
            LevelColors = levelColors;
        }
    }
}
