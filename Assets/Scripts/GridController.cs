using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public Vector3 CellPosition;
    public bool IsCellEmpty;

    public GridCell(Vector3 cellPosition, bool isIsCellEmpty)
    {
        CellPosition = cellPosition;
        IsCellEmpty = isIsCellEmpty;
    }
}

public class Grid
{
    public GridCell[][] GridMap; // Matrix of the Grid
    public Vector3 GridPos;
    public readonly int MapWidth, MapHeight;

    public Grid(Vector3 gridPos, int mapWidth, int mapHeight)
    {
        GridPos = gridPos;
        MapWidth = mapWidth;
        MapHeight = mapHeight;
        
        InitializeGridMap();
    }

    private void InitializeGridMap() //Load up the GridMap
    {
        GridMap = new GridCell[MapWidth][]; // Initialize Y axis of array

        for (int i = 0; i < GridMap.Length; i++)
        {
            GridMap[i] = new GridCell[MapHeight]; // Initialize X axis of array
        }

        for (int i = 0; i < GridMap.Length; i++)
        {
            for (int j = 0; j < GridMap[i].Length; j++)
            {
                GridMap[i][j] = new GridCell(new Vector3(i,j,0), true); // Initialize all cell positions and status
            }
        } 
    }
}
public class GridController
{
    public readonly Grid Grid;

    public GridController(Vector3 gridPos, int mapWidth, int mapHeight)
    {
        Grid = new Grid(gridPos, mapWidth, mapHeight);
    }

    public bool IsCellsAvailable(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            var x = (int) positions[i].x;
            var y = (int) positions[i].y;
            
            if (x < 0 || x >= Grid.MapWidth || y < 0 || y >= Grid.MapHeight)
            {
                return false;
            }

            if (!Grid.GridMap[x][y].IsCellEmpty)
            {
                return false;
            }
        }
        return true;
    }

    public bool TryClearFullLines(Vector3[] positions, out List<int> fullLines) // Return if any full lines and clear them.
    {
        fullLines = GetFullLines(positions);

        if (fullLines.Count == 0) return false;
        
        for (int i = 0; i < fullLines.Count; i++)
        {
            for (int j = 0; j < Grid.GridMap.Length; j++)
            {
                Grid.GridMap[j][fullLines[i]].IsCellEmpty = true;
            }
            for (int j = 0; j < Grid.GridMap.Length; j++)
            {
                for (int k = fullLines[i]+1; k < Grid.GridMap[j].Length; k++)
                {
                    Grid.GridMap[j][k-1].IsCellEmpty = Grid.GridMap[j][k].IsCellEmpty;
                }
            }
        }

        return true;
    }

    private List<int> GetFullLines(Vector3[] positions) // Get any fullLines at certain Y axises
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
            for (int j = 0; j < Grid.GridMap.Length; j++)
            {
                if (Grid.GridMap[j][rowNumbers[i]].IsCellEmpty)
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