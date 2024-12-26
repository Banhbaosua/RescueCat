using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GridData")]
public class GridData : ScriptableObject
{
    public Vector2 minAnchor;
    public Vector2 maxAnchor;
    public float unitWidth;
    public float unitHeight;

}
