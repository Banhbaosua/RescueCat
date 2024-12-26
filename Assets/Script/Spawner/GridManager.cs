using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    private GridData data;
    private Vector2[][] grid;
    public Vector2[][] Grids => grid;
    public GridManager(GridData data)
    {
        this.data = data;
    }
    public void Generate()
    {
        var width = data.maxAnchor.x - data.minAnchor.x;
        var height = data.maxAnchor.y - data.minAnchor.y;

        var widthSteps = (int)(width / data.unitWidth);
        var heightSteps = (int)(height / data.unitHeight);

        grid = new Vector2[widthSteps][];
        for(int i = 0;i < widthSteps; i++) 
        {
            grid[i] = new Vector2[heightSteps];
            for(int o =0;o < heightSteps;o++)
            {
                grid[i][o] = new Vector2(data.minAnchor.x + data.unitWidth*i,data.minAnchor.y + data.unitHeight *o);
                grid[i][o] = grid[i][o] + new Vector2(data.unitWidth /2,data.unitHeight /2);
            }
        }
    }

    public Vector2 GetUnitAtPosition(Transform transform)
    {
        var posX = transform.position.x;
        var posY = transform.position.z;
        int stepX = (int)((posX - data.minAnchor.x)/data.unitWidth);
        int stepY = (int)((posY - data.minAnchor.y)/data.unitHeight);
        Debug.Log("step X: " + stepX);
        Debug.Log("step Y: " + stepY);
        return new Vector2(grid[stepX][stepY].x, grid[stepX][stepY].y);
    }
}
