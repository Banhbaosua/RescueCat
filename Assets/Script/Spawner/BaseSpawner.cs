using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject[] carPrefab;
    [SerializeField] protected GameObject[] obstaclePrefab;
    protected MapGenerateData data;

    public void SetGenerateData( MapGenerateData data)
    {
        this.data = data;
    }

    public virtual void Spawn(int numberOfCar, bool random = false)
    {

    }
}
