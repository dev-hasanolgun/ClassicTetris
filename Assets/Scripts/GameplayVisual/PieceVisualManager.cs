using System.Collections.Generic;
using System.Diagnostics;
using devRHS.ClassicTetris.TetrominoCreator;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PieceVisualManager : MonoBehaviour
{
    public CellBlock CellBlockObj;
    public TetrominoDatabase TetrominoDatabase;
    public List<Texture2D> BlockTextures = new List<Texture2D>();
    public Transform NextPiece;
    public LineClearAnimation LineClearAnimation;
    public Animator TetrisEffectAnim;

    private List<CellBlock> _activeCellBlocks = new List<CellBlock>();
    private List<CellBlock> _currentPieceVisual = new List<CellBlock>();
    private List<CellBlock> _nextPieceVisual = new List<CellBlock>();

    
    //CopiedTexture is the original Texture  which you want to copy.
    private Texture2D GetNewTexture2D(Texture2D blockTexture, List<Color32> currentColors, List<Color32> desiredColors)
    {
        var copiedTexture = new Texture2D(blockTexture.width,blockTexture.height);
        
        for (int x = 0; x < blockTexture.width; x++)
        {
            for (int y = 0; y < blockTexture.height; y++)
            {
                for(int i = 0; i < currentColors.Count; i++)
                {
                    if (blockTexture.GetPixel(x, y).Equals(Color.white))
                    {
                        copiedTexture.SetPixel(x, y, Color.white);
                    }
                    else if(blockTexture.GetPixel(x, y).Equals(currentColors[i]))
                    {
                        //This line of code and if statement, turn Green pixels into Red pixels.
                        copiedTexture.SetPixel(x, y, desiredColors[i]);
                    }
                }
            }
        }
        //Name the texture, if you want.
 
        //This finalizes it. If you want to edit it still, do it before you finish with .Apply(). Do NOT expect to edit the image after you have applied. It did NOT work for me to edit it after this function.
        copiedTexture.Apply();
 
        //Return the variable, so you have it to assign to a permanent variable and so you can use it.
        return copiedTexture;
    }
    public bool IsNullOrEmpty<T>(List<T> list)
    {
        return list == null || list.Count == 0;
    }
    private void UpdateTextures(Dictionary<string,object> message)
    {
        var currentColors = (List<Color32>) message["currentColors"] ?? GetBlockColors();
        var desiredColors = (List<Color32>) message["desiredColors"];
        
        var colTex = GetNewTexture2D(TetrominoDatabase.ColorSchemeBlock, currentColors, desiredColors);
        TetrominoDatabase.ColorSchemeBlock.SetPixels(colTex.GetPixels());
        TetrominoDatabase.ColorSchemeBlock.Apply();
        for(int i = 0; i < BlockTextures.Count; i++)
        {
            var tex = GetNewTexture2D(BlockTextures[i], currentColors, desiredColors);
            BlockTextures[i].SetPixels(tex.GetPixels());
            BlockTextures[i].Apply();
        }
    }
    public List<Color32> GetBlockColors()
    { 
        var colorList = new List<Color32>();
        for (int x = 0; x < TetrominoDatabase.ColorSchemeBlock.width; x++)
        {
            for (int y = 0; y < TetrominoDatabase.ColorSchemeBlock.height; y++)
            {
                var currentColor = (Color32)TetrominoDatabase.ColorSchemeBlock.GetPixel(x, y);
                if(currentColor != Color.white && !colorList.Contains(currentColor))
                {
                    colorList.Add(currentColor);
                }
            }
        }
        return colorList;
    }

    private Vector3 GetCenterPos(Vector3[] positions)
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
    private List<CellBlock> InstantiateTetromino(Tetromino tetromino)
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
    private void OnAddCellVisual(Dictionary<string,object> message)
    {
        var player = (Player) message["player"];
        if (_nextPieceVisual.Count == 0)
        {
            _currentPieceVisual = InstantiateTetromino(player.Spawner.CurrentPiece);
            for (int i = 0; i < _currentPieceVisual.Count; i++)
            {
                _currentPieceVisual[i].transform.position += player.PlayerPosition;
                _activeCellBlocks.Add(_currentPieceVisual[i]);
            }
        }
        else
        {
            _currentPieceVisual.Clear();
            for (int i = 0; i < _nextPieceVisual.Count; i++)
            {
                _currentPieceVisual.Add(_nextPieceVisual[i]);
                _currentPieceVisual[i].transform.position += player.PlayerPosition - NextPiece.transform.position + GetCenterPos(player.Spawner.CurrentPiece.CellPositions);
                _currentPieceVisual[i].transform.SetParent(transform);
                _activeCellBlocks.Add(_currentPieceVisual[i]);
            }
            _currentPieceVisual = new List<CellBlock>(_nextPieceVisual);
        }
        _nextPieceVisual = InstantiateTetromino(player.Spawner.NextPiece);
        for (int i = 0; i < _nextPieceVisual.Count; i++)  
        {
            _nextPieceVisual[i].transform.position += NextPiece.transform.position - GetCenterPos(player.Spawner.NextPiece.CellPositions);
            _nextPieceVisual[i].transform.SetParent(NextPiece);
        }
    }
    public void LineClearVisual(Dictionary<string, object> message)
    {
        var fullLines = (List<int>) message["fullLines"];
        for (int i = 0; i < fullLines.Count; i++)
        {
            var lineClearAnim = PoolManager.Instance.GetObjectFromPool("lineClearAnim", LineClearAnimation);
            lineClearAnim.transform.position = new Vector3(4.5f, fullLines[i], 0);
        }

        if (fullLines.Count == 4)
        {
            TetrisEffectAnim.gameObject.SetActive(true);
            TetrisEffectAnim.Play("TetrisEffect");
        }
    }
    private void UpdatePieceVisual(Dictionary<string, object> message)
    {
        var player = (Player) message["player"];
        var positions = player.TetrominoController.CurrentTetromino.CellPositions;
        
        for (int i = 0; i < positions.Length; i++)
        {
            _currentPieceVisual[i].transform.position = positions[i] + player.PlayerPosition;
        }
    }

    private void ClearFullLines(Dictionary<string, object> message)
    {
        var fullLines = (List<int>) message["fullLines"];
        var player = (Player) message["player"];
        for (int i = 0; i < fullLines.Count; i++)
        {
            for (int j = 0; j < _activeCellBlocks.Count; j++)
            {
                if ((int)_activeCellBlocks[j].transform.position.y == fullLines[i])
                {
                    _activeCellBlocks[j].gameObject.SetActive(false);
                    _activeCellBlocks.RemoveAt(j);
                    j--;
                }
            }
            for (int j = 0; j < _activeCellBlocks.Count; j++)
            {
                if ((int)_activeCellBlocks[j].transform.position.y > fullLines[i])
                {
                    _activeCellBlocks[j].transform.position += Vector3.down;
                }
            }
        }
    }
    private void UpdateGridVisual(Dictionary<string, object> message)
    {
        var player = (Player) message["player"];
        var gridMap = player.GridManager.GridMap;
        var index = 0;
        
        for (int i = 0; i < gridMap.Length; i++)
        {
            for (int j = 0; j < gridMap[i].Length; j++)
            {
                if (!gridMap[i][j].IsCellEmpty)
                {
                    _activeCellBlocks[index].transform.position = new Vector3(i,j,0) + player.PlayerPosition;
                    index++;
                }
            }
        }

        var iteration = _activeCellBlocks.Count - index;
        for (int i = 0; i < iteration; i++)
        {
            _activeCellBlocks[index].gameObject.SetActive(false);
            _activeCellBlocks.RemoveAt(index);
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("addingCellVisual", OnAddCellVisual);
        EventManager.StartListening("updatingCellVisual", UpdatePieceVisual);
        EventManager.StartListening("updatingGridVisual", ClearFullLines);
        EventManager.StartListening("updatingTextures", UpdateTextures);
        EventManager.StartListening("onLineClear", LineClearVisual);
    }
    private void OnDisable()
    {
        EventManager.StopListening("addingCellVisual", OnAddCellVisual);
        EventManager.StopListening("updatingCellVisual", UpdatePieceVisual);
        EventManager.StopListening("updatingGridVisual", ClearFullLines);
        EventManager.StopListening("updatingTextures", UpdateTextures);
        EventManager.StopListening("onLineClear", LineClearVisual);
    }
}