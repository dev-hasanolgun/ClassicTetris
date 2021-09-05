using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ClassicTetris.TetrominoBase
{
    [CreateAssetMenu(fileName = "TetrominoDatabase", menuName = "Tetris/Tetromino Database"), InlineEditor]
    public class TetrominoDatabase : ScriptableObject
    {
        public Texture2D ColorScheme;
        public List<Tetromino> Tetrominoes = new List<Tetromino>();
    }
}
