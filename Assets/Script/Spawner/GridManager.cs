using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    private GridData data;
    private GridUnit[][] grid;
    public GridUnit[][] Grids => grid;
    public GridManager(GridData data)
    {
        this.data = data;
    }
    public GridUnit[][] Generate()
    {
        var width = data.maxAnchor.x - data.minAnchor.x;
        var height = data.maxAnchor.y - data.minAnchor.y;

        var widthSteps = (int)(width / data.unitWidth);
        var heightSteps = (int)(height / data.unitHeight);

        grid = new GridUnit[widthSteps][];
        for(int i = 0;i < widthSteps; i++) 
        {
            grid[i] = new GridUnit[heightSteps];
            for(int o =0;o < heightSteps;o++)
            {
                var pos = new Vector2(data.minAnchor.x + data.unitWidth * i, data.minAnchor.y + data.unitHeight * o);
                var worldPos = pos + new Vector2(data.unitWidth / 2, data.unitHeight / 2);
                grid[i][o] = new GridUnit(worldPos);
            }
        }
        return grid;
    }

    public GridUnit GetUnitAtPosition(Vector2 pos)
    {
        var posX = pos.x;
        var posY = pos.y;
        int stepX = (int)((posX - data.minAnchor.x)/data.unitWidth);
        int stepY = (int)((posY - data.minAnchor.y)/data.unitHeight);

        var gridUnit = grid[stepX][stepY];
        return gridUnit;
    }

    public int StepsXToReachPos(float x)
    {
        int stepX = (int)((x - data.minAnchor.x) / data.unitWidth);
        if (stepX < 0)
            stepX = 0;
        return stepX;
    }

    public int StepsYToReachPos(float y)
    {
        int stepY = (int)((y - data.minAnchor.y) / data.unitHeight);
        if (stepY < 0)
            stepY = 0;
        return stepY;
    }
    public bool CheckGridOccupiedLeft(int minXLeft,int minY, int steps, GridUnit[][] grid)
    {
        for(int i =minY;i <= steps; i++)
        {
            if(grid[minXLeft - 1][i].IsOccupied) 
                return false;
        }
        return true;
    }

    public bool CheckGridOccupiedRight(int minXRight, int minY, int steps, GridUnit[][] grid)
    {
        for (int i = minY;i<= steps;i++)
        {
            if (grid[minXRight + 1][i].IsOccupied)
                return false;
        }
        return true;
    }

    public bool OccupiedUnitsOfGrid(int minX,int maxX,int minY,int maxY, GridUnit[][] grid)
    {
        for(int i = minX;i<= maxX; i++)
        {
            if (i < 0 || i > grid.Length - 1)
                continue;
            for(int o = minY;o<= maxY; o++)
            {
                if(o < 0 || o > grid.Length - 1) 
                    continue;
                if ( grid[i][o].IsOccupied)
                    return false;

                grid[i][o].Occupied();
            }
        }
        return true;
    }
}
