
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace devRHS.ClassicTetris.Level
{
    public class LevelCreator : MonoBehaviour
    {
        [BoxGroup("Create Level")]
        [SerializeField]
        private int _levelNumber;
        
        [BoxGroup("Create Level")]
        [SerializeField]
        private int _linesToLevelUp;
        
        [BoxGroup("Create Level")]
        [SerializeField]
        private float _levelSpeed;
        
        [BoxGroup("Create Level")]
        [SerializeField]
        private List<Color32> _levelColors = new List<Color32>();
        
        [BoxGroup("Create Level")]
        [PropertyOrder(4)]
        public LevelDatabase LevelDatabase;
        
        [Button("Create Piece", ButtonSizes.Medium)]
        [GUIColor(0,1,0)]
        private void CreateLevel()
        {
            LevelDatabase.Levels.Add(new Level(_levelNumber,_linesToLevelUp,_levelSpeed,_levelColors));
        }
    }
}
