using System.Collections.Generic;
using ClassicTetris.TetrominoBase;
using UnityEngine;

namespace ClassicTetris.GameplayVisual
{
    public class TetrominoVisualManager : MonoBehaviour
    {
        public TetrominoDatabase TetrominoDatabase;
        public CellBlock CellBlockObj;
        public LineClearAnimation LineClearAnimation;
        public Animator TetrisEffectAnim;
        public List<Texture2D> BlockTextures = new List<Texture2D>();
        public Transform NextPiece;

        private List<CellBlock> _activeCellBlocks = new List<CellBlock>(); // Currently active cell blocks in the scene
        private List<CellBlock> _currentTetrominoVisual = new List<CellBlock>(); // Currently controlled tetromino visuals
        private List<CellBlock> _nextTetrominoVisual = new List<CellBlock>(); // Next tetromino visuals

    
        // Copy texture by changing chosen colors to desired ones
        private Texture2D GetNewBlockTexture2D(Texture2D blockTexture, List<Color32> currentColors, List<Color32> desiredColors)
        {
            var copiedTexture = new Texture2D(blockTexture.width,blockTexture.height);
        
            for (int x = 0; x < blockTexture.width; x++)
            {
                for (int y = 0; y < blockTexture.height; y++)
                {
                    for(int i = 0; i < currentColors.Count; i++)
                    {
                        if (blockTexture.GetPixel(x, y) == Color.white) // Check for white color pixels and set them into copiedTexture since white not included in color scheme
                        {
                            copiedTexture.SetPixel(x, y, Color.white);
                        }
                        else if(blockTexture.GetPixel(x, y) == currentColors[i]) // Set desired color for copiedTexture as position of original texture
                        {
                            copiedTexture.SetPixel(x, y, desiredColors[i]);
                        }
                    }
                }
            }
            copiedTexture.Apply();
 
            return copiedTexture;
        }

        private void UpdateTextures(Dictionary<string,object> message)
        {
            var currentColors = (List<Color32>) message["currentColors"] ?? GetBlockColors(); // Get current colors, if null, get from color scheme
            var desiredColors = (List<Color32>) message["desiredColors"];
            
            // Change color scheme colors to desired colors
            var newTex = GetNewBlockTexture2D(TetrominoDatabase.ColorScheme, currentColors, desiredColors);
            TetrominoDatabase.ColorScheme.SetPixels(newTex.GetPixels());
            TetrominoDatabase.ColorScheme.Apply();
            
            // Change all block colors to desired colors
            for(int i = 0; i < BlockTextures.Count; i++)
            {
                var tex = GetNewBlockTexture2D(BlockTextures[i], currentColors, desiredColors);
                BlockTextures[i].SetPixels(tex.GetPixels());
                BlockTextures[i].Apply();
            }
        }

        private List<Color32> GetBlockColors() // Get colors from color scheme return with a list
        { 
            var colorList = new List<Color32>();
            
            for (int x = 0; x < TetrominoDatabase.ColorScheme.width; x++)
            {
                for (int y = 0; y < TetrominoDatabase.ColorScheme.height; y++)
                {
                    var currentColor = (Color32)TetrominoDatabase.ColorScheme.GetPixel(x, y);
                    if(currentColor != Color.white && !colorList.Contains(currentColor))
                    {
                        colorList.Add(currentColor);
                    }
                }
            }
            return colorList;
        }

        private Vector3 GetCenterPos(Vector3[] positions) // Get actual center position of the all cell blocks inside a tetromino
        {
            var smallestX = float.MaxValue;
            var smallestY = float.MaxValue;
            var biggestX = float.MinValue;
            var biggestY = float.MinValue;
            
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i].x < smallestX)
                {
                    smallestX = positions[i].x;
                }
                if (positions[i].y < smallestY)
                {
                    smallestY = positions[i].y;
                }
                if (positions[i].x > biggestX)
                {
                    biggestX = positions[i].x;
                }
                if (positions[i].y > biggestY)
                {
                    biggestY = positions[i].y;
                }
            }

            var centerX = (smallestX + biggestX) / 2;
            var centerY = (smallestY + biggestY) / 2;
            return new Vector3(centerX,centerY,0);
        }
        private List<CellBlock> InstantiateTetromino(Tetromino tetromino) // Instantiate cell block visuals of the tetromino
        {
            var blockList = new List<CellBlock>();
            for (int i = 0; i < tetromino.CellPositions.Length; i++)
            {
                var cellBlock = PoolManager.Instance.GetObjectFromPool("cellBlocks", CellBlockObj);
                cellBlock.transform.position = tetromino.CellPositions[i];
                cellBlock.transform.SetParent(transform);
                cellBlock.SpriteRenderer.sprite = tetromino.CellSprite;
                blockList.Add(cellBlock);
            }

            return blockList;
        }
        private void AddCellVisual(Dictionary<string,object> message) // Add cell block visuals to spawned tetromino
        {
            var player = (Player) message["player"];
            var currentPiece = player.Spawner.CurrentPiece;
            var nextPiece = player.Spawner.NextPiece;
            var gridPos = player.GridController.Grid.GridPos;
        
            if (_nextTetrominoVisual.Count == 0) // Instantiate first tetromino since there is no tetromino to move from the next tetromino pile to spawner pile
            {
                _currentTetrominoVisual = InstantiateTetromino(currentPiece);
                for (int i = 0; i < _currentTetrominoVisual.Count; i++)
                {
                    _currentTetrominoVisual[i].transform.position += gridPos;
                    _activeCellBlocks.Add(_currentTetrominoVisual[i]);
                }
            }
            else // Move next tetromino visual to the spawner position and set as current tetromino
            {
                _currentTetrominoVisual.Clear();
                for (int i = 0; i < _nextTetrominoVisual.Count; i++)
                {
                    _currentTetrominoVisual.Add(_nextTetrominoVisual[i]);
                    _currentTetrominoVisual[i].transform.position += gridPos - NextPiece.transform.position + GetCenterPos(currentPiece.CellPositions);
                    _currentTetrominoVisual[i].transform.SetParent(transform);
                    _activeCellBlocks.Add(_currentTetrominoVisual[i]);
                }
                _currentTetrominoVisual = new List<CellBlock>(_nextTetrominoVisual);
            }
            
            // Instantiate next tetromino visual
            _nextTetrominoVisual = InstantiateTetromino(nextPiece);
            for (int i = 0; i < _nextTetrominoVisual.Count; i++)  
            {
                _nextTetrominoVisual[i].transform.position += NextPiece.transform.position - GetCenterPos(nextPiece.CellPositions);
                _nextTetrominoVisual[i].transform.SetParent(NextPiece);
            }
        }

        private void LineClearVisual(Dictionary<string, object> message) // Play animations on line clear
        {
            var fullLines = (List<int>) message["fullLines"];
            for (int i = 0; i < fullLines.Count; i++) // Play line clear animation
            {
                var lineClearAnim = PoolManager.Instance.GetObjectFromPool("lineClearAnim", LineClearAnimation);
                lineClearAnim.transform.position = new Vector3(4.5f, fullLines[i], 0);
            }

            if (fullLines.Count == 4) // Play tetris animation
            {
                TetrisEffectAnim.gameObject.SetActive(true);
                TetrisEffectAnim.Play("TetrisEffect");
            }
        }
    
        private void UpdateTetrominoVisual(Dictionary<string, object> message) // Update current piece block positions with new positions
        {
            var player = (Player) message["player"];
            var gridPos = player.GridController.Grid.GridPos;
            var positions = player.TetrominoController.CurrentTetromino.CellPositions;
        
            for (int i = 0; i < positions.Length; i++)
            {
                _currentTetrominoVisual[i].transform.position = positions[i] + gridPos;
            }
        }

        private void ClearFullLines(Dictionary<string, object> message)
        {
            var fullLines = (List<int>) message["fullLines"];
        
            for (int i = 0; i < fullLines.Count; i++)
            {
                for (int j = 0; j < _activeCellBlocks.Count; j++)
                {
                    if ((int)_activeCellBlocks[j].transform.position.y == fullLines[i]) // Clear block visuals which are equals to the y-axis of the cleared lines
                    {
                        _activeCellBlocks[j].gameObject.SetActive(false);
                        _activeCellBlocks.RemoveAt(j);
                        j--;
                    }
                }
                for (int j = 0; j < _activeCellBlocks.Count; j++)
                {
                    if ((int)_activeCellBlocks[j].transform.position.y > fullLines[i]) // Move block visuals down by 1 which are above to the y-axis of the cleared lines
                    {
                        _activeCellBlocks[j].transform.position += Vector3.down;
                    }
                }
            }
        }

        private void OnEnable()
        {
            EventManager.StartListening("addingCellVisual", AddCellVisual);
            EventManager.StartListening("updatingCellVisual", UpdateTetrominoVisual);
            EventManager.StartListening("updatingGridVisual", ClearFullLines);
            EventManager.StartListening("updatingTextures", UpdateTextures);
            EventManager.StartListening("onLineClear", LineClearVisual);
        }
        private void OnDisable()
        {
            EventManager.StopListening("addingCellVisual", AddCellVisual);
            EventManager.StopListening("updatingCellVisual", UpdateTetrominoVisual);
            EventManager.StopListening("updatingGridVisual", ClearFullLines);
            EventManager.StopListening("updatingTextures", UpdateTextures);
            EventManager.StopListening("onLineClear", LineClearVisual);
        }
    }
}