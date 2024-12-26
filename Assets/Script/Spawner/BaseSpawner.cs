using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpawner
{
    protected Vector2[][] grid;
    protected MapGenerateData data;
    public BaseSpawner(Vector2[][] grid, MapGenerateData data)
    {
        this.grid = grid;
        this.data = data;
    }

    public virtual void Spawn()
    {

    }
}
