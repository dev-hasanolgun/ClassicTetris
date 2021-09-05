using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace ClassicTetris.TetrominoBase
{
    public class TetrominoCreator : SerializedMonoBehaviour
    {
        #region Fields

        [OnInspectorInit("UpdateProperties")]
        [BoxGroup("Tetromino Creator")]
        [TableMatrix(DrawElementMethod = "DrawCell", SquareCells = true, HideColumnIndices = true, HideRowIndices = true, ResizableColumns = false)]
        [SerializeField]
        private bool[,] CellDrawingBool = new bool[5,5];
        
        [BoxGroup("Tetromino Creator")]
        [OnValueChanged("UpdateTetrominoName")]
        [VerticalGroup("Tetromino Creator/Tetromino Create Group")]
        [SuppressInvalidAttributeError]
        [SerializeField]
        private string TetrominoName;

        [BoxGroup("Tetromino Creator")]
        [VerticalGroup("Tetromino Creator/Tetromino Create Group")]
        [OnValueChanged("UpdateProperties"), OnValueChanged("ShowCurrentTetromino")]
        [PropertyRange(0,"_tetrominoLastIndex")]
        [SuppressInvalidAttributeError]
        [SerializeField]
        private int CurrentTetromino;

        [VerticalGroup("Tetromino Creator/Tetromino Create Group")]
        [OnValueChanged("ShowCurrentTetromino")]
        [PropertyRange(0,"_orientationLastIndex")]
        [SuppressInvalidAttributeError]
        [SerializeField]
        private int CurrentOrientation;

        [HideIf("_isOrientationExist")]
        [ShowIf("_isTetrominoExist")]
        [OnValueChanged("UpdateOrientableCheck")]
        [VerticalGroup("Tetromino Creator/Tetromino Create Group")]
        [SuppressInvalidAttributeError]
        [SerializeField]
        private bool IsOrientable;
        
        [ShowIf("_isTetrominoExist")]
        [OnValueChanged("UpdatePieceSprite")]
        [VerticalGroup("Tetromino Creator/Tetromino Create Group")]
        [SuppressInvalidAttributeError]
        [SerializeField]
        private Sprite CellSprite;
        
        [BoxGroup("Tetromino Creator")]
        [PropertyOrder(4)]
        [SerializeField]
        private TetrominoDatabase _tetrominoDatabase;
        
        private int _tetrominoLastIndex;
        private int _orientationLastIndex;
        private bool _isTetrominoExist;
        private bool _isOrientationExist;

        #endregion

        #region InspectorButtons

        [VerticalGroup("Tetromino Creator/Tetromino Create Group")]
        [Button("Create Tetromino", ButtonSizes.Medium)]
        [GUIColor(0,1,0)]
        private void CreateTetromino()
        {
            _tetrominoDatabase.Tetrominoes.Add(new Tetromino(new List<Orientation>{new Orientation(GetPositions())}, CellSprite, TetrominoName));
            CurrentTetromino += _tetrominoDatabase.Tetrominoes.Count > 1 ? 1 : 0;
            _isTetrominoExist = true;
            _tetrominoDatabase.Tetrominoes[CurrentTetromino].Orientable = IsOrientable;
            UpdateProperties();
            ShowCurrentTetromino();
        }
        [ShowIf("_isTetrominoExist")]
        [HorizontalGroup("Tetromino Creator/Tetromino Create Group/Update Tetromino Group 1")]
        [Button("Add Orientation", ButtonSizes.Medium)]
        [GUIColor(0,1,0)]
        private void AddOrientation()
        {
            _tetrominoDatabase.Tetrominoes[CurrentTetromino].Orientations.Add(new Orientation(GetPositions()));
            CurrentOrientation++;
            _isOrientationExist = true;
            UpdateProperties();
            ShowCurrentTetromino();
        }
        
        [ShowIf("_isTetrominoExist")]
        [HorizontalGroup("Tetromino Creator/Tetromino Create Group/Update Tetromino Group 2")]
        [Button("Update Orientation", ButtonSizes.Medium)]
        [GUIColor(0,0.5f,1)]
        private void UpdateOrientation()
        {
            if (_isOrientationExist)
            {
                _tetrominoDatabase.Tetrominoes[CurrentTetromino].Orientations[CurrentOrientation] = new Orientation(GetPositions());
            }
            UpdateProperties();
            ShowCurrentTetromino();
        }
        
        [ShowIf("_isTetrominoExist")]
        [HorizontalGroup("Tetromino Creator/Tetromino Create Group/Update Tetromino Group 1")]
        [Button("Remove Orientation", ButtonSizes.Medium)]
        [GUIColor(1,0.4f,0.4f)]
        private void RemoveOrientation()
        {
            if (_isOrientationExist)
            {
                _tetrominoDatabase.Tetrominoes[CurrentTetromino].Orientations.RemoveAt(CurrentOrientation);
                CurrentOrientation--;
            }
            UpdateProperties();
            ShowCurrentTetromino();
        }
        
        [ShowIf("_isTetrominoExist")]
        [HorizontalGroup("Tetromino Creator/Tetromino Create Group/Update Tetromino Group 2")]
        [Button("Remove Tetromino", ButtonSizes.Medium)]
        [GUIColor(1,0.2f,0.2f)]
        private void RemoveTetromino()
        {
            if (_isTetrominoExist)
            {
                _tetrominoDatabase.Tetrominoes.RemoveAt(CurrentTetromino);
            }
            UpdateProperties();
            ShowCurrentTetromino();
        }

        #endregion

        #region Methods

        private void UpdateProperties()
        {
            if (_tetrominoDatabase.Tetrominoes.Count == 0)
            {
                _isTetrominoExist = false;
                _isOrientationExist = false;
            }
            else
            {
                _isTetrominoExist = true;
            }
            
            if (_isTetrominoExist)
            {
                _tetrominoLastIndex = Mathf.Clamp(_tetrominoDatabase.Tetrominoes.Count - 1,0,int.MaxValue);
                CurrentTetromino = Mathf.Clamp(CurrentTetromino, 0, _tetrominoLastIndex);
                
                _orientationLastIndex = Mathf.Clamp(_tetrominoDatabase.Tetrominoes[CurrentTetromino].Orientations.Count - 1,0,int.MaxValue);
                CurrentOrientation = Mathf.Clamp(CurrentOrientation, 0, _orientationLastIndex);

                IsOrientable = _tetrominoDatabase.Tetrominoes[CurrentTetromino].Orientable;
                
                _isOrientationExist = _tetrominoDatabase.Tetrominoes[CurrentTetromino].Orientations.Count > 1;
            }
            else
            {
                _tetrominoLastIndex = 0;
                _orientationLastIndex = 0;
                CurrentTetromino = 0;
                CurrentOrientation = 0;
            }
        }
        private void UpdateOrientableCheck()
        {
            if (_isTetrominoExist)
            {
                _tetrominoDatabase.Tetrominoes[CurrentTetromino].Orientable = IsOrientable;
            }
        }
        private void ShowCurrentTetromino()
        {
            if (_isTetrominoExist)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        CellDrawingBool[i, j] = false;
                    }
                }

                Vector3[] orientation = _tetrominoDatabase.Tetrominoes[CurrentTetromino].Orientations[CurrentOrientation].Positions;
                for (int i = 0; i < orientation.Length; i++)
                {
                    var x = (int)orientation[i].x;
                    var y = (int)orientation[i].y;
                    CellDrawingBool[x+2, -y+2] = true;
                }
                
                CellSprite = _tetrominoDatabase.Tetrominoes[CurrentTetromino].CellSprite;
                TetrominoName = _tetrominoDatabase.Tetrominoes[CurrentTetromino].TetrominoName;
            }
        }

        private void UpdateTetrominoName()
        {
            if (_isTetrominoExist)
            {
                _tetrominoDatabase.Tetrominoes[CurrentTetromino].TetrominoName = TetrominoName;
            }
        }
        private void UpdatePieceSprite()
        {
            if (_isTetrominoExist)
            {
                _tetrominoDatabase.Tetrominoes[CurrentTetromino].CellSprite = CellSprite;
            }
        }
        private Vector3[] GetPositions()
        {
            List<Vector3> positions = new List<Vector3>();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (CellDrawingBool[i,j])
                    {
                        positions.Add(new Vector3(i-2,-j+2,0));
                    }
                }
            }
            
            return positions.ToArray();
        }

        private bool DrawCell(Rect rect, bool value)
        {
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }
            #if UNITY_EDITOR
                EditorGUI.DrawRect(rect.Padding(5), value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0,0,0, 0.5f));
            #endif

            return value;
        }
    }

    #endregion
}