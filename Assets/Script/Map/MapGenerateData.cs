using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName ="MapData",menuName ="Map/MapData")]
public class MapGenerateData : ScriptableObject
{
    [SerializeField] PropData[] propDatas;
    [SerializeField] int numberOfCar;
    [SerializeField] bool used;
    public PropData[]  PropDatas => propDatas;
    public int Cars => numberOfCar;
    public bool IsUsed=>used;
    public void SetPropsData(PropData[] propData,int cars) 
    { 
        this.propDatas = propData;
        this.numberOfCar = cars;
    }

    public PropData[] GetPropDatas()
    {
        return propDatas;
    }
    public void Used()
    {
        used = true;
    }
    public void RenewUse()
    {
        used = false;
    }
}
[Serializable]
public class PropData
{
    [SerializeField] Vector3 propSpawnPos;
    [SerializeField] Quaternion propSpawnRot;
    [SerializeField] GameObject propPrefab;

    public PropData(Vector3 propSpawnPos, Quaternion propSpawnRot, GameObject propPrefab)
    {
        this.propSpawnPos = propSpawnPos;
        this.propSpawnRot = propSpawnRot;
        this.propPrefab = propPrefab;
    }

    public Vector3 PropSpawnPos => propSpawnPos;
    public Quaternion PropSpawnRot => propSpawnRot;
    public GameObject PropPrefab => propPrefab;
}
