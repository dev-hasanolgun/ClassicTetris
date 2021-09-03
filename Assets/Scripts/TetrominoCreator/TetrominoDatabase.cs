using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace devRHS.ClassicTetris.TetrominoCreator
{
    [CreateAssetMenu(fileName = "TetrominoDatabase", menuName = "Tetris/Tetromino Database"), InlineEditor]
    public class TetrominoDatabase : ScriptableObject
    {
        public Texture2D ColorSchemeBlock;
        public List<Tetromino> Tetrominoes = new List<Tetromino>();
    }
}
