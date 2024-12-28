using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
public enum PropType
{
    Car,
    Obstacle,
}
public class PropsSpawner : BaseSpawner
{
    public override void Spawn(int numberOfCar,bool random = false)
    {
        var propData = data.PropDatas;
        int cars = data.Cars;
        for(int i  = 0; i < propData.Length; i++) 
        {
            var prefab = propData[i].PropPrefab;
            if (random)
            {
                if (cars > 0)
                {
                    prefab = GetRandomCarPrefab();
                    cars--;
                }
                else
                    prefab = GetRandomObstaclePrefab();
            }
            var newProp = Instantiate(prefab);
            newProp.transform.position = propData[i].PropSpawnPos;
            newProp.transform.rotation = propData[i].PropSpawnRot;
        }
    }
    public GameObject GetRandomCarPrefab()
    {
        var random = Random.Range(0,carPrefab.Length);
        return carPrefab[random];
    }

    public GameObject GetRandomObstaclePrefab()
    {
        var random = Random.Range(0, obstaclePrefab.Length);
        return obstaclePrefab[random];
    }
}
