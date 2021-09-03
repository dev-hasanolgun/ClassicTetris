using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Vector3 CellPosition;
    public bool IsCellEmpty;

    public Grid(Vector3 cellPosition, bool isIsCellEmpty)
    {
        CellPosition = cellPosition;
        IsCellEmpty = isIsCellEmpty;
    }
}
public class GridManager
{
    public Grid[][] GridMap;

    public GridManager()
    {
        InitializeGridMap();
    }
    
    private int MapHeight = 22, MapWidth = 10;

    public void InitializeGridMap()
    {
        GridMap = new Grid[MapWidth][]; // Initialize Y axis of array

        for (int i = 0; i < GridMap.Length; i++)
        {
            GridMap[i] = new Grid[MapHeight]; // Initialize X axis of array
        }

        for (int i = 0; i < GridMap.Length; i++)
        {
            for (int j = 0; j < GridMap[i].Length; j++)
            {
                GridMap[i][j] = new Grid(new Vector3(i,j,0), true); // Initialize all cell positions and status
            }
        } 
    }

    public bool IsCellsAvailable(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            var x = (int) positions[i].x;
            var y = (int) positions[i].y;

            //Debug.Log("x: " + x + " y: " + y);
            
            if (x < 0 || x >= MapWidth || y < 0 || y >= MapHeight)
            {
                return false;
            }

            if (!GridMap[x][y].IsCellEmpty)
            {
                return false;
            }
        }
        return true;
    }

    public void ClearFullLines(Vector3[] positions)
    {
        var fullLines = GetFullLines(positions);
        for (int i = 0; i < fullLines.Count; i++)
        {
            for (int j = 0; j < GridMap.Length; j++)
            {
                GridMap[j][fullLines[i]].IsCellEmpty = true;
            }
            for (int j = 0; j < GridMap.Length; j++)
            {
                for (int k = fullLines[i]+1; k < GridMap[j].Length; k++)
                {
                    GridMap[j][k-1].IsCellEmpty = GridMap[j][k].IsCellEmpty;
                }
            }
        }
    }
    public List<int> GetFullLines(Vector3[] positions)
    {
        var isRowFull = false;
        var rowNumbers = new List<int>();
        var fullLines = new List<int>();

        for (int i = 0; i < positions.Length; i++)
        {
            if (!rowNumbers.Contains((int)positions[i].y))
            {
                rowNumbers.Add((int)positions[i].y);
            }
        }
        rowNumbers.Sort();
        for (int i = rowNumbers.Count-1; i >= 0; i--)
        {
            for (int j = 0; j < GridMap.Length; j++)
            {
                if (GridMap[j][rowNumbers[i]].IsCellEmpty)
                {
                    isRowFull = false;
                    break;
                }

                isRowFull = true;
            }

            if (isRowFull)
            {
                fullLines.Add(rowNumbers[i]);
            }
        }
        return fullLines;
    }
}