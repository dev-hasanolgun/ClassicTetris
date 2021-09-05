using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ClassicTetris.GameLevel
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "Tetris/Level Database"), InlineEditor]
    public class LevelDatabase : ScriptableObject
    {
        public List<Level> Levels = new List<Level>();
    }
}
