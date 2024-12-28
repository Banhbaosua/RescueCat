using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUnit
{
    private Vector2 position;
    private bool isOccupied;
    public bool IsOccupied => isOccupied;
    public Vector2 Pos => position;
    public GridUnit(Vector2 position)
    {
        this.position = position;
        this.isOccupied = false;
    }

    public void Occupied()
    {
        isOccupied = true;
    }
    public bool CheckOccupied()
    {
        return isOccupied;
    }
}
