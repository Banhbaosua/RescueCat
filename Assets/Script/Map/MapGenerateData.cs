using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MapData",menuName ="Map/MapData")]
public class MapGenerateData : ScriptableObject
{
    [SerializeField] Transform[] catSpawnPos;
    [SerializeField] Transform[] propSpawnPos;
    
    public void SetCatSpawnPost(Transform[] catSpawnPos)
    {
        this.catSpawnPos = catSpawnPos;
    }

    public void SetPropsSpawnPos(Transform[] propSpawnPos) 
    { 
        this.propSpawnPos = propSpawnPos;
    }

    public Transform[] GetCatSpawnPos()
    {
        return catSpawnPos;
    }

    public Transform[] GetPropSpawnPos()
    {
        return propSpawnPos;
    }
}
